// <copyright file="ModifyInventoryWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.InventoryPageInterface
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Items;
    using Concierge.Commands;
    using Concierge.Configuration;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Primitives;
    using Concierge.Tools.Interface;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Units;
    using Concierge.Utility.Units.Enums;

    /// <summary>
    /// Interaction logic for ModifyInventoryWindow.xaml.
    /// </summary>
    public partial class ModifyInventoryWindow : ConciergeWindow
    {
        public ModifyInventoryWindow()
        {
            this.InitializeComponent();
            this.NameComboBox.ItemsSource = Constants.Inventories;
            this.ConciergePage = ConciergePage.None;
        }

        public bool ItemsAdded { get; private set; }

        private bool EquippedItem { get; set; }

        private bool Editing { get; set; }

        private string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Item";

        private Inventory SelectedItem { get; set; }

        private List<Inventory> Items { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Items = Program.CcsFile.Character.Inventories;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T inventory, bool equippedItem)
        {
            var castItem = inventory as Inventory;
            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.SelectedItem = castItem;
            this.EquippedItem = equippedItem;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields(castItem);
            this.ShowConciergeWindow();
        }

        public override bool ShowAdd<T>(T item)
        {
            var castItem = item as List<Inventory>;
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Items = castItem;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.ItemsAdded;
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
                this.AmountTextBlock.Opacity = 0.5;
                this.AmountUpDown.Opacity = 0.5;
                this.AmountUpDown.IsEnabled = false;
            }
            else
            {
                this.BagOfHoldingText.Text = "Bag of Holding:";
                this.BagOfHoldingCheckBox.IsChecked = inventory.IsInBagOfHolding;
                this.AmountTextBlock.Opacity = 1;
                this.AmountUpDown.Opacity = 1;
                this.AmountUpDown.IsEnabled = true;
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
            this.WeightUnits.Text = $"({UnitFormat.WeightPostfix})";

            this.BagOfHoldingCheckBox.UpdatedValue();
        }

        private Inventory ToInventory()
        {
            this.ItemsAdded = true;
            var item = new Inventory()
            {
                Name = this.NameComboBox.Text,
                Amount = this.AmountUpDown.Value ?? 0,
                Weight = new ConciergeDouble(this.WeightUpDown.Value ?? 0.0, AppSettingsManager.Settings.UnitOfMeasurement, Measurements.Weight),
                IsInBagOfHolding = this.BagOfHoldingCheckBox.IsChecked ?? false,
                Note = this.NotesTextBox.Text,
            };

            Program.UndoRedoService.AddCommand(new AddCommand<Inventory>(this.Items, item, this.ConciergePage));

            return item;
        }

        private void UpdateInventory(Inventory inventory)
        {
            var oldItem = inventory.DeepCopy();

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

            Program.UndoRedoService.AddCommand(new EditCommand<Inventory>(inventory, oldItem, this.ConciergePage));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.HideConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NameComboBox.Text.IsNullOrWhiteSpace())
            {
                return;
            }

            this.Items.Add(this.ToInventory());
            this.ClearFields();
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NameComboBox.Text.IsNullOrWhiteSpace())
            {
                this.HideConciergeWindow();
            }

            this.Result = ConciergeWindowResult.OK;
            if (this.Editing)
            {
                this.UpdateInventory(this.SelectedItem);
            }
            else
            {
                this.Items.Add(this.ToInventory());
            }

            this.HideConciergeWindow();

            Program.Modify();
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
            this.HideConciergeWindow();
        }
    }
}
