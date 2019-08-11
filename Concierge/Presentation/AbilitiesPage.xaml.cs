using Concierge.Characters.Collections;
using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Concierge.Presentation
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
                    Constants.Swap(Program.Character.Abilities, index, index - 1);
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
                    Constants.Swap(Program.Character.Abilities, index, index + 1);
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

        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {

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

        #endregion

    }
}
