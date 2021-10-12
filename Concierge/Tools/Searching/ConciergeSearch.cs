// <copyright file="ConciergeSearch.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Searching
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Windows.Controls;

    using Concierge.Character;
    using Concierge.Interfaces;
    using Concierge.Interfaces.AbilitiesPageInterface;
    using Concierge.Interfaces.AttackDefensePageInterface;
    using Concierge.Interfaces.CompanionPageInterface;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.DetailsPageInterface;
    using Concierge.Interfaces.EquippedItemsPageInterface;
    using Concierge.Interfaces.InventoryPageInterface;
    using Concierge.Interfaces.NotesPageInterface;
    using Concierge.Interfaces.SpellcastingPageInterface;
    using Concierge.Tools.Searching.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    public class ConciergeSearch
    {
        private const int MaxDepth = 10;

        public ConciergeSearch(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
            this.Results = new List<SearchResult>();
            this.SearchSettings = new SearchSettings();
        }

        public SearchSettings SearchSettings { get; private set; }

        private MainWindow MainWindow { get; set; }

        private ConciergeCharacter Character { get; set; }

        private List<SearchResult> Results { get; set; }

        private Regex Regex { get; set; }

        private int Depth { get; set; }

        public List<SearchResult> Search(SearchSettings searchSettings, ConciergeCharacter character)
        {
            this.Results.Clear();
            this.SearchSettings = searchSettings;
            this.Character = character;
            this.Depth = 0;

            if (searchSettings?.TextToSearch.IsNullOrWhiteSpace() ?? true)
            {
                return this.Results;
            }

            this.CreateRegex();
            this.SearchWithSettings();

            return this.Results;
        }

        public void Navigate(SearchResult searchResult)
        {
            if (!this.NavigateToDataGrid(searchResult))
            {
                this.NavigateToTreeView(searchResult);
            }
        }

        private bool NavigateToDataGrid(SearchResult searchResult)
        {
            var dataGrids = Utilities.FindVisualChildren<ConciergeDataGrid>(searchResult.ConciergePage as Page);

            foreach (var dataGrid in dataGrids)
            {
                var index = dataGrid.Items.IndexOf(searchResult.Item);
                if (index >= 0)
                {
                    Utilities.SetDataGridSelectedIndex(dataGrid, index);
                    dataGrid.ScrollIntoView(dataGrid.SelectedItem);
                    return true;
                }
            }

            return false;
        }

        private bool NavigateToTreeView(SearchResult searchResult)
        {
            var treeViews = Utilities.FindVisualChildren<TreeView>(searchResult.ConciergePage as Page);

            foreach (var treeView in treeViews)
            {
                var item = treeView.GetTreeViewItem(searchResult.Item);

                if (item is not null)
                {
                    item.IsSelected = true;
                    item.Focus();
                }
            }

            return false;
        }

        private void CreateRegex()
        {
            var pattern = this.SearchSettings.MatchWholeWord
                    ? $"\b{Regex.Escape(this.SearchSettings.TextToSearch)}\b"
                    : Regex.Escape(this.SearchSettings.TextToSearch);

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
            this.SearchPage(this.MainWindow.AbilitiesPage);
            this.SearchPage(this.MainWindow.AttackDefensePage);
            this.SearchPage(this.MainWindow.CompanionPage);
            this.SearchPage(this.MainWindow.DetailsPage);
            this.SearchPage(this.MainWindow.EquippedItemsPage);
            this.SearchPage(this.MainWindow.InventoryPage);
            this.SearchPage(this.MainWindow.NotesPage);
            this.SearchPage(this.MainWindow.SpellcastingPage);
        }

        private void SearchPage(IConciergePage conciergePage)
        {
            if (conciergePage is InventoryPage)
            {
                this.SearchList(this.Character.Inventories, conciergePage);
            }
            else if (conciergePage is AbilitiesPage)
            {
                this.SearchList(this.Character.Abilities, conciergePage);
            }
            else if (conciergePage is AttackDefensePage)
            {
                this.SearchList(this.Character.Weapons, conciergePage);
                this.SearchList(this.Character.Ammunitions, conciergePage);
            }
            else if (conciergePage is CompanionPage)
            {
                this.SearchList(this.Character.Companion.Attacks, conciergePage);
            }
            else if (conciergePage is DetailsPage)
            {
                this.SearchList(this.Character.ClassResources, conciergePage);
                this.SearchList(this.Character.Details.Languages, conciergePage);
                this.SearchList(this.Character.Proficiency, conciergePage);
            }
            else if (conciergePage is EquippedItemsPage)
            {
                this.SearchList(this.Character.EquippedItems.Head, conciergePage);
                this.SearchList(this.Character.EquippedItems.Torso, conciergePage);
                this.SearchList(this.Character.EquippedItems.Hands, conciergePage);
                this.SearchList(this.Character.EquippedItems.Legs, conciergePage);
                this.SearchList(this.Character.EquippedItems.Feet, conciergePage);
            }
            else if (conciergePage is SpellcastingPage)
            {
                this.SearchList(this.Character.Spells, conciergePage);
                this.SearchList(this.Character.MagicClasses, conciergePage);
            }
            else if (conciergePage is NotesPage)
            {
                this.SearchList(this.Character.Chapters, conciergePage);
            }
        }

        private void SearchList<T>(IEnumerable<T> list, IConciergePage conciergePage)
        {
            foreach (var item in list)
            {
                if (this.SearchObject(item, conciergePage))
                {
                    this.Results.Add(new SearchResult(this.SearchSettings.TextToSearch, item, conciergePage));
                }
            }
        }

        private bool SearchObject(object item, IConciergePage conciergePage)
        {
            try
            {
                var properties = item.GetType().GetProperties();
                foreach (var property in properties)
                {
                    this.Depth++;
                    var propertyValue = property.GetValue(item);

                    if (propertyValue == null)
                    {
                        continue;
                    }

                    if (propertyValue.IsList() && this.Depth < MaxDepth)
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
                Program.ErrorService.LogError(ex, Exceptions.Enums.Severity.Release);
            }

            this.Depth--;

            return false;
        }
    }
}
