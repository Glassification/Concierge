﻿// <copyright file="CompanionPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Items;
    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows;
    using Concierge.Services;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for CompanionPage.xaml.
    /// </summary>
    public partial class CompanionPage : Page, Concierge.Display.IConciergePage
    {
        public CompanionPage()
        {
            this.InitializeComponent();
            this.HealthDisplay.InitializeDisplay();
        }

        public ConciergePage ConciergePage => ConciergePage.Companion;

        public bool HasEditableDataGrid => true;

        private List<Weapon> DisplayList => Program.CcsFile.Character.Companion.Attacks.Filter(this.SearchFilter.FilterText).ToList();

        public void Draw()
        {
            this.DrawAttributes();
            this.DrawDetails();
            this.DrawHealth();
            this.DrawHitDice();
            this.DrawAttacks();
            this.DrawImage();
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
                typeof(AttacksWindow),
                this.Window_ApplyChanges,
                ConciergePage.Companion);
            this.DrawAttacks();
            this.WeaponDataGrid.SetSelectedIndex(index);
        }

        public void DrawDetails()
        {
            var properties = Program.CcsFile.Character.Companion.Properties;

            this.NameField.Text = properties.Name;
            this.PerceptionField.Text = properties.Perception.ToString();
            this.VisionField.Text = properties.Vision.ToString();
            this.MovementField.Text = properties.Movement.ToString();
            this.ArmorClassField.Text = properties.ArmorClass.ToString();
            this.CreatureSizeField.Text = properties.CreatureSize.ToString();
            this.InitiativeField.Text = properties.Initiative.ToString();
        }

        public void DrawAttributes()
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

        public void DrawHealth()
        {
            var vitality = Program.CcsFile.Character.Companion.Vitality;

            this.HealthDisplay.CurrentHealth = vitality.CurrentHealth;
            this.HealthDisplay.TotalHealth = vitality.Health.MaxHealth;
            this.HealthDisplay.SetHealthStyle(vitality);
        }

        public void DrawImage()
        {
            this.CompanionImage.Source = Program.CcsFile.Character.Companion.CompanionImage.ToImage();
            this.CompanionImage.Stretch = Program.CcsFile.Character.Companion.CompanionImage.Stretch;

            this.DefaultCompanionImage.Visibility = this.CompanionImage.Source == null ? Visibility.Visible : Visibility.Hidden;
        }

        public void DrawHitDice()
        {
            this.HitDiceDisplay.DrawHitDice(Program.CcsFile.Character.Companion.Vitality.HitDice);
        }

        public void DrawAttacks()
        {
            this.WeaponDataGrid.Items.Clear();

            foreach (var weapon in this.DisplayList)
            {
                this.WeaponDataGrid.Items.Add(weapon);
            }
        }

        private void WeaponDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SortListFromDataGrid(this.WeaponDataGrid, Program.CcsFile.Character.Companion.Attacks, this.ConciergePage);
        }

        private void AttacksUpButton_Click(object sender, RoutedEventArgs e)
        {
            var index = this.WeaponDataGrid.NextItem(Program.CcsFile.Character.Companion.Attacks, 0, -1, this.ConciergePage);

            if (index != -1)
            {
                this.DrawAttacks();
                this.WeaponDataGrid.SetSelectedIndex(this.WeaponDataGrid.SelectedIndex);
            }
        }

        private void AttacksDownButton_Click(object sender, RoutedEventArgs e)
        {
            var index = this.WeaponDataGrid.NextItem(Program.CcsFile.Character.Companion.Attacks, Program.CcsFile.Character.Companion.Attacks.Count - 1, 1, this.ConciergePage);

            if (index != -1)
            {
                this.DrawAttacks();
                this.WeaponDataGrid.SetSelectedIndex(this.WeaponDataGrid.SelectedIndex);
            }
        }

        private void AttacksClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.WeaponDataGrid.UnselectAll();
        }

        private void AttacksAddButton_Click(object sender, RoutedEventArgs e)
        {
            var added = ConciergeWindowService.ShowAdd<List<Weapon>>(
                Program.CcsFile.Character.Companion.Attacks,
                typeof(AttacksWindow),
                this.Window_ApplyChanges,
                ConciergePage.Companion);
            this.DrawAttacks();

            if (added)
            {
                this.WeaponDataGrid.SetSelectedIndex(this.WeaponDataGrid.LastIndex);
            }
        }

        private void AttacksEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WeaponDataGrid.SelectedItem != null)
            {
                this.Edit(this.WeaponDataGrid.SelectedItem);
            }
        }

        private void AttacksDeleteButton_Click(object sender, RoutedEventArgs e)
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

        private void TakeDamageButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowDamage<Vitality>(
                Program.CcsFile.Character.Companion.Vitality,
                typeof(HpWindow),
                this.Window_ApplyChanges,
                ConciergePage.Companion);
            this.DrawHealth();
        }

        private void HealDamageButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowHeal<Vitality>(
                Program.CcsFile.Character.Companion.Vitality,
                typeof(HpWindow),
                this.Window_ApplyChanges,
                ConciergePage.Companion);
            this.DrawHealth();
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(AttributesWindow):
                    this.DrawAttributes();
                    break;
                case nameof(HealthWindow):
                    this.DrawHealth();
                    break;
                case nameof(HitDiceWindow):
                    this.DrawHitDice();
                    break;
                case nameof(AttacksWindow):
                    this.DrawAttacks();
                    break;
                case nameof(CompanionWindow):
                    this.DrawDetails();
                    break;
                case nameof(ImageWindow):
                    this.DrawImage();
                    break;
            }
        }

        private void HitDiceDisplay_ValueChanged(object sender, RoutedEventArgs e)
        {
            this.DrawHitDice();
        }

        private void AttackDataGrid_Filtered(object sender, RoutedEventArgs e)
        {
            this.SearchFilter.SetButtonEnableState(this.AttacksUpButton);
            this.SearchFilter.SetButtonEnableState(this.AttacksDownButton);

            this.DrawAttacks();
        }
    }
}