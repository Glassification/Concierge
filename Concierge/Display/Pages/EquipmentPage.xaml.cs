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

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Character.Spellcasting;
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

        private object? SelectedItem { get; set; }

        private int SelectedIndex { get; set; }

        private bool IsSelecting { get; set; }

        private ConciergeDataGrid? SelectedDataGrid { get; set; }

        private List<Inventory> HeadDisplayList => Program.CcsFile.Character.EquippedItems.Head.Filter(this.SearchFilter.FilterText).ToList();

        private List<Inventory> TorsoDisplayList => Program.CcsFile.Character.EquippedItems.Torso.Filter(this.SearchFilter.FilterText).ToList();

        private List<Inventory> HandsDisplayList => Program.CcsFile.Character.EquippedItems.Hands.Filter(this.SearchFilter.FilterText).ToList();

        private List<Inventory> LegsDisplayList => Program.CcsFile.Character.EquippedItems.Legs.Filter(this.SearchFilter.FilterText).ToList();

        private List<Inventory> FeetDisplayList => Program.CcsFile.Character.EquippedItems.Feet.Filter(this.SearchFilter.FilterText).ToList();

        public void Draw(bool isNewCharacterSheet = false)
        {
            this.UsedAttunement.Text = $"{Program.CcsFile.Character.EquippedItems.Attuned}/{Constants.MaxAttunedItems}";

            this.DrawEquippedItems();
            this.DrawImage();
            this.DrawPreparedSpells();
        }

        public void DrawEquippedItems()
        {
            DrawEquippedItem(this.HeadDisplayList, this.HeadEquipmentDataGrid);
            DrawEquippedItem(this.TorsoDisplayList, this.TorsoEquipmentDataGrid);
            DrawEquippedItem(this.HandsDisplayList, this.HandsEquipmentDataGrid);
            DrawEquippedItem(this.LegsDisplayList, this.LegsEquipmentDataGrid);
            DrawEquippedItem(this.FeetDisplayList, this.FeetEquipmentDataGrid);
        }

        public void DrawImage()
        {
            this.CharacterImage.Source = Program.CcsFile.Character.CharacterImage.ToImage();
            this.CharacterImage.Stretch = Program.CcsFile.Character.CharacterImage.Stretch;

            this.DefaultCharacterImage.Visibility = this.CharacterImage.Source == null ? Visibility.Visible : Visibility.Hidden;
        }

        public void DrawPreparedSpells()
        {
            this.PreparedSpellsDataGrid.Items.Clear();
            foreach (var spell in Program.CcsFile.Character.Spells)
            {
                if (spell.Prepared)
                {
                    this.PreparedSpellsDataGrid.Items.Add(spell);
                }
            }
        }

        public void Edit(object itemToEdit)
        {
            if (itemToEdit is Inventory inventory && this.SelectedDataGrid is not null)
            {
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
            else if (itemToEdit is Spell spell && this.SelectedDataGrid is not null)
            {
                var index = this.SelectedDataGrid.SelectedIndex;
                ConciergeWindowService.ShowEdit<Spell>(
                    spell,
                    typeof(SpellWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.Equipment);
                this.DrawPreparedSpells();
                this.SelectedDataGrid.SetSelectedIndex(index);
            }
        }

        private static void DrawEquippedItem(List<Inventory> items, ConciergeDataGrid dataGrid)
        {
            dataGrid.Items.Clear();

            foreach (var item in items)
            {
                dataGrid.Items.Add(item);
            }
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

            if (dataGrid.Tag is not string stringTag)
            {
                return;
            }

            if (this.IsSelecting)
            {
                return;
            }

            this.IsSelecting = true;
            this.SelectedItem = dataGrid.SelectedItem;
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

            if (!stringTag.Equals("Spells"))
            {
                this.PreparedSpellsDataGrid.UnselectAll();
            }

            this.IsSelecting = false;
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedDataGrid?.Tag is not string stringTag)
            {
                return;
            }

            if (this.SelectedItem is not Inventory)
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

            if (this.SelectedItem is not Inventory)
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
            this.PreparedSpellsDataGrid.UnselectAll();

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

            if (this.SelectedItem is not Inventory item)
            {
                return;
            }

            var index = this.SelectedIndex;
            var equippedId = item.EquppedId;
            var itemIndex = item.Index;
            var slot = (EquipmentSlot)Enum.Parse(typeof(EquipmentSlot), stringTag);

            Program.CcsFile.Character.EquippedItems.Dequip(item, slot);
            Program.UndoRedoService.AddCommand(new DequipItemCommand(item, itemIndex, equippedId, slot));

            this.Draw();
            this.SelectedDataGrid.SetSelectedIndex(index);

            if (this.SelectedDataGrid.Items.Count == 0)
            {
                this.SelectedItem = null;
                this.SelectedDataGrid = null;
            }

            Program.Modify();
        }

        private void EquippedItemsDataGrid_Filtered(object sender, RoutedEventArgs e)
        {
            this.SearchFilter.SetButtonEnableState(this.UpButton);
            this.SearchFilter.SetButtonEnableState(this.DownButton);

            this.Draw();
        }

        private void CharacterImage_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ConciergeWindowService.ShowEdit<CharacterImage>(
                Program.CcsFile.Character.CharacterImage,
                typeof(ImageWindow),
                this.Window_ApplyChanges,
                ConciergePage.Equipment);
            this.DrawImage();
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
    }
}
