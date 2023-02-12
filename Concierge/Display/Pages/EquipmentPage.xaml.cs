// <copyright file="EquipmentPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows;
    using Concierge.Services;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for EquipmentPage.xaml.
    /// </summary>
    public partial class EquipmentPage : Page, IConciergePage
    {
        public EquipmentPage()
        {
            this.InitializeComponent();
        }

        public ConciergePage ConciergePage => ConciergePage.Equipment;

        public bool HasEditableDataGrid => true;

        private Inventory? SelectedItem { get; set; }

        private int SelectedIndex { get; set; }

        private ConciergeDataGrid? SelectedDataGrid { get; set; }

        private List<Inventory> HeadDisplayList => Program.CcsFile.Character.EquippedItems.Head.Filter(this.SearchFilter.FilterText).ToList();

        private List<Inventory> TorsoDisplayList => Program.CcsFile.Character.EquippedItems.Torso.Filter(this.SearchFilter.FilterText).ToList();

        private List<Inventory> HandsDisplayList => Program.CcsFile.Character.EquippedItems.Hands.Filter(this.SearchFilter.FilterText).ToList();

        private List<Inventory> LegsDisplayList => Program.CcsFile.Character.EquippedItems.Legs.Filter(this.SearchFilter.FilterText).ToList();

        private List<Inventory> FeetDisplayList => Program.CcsFile.Character.EquippedItems.Feet.Filter(this.SearchFilter.FilterText).ToList();

        public void Draw()
        {
            this.UsedAttunement.Text = $"{Program.CcsFile.Character.EquippedItems.Attuned}/{Constants.MaxAttunedItems}";

            ReadEquippedItems(this.HeadDisplayList, this.HeadEquipmentDataGrid);
            ReadEquippedItems(this.TorsoDisplayList, this.TorsoEquipmentDataGrid);
            ReadEquippedItems(this.HandsDisplayList, this.HandsEquipmentDataGrid);
            ReadEquippedItems(this.LegsDisplayList, this.LegsEquipmentDataGrid);
            ReadEquippedItems(this.FeetDisplayList, this.FeetEquipmentDataGrid);

            this.LoadImage();
        }

        public void Edit(object itemToEdit)
        {
            if (itemToEdit is not Inventory inventory || this.SelectedDataGrid is null)
            {
                return;
            }

            var index = this.SelectedDataGrid.SelectedIndex;
            ConciergeWindowService.ShowEdit<Inventory>(
                inventory,
                true,
                typeof(InventoryWindow),
                this.Window_ApplyChanges,
                ConciergePage.Equipment);
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
            if (sender is not ConciergeDataGrid dataGrid)
            {
                return;
            }

            if (dataGrid.Tag is not string tagString)
            {
                return;
            }

            var equippedItems = DisplayUtility.GetPropertyValue<List<Inventory>>(Program.CcsFile.Character.EquippedItems, tagString) ?? new List<Inventory>();
            var oldList = new List<Inventory>(equippedItems);

            equippedItems.Clear();
            foreach (var item in dataGrid.Items)
            {
                if (item is Inventory inventory)
                {
                    equippedItems.Add(inventory);
                }
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
            if (sender is not ConciergeDataGrid dataGrid)
            {
                return;
            }

            if (dataGrid.SelectedItem is not Inventory inventory)
            {
                return;
            }

            if (dataGrid.Tag is not string stringTag)
            {
                return;
            }

            this.SelectedItem = inventory;
            this.SelectedDataGrid = dataGrid;
            this.SelectedIndex = dataGrid.SelectedIndex;

            if (!stringTag.Equals(EquipmentSlot.Head.ToString()))
            {
                this.HeadEquipmentDataGrid.UnselectAll();
            }

            if (!stringTag.Equals(EquipmentSlot.Torso.ToString()))
            {
                this.TorsoEquipmentDataGrid.UnselectAll();
            }

            if (!stringTag.Equals(EquipmentSlot.Hands.ToString()))
            {
                this.HandsEquipmentDataGrid.UnselectAll();
            }

            if (!stringTag.Equals(EquipmentSlot.Legs.ToString()))
            {
                this.LegsEquipmentDataGrid.UnselectAll();
            }

            if (!stringTag.Equals(EquipmentSlot.Feet.ToString()))
            {
                this.FeetEquipmentDataGrid.UnselectAll();
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedDataGrid?.Tag is not string stringTag)
            {
                return;
            }

            var equippedItems = DisplayUtility.GetPropertyValue<List<Inventory>>(Program.CcsFile.Character.EquippedItems, stringTag) ?? new List<Inventory>();
            var index = this.SelectedDataGrid.NextItem(equippedItems, 0, -1, this.ConciergePage);
            if (index != -1)
            {
                this.Draw();
                this.SelectedDataGrid.SelectedIndex = index;
                this.SelectedDataGrid.ScrollIntoView(this.SelectedDataGrid.SelectedItem);
            }
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedDataGrid?.Tag is not string stringTag)
            {
                return;
            }

            var equippedItems = DisplayUtility.GetPropertyValue<List<Inventory>>(Program.CcsFile.Character.EquippedItems, stringTag) ?? new List<Inventory>();
            var index = this.SelectedDataGrid.NextItem(equippedItems, equippedItems.Count - 1, 1, this.ConciergePage);
            if (index != -1)
            {
                this.Draw();
                this.SelectedDataGrid.SelectedIndex = index;
                this.SelectedDataGrid.ScrollIntoView(this.SelectedDataGrid.SelectedItem);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.HeadEquipmentDataGrid.UnselectAll();
            this.TorsoEquipmentDataGrid.UnselectAll();
            this.HandsEquipmentDataGrid.UnselectAll();
            this.LegsEquipmentDataGrid.UnselectAll();
            this.FeetEquipmentDataGrid.UnselectAll();

            this.SelectedItem = null;
            this.SelectedDataGrid = null;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd<string>(
                string.Empty,
                typeof(EquipmentWindow),
                this.Window_ApplyChanges,
                ConciergePage.Equipment);
            this.Draw();

            if (added)
            {
                this.SelectedDataGrid?.SetSelectedIndex(this.SelectedDataGrid.LastIndex);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedItem == null)
            {
                return;
            }

            this.Edit(this.SelectedItem);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedItem is null || this.SelectedDataGrid is null || this.SelectedDataGrid.Tag is not string stringTag)
            {
                return;
            }

            var index = this.SelectedIndex;
            var equippedId = this.SelectedItem.EquppedId;
            var itemIndex = this.SelectedItem.Index;
            var slot = (EquipmentSlot)Enum.Parse(typeof(EquipmentSlot), stringTag);

            Program.CcsFile.Character.EquippedItems.Dequip(this.SelectedItem, slot);
            Program.UndoRedoService.AddCommand(new DequipItemCommand(this.SelectedItem, itemIndex, equippedId, slot));

            this.Draw();
            this.SelectedDataGrid.SetSelectedIndex(index);

            if (this.SelectedDataGrid.Items.Count == 0)
            {
                this.SelectedItem = null;
                this.SelectedDataGrid = null;
            }

            Program.Modify();
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(ImageWindow):
                case nameof(EquipmentWindow):
                    this.Draw();
                    break;
            }
        }

        private void EquippedItemsDataGrid_Filtered(object sender, RoutedEventArgs e)
        {
            this.SearchFilter.SetButtonEnableState(this.UpButton);
            this.SearchFilter.SetButtonEnableState(this.DownButton);

            this.Draw();
        }
    }
}
