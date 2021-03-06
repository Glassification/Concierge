﻿// <copyright file="ModifyWeaponWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.EquipmentPageUi
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters.Collections;
    using Concierge.Characters.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyWeaponWindow.xaml.
    /// </summary>
    public partial class ModifyWeaponWindow : Window
    {
        public ModifyWeaponWindow()
        {
            this.InitializeComponent();
            this.WeaponComboBox.ItemsSource = Constants.Weapons;
            this.TypeComboBox.ItemsSource = Enum.GetValues(typeof(WeaponTypes)).Cast<WeaponTypes>();
            this.AbilityComboBox.ItemsSource = Enum.GetValues(typeof(Abilities)).Cast<Abilities>();
            this.DamageTypeComboBox.ItemsSource = Enum.GetValues(typeof(DamageTypes)).Cast<DamageTypes>();
        }

        private bool Editing { get; set; }

        private Guid SelectedWeaponId { get; set; }

        public void ShowAdd()
        {
            this.HeaderTextBlock.Text = "Add Weapon";
            this.Editing = false;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.ClearFields();

            this.ShowDialog();
        }

        public void ShowEdit(Weapon weapon)
        {
            this.HeaderTextBlock.Text = "Edit Weapon";
            this.SelectedWeaponId = weapon.ID;
            this.Editing = true;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.FillFields(weapon);

            this.ShowDialog();
        }

        private void FillFields(Weapon weapon)
        {
            this.WeaponComboBox.Text = weapon.Name;
            this.TypeComboBox.Text = weapon.WeaponType.ToString();
            this.AbilityComboBox.Text = weapon.Ability.ToString();
            this.DamageTextBox.Text = weapon.Damage;
            this.MiscDamageTextBox.Text = weapon.Misc;
            this.DamageTypeComboBox.Text = weapon.DamageType.ToString();
            this.RangeTextBox.Text = weapon.Range;
            this.WeightUpDown.Value = weapon.Weight;
            this.ProficencyOverrideCheckBox.IsChecked = weapon.ProficiencyOverride;
            this.NotesTextBox.Text = weapon.Note;
        }

        private void ClearFields()
        {
            this.WeaponComboBox.Text = string.Empty;
            this.TypeComboBox.Text = string.Empty;
            this.AbilityComboBox.Text = string.Empty;
            this.DamageTextBox.Text = string.Empty;
            this.MiscDamageTextBox.Text = string.Empty;
            this.DamageTypeComboBox.Text = string.Empty;
            this.RangeTextBox.Text = string.Empty;
            this.WeightUpDown.Value = 0.0;
            this.ProficencyOverrideCheckBox.IsChecked = false;
            this.NotesTextBox.Text = string.Empty;
        }

        private void UpdateWeapon(Weapon weapon)
        {
            weapon.Name = this.WeaponComboBox.Text;
            weapon.WeaponType = (WeaponTypes)Enum.Parse(typeof(WeaponTypes), this.TypeComboBox.Text);
            weapon.Ability = (Abilities)Enum.Parse(typeof(Abilities), this.AbilityComboBox.Text);
            weapon.Damage = this.DamageTextBox.Text;
            weapon.Misc = this.MiscDamageTextBox.Text;
            weapon.DamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), this.DamageTypeComboBox.Text);
            weapon.Range = this.RangeTextBox.Text;
            weapon.Weight = this.WeightUpDown.Value ?? 0.0;
            weapon.ProficiencyOverride = this.ProficencyOverrideCheckBox.IsChecked ?? false;
            weapon.Note = this.NotesTextBox.Text;

            Program.Modified = true;
        }

        private Weapon ToWeapon()
        {
            var weapon = new Weapon()
            {
                Name = this.WeaponComboBox.Text,
                WeaponType = (WeaponTypes)Enum.Parse(typeof(WeaponTypes), this.TypeComboBox.Text),
                Ability = (Abilities)Enum.Parse(typeof(Abilities), this.AbilityComboBox.Text),
                Damage = this.DamageTextBox.Text,
                Misc = this.MiscDamageTextBox.Text,
                DamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), this.DamageTypeComboBox.Text),
                Range = this.RangeTextBox.Text,
                Weight = this.WeightUpDown.Value ?? 0.0,
                ProficiencyOverride = this.ProficencyOverrideCheckBox.IsChecked ?? false,
                Note = this.NotesTextBox.Text,
            };

            return weapon;
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
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.CcsFile.Character.Weapons.Add(this.ToWeapon());
            Program.Modified = true;
            this.ClearFields();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Editing)
            {
                this.UpdateWeapon(Program.CcsFile.Character.GetWeaponById(this.SelectedWeaponId));
            }
            else
            {
                Program.CcsFile.Character.Weapons.Add(this.ToWeapon());
                Program.Modified = true;
            }

            this.Hide();
        }

        private void WeaponComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.WeaponComboBox.SelectedItem != null)
            {
                this.FillFields(this.WeaponComboBox.SelectedItem as Weapon);
            }
        }

        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            this.CloseButton.Foreground = Brushes.Black;
        }

        private void CloseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            this.CloseButton.Foreground = Brushes.White;
        }
    }
}
