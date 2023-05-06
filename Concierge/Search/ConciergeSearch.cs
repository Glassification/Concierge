// <copyright file="ConciergeSearch.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Search
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Controls;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Display;
    using Concierge.Display.Components;
    using Concierge.Search.Enums;

    public sealed class ConciergeSearch
    {
        public ConciergeSearch(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
            this.Results = new List<SearchResult>();
            this.SearchSettings = new SearchSettings();
            this.Regex = new Regex(string.Empty);
        }

        public SearchSettings SearchSettings { get; private set; }

        private MainWindow MainWindow { get; set; }

        private List<SearchResult> Results { get; set; }

        private Regex Regex { get; set; }

        private int Depth { get; set; }

        public List<SearchResult> Search(SearchSettings searchSettings)
        {
            this.Results.Clear();
            this.SearchSettings = searchSettings;
            this.Depth = 0;

            if (searchSettings?.TextToSearch.IsNullOrWhiteSpace() ?? true)
            {
                return this.Results;
            }

            this.CreateRegex();
            this.SearchWithSettings();

            return this.Results;
        }

        private void CreateRegex()
        {
            string pattern;

            if (this.SearchSettings.UseRegex)
            {
                pattern = this.SearchSettings.TextToSearch;
            }
            else
            {
                pattern = this.SearchSettings.MatchWholeWord
                        ? @$"\b{Regex.Escape(this.SearchSettings.TextToSearch)}\b"
                        : Regex.Escape(this.SearchSettings.TextToSearch);
            }

            this.Regex = this.SearchSettings.MatchCase
                ? new Regex(pattern)
                : new Regex(pattern, RegexOptions.IgnoreCase);
        }

        private void SearchWithSettings()
        {
            switch (this.SearchSettings.SearchDomain)
            {
                case SearchDomain.CurrentPage:
                    if (this.MainWindow.CurrentPage is not null)
                    {
                        this.SearchPage(this.MainWindow.CurrentPage);
                    }

                    break;
                case SearchDomain.EntireSheet:
                    this.SearchPages();
                    break;
            }
        }

        private void SearchPages()
        {
            this.SearchPage(this.MainWindow.OverviewPage);
            this.SearchPage(this.MainWindow.DetailsPage);
            this.SearchPage(this.MainWindow.AttacksPage);
            this.SearchPage(this.MainWindow.AbilityPage);
            this.SearchPage(this.MainWindow.EquipmentPage);
            this.SearchPage(this.MainWindow.InventoryPage);
            this.SearchPage(this.MainWindow.SpellcastingPage);
            this.SearchPage(this.MainWindow.CompanionPage);
            this.SearchPage(this.MainWindow.ToolsPage);
            this.SearchPage(this.MainWindow.JournalPage);
        }

        private void SearchPage(IConciergePage conciergePage)
        {
            this.SearchDataGrids(conciergePage);
            this.SearchTextBlocks(conciergePage);
            this.SearchTreeView(conciergePage);
            this.SearchDocuments(conciergePage);
        }

        private void SearchDocuments(IConciergePage conciergePage)
        {
            if (conciergePage is not Page page)
            {
                return;
            }

            var chapters = Program.CcsFile.Character.Journal.Chapters;
            if (!chapters.Any() || !DisplayUtility.FindVisualChildren<RichTextBox>(page).Any())
            {
                return;
            }

            foreach (var chapter in chapters)
            {
                var documents = chapter.Documents;
                if (!documents.Any())
                {
                    continue;
                }

                foreach (var document in documents)
                {
                    if (this.Regex.IsMatch(document.Rtf.StripRichTextFormat() ?? string.Empty))
                    {
                        this.Results.Add(new SearchResult(this.SearchSettings.TextToSearch, document, this.Regex, conciergePage));
                    }
                }
            }
        }

        private void SearchTreeView(IConciergePage conciergePage)
        {
            if (conciergePage is not Page page)
            {
                return;
            }

            var treeViews = DisplayUtility.FindVisualChildren<TreeView>(page);
            if (!treeViews.Any())
            {
                return;
            }

            foreach (var treeView in treeViews)
            {
                if (treeView.Items.Count == 0)
                {
                    continue;
                }

                foreach (TreeViewItem treeViewItem in treeView.Items)
                {
                    if (treeViewItem.Header is not Grid grid)
                    {
                        continue;
                    }

                    var textBlock = DisplayUtility.FindVisualChildren<TextBlock>(grid).FirstOrDefault();
                    if (this.Regex.IsMatch(textBlock?.Text ?? string.Empty))
                    {
                        this.Results.Add(new SearchResult(this.SearchSettings.TextToSearch, treeViewItem, this.Regex, conciergePage));
                    }

                    if (treeViewItem.Items.Count == 0)
                    {
                        continue;
                    }

                    foreach (TreeViewItem treeViewItem2 in treeViewItem.Items)
                    {
                        if (treeViewItem2.Header is not Grid grid2)
                        {
                            continue;
                        }

                        var textBlock2 = DisplayUtility.FindVisualChildren<TextBlock>(grid2).FirstOrDefault();
                        if (this.Regex.IsMatch(textBlock2?.Text ?? string.Empty))
                        {
                            this.Results.Add(new SearchResult(this.SearchSettings.TextToSearch, treeViewItem2, this.Regex, conciergePage));
                        }
                    }
                }
            }
        }

        private void SearchTextBlocks(IConciergePage conciergePage)
        {
            if (conciergePage is not Page page)
            {
                return;
            }

            var textBlocks = DisplayUtility.FindVisualChildren<ConciergeTextBlock>(page);
            if (!textBlocks.Any())
            {
                return;
            }

            foreach (var textBlock in textBlocks)
            {
                if (this.Regex.IsMatch(textBlock.Text))
                {
                    this.Results.Add(new SearchResult(this.SearchSettings.TextToSearch, textBlock, this.Regex, conciergePage));
                }
            }
        }

        private void SearchDataGrids(IConciergePage conciergePage)
        {
            if (conciergePage is not Page page)
            {
                return;
            }

            var dataGrids = DisplayUtility.FindVisualChildren<ConciergeDataGrid>(page);
            if (!dataGrids.Any())
            {
                return;
            }

            foreach (var dataGrid in dataGrids)
            {
                this.SearchList<object>((from object item in dataGrid.Items select item).ToList(), conciergePage);
            }
        }

        private void SearchList<T>(IEnumerable<T> list, IConciergePage conciergePage)
        {
            foreach (var item in list)
            {
                if (item is null)
                {
                    continue;
                }

                if (this.SearchListObject(item, conciergePage))
                {
                    this.Results.Add(new SearchResult(this.SearchSettings.TextToSearch, item, this.Regex, conciergePage));
                }
            }
        }

        private bool SearchListObject(object item, IConciergePage conciergePage)
        {
            this.Depth++;

            try
            {
                var properties = item.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(item);

                    if (propertyValue is null)
                    {
                        continue;
                    }

                    if (propertyValue.IsList() && this.Depth < Constants.MaxDepth)
                    {
                        dynamic list = propertyValue;
                        this.SearchList(list, conciergePage);
                    }

                    if (this.Regex.IsMatch(propertyValue.ToString() ?? string.Empty))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }

            this.Depth--;

            return false;
        }
    }
}
