// <copyright file="EquipmentWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;

    using Concierge.Character.Enums;
    using Concierge.Character.Equipable;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for EquipmentWindow.xaml.
    /// </summary>
    public partial class EquipmentWindow : ConciergeWindow
    {
        private string previousSlot = string.Empty;

        public EquipmentWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.SlotComboBox.ItemsSource = ComboBoxGenerator.EquipmentSlotLevelComboBox();
            this.ConciergePage = ConciergePages.None;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.SlotComboBox);
            this.SetMouseOverEvents(this.ItemComboBox);
        }

        public override string HeaderText => "Edit Equipped Items";

        public override string WindowName => nameof(EquipmentWindow);

        public bool ItemsAdded { get; set; }

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.previousSlot = EquipmentSlot.Torso.ToString();
            this.ClearFields();
            this.ItemComboBox.ItemsSource = GetEquipableItems();
            this.OkButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.ShowConciergeWindow();

            return this.Result;
        }

        public override bool ShowAdd<T>(T item)
        {
            this.previousSlot = EquipmentSlot.Torso.ToString();
            this.ClearFields();
            this.ItemComboBox.ItemsSource = GetEquipableItems();
            this.ItemsAdded = false;

            this.ShowConciergeWindow();

            return this.ItemsAdded;
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeResult.OK;

            this.EquipItem();
            this.CloseConciergeWindow();
        }

        private static CompositeCollection GetEquipableItems()
        {
            var equipment = Program.CcsFile.Character.Equipment;
            var collection = new CompositeCollection();
            var unequippedItems = equipment.Inventory.Where(x => x.EquipmentSlot == EquipmentSlot.None && !x.Consumable).ToList();
            var unequippedWeapons = equipment.Weapons.Where(x => x.EquipmentSlot == EquipmentSlot.None).ToList();

            if (!unequippedItems.IsEmpty())
            {
                collection.Add(new CollectionContainer() { Collection = unequippedItems.Select(x => new DetailedComboBoxItemControl(x)) });
            }

            if (!unequippedWeapons.IsEmpty())
            {
                collection.Add(new CollectionContainer() { Collection = unequippedWeapons.Select(x => new DetailedComboBoxItemControl(x)) });
            }

            if (collection.Count == 2)
            {
                collection.Insert(1, new ConciergeSeparator());
            }

            return collection;
        }

        private void ClearFields()
        {
            Program.Drawing();

            this.SlotComboBox.Text = this.previousSlot;
            this.ItemComboBox.Text = string.Empty;

            Program.NotDrawing();
        }

        private bool EquipItem()
        {
            var item = this.ItemComboBox.SelectedItem;
            if (item is not DetailedComboBoxItemControl itemControl)
            {
                return false;
            }

            if ((itemControl.Item is not Inventory && itemControl.Item is not Weapon) || this.SlotComboBox.Text.IsNullOrWhiteSpace())
            {
                return false;
            }

            this.previousSlot = this.SlotComboBox.Text;
            this.ItemsAdded = true;
            var slot = this.SlotComboBox.Text.ToEnum<EquipmentSlot>();

            if (itemControl.Item is Inventory inventory)
            {
                this.EquipInventory(inventory, slot);
            }
            else if (itemControl.Item is Weapon weapon)
            {
                this.EquipWeapon(weapon, slot);
            }

            return true;
        }

        private void EquipInventory(Inventory item, EquipmentSlot slot)
        {
            var oldItem = item.DeepCopy();
            EquippedItems.Equip(item, slot);
            Program.UndoRedoService.AddCommand(new EditCommand<Inventory>(item, oldItem, this.ConciergePage));
        }

        private void EquipWeapon(Weapon weapon, EquipmentSlot slot)
        {
            var oldItem = weapon.DeepCopy();
            EquippedItems.Equip(weapon, slot);
            Program.UndoRedoService.AddCommand(new EditCommand<Weapon>(weapon, oldItem, this.ConciergePage));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.EquipItem())
            {
                return;
            }

            this.ClearFields();
            this.ItemComboBox.ItemsSource = GetEquipableItems();
            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }
    }
}
