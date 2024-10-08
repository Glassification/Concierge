﻿// <copyright file="AttacksWindow.xaml.cs" company="Thomas Beckett">
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
    using Concierge.Configuration;
    using Concierge.Data;
    using Concierge.Data.Units;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for AttacksWindow.xaml.
    /// </summary>
    public partial class AttacksWindow : ConciergeWindow
    {
        private bool editing;
        private bool equippedItem;
        private Weapon selectedAttack = new ();
        private List<Weapon> attack = [];

        public AttacksWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.AttackComboBox.ItemsSource = DefaultItems;
            this.TypeComboBox.ItemsSource = ComboBoxGenerator.WeaponTypesComboBox();
            this.AbilityComboBox.ItemsSource = ComboBoxGenerator.AbilitiesComboBox();
            this.DamageTypeComboBox.ItemsSource = ComboBoxGenerator.DamageTypesComboBox();
            this.CoinTypeComboBox.ItemsSource = ComboBoxGenerator.CoinTypesComboBox();
            this.ConciergePage = ConciergePages.None;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.AttackComboBox);
            this.SetMouseOverEvents(this.TypeComboBox);
            this.SetMouseOverEvents(this.AbilityComboBox);
            this.SetMouseOverEvents(this.DamageTextBox, this.DamageTextBackground);
            this.SetMouseOverEvents(this.MiscDamageTextBox, this.MiscDamageTextBackground);
            this.SetMouseOverEvents(this.DamageTypeComboBox);
            this.SetMouseOverEvents(this.RangeTextBox, this.RangeTextBackground);
            this.SetMouseOverEvents(this.WeightUpDown);
            this.SetMouseOverEvents(this.IgnoreWeightCheckBox);
            this.SetMouseOverEvents(this.ValueUpDown);
            this.SetMouseOverEvents(this.CoinTypeComboBox);
            this.SetMouseOverEvents(this.ProficencyOverrideCheckBox);
            this.SetMouseOverEvents(this.NotesTextBox, this.NotesTextBackground);
            this.SetMouseOverEvents(this.AttunedCheckBox);
            this.SetMouseOverEvents(this.MagicCheckBox);
        }

        public override string HeaderText => $"{(this.editing ? "Edit" : "Add")} Attack";

        public override string WindowName => nameof(AttacksWindow);

        public bool ItemsAdded { get; private set; }

        private static List<DetailedComboBoxItemControl> DefaultItems => ComboBoxGenerator.DetailedSelectorComboBox(Defaults.Weapons, Program.CustomItemService.GetItems<Weapon>());

        public override ConciergeResult ShowWizardSetup(string buttonText)
        {
            this.attack = Program.CcsFile.Character.Equipment.Weapons;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.editing = false;
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

            this.attack = castItem;
            this.editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.ItemsAdded;
        }

        public override void ShowEdit<T>(T weapon, bool equippedItem)
        {
            if (weapon is not Weapon castItem)
            {
                return;
            }

            this.editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.selectedAttack = castItem;
            this.equippedItem = equippedItem;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields(castItem);
            this.ShowConciergeWindow();
        }

        public override void ShowEdit<T>(T weapon)
        {
            if (weapon is not Weapon castItem)
            {
                return;
            }

            this.editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.selectedAttack = castItem;
            this.ApplyButton.Visibility = Visibility.Collapsed;

            this.FillFields(castItem);
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.Result = ConciergeResult.OK;

            if (this.editing)
            {
                this.UpdateWeapon(this.selectedAttack);
            }
            else
            {
                this.attack.Add(this.ToWeapon());
            }

            this.CloseConciergeWindow();
        }

        private void FillFields(Weapon weapon)
        {
            Program.Drawing();
            this.AttunedCheckBox.UpdatingValue();
            this.IgnoreWeightCheckBox.UpdatingValue();
            this.ProficencyOverrideCheckBox.UpdatingValue();
            this.MagicCheckBox.UpdatingValue();

            this.AttackComboBox.Text = weapon.Name;
            this.TypeComboBox.Text = weapon.Type.PascalCase();
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
            this.AttunedCheckBox.IsChecked = weapon.Attuned;
            this.MagicCheckBox.IsChecked = weapon.IsMagical;

            this.AttunedText.SetEnableState(this.equippedItem);
            this.AttunedCheckBox.SetEnableState(this.equippedItem);

            this.AttunedCheckBox.UpdatedValue();
            this.IgnoreWeightCheckBox.UpdatedValue();
            this.ProficencyOverrideCheckBox.UpdatedValue();
            this.MagicCheckBox.UpdatedValue();
            Program.NotDrawing();
        }

        private void ClearFields(string name = "")
        {
            Program.Drawing();
            this.AttunedCheckBox.UpdatingValue();
            this.IgnoreWeightCheckBox.UpdatingValue();
            this.ProficencyOverrideCheckBox.UpdatingValue();
            this.MagicCheckBox.UpdatingValue();

            this.AttackComboBox.Text = name;
            this.AttunedCheckBox.IsChecked = false;
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
            this.MagicCheckBox.IsChecked = false;

            this.AttunedText.SetEnableState(this.equippedItem);
            this.AttunedCheckBox.SetEnableState(this.equippedItem);

            this.AttunedCheckBox.UpdatedValue();
            this.IgnoreWeightCheckBox.UpdatedValue();
            this.ProficencyOverrideCheckBox.UpdatedValue();
            this.MagicCheckBox.UpdatedValue();
            Program.NotDrawing();
        }

        private void UpdateWeapon(Weapon weapon)
        {
            var oldItem = weapon.DeepCopy();

            weapon.Name = this.AttackComboBox.Text;
            weapon.Type = this.TypeComboBox.Text.ToEnum<WeaponTypes>();
            weapon.Ability = this.AbilityComboBox.Text.ToEnum<Abilities>();
            weapon.Damage = this.DamageTextBox.Text;
            weapon.Misc = this.MiscDamageTextBox.Text;
            weapon.DamageType = this.DamageTypeComboBox.Text.ToEnum<DamageTypes>();
            weapon.Range = this.RangeTextBox.Text;
            weapon.Weight.Value = this.WeightUpDown.Value;
            weapon.ProficiencyOverride = this.ProficencyOverrideCheckBox.IsChecked ?? false;
            weapon.IgnoreWeight = this.IgnoreWeightCheckBox.IsChecked ?? false;
            weapon.Attuned = this.AttunedCheckBox.IsChecked ?? false;
            weapon.Note = this.NotesTextBox.Text;
            weapon.Value = this.ValueUpDown.Value;
            weapon.CoinType = this.CoinTypeComboBox.Text.ToEnum<CoinType>();
            weapon.IsMagical = this.MagicCheckBox.IsChecked ?? false;

            if (!weapon.IsCustom)
            {
                Program.UndoRedoService.AddCommand(new EditCommand<Weapon>(weapon, oldItem, this.ConciergePage));
            }
        }

        private Weapon Create()
        {
            return new Weapon(Program.CcsFile.CharacterService)
            {
                Name = this.AttackComboBox.Text,
                Type = this.TypeComboBox.Text.ToEnum<WeaponTypes>(),
                Ability = this.AbilityComboBox.Text.ToEnum<Abilities>(),
                Damage = this.DamageTextBox.Text,
                Misc = this.MiscDamageTextBox.Text,
                DamageType = this.DamageTypeComboBox.Text.ToEnum<DamageTypes>(),
                Range = this.RangeTextBox.Text,
                Weight = new UnitDouble(this.WeightUpDown.Value, AppSettingsManager.UserSettings.UnitOfMeasurement, Measurements.Weight),
                ProficiencyOverride = this.ProficencyOverrideCheckBox.IsChecked ?? false,
                IgnoreWeight = this.IgnoreWeightCheckBox.IsChecked ?? false,
                Note = this.NotesTextBox.Text,
                Value = this.ValueUpDown.Value,
                Attuned = this.AttunedCheckBox.IsChecked ?? false,
                CoinType = this.CoinTypeComboBox.Text.ToEnum<CoinType>(),
                IsMagical = this.MagicCheckBox.IsChecked ?? false,
            };
        }

        private Weapon ToWeapon()
        {
            this.ItemsAdded = true;
            var weapon = this.Create();

            Program.UndoRedoService.AddCommand(new AddCommand<Weapon>(this.attack, weapon, this.ConciergePage));

            return weapon;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Exit;
            this.CloseConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeResult.Cancel;
            this.CloseConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.attack.Add(this.ToWeapon());
            this.ClearFields();
            this.InvokeApplyChanges();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void AttackComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var isLocked = this.LockButton.IsChecked ?? false;
            if (this.AttackComboBox.SelectedItem is DetailedComboBoxItemControl item && item.Item is Weapon weapon && !isLocked)
            {
                this.FillFields(weapon);
            }
            else if (!isLocked)
            {
                this.ClearFields(this.AttackComboBox.Text);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.AttackComboBox.Text.IsNullOrWhiteSpace())
            {
                ConciergeMessageBox.ShowWarning("Could not save the Attack.\nA name is required before saving a custom item.");
                return;
            }

            Program.CustomItemService.AddItem(this.Create());
            this.ClearFields();
            this.AttackComboBox.ItemsSource = DefaultItems;
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
