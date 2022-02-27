// <copyright file="CompanionPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.CompanionPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Items;
    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Interfaces.AttackDefensePageInterface;
    using Concierge.Interfaces.Enums;
    using Concierge.Interfaces.OverviewPageInterface;
    using Concierge.Services;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for ToolsPage.xaml.
    /// </summary>
    public partial class CompanionPage : Page, IConciergePage
    {
        public CompanionPage()
        {
            this.InitializeComponent();
            this.CurrentHitDiceBox = string.Empty;
        }

        public ConciergePage ConciergePage => ConciergePage.Companion;

        public bool HasEditableDataGrid => true;

        private string CurrentHitDiceBox { get; set; }

        public void Draw()
        {
            this.DrawAttributes();
            this.DrawDetails();
            this.DrawHealth();
            this.DrawHitDice();
            this.DrawAttacks();
        }

        public void Edit(object itemToEdit)
        {
            if (itemToEdit is not Weapon weapon)
            {
                return;
            }

            var index = this.WeaponDataGrid.SelectedIndex;
            ConciergeWindowService.ShowEdit<Weapon>(
                weapon,
                typeof(ModifyAttackWindow),
                this.Window_ApplyChanges,
                ConciergePage.Companion);
            this.DrawAttacks();
            this.WeaponDataGrid.SetSelectedIndex(index);
        }

        private static Brush SetHealthStyle()
        {
            int third = Program.CcsFile.Character.Companion.Vitality.Health.MaxHealth / 3;
            int hp = Program.CcsFile.Character.Companion.Vitality.CurrentHealth;

            return hp < third && hp > 0
                ? Brushes.IndianRed
                : hp >= third * 2 ? Brushes.DarkGreen : hp <= 0 ? Brushes.DarkGray : Brushes.DarkOrange;
        }

        private void DrawSpentHitDice(TextBlock spentField, Grid spentBox, Border border, int spent, int total)
        {
            spentField.Text = spent.ToString();
            spentField.Foreground = DisplayUtility.SetUsedTextStyle(total, spent);
            spentBox.Background = DisplayUtility.SetUsedBoxStyle(total, spent);
            DisplayUtility.SetBorderColour(spent, total, spentBox, border, this.CurrentHitDiceBox);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Consistency.")]
        private void DrawTotalHitDice(TextBlock totalField, Grid totalBox, int spent, int total)
        {
            totalField.Text = total.ToString();
            totalField.Foreground = DisplayUtility.SetTotalTextStyle(total, spent);
            totalBox.Background = DisplayUtility.SetTotalBoxStyle(total, spent);
        }

        private void DrawAttributes()
        {
            var attributes = Program.CcsFile.Character.Companion.Attributes;

            this.StrengthBonusField.Text = CharacterUtility.CalculateBonus(attributes.Strength).ToString();
            this.DexterityBonusField.Text = CharacterUtility.CalculateBonus(attributes.Dexterity).ToString();
            this.ConstitutionBonusField.Text = CharacterUtility.CalculateBonus(attributes.Constitution).ToString();
            this.IntelligenceBonusField.Text = CharacterUtility.CalculateBonus(attributes.Intelligence).ToString();
            this.WisdomBonusField.Text = CharacterUtility.CalculateBonus(attributes.Wisdom).ToString();
            this.CharismaBonusField.Text = CharacterUtility.CalculateBonus(attributes.Charisma).ToString();

            this.StrengthScoreField.Text = attributes.Strength.ToString();
            this.DexterityScoreField.Text = attributes.Dexterity.ToString();
            this.ConstitutionScoreField.Text = attributes.Constitution.ToString();
            this.IntelligenceScoreField.Text = attributes.Intelligence.ToString();
            this.WisdomScoreField.Text = attributes.Wisdom.ToString();
            this.CharismaScoreField.Text = attributes.Charisma.ToString();
        }

        private void DrawDetails()
        {
            var properties = Program.CcsFile.Character.Companion.Properties;

            this.NameField.Text = properties.Name;
            this.PerceptionField.Text = properties.Perception.ToString();
            this.VisionField.Text = properties.Vision.ToString();
            this.MovementField.Text = properties.Movement.ToString();
            this.ArmorClassField.Text = properties.ArmorClass.ToString();
        }

        private void DrawHealth()
        {
            this.CurrentHpField.Text = Program.CcsFile.Character.Companion.Vitality.CurrentHealth.ToString();
            this.TotalHpField.Text = "/" + Program.CcsFile.Character.Companion.Vitality.Health.MaxHealth.ToString();

            this.HpBackground.Foreground = SetHealthStyle();
            this.TotalHpField.Foreground = SetHealthStyle();
        }

        private void DrawHitDice()
        {
            var hitDice = Program.CcsFile.Character.Companion.Vitality.HitDice;

            this.DrawSpentHitDice(this.D6SpentField, this.D6SpentBox, this.D6Border, hitDice.SpentD6, hitDice.TotalD6);
            this.DrawTotalHitDice(this.D6TotalField, this.D6TotalBox, hitDice.SpentD6, hitDice.TotalD6);

            this.DrawSpentHitDice(this.D8SpentField, this.D8SpentBox, this.D8Border, hitDice.SpentD8, hitDice.TotalD8);
            this.DrawTotalHitDice(this.D8TotalField, this.D8TotalBox, hitDice.SpentD8, hitDice.TotalD8);

            this.DrawSpentHitDice(this.D10SpentField, this.D10SpentBox, this.D10Border, hitDice.SpentD10, hitDice.TotalD10);
            this.DrawTotalHitDice(this.D10TotalField, this.D10TotalBox, hitDice.SpentD10, hitDice.TotalD10);

            this.DrawSpentHitDice(this.D12SpentField, this.D12SpentBox, this.D12Border, hitDice.SpentD12, hitDice.TotalD12);
            this.DrawTotalHitDice(this.D12TotalField, this.D12TotalBox, hitDice.SpentD12, hitDice.TotalD12);
        }

        private void DrawAttacks()
        {
            this.WeaponDataGrid.Items.Clear();

            foreach (var weapon in Program.CcsFile.Character.Companion.Attacks)
            {
                this.WeaponDataGrid.Items.Add(weapon);
            }
        }

        private void EditAttributesButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<Attributes>(
                Program.CcsFile.Character.Companion.Attributes,
                typeof(ModifyAttributesWindow),
                this.Window_ApplyChanges,
                ConciergePage.Companion);
            this.DrawAttributes();
        }

        private void EditHitDiceButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<HitDice>(
                Program.CcsFile.Character.Companion.Vitality.HitDice,
                typeof(ModifyHitDiceWindow),
                this.Window_ApplyChanges,
                ConciergePage.Companion);
            this.DrawHitDice();
        }

        private void WeaponDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SortListFromDataGrid(this.WeaponDataGrid, Program.CcsFile.Character.Companion.Attacks, this.ConciergePage);
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            var index = this.WeaponDataGrid.NextItem(Program.CcsFile.Character.Companion.Attacks, 0, -1, this.ConciergePage);

            if (index != -1)
            {
                this.DrawAttacks();
                this.WeaponDataGrid.SetSelectedIndex(this.WeaponDataGrid.SelectedIndex);
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            var index = this.WeaponDataGrid.NextItem(Program.CcsFile.Character.Companion.Attacks, Program.CcsFile.Character.Companion.Attacks.Count - 1, 1, this.ConciergePage);

            if (index != -1)
            {
                this.DrawAttacks();
                this.WeaponDataGrid.SetSelectedIndex(this.WeaponDataGrid.SelectedIndex);
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this.WeaponDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd<List<Weapon>>(
                Program.CcsFile.Character.Companion.Attacks,
                typeof(ModifyAttackWindow),
                this.Window_ApplyChanges,
                ConciergePage.Companion);
            this.DrawAttacks();

            if (added)
            {
                this.WeaponDataGrid.SetSelectedIndex(this.WeaponDataGrid.LastIndex);
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.WeaponDataGrid.SelectedItem != null)
            {
                this.Edit(this.WeaponDataGrid.SelectedItem);
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.WeaponDataGrid.SelectedItem is not Weapon weapon)
            {
                return;
            }

            var index = this.WeaponDataGrid.SelectedIndex;
            Program.UndoRedoService.AddCommand(new DeleteCommand<Weapon>(Program.CcsFile.Character.Companion.Attacks, weapon, index, this.ConciergePage));
            Program.CcsFile.Character.Companion.Attacks.Remove(weapon);
            this.DrawAttacks();
            this.WeaponDataGrid.SetSelectedIndex(index);

            Program.Modify();
        }

        private void EditHealthButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<Health>(
                Program.CcsFile.Character.Companion.Vitality.Health,
                typeof(ModifyHealthWindow),
                this.Window_ApplyChanges,
                ConciergePage.Companion);
            this.DrawHealth();
        }

        private void TakeDamageButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowDamage<Vitality>(
                Program.CcsFile.Character.Companion.Vitality,
                typeof(ModifyHpWindow),
                this.Window_ApplyChanges,
                ConciergePage.Companion);
            this.DrawHealth();
        }

        private void HealDamageButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowHeal<Vitality>(
                Program.CcsFile.Character.Companion.Vitality,
                typeof(ModifyHpWindow),
                this.Window_ApplyChanges,
                ConciergePage.Companion);
            this.DrawHealth();
        }

        private void EditPropertiesButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<CompanionProperties>(
                Program.CcsFile.Character.Companion.Properties,
                typeof(ModifyCompanionPropertiesWindow),
                this.Window_ApplyChanges,
                ConciergePage.Companion);
            this.DrawDetails();
        }

        private void SpentBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2 || sender is not Grid usedBox)
            {
                return;
            }

            var hitDice = Program.CcsFile.Character.Companion.Vitality.HitDice;
            var oldItem = hitDice.DeepCopy();
            switch (usedBox.Name)
            {
                case "D6SpentBox":
                    hitDice.SpentD6 = DisplayUtility.IncrementUsedSlots(hitDice.SpentD6, hitDice.TotalD6);
                    DisplayUtility.SetCursor(hitDice.SpentD6, hitDice.TotalD6, (x, y) => x == y, Cursors.Arrow);
                    break;
                case "D8SpentBox":
                    hitDice.SpentD8 = DisplayUtility.IncrementUsedSlots(hitDice.SpentD8, hitDice.TotalD8);
                    DisplayUtility.SetCursor(hitDice.SpentD8, hitDice.TotalD8, (x, y) => x == y, Cursors.Arrow);
                    break;
                case "D10SpentBox":
                    hitDice.SpentD10 = DisplayUtility.IncrementUsedSlots(hitDice.SpentD10, hitDice.TotalD10);
                    DisplayUtility.SetCursor(hitDice.SpentD10, hitDice.TotalD10, (x, y) => x == y, Cursors.Arrow);
                    break;
                case "D12SpentBox":
                    hitDice.SpentD12 = DisplayUtility.IncrementUsedSlots(hitDice.SpentD12, hitDice.TotalD12);
                    DisplayUtility.SetCursor(hitDice.SpentD12, hitDice.TotalD12, (x, y) => x == y, Cursors.Arrow);
                    break;
            }

            Program.UndoRedoService.AddCommand(new EditCommand<HitDice>(hitDice, oldItem, this.ConciergePage));
            Program.Modify();

            this.DrawHitDice();
        }

        private void SpentBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is not Grid grid)
            {
                return;
            }

            var hitDice = Program.CcsFile.Character.Companion.Vitality.HitDice;
            this.CurrentHitDiceBox = grid.Name;
            switch (grid.Name)
            {
                case "D6SpentBox":
                    DisplayUtility.SetCursor(hitDice.SpentD6, hitDice.TotalD6, (x, y) => x != y, Cursors.Hand);
                    DisplayUtility.SetBorderColour(hitDice.SpentD6, hitDice.TotalD6, grid, this.D6Border, this.CurrentHitDiceBox);
                    break;
                case "D8SpentBox":
                    DisplayUtility.SetCursor(hitDice.SpentD8, hitDice.TotalD8, (x, y) => x != y, Cursors.Hand);
                    DisplayUtility.SetBorderColour(hitDice.SpentD8, hitDice.TotalD8, grid, this.D8Border, this.CurrentHitDiceBox);
                    break;
                case "D10SpentBox":
                    DisplayUtility.SetCursor(hitDice.SpentD10, hitDice.TotalD10, (x, y) => x != y, Cursors.Hand);
                    DisplayUtility.SetBorderColour(hitDice.SpentD10, hitDice.TotalD10, grid, this.D10Border, this.CurrentHitDiceBox);
                    break;
                case "D12SpentBox":
                    DisplayUtility.SetCursor(hitDice.SpentD12, hitDice.TotalD12, (x, y) => x != y, Cursors.Hand);
                    DisplayUtility.SetBorderColour(hitDice.SpentD12, hitDice.TotalD12, grid, this.D12Border, this.CurrentHitDiceBox);
                    break;
            }
        }

        private void SpentBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is not Grid grid)
            {
                return;
            }

            switch (grid.Name)
            {
                case "D6SpentBox":
                    this.D6Border.BorderBrush = grid.Background;
                    break;
                case "D8SpentBox":
                    this.D8Border.BorderBrush = grid.Background;
                    break;
                case "D10SpentBox":
                    this.D10Border.BorderBrush = grid.Background;
                    break;
                case "D12SpentBox":
                    this.D12Border.BorderBrush = grid.Background;
                    break;
            }

            this.CurrentHitDiceBox = string.Empty;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case "ModifyAttributesWindow":
                    this.DrawAttributes();
                    break;
                case "ModifyHealthWindow":
                    this.DrawHealth();
                    break;
                case "ModifyHitDiceWindow":
                    this.DrawHitDice();
                    break;
                case "ModifyWeaponWindow":
                    this.DrawAttacks();
                    break;
                case "ModifyPropertiesWindow":
                    this.DrawDetails();
                    break;
            }
        }
    }
}
