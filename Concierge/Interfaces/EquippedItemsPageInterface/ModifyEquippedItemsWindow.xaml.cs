// <copyright file="ModifyEquippedItemsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.EquippedItemsPageInterface
{
    using System;
    using System.Linq;
    using System.Windows;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for ModifyEquippedItemsWindow.xaml.
    /// </summary>
    public partial class ModifyEquippedItemsWindow : ConciergeWindow
    {
        public ModifyEquippedItemsWindow()
        {
            this.InitializeComponent();
            this.SlotComboBox.ItemsSource = Enum.GetValues(typeof(EquipmentSlot)).Cast<EquipmentSlot>();
            this.ConciergePage = ConciergePage.None;
        }

        public bool ItemsAdded { get; private set; }

        private string PreviousSlot { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.PreviousSlot = string.Empty;
            this.ClearFields();
            this.ItemComboBox.ItemsSource = EquippedItems.Equipable;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.ShowConciergeWindow();

            return this.Result;
        }

        public override bool ShowAdd<T>(T item)
        {
            this.PreviousSlot = string.Empty;
            this.ClearFields();
            this.ItemComboBox.ItemsSource = EquippedItems.Equipable;
            this.ItemsAdded = false;

            this.ShowConciergeWindow();

            return this.ItemsAdded;
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

            var newItem = Program.CcsFile.Character.EquippedItems.Equip(item, slot);
            Program.UndoRedoService.AddCommand(new EquipItemCommand(newItem, slot));
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
