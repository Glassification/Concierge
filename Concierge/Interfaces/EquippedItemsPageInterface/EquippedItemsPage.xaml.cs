// <copyright file="EquippedItemsPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.EquippedItemsPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Interfaces.InventoryPageInterface;
    using Concierge.Services;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for EquippedItemsPage.xaml.
    /// </summary>
    public partial class EquippedItemsPage : Page, IConciergePage
    {
        public EquippedItemsPage()
        {
            this.InitializeComponent();

            this.DataContext = this;
        }

        public ConciergePage ConciergePage => ConciergePage.EquippedItems;

        private Inventory SelectedItem { get; set; }

        private int SelectedIndex { get; set; }

        private ConciergeDataGrid SelectedDataGrid { get; set; }

        public void Draw()
        {
            this.UsedAttunement.Text = $"{Program.CcsFile.Character.EquippedItems.Attuned}/{Constants.MaxAttunedItems}";

            ReadEquippedItems(Program.CcsFile.Character.EquippedItems.Head, this.HeadEquipmentDataGrid);
            ReadEquippedItems(Program.CcsFile.Character.EquippedItems.Torso, this.TorsoEquipmentDataGrid);
            ReadEquippedItems(Program.CcsFile.Character.EquippedItems.Hands, this.HandsEquipmentDataGrid);
            ReadEquippedItems(Program.CcsFile.Character.EquippedItems.Legs, this.LegsEquipmentDataGrid);
            ReadEquippedItems(Program.CcsFile.Character.EquippedItems.Feet, this.FeetEquipmentDataGrid);

            this.LoadImage();
        }

        public void Edit(object itemToEdit)
        {
            if (itemToEdit is not Inventory)
            {
                return;
            }

            var index = this.SelectedDataGrid.SelectedIndex;
            ConciergeWindowService.ShowEdit<Inventory>(
                itemToEdit as Inventory,
                true,
                typeof(ModifyInventoryWindow),
                this.Window_ApplyChanges,
                ConciergePage.EquippedItems);
            this.Draw();
            this.SelectedDataGrid.SetSelectedIndex(index);
        }

        private static void ReadEquippedItems(List<Inventory> items, ConciergeDataGrid dataGrid)
        {
            dataGrid.Items.Clear();

            foreach (var item in items)
            {
                dataGrid.Items.Add(item);
            }
        }

        private void LoadImage()
        {
            this.CharacterImage.Source = Program.CcsFile.Character.CharacterImage.ToImage();
            this.CharacterImage.Stretch = Program.CcsFile.Character.CharacterImage.Stretch;

            this.DefaultCharacterImage.Visibility = this.CharacterImage.Source == null ? Visibility.Visible : Visibility.Hidden;
        }

        private void EquipmentDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            var dataGrid = sender as ConciergeDataGrid;
            var equippedItems = Utilities.GetPropertyValue<List<Inventory>>(Program.CcsFile.Character.EquippedItems, dataGrid.Tag as string);
            var oldList = new List<Inventory>(equippedItems);

            equippedItems.Clear();
            foreach (var item in dataGrid.Items)
            {
                equippedItems.Add(item as Inventory);
            }

            Program.UndoRedoService.AddCommand(
                new ListOrderCommand<Inventory>(
                    equippedItems,
                    oldList,
                    new List<Inventory>(equippedItems),
                    this.ConciergePage));
            Program.Modify();
        }

        private void EquipmentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = this.SelectedDataGrid = sender as ConciergeDataGrid;
            this.SelectedItem = dataGrid.SelectedItem as Inventory;
            this.SelectedIndex = dataGrid.SelectedIndex;

            if (this.SelectedItem == null)
            {
                return;
            }

            if (!(dataGrid.Tag as string).Equals(EquipmentSlot.Head.ToString()))
            {
                this.HeadEquipmentDataGrid.UnselectAll();
            }

            if (!(dataGrid.Tag as string).Equals(EquipmentSlot.Torso.ToString()))
            {
                this.TorsoEquipmentDataGrid.UnselectAll();
            }

            if (!(dataGrid.Tag as string).Equals(EquipmentSlot.Hands.ToString()))
            {
                this.HandsEquipmentDataGrid.UnselectAll();
            }

            if (!(dataGrid.Tag as string).Equals(EquipmentSlot.Legs.ToString()))
            {
                this.LegsEquipmentDataGrid.UnselectAll();
            }

            if (!(dataGrid.Tag as string).Equals(EquipmentSlot.Feet.ToString()))
            {
                this.FeetEquipmentDataGrid.UnselectAll();
            }
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedDataGrid == null)
            {
                return;
            }

            var equippedItems = Utilities.GetPropertyValue<List<Inventory>>(Program.CcsFile.Character.EquippedItems, this.SelectedDataGrid.Tag as string);
            var index = this.SelectedDataGrid.NextItem(equippedItems, 0, -1, this.ConciergePage);

            if (index != -1)
            {
                this.Draw();
                this.SelectedDataGrid.SelectedIndex = index;
                this.SelectedDataGrid.ScrollIntoView(this.SelectedDataGrid.SelectedItem);
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedDataGrid == null)
            {
                return;
            }

            var equippedItems = Utilities.GetPropertyValue<List<Inventory>>(Program.CcsFile.Character.EquippedItems, this.SelectedDataGrid.Tag as string);
            var index = this.SelectedDataGrid.NextItem(equippedItems, equippedItems.Count - 1, 1, this.ConciergePage);

            if (index != -1)
            {
                this.Draw();
                this.SelectedDataGrid.SelectedIndex = index;
                this.SelectedDataGrid.ScrollIntoView(this.SelectedDataGrid.SelectedItem);
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this.HeadEquipmentDataGrid.UnselectAll();
            this.TorsoEquipmentDataGrid.UnselectAll();
            this.HandsEquipmentDataGrid.UnselectAll();
            this.LegsEquipmentDataGrid.UnselectAll();
            this.FeetEquipmentDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd<string>(
                string.Empty,
                typeof(ModifyEquippedItemsWindow),
                this.Window_ApplyChanges,
                ConciergePage.EquippedItems);
            this.Draw();

            if (added)
            {
                this.SelectedDataGrid?.SetSelectedIndex(this.SelectedDataGrid.LastIndex);
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedItem == null)
            {
                return;
            }

            this.Edit(this.SelectedItem);
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedItem == null)
            {
                return;
            }

            var index = this.SelectedIndex;
            var equippedId = this.SelectedItem.EquppedId;
            var itemIndex = this.SelectedItem.Index;
            var slot = (EquipmentSlot)Enum.Parse(typeof(EquipmentSlot), this.SelectedDataGrid.Tag as string);

            Program.CcsFile.Character.EquippedItems.Dequip(this.SelectedItem, slot);
            Program.UndoRedoService.AddCommand(new DequipItemCommand(this.SelectedItem, itemIndex, equippedId, slot));

            this.Draw();
            this.SelectedDataGrid.SetSelectedIndex(index);

            Program.Modify();
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case "ModifyCharacterImageWindow":
                case "ModifyEquippedItemsWindow":
                    this.Draw();
                    break;
            }
        }

        private void ImageEditButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<CharacterImage>(
                Program.CcsFile.Character.CharacterImage,
                typeof(ModifyCharacterImageWindow),
                this.Window_ApplyChanges,
                ConciergePage.EquippedItems);
            this.Draw();
        }
    }
}
