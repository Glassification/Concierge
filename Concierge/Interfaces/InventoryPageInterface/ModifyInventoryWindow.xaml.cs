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
    using Concierge.Commands;
    using Concierge.Interfaces.Enums;
    using Concierge.Primatives;
    using Concierge.Tools.Interface;
    using Concierge.Utility;
    using Concierge.Utility.Enums;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Units;

    /// <summary>
    /// Interaction logic for ModifyInventoryWindow.xaml.
    /// </summary>
    public partial class ModifyInventoryWindow : Window, IConciergeModifyWindow
    {
        private readonly ConciergePage conciergePage;

        public ModifyInventoryWindow(ConciergePage conciergePage)
        {
            this.InitializeComponent();
            this.NameComboBox.ItemsSource = Constants.Inventories;
            this.conciergePage = conciergePage;
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        public bool ItemsAdded { get; private set; }

        private bool EquippedItem { get; set; }

        private bool Editing { get; set; }

        private string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Item";

        private Inventory SelectedItem { get; set; }

        private List<Inventory> Items { get; set; }

        private ConciergeWindowResult Result { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Items = Program.CcsFile.Character.Inventories;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Collapsed;

            this.ClearFields();
            this.ShowDialog();

            return this.Result;
        }

        public void ShowEdit(Inventory inventory, bool equippedItem = false)
        {
            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.SelectedItem = inventory;
            this.EquippedItem = equippedItem;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.OkButton.Visibility = Visibility.Visible;

            this.FillFields(inventory);
            this.ShowDialog();
        }

        public void ShowAdd(List<Inventory> items)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Items = items;
            this.ItemsAdded = false;

            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Visible;

            this.ClearFields();
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
            this.WeightUpDown.Value = inventory.Weight.Value;
            this.NotesTextBox.Text = inventory.Note;
            this.WeightUnits.Text = $"({UnitFormat.WeightPostfix})";

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
            this.ItemsAdded = true;
            var item = new Inventory()
            {
                Name = this.NameComboBox.Text,
                Amount = this.AmountUpDown.Value ?? 0,
                Weight = new ConciergeDouble(this.WeightUpDown.Value ?? 0.0, ConciergeSettings.UnitOfMeasurement, Measurements.Weight),
                IsInBagOfHolding = this.BagOfHoldingCheckBox.IsChecked ?? false,
                Note = this.NotesTextBox.Text,
            };

            Program.UndoRedoService.AddCommand(new AddCommand<Inventory>(this.Items, item, this.conciergePage));

            return item;
        }

        private void UpdateInventory(Inventory inventory)
        {
            var oldItem = inventory.DeepCopy() as Inventory;

            inventory.Name = this.NameComboBox.Text;
            inventory.Amount = this.AmountUpDown.Value ?? 0;
            inventory.Weight.Value = this.WeightUpDown.Value ?? 0.0;
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

            Program.UndoRedoService.AddCommand(new EditCommand<Inventory>(inventory, oldItem, this.conciergePage));
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
