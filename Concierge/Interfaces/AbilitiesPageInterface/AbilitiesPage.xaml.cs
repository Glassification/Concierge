// <copyright file="AbilitiesPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.AbilitiesPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Characteristics;
    using Concierge.Commands;
    using Concierge.Interfaces.Enums;
    using Concierge.Services;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for AbilitiesPage.xaml.
    /// </summary>
    public partial class AbilitiesPage : Page, IConciergePage
    {
        public AbilitiesPage()
        {
            this.InitializeComponent();

            this.DataContext = this;

            Program.Logger.Info($"Initialized {nameof(AbilitiesPage)}.");
        }

        public static double AbilitiesHeight => SystemParameters.PrimaryScreenHeight - 100;

        public ConciergePage ConciergePage => ConciergePage.Abilities;

        public bool HasEditableDataGrid => true;

        public void Draw()
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
            ConciergeWindowService.ShowEdit<Ability>(
                ability,
                typeof(ModifyAbilitiesWindow),
                this.Window_ApplyChanges,
                ConciergePage.Abilities);
            this.DrawAbilities();
            this.AbilitiesDataGrid.SetSelectedIndex(index);
        }

        private void DrawAbilities()
        {
            this.AbilitiesDataGrid.Items.Clear();

            foreach (var ability in Program.CcsFile.Character.Abilities)
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

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            var index = this.AbilitiesDataGrid.NextItem(Program.CcsFile.Character.Abilities, 0, -1, this.ConciergePage);

            if (index != -1)
            {
                this.DrawAbilities();
                this.AbilitiesDataGrid.SetSelectedIndex(index);
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            var index = this.AbilitiesDataGrid.NextItem(Program.CcsFile.Character.Abilities, Program.CcsFile.Character.Abilities.Count - 1, 1, this.ConciergePage);

            if (index != -1)
            {
                this.DrawAbilities();
                this.AbilitiesDataGrid.SetSelectedIndex(index);
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this.AbilitiesDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd<List<Ability>>(
                Program.CcsFile.Character.Abilities,
                typeof(ModifyAbilitiesWindow),
                this.Window_ApplyChanges,
                ConciergePage.Abilities);
            this.DrawAbilities();

            if (added)
            {
                this.AbilitiesDataGrid.SetSelectedIndex(this.AbilitiesDataGrid.LastIndex);
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.AbilitiesDataGrid.SelectedItem != null)
            {
                this.Edit(this.AbilitiesDataGrid.SelectedItem);
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.AbilitiesDataGrid.SelectedItem != null)
            {
                var ability = (Ability)this.AbilitiesDataGrid.SelectedItem;
                var index = this.AbilitiesDataGrid.SelectedIndex;

                Program.UndoRedoService.AddCommand(new DeleteCommand<Ability>(Program.CcsFile.Character.Abilities, ability, index, this.ConciergePage));
                Program.CcsFile.Character.Abilities.Remove(ability);
                this.DrawAbilities();
                this.AbilitiesDataGrid.SetSelectedIndex(index);

                Program.Modify();
            }
        }

        private void AbilitiesDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SortListFromDataGrid(this.AbilitiesDataGrid, Program.CcsFile.Character.Abilities, this.ConciergePage);
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(ModifyAbilitiesWindow):
                    this.DrawAbilities();
                    this.ScrollAbilities();
                    break;
            }
        }
    }
}
