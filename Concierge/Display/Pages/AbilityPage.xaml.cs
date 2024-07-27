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

    using Concierge.Character.Details;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows;
    using Concierge.Display.Windows.Utility;
    using Concierge.Services;
    using Concierge.Tools;

    /// <summary>
    /// Interaction logic for AbilityPage.xaml.
    /// </summary>
    public partial class AbilityPage : ConciergePage
    {
        public AbilityPage()
        {
            this.InitializeComponent();

            this.HasEditableDataGrid = true;
            this.ConciergePages = ConciergePages.Abilities;
        }

        private List<Ability> DisplayList => Program.CcsFile.Character.Detail.Abilities.Filter(this.SearchFilter.FilterText).ToList();

        public override void Draw(bool isNewCharacterSheet = false)
        {
            this.DrawAbilities();
        }

        public override void Edit(object itemToEdit)
        {
            if (itemToEdit is not Ability ability)
            {
                return;
            }

            var index = this.AbilitiesDataGrid.SelectedIndex;
            WindowService.ShowEdit(
                ability,
                typeof(AbilitiesWindow),
                this.Window_ApplyChanges,
                ConciergePages.Abilities);
            this.DrawAbilities();
            this.AbilitiesDataGrid.SetSelectedIndex(index);
        }

        private void DrawAbilities()
        {
            this.AbilitiesDataGrid.Items.Clear();
            this.DisplayList.ForEach(ability => this.AbilitiesDataGrid.Items.Add(ability));
            this.SetDataGridControlState();
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

        private void SetDataGridControlState()
        {
            this.AbilitiesDataGrid.SetButtonControlsEnableState(
                this.UpButton,
                this.DownButton,
                this.EditButton,
                this.AbilityUseButton,
                this.DeleteButton);
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            var index = this.AbilitiesDataGrid.NextItem(Program.CcsFile.Character.Detail.Abilities, 0, -1, this.ConciergePages);

            if (index != -1)
            {
                this.DrawAbilities();
                this.AbilitiesDataGrid.SetSelectedIndex(index);
            }
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            var index = this.AbilitiesDataGrid.NextItem(Program.CcsFile.Character.Detail.Abilities, Program.CcsFile.Character.Detail.Abilities.Count - 1, 1, this.ConciergePages);

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
            var added = WindowService.ShowAdd(
                Program.CcsFile.Character.Detail.Abilities,
                typeof(AbilitiesWindow),
                this.Window_ApplyChanges,
                ConciergePages.Abilities);
            this.DrawAbilities();

            if (added)
            {
                this.AbilitiesDataGrid.SetSelectedIndex(this.AbilitiesDataGrid.LastIndex);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.AbilitiesDataGrid.SelectedItem is not null)
            {
                this.Edit(this.AbilitiesDataGrid.SelectedItem);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.AbilitiesDataGrid.SelectedItem is not null)
            {
                var ability = (Ability)this.AbilitiesDataGrid.SelectedItem;
                var index = this.AbilitiesDataGrid.SelectedIndex;

                Program.UndoRedoService.AddCommand(new DeleteCommand<Ability>(Program.CcsFile.Character.Detail.Abilities, ability, index, this.ConciergePages));
                Program.CcsFile.Character.Detail.Abilities.Remove(ability);
                this.DrawAbilities();
                this.AbilitiesDataGrid.SetSelectedIndex(index);
            }
        }

        private void AbilitiesDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            this.AbilitiesDataGrid.SortListFromDataGrid(Program.CcsFile.Character.Detail.Abilities, this.ConciergePages);
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
            var result = ability.Use(UseItem.Empty);

            WindowService.ShowUseItemWindow(typeof(UseItemWindow), result);
        }

        private void AbilitiesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetDataGridControlState();
        }
    }
}
