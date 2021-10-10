// <copyright file="ConciergeSearch.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Searching
{
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Interfaces;
    using Concierge.Interfaces.AbilitiesPageInterface;
    using Concierge.Interfaces.AttackDefensePageInterface;
    using Concierge.Interfaces.CompanionPageInterface;
    using Concierge.Interfaces.DetailsPageInterface;
    using Concierge.Interfaces.EquippedItemsPageInterface;
    using Concierge.Interfaces.InventoryPageInterface;
    using Concierge.Interfaces.SpellcastingPageInterface;
    using Concierge.Utility.Extensions;

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

        private ConciergeCharacter Character { get; set; }

        private List<SearchResult> Results { get; set; }

        public List<SearchResult> Search(SearchSettings searchSettings, ConciergeCharacter character)
        {
            this.Results.Clear();
            this.SearchSettings = searchSettings;
            this.Character = character;

            if (searchSettings?.TextToSearch.IsNullOrWhiteSpace() ?? true)
            {
                return this.Results;
            }

            this.SearchWithSettings();

            return this.Results;
        }

        private void SearchWithSettings()
        {
            switch (this.SearchSettings.SearchDomain)
            {
                case Enums.SearchDomain.CurrentPage:
                    this.SearchPage(this.MainWindow.CurrentPage);
                    break;
                case Enums.SearchDomain.EntireSheet:
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
        }

        private void SearchList<T>(List<T> list, IConciergePage conciergePage)
        {
            foreach (var item in list)
            {
                if (this.SearchObject(item))
                {
                    this.Results.Add(new SearchResult(item, conciergePage));
                }
            }
        }

        private bool SearchObject(object item)
        {
            var properties = item.GetType().GetProperties();
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(item);

                if (propertyValue is not string)
                {
                    continue;
                }

                var found = this.SearchSettings.MatchCase
                    ? (propertyValue as string).Contains(this.SearchSettings.TextToSearch, System.StringComparison.InvariantCultureIgnoreCase)
                    : (propertyValue as string).Contains(this.SearchSettings.TextToSearch);

                if (found)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
