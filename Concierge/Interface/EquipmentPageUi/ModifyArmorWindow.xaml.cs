// <copyright file="ModifyArmorWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.EquipmentPageUi
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters;
    using Concierge.Characters.Enums;
    using Concierge.Characters.Items;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyArmorWindow.xaml.
    /// </summary>
    public partial class ModifyArmorWindow : Window
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

        public void ShowEdit(Armor armor)
        {
            this.SelectedArmor = armor;
            this.FillArmorDetails(this.SelectedArmor);
            this.ShowDialog();
        }

        private void FillArmorDetails(Armor armor)
        {
            this.EquipedTextBox.Text = armor.Equiped;
            this.TypeComboBox.Text = armor.Type.ToString();
            this.ArmorClassUpDown.Value = armor.ArmorClass;
            this.WeightUpDown.Value = armor.Weight;
            this.StrengthUpDown.Value = armor.Strength;
            this.StealthComboBox.Text = armor.Stealth.ToString();

            this.ShieldTextBox.Text = armor.Shield;
            this.ShieldArmorClassUpDown.Value = armor.ShieldArmorClass;
            this.ShieldWeightUpDown.Value = armor.ShieldWeight;
            this.MiscArmorClassUpDown.Value = armor.MiscArmorClass;
            this.MagicArmorClassUpDown.Value = armor.MagicArmorClass;
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
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.ButtonClick();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            ConciergeSound.ButtonClick();

            this.ToArmor(this.SelectedArmor);

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            ConciergeSound.ButtonClick();

            this.ToArmor(this.SelectedArmor);
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.ButtonClick();
            this.Hide();
        }

        private void Button_MouseEnter(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.Black;
        }

        private void Button_MouseLeave(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.White;
        }
    }
}
