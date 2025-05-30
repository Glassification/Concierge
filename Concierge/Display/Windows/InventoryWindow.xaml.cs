﻿// <copyright file="InventoryWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Enums;
    using Concierge.Character.Equipable;
    using Concierge.Commands;
    using Concierge.Common.Enums;
    using Concierge.Common.Extensions;
    using Concierge.Configuration;
    using Concierge.Data;
    using Concierge.Data.Units;
    using Concierge.Display;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;
    using MaterialDesignThemes.Wpf;

    using Constants = Concierge.Common.Constants;

    /// <summary>
    /// Interaction logic for InventoryWindow.xaml.
    /// </summary>
    public partial class InventoryWindow : ConciergeWindow
    {
        private bool equippedItem;
        private bool editing;
        private Inventory selectedItem = new ();
        private List<Inventory> items = [];

        public InventoryWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.NameComboBox.ItemsSource = DefaultItems;
            this.CategoryComboBox.ItemsSource = ComboBoxGenerator.ItemCategoriesComboBox();
            this.CoinTypeComboBox.ItemsSource = ComboBoxGenerator.CoinTypesComboBox();
            this.ConciergePage = ConciergePages.None;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.NameComboBox);
            this.SetMouseOverEvents(this.ConsumableCheckBox);
            this.SetMouseOverEvents(this.CategoryComboBox);
            this.SetMouseOverEvents(this.AmountUpDown);
            this.SetMouseOverEvents(this.WeightUpDown);
            this.SetMouseOverEvents(this.IgnoreWeightCheckBox);
            this.SetMouseOverEvents(this.AttunedCheckBox);
            this.SetMouseOverEvents(this.ValueUpDown);
            this.SetMouseOverEvents(this.CoinTypeComboBox);
            this.SetMouseOverEvents(this.NotesTextBox, this.NotesTextBackground);
            this.SetMouseOverEvents(this.DescriptionTextBox, this.DescriptionTextBackground);
        }

        public override string HeaderText => $"{(this.editing ? "Edit" : "Add")} Item";

        public override string WindowName => nameof(InventoryWindow);

        public bool ItemsAdded { get; private set; }

        private static List<DetailedComboBoxItemControl> DefaultItems => ComboBoxGenerator.DetailedSelectorComboBox(Defaults.Inventory, Program.CustomItemService.GetItems<Inventory>());

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.items = Program.CcsFile.Character.Equipment.Inventory;
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

            this.editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.selectedItem = castItem;
            this.equippedItem = equippedItem;
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

            this.editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.items = castItem;
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

            this.Result = ConciergeResult.OK;
            if (this.editing)
            {
                this.UpdateInventory(this.selectedItem);
            }
            else
            {
                this.items.Add(this.ToInventory());
            }

            this.CloseConciergeWindow();
        }

        private void FillFields(Inventory inventory)
        {
            Program.Drawing();
            this.AttunedCheckBox.UpdatingValue();
            this.IgnoreWeightCheckBox.UpdatingValue();
            this.ConsumableCheckBox.UpdatingValue();

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
            this.ConsumableCheckBox.IsChecked = inventory.Consumable;

            this.AttunedText.SetEnableState(this.equippedItem);
            this.AttunedCheckBox.SetEnableState(this.equippedItem);
            this.AmountTextBlock.SetEnableState(!this.equippedItem && inventory.Consumable);
            this.AmountUpDown.SetEnableState(!this.equippedItem && inventory.Consumable);

            this.AttunedCheckBox.UpdatedValue();
            this.IgnoreWeightCheckBox.UpdatedValue();
            this.ConsumableCheckBox.UpdatedValue();
            Program.NotDrawing();
        }

        private void ClearFields(string name = "")
        {
            Program.Drawing();
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
            this.ConsumableCheckBox.IsChecked = false;

            this.AttunedText.SetEnableState(false);
            this.AttunedCheckBox.SetEnableState(false);
            this.AmountTextBlock.SetEnableState(false);
            this.AmountUpDown.SetEnableState(false);

            this.AttunedCheckBox.UpdatedValue();
            this.IgnoreWeightCheckBox.UpdatedValue();
            Program.NotDrawing();
        }

        private Inventory ToInventory()
        {
            this.ItemsAdded = true;
            var item = this.Create();

            Program.UndoRedoService.AddCommand(new AddCommand<Inventory>(this.items, item, this.ConciergePage));

            return item;
        }

        private Inventory Create()
        {
            return new Inventory()
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
                CoinType = this.CoinTypeComboBox.Text.ToEnum<CoinType>(),
                Consumable = this.ConsumableCheckBox.IsChecked ?? false,
            };
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
            inventory.CoinType = this.CoinTypeComboBox.Text.ToEnum<CoinType>();
            inventory.IgnoreWeight = this.IgnoreWeightCheckBox.IsChecked ?? false;
            inventory.Consumable = this.ConsumableCheckBox.IsChecked ?? false;

            if (this.equippedItem)
            {
                inventory.Attuned = this.AttunedCheckBox.IsChecked ?? false;

                if (Program.CcsFile.Character.Equipment.EquippedItems.Attuned > Constants.MaxAttunedItems)
                {
                    inventory.Attuned = false;
                    ConciergeMessageBox.ShowError($"You can only attune to a max of {Constants.MaxAttunedItems} items.");
                }
            }

            if (!inventory.IsCustom)
            {
                Program.UndoRedoService.AddCommand(new EditCommand<Inventory>(inventory, oldItem, this.ConciergePage));
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NameComboBox.Text.IsNullOrWhiteSpace())
            {
                return;
            }

            this.items.Add(this.ToInventory());
            this.ClearFields();
            this.InvokeApplyChanges();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var isLocked = this.LockButton.IsChecked ?? false;
            if (this.NameComboBox.SelectedItem is DetailedComboBoxItemControl item && item.Item is Inventory inventory && !isLocked)
            {
                this.FillFields(inventory);
            }
            else if (!isLocked)
            {
                this.ClearFields();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NameComboBox.Text.IsNullOrWhiteSpace())
            {
                ConciergeMessageBox.ShowWarning("Could not save the Inventory item.\nA name is required before saving a custom item.");
                return;
            }

            Program.CustomItemService.AddItem(this.Create());
            this.ClearFields();
            this.NameComboBox.ItemsSource = DefaultItems;
        }

        private void ConsumableCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.AmountUpDown.SetEnableState(true);
            this.AmountTextBlock.SetEnableState(true);
        }

        private void ConsumableCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.AmountUpDown.Value = 1;

            this.AmountUpDown.SetEnableState(false);
            this.AmountTextBlock.SetEnableState(false);
        }

        private void LockButton_Checked(object sender, RoutedEventArgs e)
        {
            this.LockIcon.Kind = PackIconKind.Lock;
        }

        private void LockButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.LockIcon.Kind = PackIconKind.LockOpenVariant;
        }
    }
}
