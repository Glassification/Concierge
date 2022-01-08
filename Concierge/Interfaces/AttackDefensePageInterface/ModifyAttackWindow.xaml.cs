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
    public partial class ModifyAttackWindow : ConciergeWindow, IConciergeModifyWindow
    {
        private readonly ConciergePage conciergePage;

        public ModifyAttackWindow(ConciergePage conciergePage)
        {
            this.InitializeComponent();
            this.AttackComboBox.ItemsSource = Constants.Weapons;
            this.TypeComboBox.ItemsSource = Enum.GetValues(typeof(WeaponTypes)).Cast<WeaponTypes>();
            this.AbilityComboBox.ItemsSource = Enum.GetValues(typeof(Abilities)).Cast<Abilities>();
            this.DamageTypeComboBox.ItemsSource = Enum.GetValues(typeof(DamageTypes)).Cast<DamageTypes>();
            this.conciergePage = conciergePage;
        }

        public bool ItemsAdded { get; private set; }

        private bool Editing { get; set; }

        private string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Attack";

        private Weapon SelectedAttack { get; set; }

        private List<Weapon> Weapons { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.Weapons = Program.CcsFile.Character.Weapons;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public void ShowAdd(List<Weapon> weapons)
        {
            this.Weapons = weapons;
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ApplyButton.Visibility = Visibility.Visible;
            this.OkButton.Visibility = Visibility.Visible;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowConciergeWindow();
        }

        public void ShowEdit(Weapon weapon)
        {
            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.SelectedAttack = weapon;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.OkButton.Visibility = Visibility.Visible;

            this.FillFields(weapon);
            this.ShowConciergeWindow();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        private void FillFields(Weapon weapon)
        {
            this.WeightUpDown.UpdatingValue();
            this.BagOfHoldingCheckBox.UpdatingValue();
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
            this.BagOfHoldingCheckBox.IsChecked = weapon.IsInBagOfHolding;
            this.NotesTextBox.Text = weapon.Note;
            this.WeightUnits.Text = $"({UnitFormat.WeightPostfix})";

            this.BagOfHoldingCheckBox.UpdatedValue();
            this.ProficencyOverrideCheckBox.UpdatedValue();
        }

        private void ClearFields()
        {
            this.WeightUpDown.UpdatingValue();
            this.BagOfHoldingCheckBox.UpdatingValue();
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
            this.BagOfHoldingCheckBox.IsChecked = false;
            this.NotesTextBox.Text = string.Empty;
            this.WeightUnits.Text = $"({UnitFormat.WeightPostfix})";

            this.BagOfHoldingCheckBox.UpdatedValue();
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
            weapon.Weight.Value = this.WeightUpDown.Value ?? 0.0;
            weapon.ProficiencyOverride = this.ProficencyOverrideCheckBox.IsChecked ?? false;
            weapon.IsInBagOfHolding = this.BagOfHoldingCheckBox.IsChecked ?? false;
            weapon.Note = this.NotesTextBox.Text;

            Program.UndoRedoService.AddCommand(new EditCommand<Weapon>(weapon, oldItem, this.conciergePage));
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
                Weight = new ConciergeDouble(this.WeightUpDown.Value ?? 0.0, AppSettingsManager.Settings.UnitOfMeasurement, Measurements.Weight),
                ProficiencyOverride = this.ProficencyOverrideCheckBox.IsChecked ?? false,
                IsInBagOfHolding = this.BagOfHoldingCheckBox.IsChecked ?? false,
                Note = this.NotesTextBox.Text,
            };

            Program.UndoRedoService.AddCommand(new AddCommand<Weapon>(this.Weapons, weapon, this.conciergePage));

            return weapon;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.HideConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Cancel;
            this.HideConciergeWindow();
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
            this.Result = ConciergeWindowResult.OK;

            if (this.Editing)
            {
                this.UpdateWeapon(this.SelectedAttack);
            }
            else
            {
                this.Weapons.Add(this.ToWeapon());
            }

            this.HideConciergeWindow();
            Program.Modify();
        }

        private void AttackComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.AttackComboBox.SelectedItem != null)
            {
                this.FillFields(this.AttackComboBox.SelectedItem as Weapon);
            }
        }
    }
}
