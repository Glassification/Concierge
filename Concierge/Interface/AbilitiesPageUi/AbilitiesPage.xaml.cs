// <copyright file="AbilitiesPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.AbilitiesPageUi
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Characters.Characteristics;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for AbilitiesPage.xaml.
    /// </summary>
    public partial class AbilitiesPage : Page, IConciergePage
    {
        private readonly ModifyAbilitiesWindow modifyAbilitiesWindow = new ModifyAbilitiesWindow();

        public AbilitiesPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.modifyAbilitiesWindow.ApplyChanges += this.Window_ApplyChanges;

            Program.Logger.Info($"Initialized {nameof(AbilitiesPage)}.");
        }

        public double AbilitiesHeight => SystemParameters.PrimaryScreenHeight - 100;

        public void Draw()
        {
            this.DrawAbilities();
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
                this.AbilitiesDataGrid.SelectedItem = this.AbilitiesDataGrid.Items[this.AbilitiesDataGrid.Items.Count - 1];
                this.AbilitiesDataGrid.UpdateLayout();
                this.AbilitiesDataGrid.ScrollIntoView(this.AbilitiesDataGrid.SelectedItem);
            }
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            var index = this.AbilitiesDataGrid.NextItem(Program.CcsFile.Character.Abilities, 0, -1);

            if (index != -1)
            {
                this.DrawAbilities();
                this.AbilitiesDataGrid.SelectedIndex = index;
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            var index = this.AbilitiesDataGrid.NextItem(Program.CcsFile.Character.Abilities, Program.CcsFile.Character.Abilities.Count - 1, 1);

            if (index != -1)
            {
                this.DrawAbilities();
                this.AbilitiesDataGrid.SelectedIndex = index;
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this.AbilitiesDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            this.modifyAbilitiesWindow.ShowAdd(Program.CcsFile.Character.Abilities);
            this.DrawAbilities();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.AbilitiesDataGrid.SelectedItem != null)
            {
                this.modifyAbilitiesWindow.ShowEdit((Ability)this.AbilitiesDataGrid.SelectedItem);
                this.DrawAbilities();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.AbilitiesDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var ability = (Ability)this.AbilitiesDataGrid.SelectedItem;
                Program.CcsFile.Character.Abilities.Remove(ability);
                this.DrawAbilities();
            }
        }

        private void AbilitiesDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            Program.CcsFile.Character.Abilities.Clear();

            foreach (var ability in this.AbilitiesDataGrid.Items)
            {
                Program.CcsFile.Character.Abilities.Add(ability as Ability);
            }
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case "ModifyAbilitiesWindow":
                    this.DrawAbilities();
                    this.ScrollAbilities();
                    break;
            }
        }
    }
}
