// <copyright file="ModifyArmorWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.AttackDefensePageInterface
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility.Units;

    /// <summary>
    /// Interaction logic for ModifyArmorWindow.xaml.
    /// </summary>
    public partial class ModifyArmorWindow : ConciergeWindow, IConciergeModifyWindow
    {
        private readonly ConciergePage conciergePage;

        public ModifyArmorWindow(ConciergePage conciergePage)
        {
            this.InitializeComponent();
            this.TypeComboBox.ItemsSource = Enum.GetValues(typeof(ArmorType)).Cast<ArmorType>();
            this.StealthComboBox.ItemsSource = Enum.GetValues(typeof(ArmorStealth)).Cast<ArmorStealth>();
            this.conciergePage = conciergePage;
        }

        private Armor SelectedArmor { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.SelectedArmor = Program.CcsFile.Character.Armor;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields();
            this.ShowDialog();

            return this.Result;
        }

        public void ShowEdit(Armor armor)
        {
            this.SelectedArmor = armor;
            this.ApplyButton.Visibility = Visibility.Visible;

            this.FillFields();
            this.ShowDialog();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        private void FillFields()
        {
            this.ArmorClassUpDown.UpdatingValue();
            this.WeightUpDown.UpdatingValue();
            this.StrengthUpDown.UpdatingValue();
            this.ShieldArmorClassUpDown.UpdatingValue();
            this.ShieldWeightUpDown.UpdatingValue();
            this.MiscArmorClassUpDown.UpdatingValue();
            this.MagicArmorClassUpDown.UpdatingValue();

            this.EquipedTextBox.Text = this.SelectedArmor.Equiped;
            this.TypeComboBox.Text = this.SelectedArmor.Type.ToString();
            this.ArmorClassUpDown.Value = this.SelectedArmor.ArmorClass;
            this.WeightUpDown.Value = this.SelectedArmor.Weight.Value;
            this.WeightUnits.Text = $"({UnitFormat.WeightPostfix})";
            this.StrengthUpDown.Value = this.SelectedArmor.Strength;
            this.StealthComboBox.Text = this.SelectedArmor.Stealth.ToString();

            this.ShieldTextBox.Text = this.SelectedArmor.Shield;
            this.ShieldArmorClassUpDown.Value = this.SelectedArmor.ShieldArmorClass;
            this.ShieldWeightUpDown.Value = this.SelectedArmor.ShieldWeight.Value;
            this.ShieldWeightUnits.Text = $"({UnitFormat.WeightPostfix})";
            this.MiscArmorClassUpDown.Value = this.SelectedArmor.MiscArmorClass;
            this.MagicArmorClassUpDown.Value = this.SelectedArmor.MagicArmorClass;
        }

        private void UpdateArmor(Armor armor)
        {
            var oldItem = armor.DeepCopy() as Armor;

            armor.Equiped = this.EquipedTextBox.Text;
            armor.Type = (ArmorType)Enum.Parse(typeof(ArmorType), this.TypeComboBox.Text);
            armor.ArmorClass = this.ArmorClassUpDown.Value ?? 0;
            armor.Weight.Value = this.WeightUpDown.Value ?? 0.0;
            armor.Strength = this.StrengthUpDown.Value ?? 0;
            armor.Stealth = (ArmorStealth)Enum.Parse(typeof(ArmorStealth), this.StealthComboBox.Text);
            armor.Shield = this.ShieldTextBox.Text;
            armor.ShieldArmorClass = this.ShieldArmorClassUpDown.Value ?? 0;
            armor.ShieldWeight.Value = this.ShieldWeightUpDown.Value ?? 0.0;
            armor.MiscArmorClass = this.MiscArmorClassUpDown.Value ?? 0;
            armor.MagicArmorClass = this.MagicArmorClassUpDown.Value ?? 0;

            Program.UndoRedoService.AddCommand(new EditCommand<Armor>(armor, oldItem, this.conciergePage));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.UpdateArmor(this.SelectedArmor);

            this.InvokeApplyChanges();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            this.UpdateArmor(this.SelectedArmor);
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.Hide();
        }
    }
}
