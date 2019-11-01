using Concierge.Characters;
using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace Concierge.Presentation.DialogBoxes
{
    /// <summary>
    /// Interaction logic for ModifyArmorWindow.xaml
    /// </summary>
    public partial class ModifyArmorWindow : Window
    {

        #region Constructor

        public ModifyArmorWindow()
        {
            InitializeComponent();
            TypeComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ArmorType)).Cast<Constants.ArmorType>();
            StealthComboBox.ItemsSource = Enum.GetValues(typeof(Constants.ArmorStealth)).Cast<Constants.ArmorStealth>();
        }

        #endregion

        #region Methods

        public void ShowEdit()
        {
            FillArmorDetails(Program.Character.Armor);
            ShowDialog();
        }

        private void FillArmorDetails(Armor armor)
        {
            EquipedTextBox.Text = armor.Equiped;
            TypeComboBox.Text = armor.Type.ToString();
            ArmorClassUpDown.Value = armor.ArmorClass;
            WeightUpDown.Value = armor.Weight;
            StrengthUpDown.Value = armor.Strength;
            StealthComboBox.Text = armor.Stealth.ToString();

            ShieldTextBox.Text = armor.Shield;
            ShieldArmorClassUpDown.Value = armor.ShieldArmorClass;
            ShieldWeightUpDown.Value = armor.ShieldWeight;
            MiscArmorClassUpDown.Value = armor.MiscArmorClass;
            MagicArmorClassUpDown.Value = armor.MagicArmorClass;
        }

        private void ToArmor(Armor armor)
        {
            armor.Equiped = EquipedTextBox.Text;
            armor.Type = (Constants.ArmorType)Enum.Parse(typeof(Constants.ArmorType), TypeComboBox.Text);
            armor.ArmorClass = (int)ArmorClassUpDown.Value;
            armor.Weight = (double)WeightUpDown.Value;
            armor.Strength = (int)StrengthUpDown.Value;
            armor.Stealth = (Constants.ArmorStealth)Enum.Parse(typeof(Constants.ArmorStealth), StealthComboBox.Text);
            armor.Shield = ShieldTextBox.Text;
            armor.ShieldArmorClass = (int)ShieldArmorClassUpDown.Value;
            armor.ShieldWeight = (double)ShieldWeightUpDown.Value;
            armor.MiscArmorClass = (int)MiscArmorClassUpDown.Value;
            armor.MagicArmorClass = (int)MagicArmorClassUpDown.Value;
        }

        #endregion

        #region Events

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Hide();
                    break;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            ToArmor(Program.Character.Armor);
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            ToArmor(Program.Character.Armor);
            Hide();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        #endregion

    }
}
