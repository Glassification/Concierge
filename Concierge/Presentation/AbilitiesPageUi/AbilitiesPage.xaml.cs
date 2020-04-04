using Concierge.Characters.Collections;
using Concierge.Utility;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Concierge.Presentation.AbilitiesPageUi
{
    /// <summary>
    /// Interaction logic for AbilitiesPage.xaml
    /// </summary>
    public partial class AbilitiesPage : Page
    {

        #region Constructor

        public AbilitiesPage()
        {
            InitializeComponent();
            DataContext = this;
            ModifyAbilitiesWindow = new ModifyAbilitiesWindow();
        }

        #endregion

        #region Methods

        public void Draw()
        {
            FillList();
        }

        private void FillList()
        {
            AbilitiesDataGrid.Items.Clear();

            foreach (var ability in Program.Character.Abilities)
            {

                AbilitiesDataGrid.Items.Add(ability);
            }
        }

        #endregion

        #region Accessors

        public double AbilitiesHeight
        {
            get
            {
                return SystemParameters.PrimaryScreenHeight - 100;
            }
        }

        private ModifyAbilitiesWindow ModifyAbilitiesWindow { get; }

        #endregion

        #region Events

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            Ability ability;
            int index;

            if (AbilitiesDataGrid.SelectedItem != null)
            {
                ability = (Ability)AbilitiesDataGrid.SelectedItem;
                index = Program.Character.Abilities.IndexOf(ability);

                if (index != 0)
                {
                    Utilities.Swap(Program.Character.Abilities, index, index - 1);
                    FillList();
                    AbilitiesDataGrid.SelectedIndex = index - 1;
                }
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            Ability ability;
            int index;

            if (AbilitiesDataGrid.SelectedItem != null)
            {
                ability = (Ability)AbilitiesDataGrid.SelectedItem;
                index = Program.Character.Abilities.IndexOf(ability);

                if (index != Program.Character.Abilities.Count - 1)
                {
                    Utilities.Swap(Program.Character.Abilities, index, index + 1);
                    FillList();
                    AbilitiesDataGrid.SelectedIndex = index + 1;
                }
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            AbilitiesDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            ModifyAbilitiesWindow.ShowAdd();
            FillList();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (AbilitiesDataGrid.SelectedItem != null)
            {
                ModifyAbilitiesWindow.ShowEdit((Ability)AbilitiesDataGrid.SelectedItem);
                FillList();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Ability ability;

            if (AbilitiesDataGrid.SelectedItem != null)
            {
                ability = (Ability)AbilitiesDataGrid.SelectedItem;
                Program.Character.Abilities.Remove(ability);
                FillList();
            }
        }

        private void AbilitiesDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.Character.Abilities.Clear();

            foreach (var ability in AbilitiesDataGrid.Items)
            {
                Program.Character.Abilities.Add(ability as Ability);
            }
        }

        #endregion

    }
}
