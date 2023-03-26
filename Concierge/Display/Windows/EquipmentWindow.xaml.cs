// <copyright file="EquipmentWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.Linq;
    using System.Windows;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for EquipmentWindow.xaml.
    /// </summary>
    public partial class EquipmentWindow : ConciergeWindow
    {
        public EquipmentWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.SlotComboBox.ItemsSource = Enum.GetValues(typeof(EquipmentSlot)).Cast<EquipmentSlot>();
            this.ConciergePage = ConciergePage.None;
            this.PreviousSlot = string.Empty;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetFocusEvents(this.SlotComboBox);
            this.SetFocusEvents(this.ItemComboBox);
        }

        public override string HeaderText => "Edit Equipped Items";

        public override string WindowName => nameof(EquipmentWindow);

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

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;

            this.EquipItem();
            this.CloseConciergeWindow();

            Program.Modify();
        }

        private void ClearFields()
        {
            this.SlotComboBox.Text = this.PreviousSlot;
            this.ItemComboBox.Text = string.Empty;
        }

        private bool EquipItem()
        {
            if (this.ItemComboBox.SelectedItem is not Inventory item || this.SlotComboBox.Text.IsNullOrWhiteSpace())
            {
                return false;
            }

            this.PreviousSlot = this.SlotComboBox.Text;
            this.ItemsAdded = true;
            var slot = (EquipmentSlot)Enum.Parse(typeof(EquipmentSlot), this.SlotComboBox.Text);

            var newItem = Program.CcsFile.Character.EquippedItems.Equip(item, slot);
            Program.UndoRedoService.AddCommand(new EquipItemCommand(newItem, slot));

            return true;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
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
            this.ItemComboBox.ItemsSource = EquippedItems.Equipable;
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }
    }
}
