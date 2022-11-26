// <copyright file="InventoryPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.InventoryPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Items;
    using Concierge.Commands;
    using Concierge.Interfaces;
    using Concierge.Interfaces.Enums;
    using Concierge.Services;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for InventoryPage.xaml.
    /// </summary>
    public partial class InventoryPage : Page, IConciergePage
    {
        public InventoryPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.SearchFilter.FilterChanged += this.InventoryDataGrid_Filtered;
        }

        public static double InventoryHeight => SystemParameters.PrimaryScreenHeight - 100;

        public ConciergePage ConciergePage => ConciergePage.Inventory;

        public bool HasEditableDataGrid => true;

        private List<Inventory> DisplayList => Program.CcsFile.Character.Inventories.Filter(this.SearchFilter.FilterText).ToList();

        public void Draw()
        {
            this.DrawInventory();
        }

        public void Edit(object itemToEdit)
        {
            if (itemToEdit is not Inventory inventory)
            {
                return;
            }

            var index = this.InventoryDataGrid.SelectedIndex;
            ConciergeWindowService.ShowEdit<Inventory>(
                inventory,
                false,
                typeof(ModifyInventoryWindow),
                this.Window_ApplyChanges,
                ConciergePage.Inventory);
            this.DrawInventory();
            this.InventoryDataGrid.SetSelectedIndex(index);
        }

        private void DrawInventory()
        {
            this.InventoryDataGrid.Items.Clear();

            foreach (var inventory in this.DisplayList)
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
            var index = this.InventoryDataGrid.NextItem(Program.CcsFile.Character.Inventories, 0, -1, this.ConciergePage);

            if (index != -1)
            {
                this.DrawInventory();
                this.InventoryDataGrid.SetSelectedIndex(index);
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            var index = this.InventoryDataGrid.NextItem(Program.CcsFile.Character.Inventories, Program.CcsFile.Character.Inventories.Count - 1, 1, this.ConciergePage);

            if (index != -1)
            {
                this.DrawInventory();
                this.InventoryDataGrid.SetSelectedIndex(index);
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this.InventoryDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd<List<Inventory>>(
                Program.CcsFile.Character.Inventories,
                typeof(ModifyInventoryWindow),
                this.Window_ApplyChanges,
                ConciergePage.Inventory);
            this.DrawInventory();

            if (added)
            {
                this.InventoryDataGrid.SetSelectedIndex(this.InventoryDataGrid.LastIndex);
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.InventoryDataGrid.SelectedItem != null)
            {
                this.Edit(this.InventoryDataGrid.SelectedItem);
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.InventoryDataGrid.SelectedItem != null)
            {
                var inventory = (Inventory)this.InventoryDataGrid.SelectedItem;
                var index = this.InventoryDataGrid.SelectedIndex;

                Program.UndoRedoService.AddCommand(new DeleteCommand<Inventory>(Program.CcsFile.Character.Inventories, inventory, index, this.ConciergePage));
                Program.CcsFile.Character.Inventories.Remove(inventory);

                this.DrawInventory();
                this.InventoryDataGrid.SetSelectedIndex(index);

                Program.Modify();
            }
        }

        private void InventoryDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SortListFromDataGrid(this.InventoryDataGrid, Program.CcsFile.Character.Inventories, this.ConciergePage);
        }

        private void InventoryDataGrid_Filtered(object sender, RoutedEventArgs e)
        {
            this.SearchFilter.SetButtonEnableState(this.ButtonUp);
            this.SearchFilter.SetButtonEnableState(this.ButtonDown);

            this.DrawInventory();
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(ModifyInventoryWindow):
                    this.DrawInventory();
                    this.ScrollInventory();
                    break;
            }
        }
    }
}
