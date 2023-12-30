// <copyright file="AttacksWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character;
    using Concierge.Character.Enums;
    using Concierge.Character.Equipable;
    using Concierge.Commands;
    using Concierge.Common.Enums;
    using Concierge.Common.Exceptions;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Configuration;
    using Concierge.Data;
    using Concierge.Data.Units;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for AttacksWindow.xaml.
    /// </summary>
    public partial class AttacksWindow : ConciergeWindow
    {
        public AttacksWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.AttackComboBox.ItemsSource = DefaultItems;
            this.TypeComboBox.ItemsSource = ComboBoxGenerator.WeaponTypesComboBox();
            this.AbilityComboBox.ItemsSource = ComboBoxGenerator.AbilitiesComboBox();
            this.DamageTypeComboBox.ItemsSource = ComboBoxGenerator.DamageTypesComboBox();
            this.CoinTypeComboBox.ItemsSource = ComboBoxGenerator.CoinTypesComboBox();
            this.ConciergePage = ConciergePage.None;
            this.Weapons = [];
            this.SelectedAttack = new Weapon();
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
        }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} Attack";

        public override string WindowName => nameof(AttacksWindow);

        public bool ItemsAdded { get; private set; }

        private static List<DetailedComboBoxItemControl> DefaultItems => ComboBoxGenerator.DetailedSelectorComboBox(Defaults.Weapons, Program.CustomItemService.GetCustomItems<Weapon>());

        private bool Editing { get; set; }

        private bool EquippedItem { get; set; }

        private Weapon SelectedAttack { get; set; }

        private List<Weapon> Weapons { get; set; }

        private ICreature? Creature { get; set; }

        public override ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            this.Weapons = Program.CcsFile.Character.Equipment.Weapons;
            this.OkButton.Visibility = Visibility.Collapsed;
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.CancelButton.Content = buttonText;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.Result;
        }

        public override bool ShowAdd<T>(T weapons, ICreature creature)
        {
            if (weapons is not List<Weapon> castItem)
            {
                return false;
            }

            this.Weapons = castItem;
            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ItemsAdded = false;
            this.Creature = creature;

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

            this.Editing = true;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.SelectedAttack = castItem;
            this.EquippedItem = equippedItem;
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
        }

        private void FillFields(Weapon weapon)
        {
            this.AttunedCheckBox.UpdatingValue();
            this.IgnoreWeightCheckBox.UpdatingValue();
            this.ProficencyOverrideCheckBox.UpdatingValue();

            this.AttackComboBox.Text = weapon.Name;
            this.TypeComboBox.Text = weapon.Type.ToString().FormatFromPascalCase();
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

            DisplayUtility.SetControlEnableState(this.AttunedText, this.EquippedItem);
            DisplayUtility.SetControlEnableState(this.AttunedCheckBox, this.EquippedItem);

            this.AttunedCheckBox.UpdatedValue();
            this.IgnoreWeightCheckBox.UpdatedValue();
            this.ProficencyOverrideCheckBox.UpdatedValue();
        }

        private void ClearFields(string name = "")
        {
            this.AttunedCheckBox.UpdatingValue();
            this.IgnoreWeightCheckBox.UpdatingValue();
            this.ProficencyOverrideCheckBox.UpdatingValue();

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

            this.AttunedCheckBox.UpdatedValue();
            this.IgnoreWeightCheckBox.UpdatedValue();
            this.ProficencyOverrideCheckBox.UpdatedValue();
        }

        private void UpdateWeapon(Weapon weapon)
        {
            var oldItem = weapon.DeepCopy();

            weapon.Name = this.AttackComboBox.Text;
            weapon.Type = (WeaponTypes)Enum.Parse(typeof(WeaponTypes), this.TypeComboBox.Text.Strip(" "));
            weapon.Ability = (Abilities)Enum.Parse(typeof(Abilities), this.AbilityComboBox.Text);
            weapon.Damage = this.DamageTextBox.Text;
            weapon.Misc = this.MiscDamageTextBox.Text;
            weapon.DamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), this.DamageTypeComboBox.Text);
            weapon.Range = this.RangeTextBox.Text;
            weapon.Weight.Value = this.WeightUpDown.Value;
            weapon.ProficiencyOverride = this.ProficencyOverrideCheckBox.IsChecked ?? false;
            weapon.IgnoreWeight = this.IgnoreWeightCheckBox.IsChecked ?? false;
            weapon.Attuned = this.AttunedCheckBox.IsChecked ?? false;
            weapon.Note = this.NotesTextBox.Text;
            weapon.Value = this.ValueUpDown.Value;
            weapon.CoinType = (CoinType)Enum.Parse(typeof(CoinType), this.CoinTypeComboBox.Text);

            if (!weapon.IsCustom)
            {
                Program.UndoRedoService.AddCommand(new EditCommand<Weapon>(weapon, oldItem, this.ConciergePage));
            }
        }

        private Weapon Create(ICreature? creature)
        {
            return new Weapon(creature ?? throw new NullValueException(nameof(creature)))
            {
                Name = this.AttackComboBox.Text,
                Type = (WeaponTypes)Enum.Parse(typeof(WeaponTypes), this.TypeComboBox.Text.Strip(" ")),
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
                Attuned = this.AttunedCheckBox.IsChecked ?? false,
                CoinType = (CoinType)Enum.Parse(typeof(CoinType), this.CoinTypeComboBox.Text),
            };
        }

        private Weapon ToWeapon()
        {
            this.ItemsAdded = true;
            var weapon = this.Create(this.Creature);

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
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void AttackComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.AttackComboBox.SelectedItem is DetailedComboBoxItemControl item && item.Item is Weapon weapon)
            {
                this.FillFields(weapon);
            }
            else
            {
                this.ClearFields(this.AttackComboBox.Text);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.AttackComboBox.Text.IsNullOrWhiteSpace())
            {
                ConciergeMessageBox.Show(
                    "Could not save the Attack.\nA name is required before saving a custom item.",
                    "Warning",
                    ConciergeWindowButtons.Ok,
                    ConciergeWindowIcons.Alert);
                return;
            }

            Program.CustomItemService.AddCustomItem(this.Create(Program.CcsFile.Character));
            this.ClearFields();
            this.AttackComboBox.ItemsSource = DefaultItems;
        }
    }
}
