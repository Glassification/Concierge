// <copyright file="ModifyEquippedItemsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.EquipedItemsPageUi
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Characters.Enums;
    using Concierge.Characters.Items;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for ModifyEquippedItemsWindow.xaml.
    /// </summary>
    public partial class ModifyEquippedItemsWindow : Window
    {
        public ModifyEquippedItemsWindow()
        {
            this.InitializeComponent();
            this.SlotComboBox.ItemsSource = Enum.GetValues(typeof(EquipmentSlot)).Cast<EquipmentSlot>();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        public void ShowAdd()
        {
            this.ClearFields();
            this.ItemComboBox.ItemsSource = EquippedItems.Equipable;

            this.ShowDialog();
        }

        private void ClearFields()
        {
            this.SlotComboBox.Text = string.Empty;
            this.ItemComboBox.Text = string.Empty;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            if (!(this.ItemComboBox.SelectedItem is Inventory item) || this.SlotComboBox.Text.IsNullOrWhiteSpace())
            {
                return;
            }

            var slot = (EquipmentSlot)Enum.Parse(typeof(EquipmentSlot), this.SlotComboBox.Text);

            Program.CcsFile.Character.EquipedItems.Equip(item, slot);

            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            if (!(this.ItemComboBox.SelectedItem is Inventory item) || this.SlotComboBox.Text.IsNullOrWhiteSpace())
            {
                return;
            }

            var slot = (EquipmentSlot)Enum.Parse(typeof(EquipmentSlot), this.SlotComboBox.Text);

            Program.CcsFile.Character.EquipedItems.Equip(item, slot);

            this.ClearFields();
            this.ItemComboBox.ItemsSource = EquippedItems.Equipable;

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Hide();
                    break;
            }
        }

        private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            ConciergeSound.UpdateValue();
        }
    }
}
