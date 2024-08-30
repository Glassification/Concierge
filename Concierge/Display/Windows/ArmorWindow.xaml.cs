// <copyright file="ArmorWindow.xaml.cs" company="Thomas Beckett">
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
    using Concierge.Data;
    using Concierge.Data.Units;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for ArmorWindow.xaml.
    /// </summary>
    public partial class ArmorWindow : ConciergeWindow
    {
        private Defense selectedDefense = new ();

        public ArmorWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ArmorNameComboBox.ItemsSource = DefaultItems;
            this.TypeComboBox.ItemsSource = ComboBoxGenerator.ArmorTypeComboBox();
            this.StealthComboBox.ItemsSource = ComboBoxGenerator.StealthComboBox();
            this.StatusComboBox.ItemsSource = ComboBoxGenerator.ArmorStatusComboBox();
            this.ConciergePage = ConciergePages.None;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.ArmorNameComboBox);
            this.SetMouseOverEvents(this.TypeComboBox);
            this.SetMouseOverEvents(this.ArmorClassUpDown);
            this.SetMouseOverEvents(this.WeightUpDown);
            this.SetMouseOverEvents(this.StrengthUpDown);
            this.SetMouseOverEvents(this.StealthComboBox);
            this.SetMouseOverEvents(this.ShieldTextBox, this.ShieldTextBackground);
            this.SetMouseOverEvents(this.ShieldArmorClassUpDown);
            this.SetMouseOverEvents(this.MiscArmorClassUpDown);
            this.SetMouseOverEvents(this.MagicArmorClassUpDown);
            this.SetMouseOverEvents(this.StatusComboBox);
            this.SetMouseOverEvents(this.FullAcCheckBox);
        }

        public override string HeaderText => "Edit Defense";

        public override string WindowName => nameof(ArmorWindow);

        private static List<DetailedComboBoxItemControl> DefaultItems => ComboBoxGenerator.DetailedSelectorComboBox(Defaults.Armor, Program.CustomItemService.GetItems<Armor>());

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.selectedDefense = Program.CcsFile.Character.Equipment.Defense;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CancelButton.Content = buttonText;

            this.FillFields(this.selectedDefense.Armor);
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override void ShowEdit<T>(T item)
        {
            if (item is Defense defense)
            {
                this.selectedDefense = defense;
                this.FillFields(this.selectedDefense.Armor);
                this.ShowConciergeWindow();
            }
            else if (item is Armor armor)
            {
                this.selectedDefense = new Defense(armor);
                this.FillFields(armor);
                this.ShowConciergeWindow();
            }
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeResult.OK;

            this.UpdateDefense(this.selectedDefense);
            this.CloseConciergeWindow();
        }

        private void FillFields(Armor armor)
        {
            Program.Drawing();
            this.FullAcCheckBox.UpdatingValue();

            this.ArmorNameComboBox.Text = armor.Name;
            this.TypeComboBox.Text = armor.Type.ToString();
            this.ArmorClassUpDown.Value = armor.Ac;
            this.WeightUpDown.Value = armor.Weight.Value;
            this.WeightUnits.Text = $"({UnitFormat.WeightPostfix})";
            this.StrengthUpDown.Value = armor.Strength;
            this.StealthComboBox.Text = armor.Stealth.ToString();
            this.FullAcCheckBox.IsChecked = armor.FullDex;
            this.ShieldTextBox.Text = this.selectedDefense.Shield;
            this.ShieldArmorClassUpDown.Value = this.selectedDefense.ShieldAc;
            this.MiscArmorClassUpDown.Value = this.selectedDefense.MiscAc;
            this.MagicArmorClassUpDown.Value = this.selectedDefense.MagicAc;
            this.StatusComboBox.Text = this.selectedDefense.ArmorStatus.ToString();

            this.FullAcCheckBox.UpdatedValue();
            Program.NotDrawing();
        }

        private void UpdateDefense(Defense defense)
        {
            var oldItem = defense.DeepCopy();

            defense.Armor.Name = this.ArmorNameComboBox.Text;
            defense.Armor.Type = this.TypeComboBox.Text.ToEnum<ArmorType>();
            defense.Armor.Ac = this.ArmorClassUpDown.Value;
            defense.Armor.Weight.Value = this.WeightUpDown.Value;
            defense.Armor.Strength = this.StrengthUpDown.Value;
            defense.Armor.Stealth = this.StealthComboBox.Text.ToEnum<ArmorStealth>();
            defense.Armor.FullDex = this.FullAcCheckBox.IsChecked ?? false;
            defense.Shield = this.ShieldTextBox.Text;
            defense.ShieldAc = this.ShieldArmorClassUpDown.Value;
            defense.MiscAc = this.MiscArmorClassUpDown.Value;
            defense.MagicAc = this.MagicArmorClassUpDown.Value;
            defense.ArmorStatus = this.StatusComboBox.Text.ToEnum<ArmorStatus>();

            if (!defense.Armor.IsCustom)
            {
                Program.UndoRedoService.AddCommand(new EditCommand<Defense>(defense, oldItem, this.ConciergePage));
            }
        }

        private void ClearFields(string name = "")
        {
            Program.Drawing();

            this.ArmorNameComboBox.Text = name;
            this.TypeComboBox.Text = ArmorType.None.ToString();
            this.ArmorClassUpDown.Value = 0;
            this.FullAcCheckBox.IsChecked = false;
            this.WeightUpDown.Value = 0.0;
            this.WeightUnits.Text = $"({UnitFormat.WeightPostfix})";
            this.StrengthUpDown.Value = 0;
            this.StealthComboBox.Text = ArmorStealth.Normal.ToString();

            Program.NotDrawing();
        }

        private Armor Create()
        {
            return new Armor()
            {
                Name = this.ArmorNameComboBox.Text,
                Type = this.TypeComboBox.Text.ToEnum<ArmorType>(),
                Ac = this.ArmorClassUpDown.Value,
                FullDex = this.FullAcCheckBox.IsChecked ?? false,
                Weight = new UnitDouble(this.WeightUpDown.Value, UnitTypes.Imperial, Measurements.Weight),
                Strength = this.StrengthUpDown.Value,
                Stealth = this.StealthComboBox.Text.ToEnum<ArmorStealth>(),
            };
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateDefense(this.selectedDefense);
            this.InvokeApplyChanges();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void ArmorNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var isLocked = this.LockButton.IsChecked ?? false;
            if (this.ArmorNameComboBox.SelectedItem is DetailedComboBoxItemControl item && item.Item is Armor armor && !isLocked)
            {
                this.FillFields(armor);
            }
            else if (!isLocked)
            {
                this.ClearFields(this.ArmorNameComboBox.Text);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ArmorNameComboBox.Text.IsNullOrWhiteSpace())
            {
                ConciergeMessageBox.ShowWarning("Could not save the Armor.\nA name is required before saving a custom item.");
                return;
            }

            Program.CustomItemService.AddItem(this.Create());
            this.ClearFields();
            this.ArmorNameComboBox.ItemsSource = DefaultItems;
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.TypeComboBox.SelectedItem is not ComboBoxItemControl item)
            {
                return;
            }

            var type = item.Text.ToEnum<ArmorType>();
            var enabled = type != ArmorType.None && type != ArmorType.Light;
            if (!enabled)
            {
                this.FullAcCheckBox.IsChecked = false;
            }

            this.FullAcCheckBox.SetEnableState(enabled);
            this.FullAcLabel.SetEnableState(enabled);
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
