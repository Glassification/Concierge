// <copyright file="ModifyAttackWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.AttackDefensePageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Commands;
    using Concierge.Configuration;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Primitives;
    using Concierge.Utility;
    using Concierge.Utility.Units;
    using Concierge.Utility.Units.Enums;

    /// <summary>
    /// Interaction logic for ModifyWeaponWindow.xaml.
    /// </summary>
    public partial class ModifyAttackWindow : ConciergeWindow
    {
        public ModifyAttackWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.AttackComboBox.ItemsSource = Constants.Weapons;
            this.TypeComboBox.ItemsSource = Enum.GetValues(typeof(WeaponTypes)).Cast<WeaponTypes>();
            this.AbilityComboBox.ItemsSource = Enum.GetValues(typeof(Abilities)).Cast<Abilities>();
            this.DamageTypeComboBox.ItemsSource = Enum.GetValues(typeof(DamageTypes)).Cast<DamageTypes>();
            this.CoinTypeComboBox.ItemsSource = Enum.GetValues(typeof(CoinType)).Cast<CoinType>();
            this.ConciergePage = ConciergePage.None;
            this.Weapons = new List<Weapon>();
            this.SelectedAttack = new Weapon();
        }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Attack";

        public bool ItemsAdded { get; private set; }

        private bool Editing { get; set; }

        private Weapon SelectedAttack { get; set; }

        private List<Weapon> Weapons { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Weapons = Program.CcsFile.Character.Weapons;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.CancelButton.Content = buttonText;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override bool ShowAdd<T>(T weapons)
        {
            if (weapons is not List<Weapon> castItem)
            {
                return false;
            }

            this.Weapons = castItem;
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.ItemsAdded;
        }

        public override void ShowEdit<T>(T weapon)
        {
            if (weapon is not Weapon castItem)
            {
                return;
            }

            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.SelectedAttack = castItem;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields(castItem);
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateWeapon(this.SelectedAttack);
            }
            else
            {
                this.Weapons.Add(this.ToWeapon());
            }

            this.CloseConciergeWindow();
            Program.Modify();
        }

        private void FillFields(Weapon weapon)
        {
            this.IgnoreWeightCheckBox.UpdatingValue();
            this.ProficencyOverrideCheckBox.UpdatingValue();

            this.AttackComboBox.Text = weapon.Name;
            this.TypeComboBox.Text = weapon.WeaponType.ToString();
            this.AbilityComboBox.Text = weapon.Ability.ToString();
            this.DamageTextBox.Text = weapon.Damage;
            this.MiscDamageTextBox.Text = weapon.Misc;
            this.DamageTypeComboBox.Text = weapon.DamageType.ToString();
            this.RangeTextBox.Text = weapon.Range;
            this.WeightUpDown.Value = weapon.Weight.Value;
            this.ProficencyOverrideCheckBox.IsChecked = weapon.ProficiencyOverride;
            this.IgnoreWeightCheckBox.IsChecked = weapon.IgnoreWeight;
            this.NotesTextBox.Text = weapon.Note;
            this.WeightUnits.Text = $"({UnitFormat.WeightPostfix})";
            this.ValueUpDown.Value = weapon.Value;
            this.CoinTypeComboBox.Text = weapon.CoinType.ToString();

            this.IgnoreWeightCheckBox.UpdatedValue();
            this.ProficencyOverrideCheckBox.UpdatedValue();
        }

        private void ClearFields()
        {
            this.IgnoreWeightCheckBox.UpdatingValue();
            this.ProficencyOverrideCheckBox.UpdatingValue();

            this.AttackComboBox.Text = string.Empty;
            this.TypeComboBox.Text = WeaponTypes.None.ToString();
            this.AbilityComboBox.Text = Abilities.NONE.ToString();
            this.DamageTextBox.Text = string.Empty;
            this.MiscDamageTextBox.Text = string.Empty;
            this.DamageTypeComboBox.Text = DamageTypes.None.ToString();
            this.RangeTextBox.Text = string.Empty;
            this.WeightUpDown.Value = 0.0;
            this.ProficencyOverrideCheckBox.IsChecked = false;
            this.IgnoreWeightCheckBox.IsChecked = false;
            this.NotesTextBox.Text = string.Empty;
            this.WeightUnits.Text = $"({UnitFormat.WeightPostfix})";
            this.ValueUpDown.Value = 0;
            this.CoinTypeComboBox.Text = CoinType.Copper.ToString();

            this.IgnoreWeightCheckBox.UpdatedValue();
            this.ProficencyOverrideCheckBox.UpdatedValue();
        }

        private void UpdateWeapon(Weapon weapon)
        {
            var oldItem = weapon.DeepCopy();

            weapon.Name = this.AttackComboBox.Text;
            weapon.WeaponType = (WeaponTypes)Enum.Parse(typeof(WeaponTypes), this.TypeComboBox.Text);
            weapon.Ability = (Abilities)Enum.Parse(typeof(Abilities), this.AbilityComboBox.Text);
            weapon.Damage = this.DamageTextBox.Text;
            weapon.Misc = this.MiscDamageTextBox.Text;
            weapon.DamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), this.DamageTypeComboBox.Text);
            weapon.Range = this.RangeTextBox.Text;
            weapon.Weight.Value = this.WeightUpDown.Value;
            weapon.ProficiencyOverride = this.ProficencyOverrideCheckBox.IsChecked ?? false;
            weapon.IgnoreWeight = this.IgnoreWeightCheckBox.IsChecked ?? false;
            weapon.Note = this.NotesTextBox.Text;
            weapon.Value = this.ValueUpDown.Value;
            weapon.CoinType = (CoinType)Enum.Parse(typeof(CoinType), this.CoinTypeComboBox.Text);

            Program.UndoRedoService.AddCommand(new EditCommand<Weapon>(weapon, oldItem, this.ConciergePage));
        }

        private Weapon ToWeapon()
        {
            this.ItemsAdded = true;

            var weapon = new Weapon()
            {
                Name = this.AttackComboBox.Text,
                WeaponType = (WeaponTypes)Enum.Parse(typeof(WeaponTypes), this.TypeComboBox.Text),
                Ability = (Abilities)Enum.Parse(typeof(Abilities), this.AbilityComboBox.Text),
                Damage = this.DamageTextBox.Text,
                Misc = this.MiscDamageTextBox.Text,
                DamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), this.DamageTypeComboBox.Text),
                Range = this.RangeTextBox.Text,
                Weight = new UnitDouble(this.WeightUpDown.Value, AppSettingsManager.UserSettings.UnitOfMeasurement, Measurements.Weight),
                ProficiencyOverride = this.ProficencyOverrideCheckBox.IsChecked ?? false,
                IgnoreWeight = this.IgnoreWeightCheckBox.IsChecked ?? false,
                Note = this.NotesTextBox.Text,
                Value = this.ValueUpDown.Value,
                CoinType = (CoinType)Enum.Parse(typeof(CoinType), this.CoinTypeComboBox.Text),
            };

            Program.UndoRedoService.AddCommand(new AddCommand<Weapon>(this.Weapons, weapon, this.ConciergePage));

            return weapon;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.CloseConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Weapons.Add(this.ToWeapon());
            this.ClearFields();
            this.InvokeApplyChanges();

            Program.Modify();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void AttackComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.AttackComboBox.SelectedItem is Weapon weapon)
            {
                this.FillFields(weapon);
            }
        }
    }
}
