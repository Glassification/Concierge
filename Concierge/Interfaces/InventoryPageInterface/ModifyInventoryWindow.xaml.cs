// <copyright file="ModifyInventoryWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.InventoryPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.Items;
    using Concierge.Interfaces.Enums;
    using Concierge.Tools;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for ModifyInventoryWindow.xaml.
    /// </summary>
    public partial class ModifyInventoryWindow : Window, IConciergeWindow
    {
        public ModifyInventoryWindow()
        {
            this.InitializeComponent();
            this.NameComboBox.ItemsSource = Constants.Inventories;
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private bool EquippedItem { get; set; }

        private bool Editing { get; set; }

        private Inventory SelectedItem { get; set; }

        private List<Inventory> Items { get; set; }

        private ConciergeWindowResult Result { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = "Add Item";
            this.Items = Program.CcsFile.Character.Inventories;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Collapsed;

            this.ClearFields();
            this.ShowDialog();

            return this.Result;
        }

        public void ShowEdit(Inventory inventory, bool equippedItem = false)
        {
            this.HeaderTextBlock.Text = "Edit Item";
            this.SelectedItem = inventory;
            this.Editing = true;
            this.EquippedItem = equippedItem;
            this.FillFields(inventory);
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.OkButton.Visibility = Visibility.Visible;

            this.ShowDialog();
        }

        public void ShowAdd(List<Inventory> items)
        {
            this.HeaderTextBlock.Text = "Add Item";
            this.Items = items;
            this.Editing = false;
            this.ClearFields();
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Visible;

            this.ShowDialog();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void FillFields(Inventory inventory)
        {
            this.AmountUpDown.UpdatingValue();
            this.WeightUpDown.UpdatingValue();
            this.BagOfHoldingCheckBox.UpdatingValue();

            this.NameComboBox.Text = inventory.Name;
            this.AmountUpDown.Value = inventory.Amount;
            this.WeightUpDown.Value = inventory.Weight;
            this.NotesTextBox.Text = inventory.Note;

            if (this.EquippedItem)
            {
                this.BagOfHoldingText.Text = "Attuned:";
                this.BagOfHoldingCheckBox.IsChecked = inventory.Attuned;
            }
            else
            {
                this.BagOfHoldingText.Text = "Bag of Holding:";
                this.BagOfHoldingCheckBox.IsChecked = inventory.IsInBagOfHolding;
            }

            this.BagOfHoldingCheckBox.UpdatedValue();
        }

        private void ClearFields()
        {
            this.AmountUpDown.UpdatingValue();
            this.WeightUpDown.UpdatingValue();
            this.BagOfHoldingCheckBox.UpdatingValue();

            this.NameComboBox.Text = string.Empty;
            this.AmountUpDown.Value = 0;
            this.WeightUpDown.Value = 0.0;
            this.BagOfHoldingCheckBox.IsChecked = false;
            this.NotesTextBox.Text = string.Empty;

            this.BagOfHoldingCheckBox.UpdatedValue();
        }

        private Inventory ToInventory()
        {
            return new Inventory()
            {
                Name = this.NameComboBox.Text,
                Amount = this.AmountUpDown.Value ?? 0,
                Weight = this.WeightUpDown.Value ?? 0.0,
                IsInBagOfHolding = this.BagOfHoldingCheckBox.IsChecked ?? false,
                Note = this.NotesTextBox.Text,
            };
        }

        private void UpdateInventory(Inventory inventory)
        {
            inventory.Name = this.NameComboBox.Text;
            inventory.Amount = this.AmountUpDown.Value ?? 0;
            inventory.Weight = this.WeightUpDown.Value ?? 0.0;
            inventory.Note = this.NotesTextBox.Text;

            if (this.EquippedItem)
            {
                inventory.Attuned = this.BagOfHoldingCheckBox.IsChecked ?? false;

                if (Program.CcsFile.Character.EquippedItems.Attuned > Constants.MaxAttunedItems)
                {
                    inventory.Attuned = false;
                    ConciergeMessageBox.Show(
                        $"You can only attune to a max of {Constants.MaxAttunedItems} items.",
                        "Error",
                        ConciergeWindowButtons.Ok,
                        ConciergeWindowIcons.Error);
                }
            }
            else
            {
                inventory.IsInBagOfHolding = this.BagOfHoldingCheckBox.IsChecked ?? false;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Result = ConciergeWindowResult.Exit;
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NameComboBox.Text.IsNullOrWhiteSpace())
            {
                return;
            }

            Program.Modify();

            this.Items.Add(this.ToInventory());
            this.ClearFields();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NameComboBox.Text.IsNullOrWhiteSpace())
            {
                this.Hide();
            }

            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateInventory(this.SelectedItem);
            }
            else
            {
                this.Items.Add(this.ToInventory());
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
            this.Result = ConciergeWindowResult.Cancel;
            this.Hide();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
