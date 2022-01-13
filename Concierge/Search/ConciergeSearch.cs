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

    using Concierge.Interfaces;
    using Concierge.Interfaces.Components;
    using Concierge.Search.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Utilities;

    public class ConciergeSearch
    {
        public ConciergeSearch(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
            this.Results = new List<SearchResult>();
            this.SearchSettings = new SearchSettings();
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
                    this.SearchPage(this.MainWindow.CurrentPage);
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
            this.SearchPage(this.MainWindow.AttackDefensePage);
            this.SearchPage(this.MainWindow.AbilitiesPage);
            this.SearchPage(this.MainWindow.EquippedItemsPage);
            this.SearchPage(this.MainWindow.InventoryPage);
            this.SearchPage(this.MainWindow.SpellcastingPage);
            this.SearchPage(this.MainWindow.CompanionPage);
            this.SearchPage(this.MainWindow.ToolsPage);
            this.SearchPage(this.MainWindow.NotesPage);
        }

        private void SearchPage(IConciergePage conciergePage)
        {
            this.SearchDataGrids(conciergePage);
            this.SearchTextBlocks(conciergePage);
        }

        private void SearchTextBlocks(IConciergePage conciergePage)
        {
            var textBlocks = DisplayUtility.FindVisualChildren<ConciergeTextBlock>(conciergePage as Page);

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
            var dataGrids = DisplayUtility.FindVisualChildren<ConciergeDataGrid>(conciergePage as Page);

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

                    if (propertyValue == null)
                    {
                        continue;
                    }

                    if (propertyValue.IsList() && this.Depth < Constants.MaxDepth)
                    {
                        dynamic list = propertyValue;
                        this.SearchList(list, conciergePage);
                    }

                    if (this.Regex.IsMatch(propertyValue.ToString()))
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
