// <copyright file="ModifyEquippedItemsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.EquippedItemsPageInterface
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for ModifyEquippedItemsWindow.xaml.
    /// </summary>
    public partial class ModifyEquippedItemsWindow : ConciergeWindow, IConciergeModifyWindow
    {
        public ModifyEquippedItemsWindow()
        {
            this.InitializeComponent();
            this.SlotComboBox.ItemsSource = Enum.GetValues(typeof(EquipmentSlot)).Cast<EquipmentSlot>();
        }

        public bool ItemsAdded { get; private set; }

        private string PreviousSlot { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.PreviousSlot = string.Empty;
            this.ClearFields();
            this.ItemComboBox.ItemsSource = EquippedItems.Equipable;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Collapsed;

            this.ShowConciergeWindow();

            return this.Result;
        }

        public void ShowAdd()
        {
            this.PreviousSlot = string.Empty;
            this.ClearFields();
            this.ItemComboBox.ItemsSource = EquippedItems.Equipable;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Visible;
            this.ItemsAdded = false;

            this.ShowConciergeWindow();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        private void ClearFields()
        {
            this.SlotComboBox.Text = this.PreviousSlot;
            this.ItemComboBox.Text = string.Empty;
        }

        private void EquipItem()
        {
            if (this.ItemComboBox.SelectedItem is not Inventory item || this.SlotComboBox.Text.IsNullOrWhiteSpace())
            {
                return;
            }

            this.PreviousSlot = this.SlotComboBox.Text;
            this.ItemsAdded = true;
            var slot = (EquipmentSlot)Enum.Parse(typeof(EquipmentSlot), this.SlotComboBox.Text);
            var originalEquipped = Program.CcsFile.Character.EquippedItems.DeepCopy();
            var originalInventory = Program.CcsFile.Character.Inventories.DeepCopy().ToList();

            Program.CcsFile.Character.EquippedItems.Equip(item, slot);
            Program.UndoRedoService.AddCommand(
                new EquipmentCommand(
                    originalEquipped,
                    originalInventory,
                    Program.CcsFile.Character.EquippedItems.DeepCopy(),
                    Program.CcsFile.Character.Inventories.DeepCopy().ToList()));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.HideConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.OK;

            this.EquipItem();
            this.HideConciergeWindow();

            Program.Modify();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.EquipItem();
            this.ClearFields();
            this.ItemComboBox.ItemsSource = EquippedItems.Equipable;
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.HideConciergeWindow();
        }
    }
}
