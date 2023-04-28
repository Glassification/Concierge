// <copyright file="InventoryWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Commands;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Configuration;
    using Concierge.Display;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Persistence;
    using Concierge.Primitives;
    using Concierge.Primitives.Units;
    using Concierge.Primitives.Units.Enums;

    /// <summary>
    /// Interaction logic for InventoryWindow.xaml.
    /// </summary>
    public partial class InventoryWindow : ConciergeWindow
    {
        public InventoryWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.NameComboBox.ItemsSource = Defaults.Inventories;
            this.CategoryComboBox.ItemsSource = Defaults.ItemCategories;
            this.CoinTypeComboBox.ItemsSource = Enum.GetValues(typeof(CoinType)).Cast<CoinType>();
            this.ConciergePage = ConciergePage.None;
            this.SelectedItem = new Inventory();
            this.Items = new List<Inventory>();
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetFocusEvents(this.NameComboBox);
            this.SetFocusEvents(this.CategoryComboBox);
            this.SetFocusEvents(this.AmountUpDown);
            this.SetFocusEvents(this.WeightUpDown);
            this.SetFocusEvents(this.IgnoreWeightCheckBox);
            this.SetFocusEvents(this.AttunedCheckBox);
            this.SetFocusEvents(this.ValueUpDown);
            this.SetFocusEvents(this.CoinTypeComboBox);
            this.SetFocusEvents(this.NotesTextBox);
            this.SetFocusEvents(this.DescriptionTextBox);
        }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Item";

        public override string WindowName => nameof(InventoryWindow);

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
            this.AttunedCheckBox.UpdatingValue();
            this.IgnoreWeightCheckBox.UpdatingValue();

            this.NameComboBox.Text = inventory.Name;
            this.AmountUpDown.Value = inventory.Amount;
            this.WeightUpDown.Value = inventory.Weight.Value;
            this.NotesTextBox.Text = inventory.Notes;
            this.WeightUnits.Text = $"({UnitFormat.WeightPostfix})";
            this.DescriptionTextBox.Text = inventory.Description;
            this.CategoryComboBox.Text = inventory.ItemCategory;
            this.ValueUpDown.Value = inventory.Value;
            this.CoinTypeComboBox.Text = inventory.CoinType.ToString();
            this.IgnoreWeightCheckBox.IsChecked = inventory.IgnoreWeight;
            this.AttunedCheckBox.IsChecked = inventory.Attuned;

            if (this.EquippedItem)
            {
                this.AttunedText.Opacity = 1;
                this.AttunedCheckBox.Opacity = 1;
                this.AttunedCheckBox.IsEnabled = true;

                this.AmountTextBlock.Opacity = 0.5;
                this.AmountUpDown.Opacity = 0.5;
                this.AmountUpDown.IsEnabled = false;
            }
            else
            {
                this.AttunedText.Opacity = 0.5;
                this.AttunedCheckBox.Opacity = 0.5;
                this.AttunedCheckBox.IsEnabled = false;

                this.AmountTextBlock.Opacity = 1;
                this.AmountUpDown.Opacity = 1;
                this.AmountUpDown.IsEnabled = true;
            }

            this.AttunedCheckBox.UpdatedValue();
            this.IgnoreWeightCheckBox.UpdatedValue();
        }

        private void ClearFields(string name = "")
        {
            this.AttunedCheckBox.UpdatingValue();
            this.IgnoreWeightCheckBox.UpdatingValue();

            this.NameComboBox.Text = name;
            this.AmountUpDown.Value = 1;
            this.WeightUpDown.Value = 0.0;
            this.IgnoreWeightCheckBox.IsChecked = false;
            this.AttunedCheckBox.IsChecked = false;
            this.NotesTextBox.Text = string.Empty;
            this.WeightUnits.Text = $"({UnitFormat.WeightPostfix})";
            this.DescriptionTextBox.Text = string.Empty;
            this.CategoryComboBox.Text = Defaults.ItemCategories[0];
            this.ValueUpDown.Value = 0;
            this.CoinTypeComboBox.Text = CoinType.Copper.ToString();

            this.AttunedText.Opacity = 0.5;
            this.AttunedCheckBox.Opacity = 0.5;
            this.AttunedCheckBox.IsEnabled = false;

            this.AttunedCheckBox.UpdatedValue();
            this.IgnoreWeightCheckBox.UpdatedValue();
        }

        private Inventory ToInventory()
        {
            this.ItemsAdded = true;
            var item = new Inventory()
            {
                Name = this.NameComboBox.Text,
                Amount = this.AmountUpDown.Value,
                Weight = new UnitDouble(this.WeightUpDown.Value, AppSettingsManager.UserSettings.UnitOfMeasurement, Measurements.Weight),
                IgnoreWeight = this.IgnoreWeightCheckBox.IsChecked ?? false,
                Attuned = this.AttunedCheckBox.IsChecked ?? false,
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
            inventory.IgnoreWeight = this.IgnoreWeightCheckBox.IsChecked ?? false;

            if (this.EquippedItem)
            {
                inventory.Attuned = this.AttunedCheckBox.IsChecked ?? false;

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
            else
            {
                this.ClearFields(this.NameComboBox.Text);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }
    }
}
