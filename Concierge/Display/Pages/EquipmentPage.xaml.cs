// <copyright file="EquipmentPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Character.Equipable;
    using Concierge.Character.Magic;
    using Concierge.Commands;
    using Concierge.Common;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows;
    using Concierge.Display.Windows.Utility;
    using Concierge.Persistence;
    using Concierge.Services;
    using Concierge.Tools;

    /// <summary>
    /// Interaction logic for EquipmentPage.xaml.
    /// </summary>
    public partial class EquipmentPage : ConciergePage
    {
        private readonly ImageEncoding encodingService = new (Program.ErrorService);

        public EquipmentPage()
        {
            this.InitializeComponent();

            this.HasEditableDataGrid = true;
            this.ConciergePages = ConciergePages.Equipment;
        }

        private object? SelectedItem { get; set; }

        private int SelectedIndex { get; set; }

        private bool IsSelecting { get; set; }

        private ConciergeDataGrid? SelectedDataGrid { get; set; }

        public override void Draw(bool isNewCharacterSheet = false)
        {
            this.DrawAttunement();
            this.DrawEquippedItems();
            this.DrawImage();
            this.DrawPreparedSpells();
        }

        public override void Edit(object itemToEdit)
        {
            if (itemToEdit is Inventory inventory && this.SelectedDataGrid is not null)
            {
                var index = this.SelectedDataGrid.SelectedIndex;
                WindowService.ShowEdit(
                    inventory,
                    true,
                    typeof(InventoryWindow),
                    this.Window_ApplyChanges,
                    ConciergePages.Equipment);
                this.Draw();
                this.SelectedDataGrid.SetSelectedIndex(index);
            }
            else if (itemToEdit is Weapon weapon && this.SelectedDataGrid is not null)
            {
                var index = this.SelectedDataGrid.SelectedIndex;
                WindowService.ShowEdit(
                    weapon,
                    true,
                    typeof(AttacksWindow),
                    this.Window_ApplyChanges,
                    ConciergePages.Equipment);
                this.Draw();
                this.SelectedDataGrid.SetSelectedIndex(index);
            }
            else if (itemToEdit is Spell spell && this.SelectedDataGrid is not null)
            {
                var index = this.SelectedDataGrid.SelectedIndex;
                WindowService.ShowEdit(
                    spell,
                    typeof(SpellWindow),
                    this.Window_ApplyChanges,
                    ConciergePages.Equipment);
                this.DrawPreparedSpells();
                this.SelectedDataGrid.SetSelectedIndex(index);
            }
        }

        public void DrawAttunement()
        {
            var attunedItems = Program.CcsFile.Character.Equipment.EquippedItems.Attuned;
            var third = Math.Ceiling(Constants.MaxAttunedItems / 3.0);
            var brush = attunedItems <= third ?
                ConciergeBrushes.Mint :
                attunedItems == Constants.MaxAttunedItems ?
                    Brushes.IndianRed :
                    ConciergeBrushes.Deer;

            this.UsedAttunement.Text = $"{attunedItems}/{Constants.MaxAttunedItems}";
            this.UsedAttunement.Foreground = brush;
        }

        public void DrawEquippedItems()
        {
            var equippedItems = Program.CcsFile.Character.Equipment.EquippedItems;

            this.DrawEquippedItem(equippedItems.Head, this.HeadEquipmentDataGrid);
            this.DrawEquippedItem(equippedItems.Torso, this.TorsoEquipmentDataGrid);
            this.DrawEquippedItem(equippedItems.Hands, this.HandsEquipmentDataGrid);
            this.DrawEquippedItem(equippedItems.Legs, this.LegsEquipmentDataGrid);
            this.DrawEquippedItem(equippedItems.Feet, this.FeetEquipmentDataGrid);
        }

        public void DrawImage()
        {
            var portrait = Program.CcsFile.Character.Detail.Portrait;
            this.CharacterImage.Source = portrait.UseCustomImage ? this.encodingService.Decode(portrait.Encoded) : null;
            this.CharacterImage.Stretch = portrait.Stretch;

            this.DefaultCharacterImage.Visibility = this.CharacterImage.Source is null ? Visibility.Visible : Visibility.Hidden;
        }

        public void DrawPreparedSpells()
        {
            this.PreparedSpellsDataGrid.Items.Clear();
            Program.CcsFile.Character.SpellCasting.PreparedSpells.ForEach(spell => this.PreparedSpellsDataGrid.Items.Add(spell));
        }

        private void DrawEquippedItem(List<IEquipable> items, ConciergeDataGrid dataGrid)
        {
            dataGrid.Items.Clear();
            items.ForEach(item => dataGrid.Items.Add(item));
            this.SetEquipmetDataGridControlState(dataGrid);
        }

        private void DequipInventory(Inventory item)
        {
            var oldItem = item.DeepCopy();
            EquippedItems.Dequip(item);
            Program.UndoRedoService.AddCommand(new EditCommand<Inventory>(item, oldItem, this.ConciergePages));
        }

        private void DequipWeapon(Weapon weapon)
        {
            var oldItem = weapon.DeepCopy();
            EquippedItems.Dequip(weapon);
            Program.UndoRedoService.AddCommand(new EditCommand<Weapon>(weapon, oldItem, this.ConciergePages));
        }

        private bool SetEquipmetDataGridControlState(ConciergeDataGrid dataGrid)
        {
            return dataGrid.SetButtonControlsEnableState(this.EditButton, this.ItemUseButton, this.DeleteButton);
        }

        private void CheckAndSet()
        {
            if (this.SetEquipmetDataGridControlState(this.HeadEquipmentDataGrid))
            {
                return;
            }

            if (this.SetEquipmetDataGridControlState(this.TorsoEquipmentDataGrid))
            {
                return;
            }

            if (this.SetEquipmetDataGridControlState(this.HandsEquipmentDataGrid))
            {
                return;
            }

            if (this.SetEquipmetDataGridControlState(this.LegsEquipmentDataGrid))
            {
                return;
            }

            if (this.SetEquipmetDataGridControlState(this.FeetEquipmentDataGrid))
            {
                return;
            }

            if (this.SetEquipmetDataGridControlState(this.PreparedSpellsDataGrid))
            {
                return;
            }
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

            this.CheckAndSet();
            this.IsSelecting = false;
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
            var added = WindowService.ShowAdd(
                string.Empty,
                typeof(EquipmentWindow),
                this.Window_ApplyChanges,
                ConciergePages.Equipment);
            this.Draw();

            if (added)
            {
                this.SelectedDataGrid?.SetSelectedIndex(this.SelectedDataGrid.LastIndex);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedItem is null)
            {
                return;
            }

            this.Edit(this.SelectedItem);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedItem is null || this.SelectedDataGrid is null)
            {
                return;
            }

            if (this.SelectedItem is not Inventory && this.SelectedItem is not Weapon)
            {
                return;
            }

            var index = this.SelectedIndex;
            if (this.SelectedItem is Inventory inventory)
            {
                this.DequipInventory(inventory);
            }
            else if (this.SelectedItem is Weapon weapon)
            {
                this.DequipWeapon(weapon);
            }

            this.Draw();
            this.SelectedDataGrid.SetSelectedIndex(index);

            if (this.SelectedDataGrid.Items.Count == 0)
            {
                this.SelectedItem = null;
                this.SelectedDataGrid = null;
            }
        }

        private void CharacterImage_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SoundService.PlayNavigation();

            WindowService.ShowEdit(
                Program.CcsFile.Character.Detail.Portrait,
                typeof(ImageWindow),
                this.Window_ApplyChanges,
                ConciergePages.Equipment);
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

        private void ItemUseButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedItem is IUsable usable)
            {
                var result = usable.Use(UseItem.Empty);
                WindowService.ShowUseItemWindow(typeof(UseItemWindow), result);
            }
        }
    }
}
