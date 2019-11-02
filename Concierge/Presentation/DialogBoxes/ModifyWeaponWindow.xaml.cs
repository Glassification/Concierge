using Concierge.Characters.Collections;
using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Concierge.Presentation.DialogBoxes
{
    /// <summary>
    /// Interaction logic for ModifyWeaponWindow.xaml
    /// </summary>
    public partial class ModifyWeaponWindow : Window
    {
        public ModifyWeaponWindow()
        {
            InitializeComponent();
            WeaponComboBox.ItemsSource = Constants.Weapons;
            TypeComboBox.ItemsSource = Enum.GetValues(typeof(Constants.WeaponTypes)).Cast<Constants.WeaponTypes>();
            AbilityComboBox.ItemsSource = Enum.GetValues(typeof(Constants.Abilities)).Cast<Constants.Abilities>();
            DamageTypeComboBox.ItemsSource = Enum.GetValues(typeof(Constants.DamageTypes)).Cast<Constants.DamageTypes>();
        }

        public void ShowAdd()
        {
            HeaderTextBlock.Text = "Add Weapon";
            Editing = true;
            ApplyButton.Visibility = Visibility.Visible;
            ClearFields();

            ShowDialog();
        }

        public void ShowEdit(Weapon weapon)
        {
            HeaderTextBlock.Text = "Edit Weapon";
            SelectedWeaponId = weapon.ID;
            Editing = true;
            ApplyButton.Visibility = Visibility.Collapsed;
            FillFields(weapon);

            ShowDialog();
        }

        private void FillFields(Weapon weapon)
        {
            WeaponComboBox.Text = weapon.Name;
            TypeComboBox.Text = weapon.WeaponType.ToString();
            AbilityComboBox.Text = weapon.Ability.ToString();
            DamageTextBox.Text = weapon.Damage;
            MiscDamageTextBox.Text = weapon.Misc;
            DamageTypeComboBox.Text = weapon.DamageType.ToString();
            RangeTextBox.Text = weapon.Range;
            WeightUpDown.Value = weapon.Weight;
            ProficencyOverrideCheckBox.IsChecked = weapon.ProficiencyOverride;
            NotesTextBox.Text = weapon.Note;
        }

        private void ClearFields()
        {
            WeaponComboBox.Text = string.Empty;
            TypeComboBox.Text = string.Empty;
            AbilityComboBox.Text = string.Empty;
            DamageTextBox.Text = string.Empty;
            MiscDamageTextBox.Text = string.Empty;
            DamageTypeComboBox.Text = string.Empty;
            RangeTextBox.Text = string.Empty;
            WeightUpDown.Value = 0.0;
            ProficencyOverrideCheckBox.IsChecked = false;
            NotesTextBox.Text = string.Empty;
        }

        private void UpdateWeapon(Weapon weapon)
        {
            weapon.Name = WeaponComboBox.Text;
            weapon.WeaponType = (Constants.WeaponTypes)Enum.Parse(typeof(Constants.WeaponTypes), TypeComboBox.Text);
            weapon.Ability = (Constants.Abilities)Enum.Parse(typeof(Constants.Abilities), AbilityComboBox.Text);
            weapon.Damage = DamageTextBox.Text;
            weapon.Misc = MiscDamageTextBox.Text;
            weapon.DamageType = (Constants.DamageTypes)Enum.Parse(typeof(Constants.DamageTypes), DamageTypeComboBox.Text);
            weapon.Range = RangeTextBox.Text;
            weapon.Weight = WeightUpDown.Value ?? 0.0;
            weapon.ProficiencyOverride = ProficencyOverrideCheckBox.IsChecked ?? false;
            weapon.Note = NotesTextBox.Text;
        }

        private Weapon ToWeapon()
        {
            Weapon weapon = new Weapon()
            {
                Name = WeaponComboBox.Text,
                WeaponType = (Constants.WeaponTypes)Enum.Parse(typeof(Constants.WeaponTypes), TypeComboBox.Text),
                Ability = (Constants.Abilities)Enum.Parse(typeof(Constants.Abilities), AbilityComboBox.Text),
                Damage = DamageTextBox.Text,
                Misc = MiscDamageTextBox.Text,
                DamageType = (Constants.DamageTypes)Enum.Parse(typeof(Constants.DamageTypes), DamageTypeComboBox.Text),
                Range = RangeTextBox.Text,
                Weight = WeightUpDown.Value ?? 0.0,
                ProficiencyOverride = ProficencyOverrideCheckBox.IsChecked ?? false,
                Note = NotesTextBox.Text
            };

            return weapon;
        }

        private bool Editing { get; set; }
        private Guid SelectedWeaponId { get; set; }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Character.Weapons.Add(ToWeapon());
            ClearFields();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Editing)
            {
                UpdateWeapon(Program.Character.GetWeaponById(SelectedWeaponId));
            }
            else
            {
                Program.Character.Weapons.Add(ToWeapon());
            }

            Hide();
        }

        private void WeaponComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeaponComboBox.SelectedItem != null)
            {
                FillFields(WeaponComboBox.SelectedItem as Weapon);
            }
        }
    }
}
