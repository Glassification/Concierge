// <copyright file="InventoryPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.InventoryPageUi
{
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Characters.Items;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for InventoryPage.xaml.
    /// </summary>
    [System.Runtime.InteropServices.Guid("09356E68-3748-4686-8507-80745407DAF7")]
    public partial class InventoryPage : Page
    {
        public InventoryPage()
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.ModifyInventoryWindow = new ModifyInventoryWindow();
        }

        public double InventoryHeight => SystemParameters.PrimaryScreenHeight - 100;

        private ModifyInventoryWindow ModifyInventoryWindow { get; }

        public void Draw()
        {
            this.FillList();
        }

        private void FillList()
        {
            this.InventoryDataGrid.Items.Clear();

            foreach (var inventory in Program.CcsFile.Character.Inventories)
            {
                this.InventoryDataGrid.Items.Add(inventory);
            }
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            if (this.InventoryDataGrid.SelectedItem != null)
            {
                var inventory = (Inventory)this.InventoryDataGrid.SelectedItem;
                var index = Program.CcsFile.Character.Inventories.IndexOf(inventory);

                if (index != 0)
                {
                    Utilities.Swap(Program.CcsFile.Character.Inventories, index, index - 1);
                    this.FillList();
                    this.InventoryDataGrid.SelectedIndex = index - 1;
                }
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            if (this.InventoryDataGrid.SelectedItem != null)
            {
                var inventory = (Inventory)this.InventoryDataGrid.SelectedItem;
                var index = Program.CcsFile.Character.Inventories.IndexOf(inventory);

                if (index != Program.CcsFile.Character.Inventories.Count - 1)
                {
                    Utilities.Swap(Program.CcsFile.Character.Inventories, index, index + 1);
                    this.FillList();
                    this.InventoryDataGrid.SelectedIndex = index + 1;
                }
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this.InventoryDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            this.ModifyInventoryWindow.ShowAdd();
            this.FillList();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.InventoryDataGrid.SelectedItem != null)
            {
                var inventory = (Inventory)this.InventoryDataGrid.SelectedItem;
                this.ModifyInventoryWindow.ShowEdit(inventory);
                this.FillList();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.InventoryDataGrid.SelectedItem != null)
            {
                var inventory = (Inventory)this.InventoryDataGrid.SelectedItem;
                Program.CcsFile.Character.Inventories.Remove(inventory);
                this.FillList();
            }
        }

        private void InventoryDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.CcsFile.Character.Inventories.Clear();

            foreach (var item in this.InventoryDataGrid.Items)
            {
                Program.CcsFile.Character.Inventories.Add(item as Inventory);
            }
        }
    }
}
