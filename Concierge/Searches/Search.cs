// <copyright file="Search.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Searches
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
    using Concierge.Searches.Enums;

    /// <summary>
    /// Represents a utility class for conducting searches within the Concierge application.
    /// </summary>
    public sealed class Search
    {
        private readonly MainWindow mainWindow;
        private readonly List<SearchResult> results;

        private Regex regex;
        private int depth;

        /// <summary>
        /// Initializes a new instance of the <see cref="Search"/> class with the specified main window.
        /// </summary>
        /// <param name="mainWindow">The main window of the Concierge application.</param>
        public Search(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.results = [];
            this.SearchSettings = new SearchSettings();
            this.regex = new Regex(string.Empty);
        }

        /// <summary>
        /// Gets the search settings used for the search operation.
        /// </summary>
        public SearchSettings SearchSettings { get; private set; }

        /// <summary>
        /// Searches for the specified search settings and returns the list of search results.
        /// </summary>
        /// <param name="searchSettings">The search settings to use for the search operation.</param>
        /// <returns>The list of search results.</returns>
        public List<SearchResult> Start(SearchSettings searchSettings)
        {
            this.results.Clear();
            this.SearchSettings = searchSettings;
            this.depth = 0;

            if (searchSettings?.TextToSearch.IsNullOrWhiteSpace() ?? true)
            {
                return this.results;
            }

            this.CreateRegex();
            this.SearchWithSettings();

            return this.results;
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

            this.regex = this.SearchSettings.MatchCase
                ? new Regex(pattern)
                : new Regex(pattern, RegexOptions.IgnoreCase);
        }

        private void SearchWithSettings()
        {
            switch (this.SearchSettings.SearchDomain)
            {
                case SearchDomain.CurrentPage:
                    if (this.mainWindow.CurrentPage is not null)
                    {
                        this.SearchPage(this.mainWindow.CurrentPage);
                    }

                    break;
                case SearchDomain.EntireSheet:
                    this.SearchPages();
                    break;
            }
        }

        private void SearchPages()
        {
            this.SearchPage(this.mainWindow.OverviewPage);
            this.SearchPage(this.mainWindow.DetailsPage);
            this.SearchPage(this.mainWindow.AttacksPage);
            this.SearchPage(this.mainWindow.AbilityPage);
            this.SearchPage(this.mainWindow.EquipmentPage);
            this.SearchPage(this.mainWindow.InventoryPage);
            this.SearchPage(this.mainWindow.SpellcastingPage);
            this.SearchPage(this.mainWindow.CompanionPage);
            this.SearchPage(this.mainWindow.ToolsPage);
            this.SearchPage(this.mainWindow.JournalPage);
        }

        private void SearchPage(ConciergePage conciergePage)
        {
            this.SearchDataGrids(conciergePage);
            this.SearchTextBlocks(conciergePage);
            this.SearchTreeView(conciergePage);
            this.SearchDocuments(conciergePage);
        }

        private void SearchDocuments(ConciergePage conciergePage)
        {
            var chapters = Program.CcsFile.Character.Journal.Chapters;
            if (chapters.Count == 0 || !DisplayUtility.FindVisualChildren<RichTextBox>(conciergePage).Any())
            {
                return;
            }

            foreach (var chapter in chapters)
            {
                var documents = chapter.Documents;
                if (documents.IsEmpty())
                {
                    continue;
                }

                foreach (var document in documents)
                {
                    if (this.regex.IsMatch(document.Rtf.StripRichTextFormat() ?? string.Empty))
                    {
                        this.results.Add(new SearchResult(this.SearchSettings.TextToSearch, document, this.regex, conciergePage));
                    }
                }
            }
        }

        private void SearchTreeView(ConciergePage conciergePage)
        {
            var treeViews = DisplayUtility.FindVisualChildren<TreeView>(conciergePage);
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
                    if (this.regex.IsMatch(textBlock?.Text ?? string.Empty))
                    {
                        this.results.Add(new SearchResult(this.SearchSettings.TextToSearch, treeViewItem, this.regex, conciergePage));
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
                        if (this.regex.IsMatch(textBlock2?.Text ?? string.Empty))
                        {
                            this.results.Add(new SearchResult(this.SearchSettings.TextToSearch, treeViewItem2, this.regex, conciergePage));
                        }
                    }
                }
            }
        }

        private void SearchTextBlocks(ConciergePage conciergePage)
        {
            var textBlocks = DisplayUtility.FindVisualChildren<ConciergeTextBlock>(conciergePage);
            if (!textBlocks.Any())
            {
                return;
            }

            foreach (var textBlock in textBlocks)
            {
                if (this.regex.IsMatch(textBlock.Text))
                {
                    this.results.Add(new SearchResult(this.SearchSettings.TextToSearch, textBlock, this.regex, conciergePage));
                }
            }
        }

        private void SearchDataGrids(ConciergePage conciergePage)
        {
            var dataGrids = DisplayUtility.FindVisualChildren<ConciergeDataGrid>(conciergePage);
            if (!dataGrids.Any())
            {
                return;
            }

            foreach (var dataGrid in dataGrids)
            {
                this.SearchList<object>((from object item in dataGrid.Items select item).ToList(), conciergePage);
            }
        }

        private void SearchList<T>(IEnumerable<T> list, ConciergePage conciergePage)
        {
            foreach (var item in list)
            {
                if (item is null)
                {
                    continue;
                }

                if (this.SearchListObject(item, conciergePage))
                {
                    this.results.Add(new SearchResult(this.SearchSettings.TextToSearch, item, this.regex, conciergePage));
                }
            }
        }

        private bool SearchListObject(object item, ConciergePage conciergePage)
        {
            this.depth++;

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

                    if (propertyValue.IsList() && this.depth < Constants.MaxDepth)
                    {
                        dynamic list = propertyValue;
                        this.SearchList(list, conciergePage);
                    }

                    if (this.regex.IsMatch(propertyValue.ToString() ?? string.Empty))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }

            this.depth--;

            return false;
        }
    }
}
