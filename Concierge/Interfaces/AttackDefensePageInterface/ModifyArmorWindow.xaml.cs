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
    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyArmorWindow.xaml.
    /// </summary>
    public partial class ModifyArmorWindow : Window, IConciergeWindow
    {
        public ModifyArmorWindow()
        {
            this.InitializeComponent();
            this.TypeComboBox.ItemsSource = Enum.GetValues(typeof(ArmorType)).Cast<ArmorType>();
            this.StealthComboBox.ItemsSource = Enum.GetValues(typeof(ArmorStealth)).Cast<ArmorStealth>();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private Armor SelectedArmor { get; set; }

        private ConciergeWindowResult Result { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.SelectedArmor = Program.CcsFile.Character.Armor;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillArmorDetails();
            this.ShowDialog();

            return this.Result;
        }

        public void ShowEdit(Armor armor)
        {
            this.SelectedArmor = armor;
            this.ApplyButton.Visibility = Visibility.Visible;

            this.FillArmorDetails();
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

        private void FillArmorDetails()
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
            this.WeightUpDown.Value = this.SelectedArmor.Weight;
            this.StrengthUpDown.Value = this.SelectedArmor.Strength;
            this.StealthComboBox.Text = this.SelectedArmor.Stealth.ToString();

            this.ShieldTextBox.Text = this.SelectedArmor.Shield;
            this.ShieldArmorClassUpDown.Value = this.SelectedArmor.ShieldArmorClass;
            this.ShieldWeightUpDown.Value = this.SelectedArmor.ShieldWeight;
            this.MiscArmorClassUpDown.Value = this.SelectedArmor.MiscArmorClass;
            this.MagicArmorClassUpDown.Value = this.SelectedArmor.MagicArmorClass;
        }

        private void ToArmor(Armor armor)
        {
            armor.Equiped = this.EquipedTextBox.Text;
            armor.Type = (ArmorType)Enum.Parse(typeof(ArmorType), this.TypeComboBox.Text);
            armor.ArmorClass = (int)this.ArmorClassUpDown.Value;
            armor.Weight = (double)this.WeightUpDown.Value;
            armor.Strength = (int)this.StrengthUpDown.Value;
            armor.Stealth = (ArmorStealth)Enum.Parse(typeof(ArmorStealth), this.StealthComboBox.Text);
            armor.Shield = this.ShieldTextBox.Text;
            armor.ShieldArmorClass = (int)this.ShieldArmorClassUpDown.Value;
            armor.ShieldWeight = (double)this.ShieldWeightUpDown.Value;
            armor.MiscArmorClass = (int)this.MiscArmorClassUpDown.Value;
            armor.MagicArmorClass = (int)this.MagicArmorClassUpDown.Value;
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
            Program.Modify();

            this.ToArmor(this.SelectedArmor);

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            this.ToArmor(this.SelectedArmor);
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.Hide();
        }
    }
}
