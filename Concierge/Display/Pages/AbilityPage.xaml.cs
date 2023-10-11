// <copyright file="AbilityPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Characteristics;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows;
    using Concierge.Display.Windows.Utility;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for AbilityPage.xaml.
    /// </summary>
    public partial class AbilityPage : Page, IConciergePage
    {
        public AbilityPage()
        {
            this.InitializeComponent();
        }

        public bool HasEditableDataGrid => true;

        public ConciergePage ConciergePage => ConciergePage.Inventory;

        private List<Ability> DisplayList => Program.CcsFile.Character.Characteristic.Abilities.Filter(this.SearchFilter.FilterText).ToList();

        public void Draw(bool isNewCharacterSheet = false)
        {
            this.DrawAbilities();
        }

        public void Edit(object itemToEdit)
        {
            if (itemToEdit is not Ability ability)
            {
                return;
            }

            var index = this.AbilitiesDataGrid.SelectedIndex;
            ConciergeWindowService.ShowEdit(
                ability,
                typeof(AbilitiesWindow),
                this.Window_ApplyChanges,
                ConciergePage.Abilities);
            this.DrawAbilities();
            this.AbilitiesDataGrid.SetSelectedIndex(index);
        }

        private void DrawAbilities()
        {
            this.AbilitiesDataGrid.Items.Clear();

            foreach (var ability in this.DisplayList)
            {
                this.AbilitiesDataGrid.Items.Add(ability);
            }
        }

        private void ScrollAbilities()
        {
            if (this.AbilitiesDataGrid.Items.Count > 0)
            {
                this.AbilitiesDataGrid.SelectedItem = this.AbilitiesDataGrid.Items[^1];
                this.AbilitiesDataGrid.UpdateLayout();
                this.AbilitiesDataGrid.ScrollIntoView(this.AbilitiesDataGrid.SelectedItem);
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            var index = this.AbilitiesDataGrid.NextItem(Program.CcsFile.Character.Characteristic.Abilities, 0, -1, this.ConciergePage);

            if (index != -1)
            {
                this.DrawAbilities();
                this.AbilitiesDataGrid.SetSelectedIndex(index);
            }
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            var index = this.AbilitiesDataGrid.NextItem(Program.CcsFile.Character.Characteristic.Abilities, Program.CcsFile.Character.Characteristic.Abilities.Count - 1, 1, this.ConciergePage);

            if (index != -1)
            {
                this.DrawAbilities();
                this.AbilitiesDataGrid.SetSelectedIndex(index);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.AbilitiesDataGrid.UnselectAll();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd(
                Program.CcsFile.Character.Characteristic.Abilities,
                typeof(AbilitiesWindow),
                this.Window_ApplyChanges,
                ConciergePage.Abilities);
            this.DrawAbilities();

            if (added)
            {
                this.AbilitiesDataGrid.SetSelectedIndex(this.AbilitiesDataGrid.LastIndex);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.AbilitiesDataGrid.SelectedItem != null)
            {
                this.Edit(this.AbilitiesDataGrid.SelectedItem);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.AbilitiesDataGrid.SelectedItem != null)
            {
                var ability = (Ability)this.AbilitiesDataGrid.SelectedItem;
                var index = this.AbilitiesDataGrid.SelectedIndex;

                Program.UndoRedoService.AddCommand(new DeleteCommand<Ability>(Program.CcsFile.Character.Characteristic.Abilities, ability, index, this.ConciergePage));
                Program.CcsFile.Character.Characteristic.Abilities.Remove(ability);
                this.DrawAbilities();
                this.AbilitiesDataGrid.SetSelectedIndex(index);
            }
        }

        private void AbilitiesDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            this.AbilitiesDataGrid.SortListFromDataGrid(Program.CcsFile.Character.Characteristic.Abilities, this.ConciergePage);
        }

        private void AbilityDataGrid_Filtered(object sender, RoutedEventArgs e)
        {
            this.SearchFilter.SetButtonEnableState(this.UpButton);
            this.SearchFilter.SetButtonEnableState(this.DownButton);

            this.DrawAbilities();
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(AbilitiesWindow):
                    this.DrawAbilities();
                    this.ScrollAbilities();
                    break;
            }
        }

        private void AbilityUseButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.AbilitiesDataGrid.SelectedItem is null)
            {
                return;
            }

            var ability = (Ability)this.AbilitiesDataGrid.SelectedItem;
            var result = ability.Use();

            ConciergeWindowService.ShowUseItemWindow(typeof(UseItemWindow), result);
        }
    }
}
