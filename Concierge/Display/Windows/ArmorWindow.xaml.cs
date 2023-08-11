// <copyright file="ArmorWindow.xaml.cs" company="Thomas Beckett">
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
    using Concierge.Character.Equipable;
    using Concierge.Commands;
    using Concierge.Common.Enums;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Data;
    using Concierge.Data.Units;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for ArmorWindow.xaml.
    /// </summary>
    public partial class ArmorWindow : ConciergeWindow
    {
        public ArmorWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ArmorNameComboBox.ItemsSource = DefaultItems;
            this.TypeComboBox.ItemsSource = Enum.GetValues(typeof(ArmorType)).Cast<ArmorType>();
            this.StealthComboBox.ItemsSource = Enum.GetValues(typeof(ArmorStealth)).Cast<ArmorStealth>();
            this.StatusComboBox.ItemsSource = Enum.GetValues(typeof(ArmorStatus)).Cast<ArmorStatus>();
            this.ConciergePage = ConciergePage.None;
            this.SelectedDefense = new Defense();
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetFocusEvents(this.ArmorNameComboBox);
            this.SetFocusEvents(this.TypeComboBox);
            this.SetFocusEvents(this.ArmorClassUpDown);
            this.SetFocusEvents(this.WeightUpDown);
            this.SetFocusEvents(this.StrengthUpDown);
            this.SetFocusEvents(this.StealthComboBox);
            this.SetFocusEvents(this.ShieldTextBox);
            this.SetFocusEvents(this.ShieldArmorClassUpDown);
            this.SetFocusEvents(this.ShieldWeightUpDown);
            this.SetFocusEvents(this.MiscArmorClassUpDown);
            this.SetFocusEvents(this.MagicArmorClassUpDown);
            this.SetFocusEvents(this.StatusComboBox);
        }

        public override string HeaderText => "Edit Defense";

        public override string WindowName => nameof(ArmorWindow);

        private static List<ComboBoxItem> DefaultItems => DisplayUtility.GenerateSelectorComboBox(Defaults.Armor, Program.CustomItemService.GetCustomItems<Armor>());

        private Defense SelectedDefense { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.SelectedDefense = Program.CcsFile.Character.Equipment.Defense;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.FillFields(this.SelectedDefense.Armor);
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T item)
        {
            if (item is Defense defense)
            {
                this.SelectedDefense = defense;
                this.FillFields(this.SelectedDefense.Armor);
                this.ShowConciergeWindow();
            }
            else if (item is Armor armor)
            {
                this.SelectedDefense = new Defense(armor);
                this.FillFields(armor);
                this.ShowConciergeWindow();
            }
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;

            this.UpdateDefense(this.SelectedDefense);
            this.CloseConciergeWindow();

            Program.Modify();
        }

        private void FillFields(Armor armor)
        {
            this.ArmorNameComboBox.Text = armor.Name;
            this.TypeComboBox.Text = armor.Type.ToString();
            this.ArmorClassUpDown.Value = armor.Ac;
            this.WeightUpDown.Value = armor.Weight.Value;
            this.WeightUnits.Text = $"({UnitFormat.WeightPostfix})";
            this.StrengthUpDown.Value = armor.Strength;
            this.StealthComboBox.Text = armor.Stealth.ToString();

            this.ShieldTextBox.Text = this.SelectedDefense.Shield;
            this.ShieldArmorClassUpDown.Value = this.SelectedDefense.ShieldAc;
            this.ShieldWeightUpDown.Value = this.SelectedDefense.ShieldWeight.Value;
            this.ShieldWeightUnits.Text = $"({UnitFormat.WeightPostfix})";
            this.MiscArmorClassUpDown.Value = this.SelectedDefense.MiscAc;
            this.MagicArmorClassUpDown.Value = this.SelectedDefense.MagicAc;
            this.StatusComboBox.Text = this.SelectedDefense.ArmorStatus.ToString();
        }

        private void UpdateDefense(Defense defense)
        {
            var oldItem = defense.DeepCopy();

            defense.Armor.Name = this.ArmorNameComboBox.Text;
            defense.Armor.Type = (ArmorType)Enum.Parse(typeof(ArmorType), this.TypeComboBox.Text);
            defense.Armor.Ac = this.ArmorClassUpDown.Value;
            defense.Armor.Weight.Value = this.WeightUpDown.Value;
            defense.Armor.Strength = this.StrengthUpDown.Value;
            defense.Armor.Stealth = (ArmorStealth)Enum.Parse(typeof(ArmorStealth), this.StealthComboBox.Text);
            defense.Shield = this.ShieldTextBox.Text;
            defense.ShieldAc = this.ShieldArmorClassUpDown.Value;
            defense.ShieldWeight.Value = this.ShieldWeightUpDown.Value;
            defense.MiscAc = this.MiscArmorClassUpDown.Value;
            defense.MagicAc = this.MagicArmorClassUpDown.Value;
            defense.ArmorStatus = (ArmorStatus)Enum.Parse(typeof(ArmorStatus), this.StatusComboBox.Text);

            if (!defense.Armor.IsCustom)
            {
                Program.UndoRedoService.AddCommand(new EditCommand<Defense>(defense, oldItem, this.ConciergePage));
            }
        }

        private void ClearFields(string name = "")
        {
            this.ArmorNameComboBox.Text = name;
            this.TypeComboBox.Text = ArmorType.None.ToString();
            this.ArmorClassUpDown.Value = 0;
            this.WeightUpDown.Value = 0.0;
            this.WeightUnits.Text = $"({UnitFormat.WeightPostfix})";
            this.StrengthUpDown.Value = 0;
            this.StealthComboBox.Text = ArmorStealth.Normal.ToString();
        }

        private Armor Create()
        {
            return new Armor()
            {
                Name = this.ArmorNameComboBox.Text,
                Type = (ArmorType)Enum.Parse(typeof(ArmorType), this.TypeComboBox.Text),
                Ac = this.ArmorClassUpDown.Value,
                Weight = new UnitDouble(this.WeightUpDown.Value, UnitTypes.Imperial, Measurements.Weight),
                Strength = this.StrengthUpDown.Value,
                Stealth = (ArmorStealth)Enum.Parse(typeof(ArmorStealth), this.StealthComboBox.Text),
            };
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.CloseConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateDefense(this.SelectedDefense);
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void ArmorNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ArmorNameComboBox.SelectedItem is ComboBoxItem item && item.Tag is Armor armor)
            {
                this.FillFields(armor);
            }
            else
            {
                this.ClearFields(this.ArmorNameComboBox.Text);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ArmorNameComboBox.Text.IsNullOrWhiteSpace())
            {
                return;
            }

            Program.CustomItemService.AddCustomItem(this.Create());
            this.ClearFields();
            this.ArmorNameComboBox.ItemsSource = DefaultItems;
        }
    }
}
