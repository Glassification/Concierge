// <copyright file="ModifyInventoryWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.InventoryPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Enums;
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
            this.CategoryComboBox.ItemsSource = Constants.ItemCategories;
            this.CoinTypeComboBox.ItemsSource = Enum.GetValues(typeof(CoinType)).Cast<CoinType>();
            this.ConciergePage = ConciergePage.None;
            this.SelectedItem = new Inventory();
            this.Items = new List<Inventory>();
        }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Item";

        public bool ItemsAdded { get; private set; }

        private bool EquippedItem { get; set; }

        private bool Editing { get; set; }

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
            if (inventory is not Inventory castItem)
            {
                return;
            }

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
            if (item is not List<Inventory> castItem)
            {
                return false;
            }

            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.Items = castItem;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.ItemsAdded;
        }

        protected override void ReturnAndClose()
        {
            if (this.NameComboBox.Text.IsNullOrWhiteSpace())
            {
                this.CloseConciergeWindow();
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

            this.CloseConciergeWindow();

            Program.Modify();
        }

        private void FillFields(Inventory inventory)
        {
            this.BagOfHoldingCheckBox.UpdatingValue();

            this.NameComboBox.Text = inventory.Name;
            this.AmountUpDown.Value = inventory.Amount;
            this.WeightUpDown.Value = inventory.Weight.Value;
            this.NotesTextBox.Text = inventory.Notes;
            this.WeightUnits.Text = $"({UnitFormat.WeightPostfix})";
            this.DescriptionTextBox.Text = inventory.Description;
            this.CategoryComboBox.Text = inventory.ItemCategory;
            this.ValueUpDown.Value = inventory.Value;
            this.CoinTypeComboBox.Text = inventory.CoinType.ToString();

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
                this.BagOfHoldingText.Text = "Ignore Weight:";
                this.BagOfHoldingCheckBox.IsChecked = inventory.IsInBagOfHolding;
                this.AmountTextBlock.Opacity = 1;
                this.AmountUpDown.Opacity = 1;
                this.AmountUpDown.IsEnabled = true;
            }

            this.BagOfHoldingCheckBox.UpdatedValue();
        }

        private void ClearFields()
        {
            this.BagOfHoldingCheckBox.UpdatingValue();

            this.NameComboBox.Text = string.Empty;
            this.AmountUpDown.Value = 1;
            this.WeightUpDown.Value = 0.0;
            this.BagOfHoldingCheckBox.IsChecked = false;
            this.NotesTextBox.Text = string.Empty;
            this.WeightUnits.Text = $"({UnitFormat.WeightPostfix})";
            this.DescriptionTextBox.Text = string.Empty;
            this.CategoryComboBox.Text = Constants.ItemCategories[0];
            this.ValueUpDown.Value = 0;
            this.CoinTypeComboBox.Text = CoinType.None.ToString();

            this.BagOfHoldingCheckBox.UpdatedValue();
        }

        private Inventory ToInventory()
        {
            this.ItemsAdded = true;
            var item = new Inventory()
            {
                Name = this.NameComboBox.Text,
                Amount = this.AmountUpDown.Value,
                Weight = new UnitDouble(this.WeightUpDown.Value, AppSettingsManager.UserSettings.UnitOfMeasurement, Measurements.Weight),
                IsInBagOfHolding = this.BagOfHoldingCheckBox.IsChecked ?? false,
                Notes = this.NotesTextBox.Text,
                Description = this.DescriptionTextBox.Text,
                ItemCategory = this.CategoryComboBox.Text,
                Value = this.ValueUpDown.Value,
                CoinType = (CoinType)Enum.Parse(typeof(CoinType), this.CoinTypeComboBox.Text),
            };

            Program.UndoRedoService.AddCommand(new AddCommand<Inventory>(this.Items, item, this.ConciergePage));

            return item;
        }

        private void UpdateInventory(Inventory inventory)
        {
            var oldItem = inventory.DeepCopy();

            inventory.Name = this.NameComboBox.Text;
            inventory.Amount = this.AmountUpDown.Value;
            inventory.Weight.Value = this.WeightUpDown.Value;
            inventory.Notes = this.NotesTextBox.Text;
            inventory.Description = this.DescriptionTextBox.Text;
            inventory.ItemCategory = this.CategoryComboBox.Text;
            inventory.Value = this.ValueUpDown.Value;
            inventory.CoinType = (CoinType)Enum.Parse(typeof(CoinType), this.CoinTypeComboBox.Text);

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
            this.CloseConciergeWindow();
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
            this.ReturnAndClose();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.NameComboBox.SelectedItem is Inventory inventory)
            {
                this.FillFields(inventory);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }
    }
}
