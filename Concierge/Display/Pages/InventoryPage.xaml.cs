namespace Concierge.Display.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Items;
    using Concierge.Commands;
    using Concierge.Display.Enums;
    using Concierge.Interfaces.InventoryPageInterface;
    using Concierge.Services;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for InventoryPage.xaml.
    /// </summary>
    public partial class InventoryPage : Page, Concierge.Interfaces.IConciergePage
    {
        public InventoryPage()
        {
            this.InitializeComponent();
        }

        public bool HasEditableDataGrid => true;

        public Interfaces.Enums.ConciergePage ConciergePage => Interfaces.Enums.ConciergePage.Inventory;

        public ConciergePage ConciergePage2 => Display.Enums.ConciergePage.Inventory;

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
                Interfaces.Enums.ConciergePage.Inventory);
            this.DrawInventory();
            this.InventoryDataGrid.SetSelectedIndex(index);
        }

        public void DrawInventory()
        {
            var count = Program.CcsFile.Character.Inventories.Count;

            this.ItemTotalField.Text = $"({count} Item{(count == 1 ? string.Empty : "s")})";
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

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            var index = this.InventoryDataGrid.NextItem(Program.CcsFile.Character.Inventories, 0, -1, this.ConciergePage2);

            if (index != -1)
            {
                this.DrawInventory();
                this.InventoryDataGrid.SetSelectedIndex(index);
            }
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            var index = this.InventoryDataGrid.NextItem(Program.CcsFile.Character.Inventories, Program.CcsFile.Character.Inventories.Count - 1, 1, this.ConciergePage2);

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
            var added = ConciergeWindowService.ShowAdd<List<Inventory>>(
                Program.CcsFile.Character.Inventories,
                typeof(ModifyInventoryWindow),
                this.Window_ApplyChanges,
                Interfaces.Enums.ConciergePage.Inventory);
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

                Program.UndoRedoService.AddCommand(new DeleteCommand<Inventory>(Program.CcsFile.Character.Inventories, inventory, index, this.ConciergePage));
                Program.CcsFile.Character.Inventories.Remove(inventory);

                this.DrawInventory();
                this.InventoryDataGrid.SetSelectedIndex(index);

                Program.Modify();
            }
        }

        private void InventoryDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            //DisplayUtility.SortListFromDataGrid(this.InventoryDataGrid, Program.CcsFile.Character.Inventories, this.ConciergePage);
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
                case nameof(ModifyInventoryWindow):
                    this.DrawInventory();
                    this.ScrollInventory();
                    break;
            }
        }
    }
}
