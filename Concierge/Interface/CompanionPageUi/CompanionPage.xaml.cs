// <copyright file="CompanionPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.CompanionPageUi
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Characters.Items;
    using Concierge.Interface.EquipmentPageUi;
    using Concierge.Interface.OverviewPageUi;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for ToolsPage.xaml.
    /// </summary>
    public partial class CompanionPage : Page
    {
        private readonly ModifyAttributesWindow modifyAttributesWindow = new ModifyAttributesWindow();
        private readonly ModifyHealthWindow modifyHealthWindow = new ModifyHealthWindow();
        private readonly ModifyHpWindow modifyHpWindow = new ModifyHpWindow();
        private readonly ModifyHitDiceWindow modifyHitDiceWindow = new ModifyHitDiceWindow();
        private readonly ModifyWeaponWindow modifyWeaponWindow = new ModifyWeaponWindow();
        private readonly ModifyPropertiesWindow modifyPropertiesWindow = new ModifyPropertiesWindow();

        public CompanionPage()
        {
            this.InitializeComponent();

            this.modifyAttributesWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyHealthWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyHitDiceWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyWeaponWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyPropertiesWindow.ApplyChanges += this.Window_ApplyChanges;
        }

        public int HeartWidth => (int)this.HealthBox.RenderSize.Width;

        public int HeartHeight => (int)this.HealthBox.RenderSize.Height;

        public int ShieldWidth => (int)this.ArmorClassBox.RenderSize.Width;

        public int ShieldHeight => (int)this.ArmorClassBox.RenderSize.Height;

        public void Draw()
        {
            this.DrawAttributes();
            this.DrawDetails();
            this.DrawHealth();
            this.DrawHitDice();
            this.DrawAttacks();
        }

        private static void SetCursor(int spent, int total, Func<int, int, bool> func, Cursor cursor)
        {
            if (func(spent, total))
            {
                Mouse.OverrideCursor = cursor;
            }
        }

        private static Brush SetHealthStyle()
        {
            int third = Program.CcsFile.Character.Companion.Vitality.MaxHealth / 3;
            int hp = Program.CcsFile.Character.Companion.Vitality.CurrentHealth;

            return hp < third && hp > 0
                ? Brushes.IndianRed
                : hp >= third * 2 ? Brushes.DarkGreen : hp <= 0 ? Brushes.DarkGray : Brushes.DarkOrange;
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

            this.CurrentHpField.Foreground = SetHealthStyle();
            this.TotalHpField.Foreground = SetHealthStyle();
        }

        private void DrawHitDice()
        {
            var hitDice = Program.CcsFile.Character.Companion.Vitality.HitDice;

            this.D6TotalField.Text = hitDice.TotalD6.ToString();
            this.D6TotalField.Foreground = Utilities.SetTotalTextStyle(hitDice.TotalD6, hitDice.SpentD6);
            this.D6TotalBox.Background = Utilities.SetTotalBoxStyle(hitDice.TotalD6, hitDice.SpentD6);
            this.D6SpentField.Text = hitDice.SpentD6.ToString();
            this.D6SpentField.Foreground = Utilities.SetUsedTextStyle(hitDice.TotalD6, hitDice.SpentD6);
            this.D6SpentBox.Background = Utilities.SetUsedBoxStyle(hitDice.TotalD6, hitDice.SpentD6);

            this.D8TotalField.Text = hitDice.TotalD8.ToString();
            this.D8TotalField.Foreground = Utilities.SetTotalTextStyle(hitDice.TotalD8, hitDice.SpentD8);
            this.D8SpentField.Text = hitDice.SpentD8.ToString();
            this.D8SpentField.Foreground = Utilities.SetUsedTextStyle(hitDice.TotalD8, hitDice.SpentD8);
            this.D8SpentBox.Background = Utilities.SetUsedBoxStyle(hitDice.TotalD8, hitDice.SpentD8);
            this.D8TotalBox.Background = Utilities.SetTotalBoxStyle(hitDice.TotalD8, hitDice.SpentD8);

            this.D10TotalField.Text = hitDice.TotalD10.ToString();
            this.D10TotalField.Foreground = Utilities.SetTotalTextStyle(hitDice.TotalD10, hitDice.SpentD10);
            this.D10SpentField.Text = hitDice.SpentD10.ToString();
            this.D10SpentField.Foreground = Utilities.SetUsedTextStyle(hitDice.TotalD10, hitDice.SpentD10);
            this.D10SpentBox.Background = Utilities.SetUsedBoxStyle(hitDice.TotalD10, hitDice.SpentD10);
            this.D10TotalBox.Background = Utilities.SetTotalBoxStyle(hitDice.TotalD10, hitDice.SpentD10);

            this.D12TotalField.Text = hitDice.TotalD12.ToString();
            this.D12TotalField.Foreground = Utilities.SetTotalTextStyle(hitDice.TotalD12, hitDice.SpentD12);
            this.D12SpentField.Text = hitDice.SpentD12.ToString();
            this.D12SpentField.Foreground = Utilities.SetUsedTextStyle(hitDice.TotalD12, hitDice.SpentD12);
            this.D12SpentBox.Background = Utilities.SetUsedBoxStyle(hitDice.TotalD12, hitDice.SpentD12);
            this.D12TotalBox.Background = Utilities.SetTotalBoxStyle(hitDice.TotalD12, hitDice.SpentD12);
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
            ConciergeSound.TapNavigation();
            this.modifyAttributesWindow.EditAttributes(Program.CcsFile.Character.Companion.Attributes);
            this.DrawAttributes();
        }

        private void EditHitDiceButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.modifyHitDiceWindow.ModifyHitDice(Program.CcsFile.Character.Companion.Vitality.HitDice);
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
            if (this.WeaponDataGrid.SelectedItem == null)
            {
                return;
            }

            Program.Modify();
            ConciergeSound.TapNavigation();

            var weapon = this.WeaponDataGrid.SelectedItem as Weapon;
            var index = Program.CcsFile.Character.Companion.Attacks.IndexOf(weapon);

            if (index != 0)
            {
                Utilities.Swap(Program.CcsFile.Character.Companion.Attacks, index, index - 1);
                this.DrawAttacks();
                this.WeaponDataGrid.SelectedIndex = index - 1;
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            if (this.WeaponDataGrid.SelectedItem == null)
            {
                return;
            }

            Program.Modify();
            ConciergeSound.TapNavigation();

            var weapon = this.WeaponDataGrid.SelectedItem as Weapon;
            var index = Program.CcsFile.Character.Companion.Attacks.IndexOf(weapon);

            if (index != Program.CcsFile.Character.Companion.Attacks.Count - 1)
            {
                Utilities.Swap(Program.CcsFile.Character.Companion.Attacks, index, index + 1);
                this.DrawAttacks();
                this.WeaponDataGrid.SelectedIndex = index + 1;
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.WeaponDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.modifyWeaponWindow.ShowAdd(Program.CcsFile.Character.Companion.Attacks);
            this.DrawAttacks();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.WeaponDataGrid.SelectedItem != null)
            {
                ConciergeSound.TapNavigation();
                this.modifyWeaponWindow.ShowEdit(this.WeaponDataGrid.SelectedItem as Weapon);
                this.DrawAttacks();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.WeaponDataGrid.SelectedItem != null)
            {
                Program.Modify();
                ConciergeSound.TapNavigation();

                var weapon = this.WeaponDataGrid.SelectedItem as Weapon;
                Program.CcsFile.Character.Companion.Attacks.Remove(weapon);
                this.DrawAttacks();
            }
        }

        private void EditHealthButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.modifyHealthWindow.EditHealth(Program.CcsFile.Character.Companion.Vitality);
            this.DrawHealth();
        }

        private void TakeDamageButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.modifyHpWindow.SubtractHP(Program.CcsFile.Character.Companion.Vitality);
            this.DrawHealth();
        }

        private void HealDamageButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.modifyHpWindow.AddHP(Program.CcsFile.Character.Companion.Vitality);
            this.DrawHealth();
        }

        private void EditPropertiesButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            this.modifyPropertiesWindow.EditProperties();
            this.DrawDetails();
        }

        private void SpentBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
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
            var hitDice = Program.CcsFile.Character.Companion.Vitality.HitDice;
            switch ((sender as Grid).Name)
            {
                case "D6SpentBox":
                    SetCursor(hitDice.SpentD6, hitDice.TotalD6, (x, y) => x != y, Cursors.Hand);
                    break;
                case "D8SpentBox":
                    SetCursor(hitDice.SpentD8, hitDice.TotalD8, (x, y) => x != y, Cursors.Hand);
                    break;
                case "D10SpentBox":
                    SetCursor(hitDice.SpentD10, hitDice.TotalD10, (x, y) => x != y, Cursors.Hand);
                    break;
                case "D12SpentBox":
                    SetCursor(hitDice.SpentD12, hitDice.TotalD12, (x, y) => x != y, Cursors.Hand);
                    break;
            }
        }

        private void SpentBox_MouseLeave(object sender, MouseEventArgs e)
        {
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
