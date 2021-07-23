// <copyright file="AbilitiesPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.AbilitiesPageUi
{
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Characters.Characteristics;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for AbilitiesPage.xaml.
    /// </summary>
    public partial class AbilitiesPage : Page
    {
        public AbilitiesPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.ModifyAbilitiesWindow = new ModifyAbilitiesWindow();

            Program.Logger.Info($"Initialized {nameof(AbilitiesPage)}.");
        }

        public double AbilitiesHeight => SystemParameters.PrimaryScreenHeight - 100;

        private ModifyAbilitiesWindow ModifyAbilitiesWindow { get; }

        public void Draw()
        {
            this.FillList();
        }

        private void FillList()
        {
            this.AbilitiesDataGrid.Items.Clear();

            foreach (var ability in Program.CcsFile.Character.Abilities)
            {
                this.AbilitiesDataGrid.Items.Add(ability);
            }
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            if (this.AbilitiesDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var ability = (Ability)this.AbilitiesDataGrid.SelectedItem;
                var index = Program.CcsFile.Character.Abilities.IndexOf(ability);

                if (index != 0)
                {
                    Utilities.Swap(Program.CcsFile.Character.Abilities, index, index - 1);
                    this.FillList();
                    this.AbilitiesDataGrid.SelectedIndex = index - 1;
                }
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            if (this.AbilitiesDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var ability = (Ability)this.AbilitiesDataGrid.SelectedItem;
                var index = Program.CcsFile.Character.Abilities.IndexOf(ability);

                if (index != Program.CcsFile.Character.Abilities.Count - 1)
                {
                    Utilities.Swap(Program.CcsFile.Character.Abilities, index, index + 1);
                    this.FillList();
                    this.AbilitiesDataGrid.SelectedIndex = index + 1;
                }
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this.AbilitiesDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            this.ModifyAbilitiesWindow.ShowAdd(Program.CcsFile.Character.Abilities);
            this.FillList();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.AbilitiesDataGrid.SelectedItem != null)
            {
                this.ModifyAbilitiesWindow.ShowEdit((Ability)this.AbilitiesDataGrid.SelectedItem);
                this.FillList();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.AbilitiesDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var ability = (Ability)this.AbilitiesDataGrid.SelectedItem;
                Program.CcsFile.Character.Abilities.Remove(ability);
                this.FillList();
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
    }
}
