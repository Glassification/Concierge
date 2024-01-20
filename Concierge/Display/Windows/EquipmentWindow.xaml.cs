// <copyright file="EquipmentWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;

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
        public EquipmentWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.SlotComboBox.ItemsSource = ComboBoxGenerator.EquipmentSlotLevelComboBox();
            this.ConciergePage = ConciergePage.None;
            this.PreviousSlot = string.Empty;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.SlotComboBox);
            this.SetMouseOverEvents(this.ItemComboBox);
        }

        public override string HeaderText => "Edit Equipped Items";

        public override string WindowName => nameof(EquipmentWindow);

        public bool ItemsAdded { get; private set; }

        private string PreviousSlot { get; set; }

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.PreviousSlot = EquipmentSlot.Torso.ToString();
            this.ClearFields();
            this.ItemComboBox.ItemsSource = Program.CcsFile.Character.Equipment.EquippedItems.Equipable;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.ShowConciergeWindow();

            return this.Result;
        }

        public override bool ShowAdd<T>(T item)
        {
            this.PreviousSlot = EquipmentSlot.Torso.ToString();
            this.ClearFields();
            this.ItemComboBox.ItemsSource = Program.CcsFile.Character.Equipment.EquippedItems.Equipable;
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

        private void ClearFields()
        {
            this.SlotComboBox.Text = this.PreviousSlot;
            this.ItemComboBox.Text = string.Empty;
        }

        private bool EquipItem()
        {
            var item = this.ItemComboBox.SelectedItem;
            if (item is not ComboBoxItemControl itemControl)
            {
                return false;
            }

            if ((itemControl.Item is not Inventory && itemControl.Item is not Weapon) || this.SlotComboBox.Text.IsNullOrWhiteSpace())
            {
                return false;
            }

            this.PreviousSlot = this.SlotComboBox.Text;
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
            this.ItemComboBox.ItemsSource = Program.CcsFile.Character.Equipment.EquippedItems.Equipable;
            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }
    }
}
