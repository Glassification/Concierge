// <copyright file="ModifyInventoryWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.InventoryPageUi
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters.Collections;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyInventoryWindow.xaml.
    /// </summary>
    public partial class ModifyInventoryWindow : Window
    {
        public ModifyInventoryWindow()
        {
            this.InitializeComponent();
            this.NameComboBox.ItemsSource = Constants.Inventories;
        }

        private bool Editing { get; set; }

        private Guid SelectedItemId { get; set; }

        public void ShowEdit(Inventory inventory)
        {
            this.HeaderTextBlock.Text = "Edit Item";
            this.SelectedItemId = inventory.ID;
            this.Editing = true;
            this.FillFields(inventory);
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.ShowDialog();
        }

        public void ShowAdd()
        {
            this.HeaderTextBlock.Text = "Add Item";
            this.Editing = false;
            this.ClearFields();
            this.ApplyButton.Visibility = Visibility.Visible;

            this.ShowDialog();
        }

        private void FillFields(Inventory inventory)
        {
            this.NameComboBox.Text = inventory.Name;
            this.AmountUpDown.Value = inventory.Amount;
            this.WeightUpDown.Value = inventory.Weight;
            this.NotesTextBox.Text = inventory.Note;
        }

        private void ClearFields()
        {
            this.NameComboBox.Text = string.Empty;
            this.AmountUpDown.Value = 0;
            this.WeightUpDown.Value = 0.0;
            this.NotesTextBox.Text = string.Empty;
        }

        private Inventory ToInventory()
        {
            var inventory = new Inventory()
            {
                Name = this.NameComboBox.Text,
                Amount = this.AmountUpDown.Value ?? 0,
                Weight = this.WeightUpDown.Value ?? 0.0,
                Note = this.NotesTextBox.Text,
            };

            return inventory;
        }

        private void UpdateInventory(Inventory inventory)
        {
            inventory.Name = this.NameComboBox.Text;
            inventory.Amount = this.AmountUpDown.Value ?? 0;
            inventory.Weight = this.WeightUpDown.Value ?? 0.0;
            inventory.Note = this.NotesTextBox.Text;

            Program.Modified = true;
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Character.Inventories.Add(this.ToInventory());
            Program.Modified = true;
            this.ClearFields();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Editing)
            {
                this.UpdateInventory(Program.Character.GetInventoryById(this.SelectedItemId));
            }
            else
            {
                Program.Character.Inventories.Add(this.ToInventory());
                Program.Modified = true;
            }

            this.Hide();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.NameComboBox.SelectedItem != null)
            {
                this.FillFields(this.NameComboBox.SelectedItem as Inventory);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            this.CloseButton.Foreground = Brushes.Black;
        }

        private void CloseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            this.CloseButton.Foreground = Brushes.White;
        }
    }
}
