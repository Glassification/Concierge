// <copyright file="InventoryPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.InventoryPageInterface
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Items;
    using Concierge.Interfaces;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for InventoryPage.xaml.
    /// </summary>
    public partial class InventoryPage : Page, IConciergePage
    {
        private readonly ModifyInventoryWindow modifyInventoryWindow = new ();

        public InventoryPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.modifyInventoryWindow.ApplyChanges += this.Window_ApplyChanges;
        }

        public double InventoryHeight => SystemParameters.PrimaryScreenHeight - 100;

        public void Draw()
        {
            this.DrawInventory();
        }

        private void DrawInventory()
        {
            this.InventoryDataGrid.Items.Clear();

            foreach (var inventory in Program.CcsFile.Character.Inventories)
            {
                this.InventoryDataGrid.Items.Add(inventory);
            }
        }

        private void ScrollInventory()
        {
            if (this.InventoryDataGrid.Items.Count > 0)
            {
                this.InventoryDataGrid.SelectedItem = this.InventoryDataGrid.Items[^1];
                this.InventoryDataGrid.UpdateLayout();
                this.InventoryDataGrid.ScrollIntoView(this.InventoryDataGrid.SelectedItem);
            }
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            var index = this.InventoryDataGrid.NextItem(Program.CcsFile.Character.Inventories, 0, -1);

            if (index != -1)
            {
                this.DrawInventory();
                this.InventoryDataGrid.SelectedIndex = index;
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            var index = this.InventoryDataGrid.NextItem(Program.CcsFile.Character.Inventories, Program.CcsFile.Character.Inventories.Count - 1, 1);

            if (index != -1)
            {
                this.DrawInventory();
                this.InventoryDataGrid.SelectedIndex = index;
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this.InventoryDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            this.modifyInventoryWindow.ShowAdd(Program.CcsFile.Character.Inventories);
            this.DrawInventory();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.InventoryDataGrid.SelectedItem != null)
            {
                var inventory = (Inventory)this.InventoryDataGrid.SelectedItem;
                this.modifyInventoryWindow.ShowEdit(inventory);
                this.DrawInventory();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.InventoryDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var inventory = (Inventory)this.InventoryDataGrid.SelectedItem;
                var index = this.InventoryDataGrid.SelectedIndex;

                Program.CcsFile.Character.Inventories.Remove(inventory);
                this.DrawInventory();
                Utilities.SetDataGridSelectedIndex(this.InventoryDataGrid, index);
            }
        }

        private void InventoryDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            Program.CcsFile.Character.Inventories.Clear();

            foreach (var item in this.InventoryDataGrid.Items)
            {
                Program.CcsFile.Character.Inventories.Add(item as Inventory);
            }
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case "ModifyInventoryWindow":
                    this.DrawInventory();
                    this.ScrollInventory();
                    break;
            }
        }
    }
}
