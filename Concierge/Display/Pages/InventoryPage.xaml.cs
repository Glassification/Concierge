// <copyright file="InventoryPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Equipable;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows;
    using Concierge.Display.Windows.Utility;
    using Concierge.Services;
    using Concierge.Tools;

    /// <summary>
    /// Interaction logic for InventoryPage.xaml.
    /// </summary>
    public partial class InventoryPage : ConciergePage
    {
        public InventoryPage()
        {
            this.InitializeComponent();

            this.HasEditableDataGrid = true;
            this.ConciergePages = ConciergePages.Inventory;
        }

        private List<Inventory> DisplayList => Program.CcsFile.Character.Equipment.Inventory.Filter(this.SearchFilter.FilterText).ToList();

        public override void Draw(bool isNewCharacterSheet = false)
        {
            this.DrawInventory();
        }

        public override void Edit(object itemToEdit)
        {
            if (itemToEdit is not Inventory inventory)
            {
                return;
            }

            var index = this.InventoryDataGrid.SelectedIndex;
            ConciergeWindowService.ShowEdit(
                inventory,
                false,
                typeof(InventoryWindow),
                this.Window_ApplyChanges,
                ConciergePages.Inventory);

            this.DrawInventory();
            this.InventoryDataGrid.SetSelectedIndex(index);
        }

        public void DrawInventory()
        {
            var count = Program.CcsFile.Character.Equipment.Inventory.Count;

            this.ItemTotalField.Text = $"({count} {"Item".Pluralize("s", count)})";
            this.InventoryDataGrid.Items.Clear();
            this.DisplayList.ForEach(item => this.InventoryDataGrid.Items.Add(item));
            this.SetInventoryDataGridControlState();
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

        private void SetInventoryDataGridControlState()
        {
            this.InventoryDataGrid.SetButtonControlsEnableState(
                this.UpButton,
                this.DownButton,
                this.EditButton,
                this.ItemUseButton,
                this.DeleteButton);
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            var index = this.InventoryDataGrid.NextItem(Program.CcsFile.Character.Equipment.Inventory, 0, -1, this.ConciergePages);

            if (index != -1)
            {
                this.DrawInventory();
                this.InventoryDataGrid.SetSelectedIndex(index);
            }
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            var index = this.InventoryDataGrid.NextItem(Program.CcsFile.Character.Equipment.Inventory, Program.CcsFile.Character.Equipment.Inventory.Count - 1, 1, this.ConciergePages);

            if (index != -1)
            {
                this.DrawInventory();
                this.InventoryDataGrid.SetSelectedIndex(index);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.InventoryDataGrid.UnselectAll();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd(
                Program.CcsFile.Character.Equipment.Inventory,
                typeof(InventoryWindow),
                this.Window_ApplyChanges,
                ConciergePages.Inventory);
            this.DrawInventory();

            if (added)
            {
                this.InventoryDataGrid.SetSelectedIndex(this.InventoryDataGrid.LastIndex);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.InventoryDataGrid.SelectedItem != null)
            {
                this.Edit(this.InventoryDataGrid.SelectedItem);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.InventoryDataGrid.SelectedItem != null)
            {
                var inventory = (Inventory)this.InventoryDataGrid.SelectedItem;
                var index = this.InventoryDataGrid.SelectedIndex;

                Program.UndoRedoService.AddCommand(new DeleteCommand<Inventory>(Program.CcsFile.Character.Equipment.Inventory, inventory, index, this.ConciergePages));
                Program.CcsFile.Character.Equipment.Inventory.Remove(inventory);

                this.DrawInventory();
                this.InventoryDataGrid.SetSelectedIndex(index);
            }
        }

        private void InventoryDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            this.InventoryDataGrid.SortListFromDataGrid(Program.CcsFile.Character.Equipment.Inventory, this.ConciergePages);
        }

        private void InventoryDataGrid_Filtered(object sender, RoutedEventArgs e)
        {
            this.SearchFilter.SetButtonEnableState(this.UpButton);
            this.SearchFilter.SetButtonEnableState(this.DownButton);

            this.DrawInventory();
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(InventoryWindow):
                    this.DrawInventory();
                    this.ScrollInventory();
                    break;
            }
        }

        private void ItemUseButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.InventoryDataGrid.SelectedItem is null)
            {
                return;
            }

            var item = (Inventory)this.InventoryDataGrid.SelectedItem;
            if (item.Amount == 0)
            {
                return;
            }

            var oldItem = item.DeepCopy();
            var result = item.Use(UseItem.Empty);

            var windowResult = ConciergeWindowService.ShowUseItemWindow(typeof(UseItemWindow), result);
            if (windowResult != ConciergeResult.OK)
            {
                item.Amount = oldItem.Amount;
                return;
            }

            var index = this.InventoryDataGrid.SelectedIndex;
            this.DrawInventory();
            this.InventoryDataGrid.SetSelectedIndex(index);

            Program.UndoRedoService.AddCommand(new EditCommand<Inventory>(item, oldItem, this.ConciergePages));
        }

        private void InventoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetInventoryDataGridControlState();
        }
    }
}
