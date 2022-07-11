// <copyright file="CompanionPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.CompanionPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Items;
    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Interfaces.AttackDefensePageInterface;
    using Concierge.Interfaces.Enums;
    using Concierge.Interfaces.EquippedItemsPageInterface;
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

            this.StrengthAttributeDisplay.InitializeFontSize();
            this.DexterityAttributeDisplay.InitializeFontSize();
            this.ConstitutionAttributeDisplay.InitializeFontSize();
            this.IntelligenceAttributeDisplay.InitializeFontSize();
            this.WisdomAttributeDisplay.InitializeFontSize();
            this.CharismaAttributeDisplay.InitializeFontSize();

            this.HealthDisplay.InitializeDisplay();
        }

        public ConciergePage ConciergePage => ConciergePage.Companion;

        public bool HasEditableDataGrid => true;

        public void Draw()
        {
            this.DrawAttributes();
            this.DrawDetails();
            this.DrawHealth();
            this.DrawHitDice();
            this.DrawAttacks();
            this.LoadImage();
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

        private void DrawAttributes()
        {
            var attributes = Program.CcsFile.Character.Companion.Attributes;

            this.StrengthAttributeDisplay.Bonus = CharacterUtility.CalculateBonus(attributes.Strength);
            this.DexterityAttributeDisplay.Bonus = CharacterUtility.CalculateBonus(attributes.Dexterity);
            this.ConstitutionAttributeDisplay.Bonus = CharacterUtility.CalculateBonus(attributes.Constitution);
            this.IntelligenceAttributeDisplay.Bonus = CharacterUtility.CalculateBonus(attributes.Intelligence);
            this.WisdomAttributeDisplay.Bonus = CharacterUtility.CalculateBonus(attributes.Wisdom);
            this.CharismaAttributeDisplay.Bonus = CharacterUtility.CalculateBonus(attributes.Charisma);

            this.StrengthAttributeDisplay.Score = attributes.Strength;
            this.DexterityAttributeDisplay.Score = attributes.Dexterity;
            this.ConstitutionAttributeDisplay.Score = attributes.Constitution;
            this.IntelligenceAttributeDisplay.Score = attributes.Intelligence;
            this.WisdomAttributeDisplay.Score = attributes.Wisdom;
            this.CharismaAttributeDisplay.Score = attributes.Charisma;
        }

        private void DrawDetails()
        {
            var properties = Program.CcsFile.Character.Companion.Properties;

            this.NameField.Text = properties.Name;
            this.PerceptionField.Text = properties.Perception.ToString();
            this.VisionField.Text = properties.Vision.ToString();
            this.MovementField.Text = properties.Movement.ToString();
            this.ArmorClassField.Text = properties.ArmorClass.ToString();
            this.CreatureSizeField.Text = properties.CreatureSize.ToString();
        }

        private void DrawHealth()
        {
            var vitality = Program.CcsFile.Character.Companion.Vitality;

            this.HealthDisplay.CurrentHealth = vitality.CurrentHealth;
            this.HealthDisplay.TotalHealth = vitality.Health.MaxHealth;
            this.HealthDisplay.SetHealthStyle(vitality);
        }

        private void DrawHitDice()
        {
            this.HitDiceDisplay.DrawSpentHitDice(Program.CcsFile.Character.Companion.Vitality.HitDice);
            this.HitDiceDisplay.DrawTotalHitDice(Program.CcsFile.Character.Companion.Vitality.HitDice);
        }

        private void DrawAttacks()
        {
            this.WeaponDataGrid.Items.Clear();

            foreach (var weapon in Program.CcsFile.Character.Companion.Attacks)
            {
                this.WeaponDataGrid.Items.Add(weapon);
            }
        }

        private void LoadImage()
        {
            this.CompanionImage.Source = Program.CcsFile.Character.Companion.CompanionImage.ToImage();
            this.CompanionImage.Stretch = Program.CcsFile.Character.Companion.CompanionImage.Stretch;

            this.DefaultCompanionImage.Visibility = this.CompanionImage.Source == null ? Visibility.Visible : Visibility.Hidden;
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

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(ModifyAttributesWindow):
                    this.DrawAttributes();
                    break;
                case nameof(ModifyHealthWindow):
                    this.DrawHealth();
                    break;
                case nameof(ModifyHitDiceWindow):
                    this.DrawHitDice();
                    break;
                case nameof(ModifyAttackWindow):
                    this.DrawAttacks();
                    break;
                case nameof(ModifyCompanionPropertiesWindow):
                    this.DrawDetails();
                    break;
                case nameof(ModifyCharacterImageWindow):
                    this.LoadImage();
                    break;
            }
        }

        private void HitDiceDisplay_ValueChanged(object sender, RoutedEventArgs e)
        {
            this.DrawHitDice();
        }

        private void ImageEditButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<CharacterImage>(
                Program.CcsFile.Character.Companion.CompanionImage,
                typeof(ModifyCharacterImageWindow),
                this.Window_ApplyChanges,
                ConciergePage.EquippedItems);
            this.LoadImage();
        }
    }
}
