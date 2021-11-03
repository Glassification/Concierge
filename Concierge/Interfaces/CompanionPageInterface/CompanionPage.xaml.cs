// <copyright file="CompanionPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.CompanionPageInterface
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Character.Items;
    using Concierge.Commands;
    using Concierge.Interfaces.AttackDefensePageInterface;
    using Concierge.Interfaces.Enums;
    using Concierge.Interfaces.OverviewPageInterface;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ToolsPage.xaml.
    /// </summary>
    public partial class CompanionPage : Page, IConciergePage
    {
        private readonly ModifyAttributesWindow modifyAttributesWindow = new ();
        private readonly ModifyHealthWindow modifyHealthWindow = new ();
        private readonly ModifyHpWindow modifyHpWindow = new ();
        private readonly ModifyHitDiceWindow modifyHitDiceWindow = new ();
        private readonly ModifyAttackWindow modifyAttackWindow = new ();
        private readonly ModifyPropertiesWindow modifyPropertiesWindow = new ();

        public CompanionPage()
        {
            this.InitializeComponent();

            this.modifyAttributesWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyHealthWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyHitDiceWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyAttackWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyPropertiesWindow.ApplyChanges += this.Window_ApplyChanges;

            this.CurrentHitDiceBox = string.Empty;
        }

        public ConciergePage ConciergePage => ConciergePage.Companion;

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
            if (itemToEdit is not Weapon)
            {
                return;
            }

            var index = this.WeaponDataGrid.SelectedIndex;
            this.modifyAttackWindow.ShowEdit(itemToEdit as Weapon);
            this.DrawAttacks();
            this.WeaponDataGrid.SetSelectedIndex(index);
        }

        private static Brush SetHealthStyle()
        {
            int third = Program.CcsFile.Character.Companion.Vitality.MaxHealth / 3;
            int hp = Program.CcsFile.Character.Companion.Vitality.CurrentHealth;

            return hp < third && hp > 0
                ? Brushes.IndianRed
                : hp >= third * 2 ? Brushes.DarkGreen : hp <= 0 ? Brushes.DarkGray : Brushes.DarkOrange;
        }

        private void DrawSpentHitDice(TextBlock spentField, Grid spentBox, Border border, int spent, int total)
        {
            spentField.Text = spent.ToString();
            spentField.Foreground = Utilities.SetUsedTextStyle(total, spent);
            spentBox.Background = Utilities.SetUsedBoxStyle(total, spent);
            Utilities.SetBorderColour(spent, total, spentBox, border, this.CurrentHitDiceBox);
        }

        private void DrawTotalHitDice(TextBlock totalField, Grid totalBox, int spent, int total)
        {
            totalField.Text = total.ToString();
            totalField.Foreground = Utilities.SetTotalTextStyle(total, spent);
            totalBox.Background = Utilities.SetTotalBoxStyle(total, spent);
        }

        private void DrawAttributes()
        {
            var attributes = Program.CcsFile.Character.Companion.Attributes;

            this.StrengthBonusField.Text = Utilities.CalculateBonus(attributes.Strength).ToString();
            this.DexterityBonusField.Text = Utilities.CalculateBonus(attributes.Dexterity).ToString();
            this.ConstitutionBonusField.Text = Utilities.CalculateBonus(attributes.Constitution).ToString();
            this.IntelligenceBonusField.Text = Utilities.CalculateBonus(attributes.Intelligence).ToString();
            this.WisdomBonusField.Text = Utilities.CalculateBonus(attributes.Wisdom).ToString();
            this.CharismaBonusField.Text = Utilities.CalculateBonus(attributes.Charisma).ToString();

            this.StrengthScoreField.Text = attributes.Strength.ToString();
            this.DexterityScoreField.Text = attributes.Dexterity.ToString();
            this.ConstitutionScoreField.Text = attributes.Constitution.ToString();
            this.IntelligenceScoreField.Text = attributes.Intelligence.ToString();
            this.WisdomScoreField.Text = attributes.Wisdom.ToString();
            this.CharismaScoreField.Text = attributes.Charisma.ToString();
        }

        private void DrawDetails()
        {
            this.NameField.Text = Program.CcsFile.Character.Companion.Name;
            this.PerceptionField.Text = Program.CcsFile.Character.Companion.Perception.ToString();
            this.VisionField.Text = Program.CcsFile.Character.Companion.Vision.ToString();
            this.MovementField.Text = Program.CcsFile.Character.Companion.Movement.ToString();
            this.ArmorClassField.Text = Program.CcsFile.Character.Companion.ArmorClass.ToString();
        }

        private void DrawHealth()
        {
            this.CurrentHpField.Text = Program.CcsFile.Character.Companion.Vitality.CurrentHealth.ToString();
            this.TotalHpField.Text = "/" + Program.CcsFile.Character.Companion.Vitality.MaxHealth.ToString();

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
            this.modifyAttributesWindow.EditAttributes(Program.CcsFile.Character.Companion.Attributes);
            this.DrawAttributes();
        }

        private void EditHitDiceButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifyHitDiceWindow.ShowEdit(Program.CcsFile.Character.Companion.Vitality.HitDice);
            this.DrawHitDice();
        }

        private void WeaponDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            Program.CcsFile.Character.Companion.Attacks.Clear();

            foreach (var weapon in this.WeaponDataGrid.Items)
            {
                Program.CcsFile.Character.Companion.Attacks.Add(weapon as Weapon);
            }
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            var index = this.WeaponDataGrid.NextItem(Program.CcsFile.Character.Companion.Attacks, 0, -1);

            if (index != -1)
            {
                this.DrawAttacks();
                this.WeaponDataGrid.SetSelectedIndex(this.WeaponDataGrid.SelectedIndex);
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            var index = this.WeaponDataGrid.NextItem(Program.CcsFile.Character.Companion.Attacks, Program.CcsFile.Character.Companion.Attacks.Count - 1, 1);

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
            this.modifyAttackWindow.ShowAdd(Program.CcsFile.Character.Companion.Attacks);
            this.DrawAttacks();

            if (this.modifyAttackWindow.ItemsAdded)
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
            if (this.WeaponDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var weapon = this.WeaponDataGrid.SelectedItem as Weapon;
                var index = this.WeaponDataGrid.SelectedIndex;

                Program.UndoRedoService.AddCommand(new DeleteCommand<Weapon>(Program.CcsFile.Character.Companion.Attacks, weapon, index));
                Program.CcsFile.Character.Companion.Attacks.Remove(weapon);
                this.DrawAttacks();
                this.WeaponDataGrid.SetSelectedIndex(index);
            }
        }

        private void EditHealthButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifyHealthWindow.EditHealth(Program.CcsFile.Character.Companion.Vitality);
            this.DrawHealth();
        }

        private void TakeDamageButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifyHpWindow.ShowDamage(Program.CcsFile.Character.Companion.Vitality);
            this.DrawHealth();
        }

        private void HealDamageButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifyHpWindow.ShowHeal(Program.CcsFile.Character.Companion.Vitality);
            this.DrawHealth();
        }

        private void EditPropertiesButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifyPropertiesWindow.ShowEdit();
            this.DrawDetails();
        }

        private void SpentBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2)
            {
                return;
            }

            var hitDice = Program.CcsFile.Character.Companion.Vitality.HitDice;
            switch ((sender as Grid).Name)
            {
                case "D6SpentBox":
                    hitDice.SpentD6 = Utilities.IncrementUsedSlots(hitDice.SpentD6, hitDice.TotalD6);
                    Utilities.SetCursor(hitDice.SpentD6, hitDice.TotalD6, (x, y) => x == y, Cursors.Arrow);
                    break;
                case "D8SpentBox":
                    hitDice.SpentD8 = Utilities.IncrementUsedSlots(hitDice.SpentD8, hitDice.TotalD8);
                    Utilities.SetCursor(hitDice.SpentD8, hitDice.TotalD8, (x, y) => x == y, Cursors.Arrow);
                    break;
                case "D10SpentBox":
                    hitDice.SpentD10 = Utilities.IncrementUsedSlots(hitDice.SpentD10, hitDice.TotalD10);
                    Utilities.SetCursor(hitDice.SpentD10, hitDice.TotalD10, (x, y) => x == y, Cursors.Arrow);
                    break;
                case "D12SpentBox":
                    hitDice.SpentD12 = Utilities.IncrementUsedSlots(hitDice.SpentD12, hitDice.TotalD12);
                    Utilities.SetCursor(hitDice.SpentD12, hitDice.TotalD12, (x, y) => x == y, Cursors.Arrow);
                    break;
            }

            this.DrawHitDice();
        }

        private void SpentBox_MouseEnter(object sender, MouseEventArgs e)
        {
            var grid = sender as Grid;
            var hitDice = Program.CcsFile.Character.Companion.Vitality.HitDice;

            this.CurrentHitDiceBox = grid.Name;

            switch (grid.Name)
            {
                case "D6SpentBox":
                    Utilities.SetCursor(hitDice.SpentD6, hitDice.TotalD6, (x, y) => x != y, Cursors.Hand);
                    Utilities.SetBorderColour(hitDice.SpentD6, hitDice.TotalD6, grid, this.D6Border, this.CurrentHitDiceBox);
                    break;
                case "D8SpentBox":
                    Utilities.SetCursor(hitDice.SpentD8, hitDice.TotalD8, (x, y) => x != y, Cursors.Hand);
                    Utilities.SetBorderColour(hitDice.SpentD8, hitDice.TotalD8, grid, this.D8Border, this.CurrentHitDiceBox);
                    break;
                case "D10SpentBox":
                    Utilities.SetCursor(hitDice.SpentD10, hitDice.TotalD10, (x, y) => x != y, Cursors.Hand);
                    Utilities.SetBorderColour(hitDice.SpentD10, hitDice.TotalD10, grid, this.D10Border, this.CurrentHitDiceBox);
                    break;
                case "D12SpentBox":
                    Utilities.SetCursor(hitDice.SpentD12, hitDice.TotalD12, (x, y) => x != y, Cursors.Hand);
                    Utilities.SetBorderColour(hitDice.SpentD12, hitDice.TotalD12, grid, this.D12Border, this.CurrentHitDiceBox);
                    break;
            }
        }

        private void SpentBox_MouseLeave(object sender, MouseEventArgs e)
        {
            var grid = sender as Grid;

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
