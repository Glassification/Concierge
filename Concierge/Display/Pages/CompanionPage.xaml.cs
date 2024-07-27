// <copyright file="CompanionPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Companions;
    using Concierge.Commands;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows;
    using Concierge.Persistence;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for CompanionPage.xaml.
    /// </summary>
    public partial class CompanionPage : ConciergePage
    {
        private readonly ImageEncoding imageEncoding = new (Program.ErrorService);

        public CompanionPage()
        {
            this.InitializeComponent();

            this.HasEditableDataGrid = true;
            this.ConciergePages = ConciergePages.Companion;
            this.HealthDisplay.InitializeDisplay();
        }

        private List<CompanionWeapon> DisplayList => Program.CcsFile.Character.Companion.Weapons.Filter(this.SearchFilter.FilterText).ToList();

        public override void Draw(bool isNewCharacterSheet = false)
        {
            this.DrawAttributes();
            this.DrawDetails();
            this.DrawHealth();
            this.DrawHitDice();
            this.DrawAttacks();
            this.DrawImage();
        }

        public override void Edit(object itemToEdit)
        {
            if (itemToEdit is not CompanionWeapon weapon)
            {
                return;
            }

            var index = this.WeaponDataGrid.SelectedIndex;
            WindowService.ShowEdit(
                weapon,
                typeof(CompanionAttacksWindow),
                this.Window_ApplyChanges,
                ConciergePages.Companion);
            this.DrawAttacks();
            this.WeaponDataGrid.SetSelectedIndex(index);
        }

        public void DrawDetails()
        {
            var properties = Program.CcsFile.Character.Companion.Properties;

            this.NameField.Text = properties.Name;
            this.PerceptionField.Text = properties.Perception.ToString();
            this.VisionField.Text = properties.Vision.PascalCase();
            this.MovementField.Text = properties.Movement.ToString();
            this.ArmorClassField.Text = properties.ArmorClass.ToString();
            this.CreatureSizeField.Text = properties.CreatureSize.ToString();
            this.InitiativeField.Text = properties.Initiative.ToString();
        }

        public void DrawAttributes()
        {
            var attributes = Program.CcsFile.Character.Companion.Attributes;

            this.StrengthAttributeDisplay.Bonus = ConciergeMath.Bonus(attributes.Strength);
            this.DexterityAttributeDisplay.Bonus = ConciergeMath.Bonus(attributes.Dexterity);
            this.ConstitutionAttributeDisplay.Bonus = ConciergeMath.Bonus(attributes.Constitution);
            this.IntelligenceAttributeDisplay.Bonus = ConciergeMath.Bonus(attributes.Intelligence);
            this.WisdomAttributeDisplay.Bonus = ConciergeMath.Bonus(attributes.Wisdom);
            this.CharismaAttributeDisplay.Bonus = ConciergeMath.Bonus(attributes.Charisma);

            this.StrengthAttributeDisplay.Score = attributes.Strength;
            this.DexterityAttributeDisplay.Score = attributes.Dexterity;
            this.ConstitutionAttributeDisplay.Score = attributes.Constitution;
            this.IntelligenceAttributeDisplay.Score = attributes.Intelligence;
            this.WisdomAttributeDisplay.Score = attributes.Wisdom;
            this.CharismaAttributeDisplay.Score = attributes.Charisma;
        }

        public void DrawHealth()
        {
            this.HealthDisplay.Draw(Program.CcsFile.Character.Companion.Health);
        }

        public void DrawImage()
        {
            var image = Program.CcsFile.Character.Companion.CompanionImage;
            this.CompanionImage.Source = image.UseCustomImage ? this.imageEncoding.Decode(image.Encoded) : null;
            this.CompanionImage.Stretch = image.Stretch;

            this.DefaultCompanionImage.Visibility = this.CompanionImage.Source is null ? Visibility.Visible : Visibility.Hidden;
        }

        public void DrawHitDice()
        {
            this.HitDiceDisplay.DrawHitDice(Program.CcsFile.Character.Companion.HitDice);
        }

        public void DrawAttacks()
        {
            this.WeaponDataGrid.Items.Clear();
            this.DisplayList.ForEach(weapon => this.WeaponDataGrid.Items.Add(weapon));
            this.SetWeaponDataGridControlState();
        }

        private void SetWeaponDataGridControlState()
        {
            this.WeaponDataGrid.SetButtonControlsEnableState(
                this.AttacksUpButton,
                this.AttacksDownButton,
                this.AttacksEditButton,
                this.AttacksDeleteButton);
        }

        private void WeaponDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            this.WeaponDataGrid.SortListFromDataGrid(Program.CcsFile.Character.Companion.Weapons, this.ConciergePages);
        }

        private void AttacksUpButton_Click(object sender, RoutedEventArgs e)
        {
            var index = this.WeaponDataGrid.NextItem(Program.CcsFile.Character.Companion.Weapons, 0, -1, this.ConciergePages);

            if (index != -1)
            {
                this.DrawAttacks();
                this.WeaponDataGrid.SetSelectedIndex(this.WeaponDataGrid.SelectedIndex);
            }
        }

        private void AttacksDownButton_Click(object sender, RoutedEventArgs e)
        {
            var index = this.WeaponDataGrid.NextItem(Program.CcsFile.Character.Equipment.Weapons, Program.CcsFile.Character.Companion.Weapons.Count - 1, 1, this.ConciergePages);

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
            var added = WindowService.ShowAdd(
                Program.CcsFile.Character.Companion.Weapons,
                typeof(CompanionAttacksWindow),
                this.Window_ApplyChanges,
                ConciergePages.Companion);
            this.DrawAttacks();

            if (added)
            {
                this.WeaponDataGrid.SetSelectedIndex(this.WeaponDataGrid.LastIndex);
            }
        }

        private void AttacksEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WeaponDataGrid.SelectedItem is not null)
            {
                this.Edit(this.WeaponDataGrid.SelectedItem);
            }
        }

        private void AttacksDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WeaponDataGrid.SelectedItem is not CompanionWeapon weapon)
            {
                return;
            }

            var index = this.WeaponDataGrid.SelectedIndex;
            Program.UndoRedoService.AddCommand(new DeleteCommand<CompanionWeapon>(Program.CcsFile.Character.Companion.Weapons, weapon, index, this.ConciergePages));
            Program.CcsFile.Character.Companion.Weapons.Remove(weapon);
            this.DrawAttacks();
            this.WeaponDataGrid.SetSelectedIndex(index);
        }

        private void TakeDamageButton_Click(object sender, RoutedEventArgs e)
        {
            WindowService.ShowDamage(
                Program.CcsFile.Character.Companion.Health,
                typeof(HpWindow),
                this.Window_ApplyChanges,
                ConciergePages.Companion);
            this.DrawHealth();
        }

        private void HealDamageButton_Click(object sender, RoutedEventArgs e)
        {
            WindowService.ShowHeal(
                Program.CcsFile.Character.Companion.Health,
                typeof(HpWindow),
                this.Window_ApplyChanges,
                ConciergePages.Companion);
            this.DrawHealth();
        }

        private void HitDiceDisplay_ValueChanged(object sender, RoutedEventArgs e)
        {
            this.DrawHitDice();
            this.DrawHealth();
        }

        private void AttackDataGrid_Filtered(object sender, RoutedEventArgs e)
        {
            this.SearchFilter.SetButtonEnableState(this.AttacksUpButton);
            this.SearchFilter.SetButtonEnableState(this.AttacksDownButton);

            this.DrawAttacks();
        }

        private void HealthDisplay_EditClicked(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();

            WindowService.ShowEdit(
                Program.CcsFile.Character.Companion.Health,
                typeof(HealthWindow),
                this.Window_ApplyChanges,
                ConciergePages.Companion);
            this.DrawHealth();
        }

        private void HitDiceDisplay_EditClicked(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();

            WindowService.ShowEdit(
                Program.CcsFile.Character.Companion.HitDice,
                typeof(HitDiceWindow),
                this.Window_ApplyChanges,
                ConciergePages.Companion);
            this.DrawHealth();
            this.DrawHitDice();
        }

        private void AttributeDisplay_EditClicked(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();

            WindowService.ShowEdit(
                Program.CcsFile.Character.Companion.Attributes,
                typeof(CompanionAttributesWindow),
                this.Window_ApplyChanges,
                ConciergePages.Companion);
            this.DrawAttributes();
            this.DrawAttacks();
        }

        private void CompanionDetails_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SoundService.PlayNavigation();

            WindowService.ShowEdit(
                Program.CcsFile.Character.Companion.Properties,
                typeof(CompanionWindow),
                this.Window_ApplyChanges,
                ConciergePages.Companion);
            this.DrawDetails();
        }

        private void CompanionImage_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SoundService.PlayNavigation();

            WindowService.ShowEdit(
                Program.CcsFile.Character.Companion.CompanionImage,
                typeof(ImageWindow),
                this.Window_ApplyChanges,
                ConciergePages.Companion);
            this.DrawImage();
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(CompanionAttributesWindow):
                    this.DrawAttributes();
                    this.DrawAttacks();
                    break;
                case nameof(HealthWindow):
                    this.DrawHealth();
                    break;
                case nameof(HitDiceWindow):
                    this.DrawHitDice();
                    break;
                case nameof(CompanionAttacksWindow):
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

        private void WeaponDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetWeaponDataGridControlState();
        }
    }
}
