// <copyright file="ModifyWeaponWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.EquipmentPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ModifyWeaponWindow.xaml.
    /// </summary>
    public partial class ModifyWeaponWindow : Window, IConciergeWindow
    {
        public ModifyWeaponWindow()
        {
            this.InitializeComponent();
            this.WeaponComboBox.ItemsSource = Constants.Weapons;
            this.TypeComboBox.ItemsSource = Enum.GetValues(typeof(WeaponTypes)).Cast<WeaponTypes>();
            this.AbilityComboBox.ItemsSource = Enum.GetValues(typeof(Abilities)).Cast<Abilities>();
            this.DamageTypeComboBox.ItemsSource = Enum.GetValues(typeof(DamageTypes)).Cast<DamageTypes>();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private bool Editing { get; set; }

        private Weapon SelectedWeapon { get; set; }

        private List<Weapon> Weapons { get; set; }

        private ConciergeWindowResult Result { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.Weapons = Program.CcsFile.Character.Weapons;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.Editing = false;
            this.HeaderTextBlock.Text = "Add Weapon";

            this.ClearFields();
            this.ShowDialog();

            return this.Result;
        }

        public void ShowAdd(List<Weapon> weapons)
        {
            this.Weapons = weapons;
            this.Editing = false;
            this.HeaderTextBlock.Text = "Add Weapon";
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Visible;

            this.ClearFields();
            this.ShowDialog();
        }

        public void ShowEdit(Weapon weapon)
        {
            this.HeaderTextBlock.Text = "Edit Weapon";
            this.Editing = true;
            this.SelectedWeapon = weapon;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.OkButton.Visibility = Visibility.Visible;

            this.FillFields(weapon);
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

        private void FillFields(Weapon weapon)
        {
            this.WeightUpDown.UpdatingValue();
            this.BagOfHoldingCheckBox.UpdatingValue();
            this.ProficencyOverrideCheckBox.UpdatingValue();

            this.WeaponComboBox.Text = weapon.Name;
            this.TypeComboBox.Text = weapon.WeaponType.ToString();
            this.AbilityComboBox.Text = weapon.Ability.ToString();
            this.DamageTextBox.Text = weapon.Damage;
            this.MiscDamageTextBox.Text = weapon.Misc;
            this.DamageTypeComboBox.Text = weapon.DamageType.ToString();
            this.RangeTextBox.Text = weapon.Range;
            this.WeightUpDown.Value = weapon.Weight;
            this.ProficencyOverrideCheckBox.IsChecked = weapon.ProficiencyOverride;
            this.BagOfHoldingCheckBox.IsChecked = weapon.IsInBagOfHolding;
            this.NotesTextBox.Text = weapon.Note;

            this.BagOfHoldingCheckBox.UpdatedValue();
            this.ProficencyOverrideCheckBox.UpdatedValue();
        }

        private void ClearFields()
        {
            this.WeightUpDown.UpdatingValue();
            this.BagOfHoldingCheckBox.UpdatingValue();
            this.ProficencyOverrideCheckBox.UpdatingValue();

            this.WeaponComboBox.Text = string.Empty;
            this.TypeComboBox.Text = WeaponTypes.None.ToString();
            this.AbilityComboBox.Text = Abilities.NONE.ToString();
            this.DamageTextBox.Text = string.Empty;
            this.MiscDamageTextBox.Text = string.Empty;
            this.DamageTypeComboBox.Text = DamageTypes.None.ToString();
            this.RangeTextBox.Text = string.Empty;
            this.WeightUpDown.Value = 0.0;
            this.ProficencyOverrideCheckBox.IsChecked = false;
            this.BagOfHoldingCheckBox.IsChecked = false;
            this.NotesTextBox.Text = string.Empty;

            this.BagOfHoldingCheckBox.UpdatedValue();
            this.ProficencyOverrideCheckBox.UpdatedValue();
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
            weapon.IsInBagOfHolding = this.BagOfHoldingCheckBox.IsChecked ?? false;
            weapon.Note = this.NotesTextBox.Text;
        }

        private Weapon ToWeapon()
        {
            return new Weapon()
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
                IsInBagOfHolding = this.BagOfHoldingCheckBox.IsChecked ?? false,
                Note = this.NotesTextBox.Text,
            };
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.Weapons.Add(this.ToWeapon());
            this.ClearFields();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateWeapon(this.SelectedWeapon);
            }
            else
            {
                this.Weapons.Add(this.ToWeapon());
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
    }
}
