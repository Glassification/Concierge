// <copyright file="CompanionAttacksWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Companions;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for CompanionAttacksWindow.xaml.
    /// </summary>
    public partial class CompanionAttacksWindow : ConciergeWindow
    {
        private bool editing;
        private CompanionWeapon selectedAttack = new ();
        private List<CompanionWeapon> weapons = [];

        public CompanionAttacksWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.AttackComboBox.ItemsSource = DefaultItems;
            this.AbilityComboBox.ItemsSource = ComboBoxGenerator.AbilitiesComboBox();
            this.DamageTypeComboBox.ItemsSource = ComboBoxGenerator.DamageTypesComboBox();
            this.ConciergePage = ConciergePages.None;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.AttackComboBox);
            this.SetMouseOverEvents(this.AbilityComboBox);
            this.SetMouseOverEvents(this.DamageTextBox, this.DamageTextBackground);
            this.SetMouseOverEvents(this.MiscDamageTextBox, this.MiscDamageTextBackground);
            this.SetMouseOverEvents(this.DamageTypeComboBox);
            this.SetMouseOverEvents(this.RangeTextBox, this.RangeTextBackground);
            this.SetMouseOverEvents(this.ProficencyOverrideCheckBox);
            this.SetMouseOverEvents(this.NotesTextBox, this.NotesTextBackground);
        }

        public override string HeaderText => $"{(this.editing ? "Edit" : "Add")} Attack";

        public override string WindowName => nameof(CompanionAttacksWindow);

        public bool ItemsAdded { get; private set; }

        private static List<DetailedComboBoxItemControl> DefaultItems => ComboBoxGenerator.DetailedSelectorComboBox(Program.CustomItemService.GetItems<CompanionWeapon>());

        public override bool ShowAdd<T>(T weapons)
        {
            if (weapons is not List<CompanionWeapon> castItem)
            {
                return false;
            }

            this.weapons = castItem;
            this.editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.ItemsAdded = false;

            this.ClearFields();
            this.ShowConciergeWindow();

            return this.ItemsAdded;
        }

        public override void ShowEdit<T>(T weapon)
        {
            if (weapon is not CompanionWeapon castItem)
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
                this.weapons.Add(this.ToWeapon());
            }

            this.CloseConciergeWindow();
        }

        private void FillFields(CompanionWeapon weapon)
        {
            Program.Drawing();
            this.ProficencyOverrideCheckBox.UpdatingValue();

            this.AttackComboBox.Text = weapon.Name;
            this.AbilityComboBox.Text = weapon.Ability.ToString();
            this.DamageTextBox.Text = weapon.Damage;
            this.MiscDamageTextBox.Text = weapon.Misc;
            this.DamageTypeComboBox.Text = weapon.DamageType.ToString();
            this.RangeTextBox.Text = weapon.Range;
            this.ProficencyOverrideCheckBox.IsChecked = weapon.ProficiencyOverride;
            this.NotesTextBox.Text = weapon.Note;

            this.ProficencyOverrideCheckBox.UpdatedValue();
            Program.NotDrawing();
        }

        private void ClearFields(string name = "")
        {
            Program.Drawing();
            this.ProficencyOverrideCheckBox.UpdatingValue();

            this.AttackComboBox.Text = name;
            this.AbilityComboBox.Text = Abilities.NONE.ToString();
            this.DamageTextBox.Text = string.Empty;
            this.MiscDamageTextBox.Text = string.Empty;
            this.DamageTypeComboBox.Text = DamageTypes.None.ToString();
            this.RangeTextBox.Text = string.Empty;
            this.ProficencyOverrideCheckBox.IsChecked = false;
            this.NotesTextBox.Text = string.Empty;

            this.ProficencyOverrideCheckBox.UpdatedValue();
            Program.NotDrawing();
        }

        private void UpdateWeapon(CompanionWeapon weapon)
        {
            var oldItem = weapon.DeepCopy();

            weapon.Name = this.AttackComboBox.Text;
            weapon.Ability = this.AbilityComboBox.Text.ToEnum<Abilities>();
            weapon.Damage = this.DamageTextBox.Text;
            weapon.Misc = this.MiscDamageTextBox.Text;
            weapon.DamageType = this.DamageTypeComboBox.Text.ToEnum<DamageTypes>();
            weapon.Range = this.RangeTextBox.Text;
            weapon.ProficiencyOverride = this.ProficencyOverrideCheckBox.IsChecked ?? false;
            weapon.Note = this.NotesTextBox.Text;

            if (!weapon.IsCustom)
            {
                Program.UndoRedoService.AddCommand(new EditCommand<CompanionWeapon>(weapon, oldItem, this.ConciergePage));
            }
        }

        private CompanionWeapon Create()
        {
            return new CompanionWeapon(Program.CcsFile.CharacterService)
            {
                Name = this.AttackComboBox.Text,
                Ability = this.AbilityComboBox.Text.ToEnum<Abilities>(),
                Damage = this.DamageTextBox.Text,
                Misc = this.MiscDamageTextBox.Text,
                DamageType = this.DamageTypeComboBox.Text.ToEnum<DamageTypes>(),
                Range = this.RangeTextBox.Text,
                ProficiencyOverride = this.ProficencyOverrideCheckBox.IsChecked ?? false,
                Note = this.NotesTextBox.Text,
            };
        }

        private CompanionWeapon ToWeapon()
        {
            this.ItemsAdded = true;
            var weapon = this.Create();

            Program.UndoRedoService.AddCommand(new AddCommand<CompanionWeapon>(this.weapons, weapon, this.ConciergePage));

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
            this.weapons.Add(this.ToWeapon());
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
            if (this.AttackComboBox.SelectedItem is DetailedComboBoxItemControl item && item.Item is CompanionWeapon weapon && !isLocked)
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
