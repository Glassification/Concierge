using Concierge.Characters.Collections;
using Concierge.Utility;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Concierge.Presentation.InventoryPageUi
{
    /// <summary>
    /// Interaction logic for InventoryPage.xaml
    /// </summary>
    public partial class InventoryPage : Page
    {
        #region Constructor

        public InventoryPage()
        {
            InitializeComponent();
            DataContext = this;

            ModifyInventoryWindow = new ModifyInventoryWindow();
        }

        #endregion

        #region Methods

        public void Draw()
        {
            FillList();
        }

        private void FillList()
        {
            InventoryDataGrid.Items.Clear();

            foreach (var inventory in Program.Character.Inventories)
            {

                InventoryDataGrid.Items.Add(inventory);
            }
        }

        #endregion

        #region Accessors

        public double InventoryHeight
        {
            get
            {
                return SystemParameters.PrimaryScreenHeight - 100;
            }
        }

        private ModifyInventoryWindow ModifyInventoryWindow { get; }

        #endregion

        #region Events

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            Inventory inventory;
            int index;

            if (InventoryDataGrid.SelectedItem != null)
            {
                inventory = (Inventory)InventoryDataGrid.SelectedItem;
                index = Program.Character.Inventories.IndexOf(inventory);

                if (index != 0)
                {
                    Constants.Swap(Program.Character.Inventories, index, index - 1);
                    FillList();
                    InventoryDataGrid.SelectedIndex = index - 1;
                }
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            Inventory inventory;
            int index;

            if (InventoryDataGrid.SelectedItem != null)
            {
                inventory = (Inventory)InventoryDataGrid.SelectedItem;
                index = Program.Character.Inventories.IndexOf(inventory);

                if (index != Program.Character.Inventories.Count - 1)
                {
                    Constants.Swap(Program.Character.Inventories, index, index + 1);
                    FillList();
                    InventoryDataGrid.SelectedIndex = index + 1;
                }
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            InventoryDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            ModifyInventoryWindow.ShowAdd();
            FillList();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            Inventory inventory;

            if (InventoryDataGrid.SelectedItem != null)
            {
                inventory = (Inventory)InventoryDataGrid.SelectedItem;
                ModifyInventoryWindow.ShowEdit(inventory);
                FillList();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Inventory inventory;

            if (InventoryDataGrid.SelectedItem != null)
            {
                inventory = (Inventory)InventoryDataGrid.SelectedItem;
                Program.Character.Inventories.Remove(inventory);
                FillList();
            }
        }

        #endregion

    }
}
