﻿// <copyright file="OverviewPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Pages
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Concierge.Character.AbilitySavingThrows;
    using Concierge.Character.AbilitySkills;
    using Concierge.Character.Characteristics;
    using Concierge.Character.Statuses;
    using Concierge.Configuration;
    using Concierge.Display;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows;
    using Concierge.Services;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for OverviewPage.xaml.
    /// </summary>
    public partial class OverviewPage : Page, IConciergePage
    {
        public OverviewPage()
        {
            this.InitializeComponent();
        }

        public bool HasEditableDataGrid => false;

        public ConciergePage ConciergePage => ConciergePage.Overview;

        public void Draw()
        {
            this.DrawDetails();
            this.DrawAttributes();
            this.DrawSavingThrows();
            this.DrawSkills();
            this.DrawHealth();
            this.DrawArmorClass();
            this.DrawHitDice();
            this.DrawWealth();
            this.DrawWeight();
        }

        public void DrawDetails()
        {
            this.InitiativeLabel.Value = Program.CcsFile.Character.Initiative.ToString();
            this.PerceptionLabel.Value = Program.CcsFile.Character.PassivePerception.ToString();
            this.VisionLabel.Value = Program.CcsFile.Character.Senses.Vision.ToString().FormatFromEnum();
            this.MovementLabel.Value = Program.CcsFile.Character.Senses.Movement.ToString();
        }

        public void DrawAttributes()
        {
            var attributes = Program.CcsFile.Character.Attributes;

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

        public void DrawSavingThrows()
        {
            var savingThrow = Program.CcsFile.Character.SavingThrow;

            this.StrengthSavingThrow.SavingThrowBonus = savingThrow.Strength.Bonus.ToString();
            this.DexteritySavingThrow.SavingThrowBonus = savingThrow.Dexterity.Bonus.ToString();
            this.ConstitutionSavingThrow.SavingThrowBonus = savingThrow.Constitution.Bonus.ToString();
            this.IntelligenceSavingThrow.SavingThrowBonus = savingThrow.Intelligence.Bonus.ToString();
            this.WisdomSavingThrow.SavingThrowBonus = savingThrow.Wisdom.Bonus.ToString();
            this.CharismaSavingThrow.SavingThrowBonus = savingThrow.Charisma.Bonus.ToString();

            this.StrengthSavingThrow.SetStyle(savingThrow.Strength.Proficiency, savingThrow.Strength.StatusChecks);
            this.DexteritySavingThrow.SetStyle(savingThrow.Dexterity.Proficiency, savingThrow.Dexterity.StatusChecks);
            this.ConstitutionSavingThrow.SetStyle(savingThrow.Constitution.Proficiency, savingThrow.Constitution.StatusChecks);
            this.IntelligenceSavingThrow.SetStyle(savingThrow.Intelligence.Proficiency, savingThrow.Intelligence.StatusChecks);
            this.WisdomSavingThrow.SetStyle(savingThrow.Wisdom.Proficiency, savingThrow.Wisdom.StatusChecks);
            this.CharismaSavingThrow.SetStyle(savingThrow.Charisma.Proficiency, savingThrow.Charisma.StatusChecks);
        }

        public void DrawSkills()
        {
            var skill = Program.CcsFile.Character.Skill;

            this.AthleticsSkill.SkillBonus = skill.Athletics.Bonus.ToString();
            this.AcrobaticsSkill.SkillBonus = skill.Acrobatics.Bonus.ToString();
            this.SleightOfHandSkill.SkillBonus = skill.SleightOfHand.Bonus.ToString();
            this.StealthSkill.SkillBonus = skill.Stealth.Bonus.ToString();
            this.ArcanaSkill.SkillBonus = skill.Arcana.Bonus.ToString();
            this.HistorySkill.SkillBonus = skill.History.Bonus.ToString();
            this.InvestigationSkill.SkillBonus = skill.Investigation.Bonus.ToString();
            this.NatureSkill.SkillBonus = skill.Nature.Bonus.ToString();
            this.ReligionSkill.SkillBonus = skill.Religion.Bonus.ToString();
            this.AnimalHandlingSkill.SkillBonus = skill.AnimalHandling.Bonus.ToString();
            this.InsightSkill.SkillBonus = skill.Insight.Bonus.ToString();
            this.MedicineSkill.SkillBonus = skill.Medicine.Bonus.ToString();
            this.PerceptionSkill.SkillBonus = skill.Perception.Bonus.ToString();
            this.SurvivalSkill.SkillBonus = skill.Survival.Bonus.ToString();
            this.DeceptionSkill.SkillBonus = skill.Deception.Bonus.ToString();
            this.IntimidationSkill.SkillBonus = skill.Intimidation.Bonus.ToString();
            this.PerformanceSkill.SkillBonus = skill.Performance.Bonus.ToString();
            this.PersuasionSkill.SkillBonus = skill.Persuasion.Bonus.ToString();

            this.AthleticsSkill.SetStyle(skill.Athletics.Proficiency, skill.Athletics.Expertise, skill.Athletics.Checks);
            this.AcrobaticsSkill.SetStyle(skill.Acrobatics.Proficiency, skill.Acrobatics.Expertise, skill.Acrobatics.Checks);
            this.SleightOfHandSkill.SetStyle(skill.SleightOfHand.Proficiency, skill.SleightOfHand.Expertise, skill.SleightOfHand.Checks);
            this.StealthSkill.SetStyle(skill.Stealth.Proficiency, skill.Stealth.Expertise, skill.Stealth.Checks);
            this.ArcanaSkill.SetStyle(skill.Arcana.Proficiency, skill.Arcana.Expertise, skill.Arcana.Checks);
            this.HistorySkill.SetStyle(skill.History.Proficiency, skill.History.Expertise, skill.History.Checks);
            this.InvestigationSkill.SetStyle(skill.Investigation.Proficiency, skill.Investigation.Expertise, skill.Investigation.Checks);
            this.NatureSkill.SetStyle(skill.Nature.Proficiency, skill.Nature.Expertise, skill.Nature.Checks);
            this.ReligionSkill.SetStyle(skill.Religion.Proficiency, skill.Religion.Expertise, skill.Religion.Checks);
            this.AnimalHandlingSkill.SetStyle(skill.AnimalHandling.Proficiency, skill.AnimalHandling.Expertise, skill.AnimalHandling.Checks);
            this.InsightSkill.SetStyle(skill.Insight.Proficiency, skill.Insight.Expertise, skill.Insight.Checks);
            this.MedicineSkill.SetStyle(skill.Medicine.Proficiency, skill.Medicine.Expertise, skill.Medicine.Checks);
            this.PerceptionSkill.SetStyle(skill.Perception.Proficiency, skill.Perception.Expertise, skill.Perception.Checks);
            this.SurvivalSkill.SetStyle(skill.Survival.Proficiency, skill.Survival.Expertise, skill.Survival.Checks);
            this.DeceptionSkill.SetStyle(skill.Deception.Proficiency, skill.Deception.Expertise, skill.Deception.Checks);
            this.IntimidationSkill.SetStyle(skill.Intimidation.Proficiency, skill.Intimidation.Expertise, skill.Intimidation.Checks);
            this.PerformanceSkill.SetStyle(skill.Performance.Proficiency, skill.Performance.Expertise, skill.Performance.Checks);
            this.PersuasionSkill.SetStyle(skill.Persuasion.Proficiency, skill.Persuasion.Expertise, skill.Persuasion.Checks);
        }

        public void DrawHealth()
        {
            var vitality = Program.CcsFile.Character.Vitality;

            this.HealthDisplay.CurrentHealth = vitality.CurrentHealth;
            this.HealthDisplay.TotalHealth = vitality.Health.MaxHealth;
            this.HealthDisplay.SetHealthStyle(vitality);
        }

        public void DrawArmorClass()
        {
            this.ArmorClassField.Text = Program.CcsFile.Character.Armor.TotalArmorClass.ToString();
        }

        public void DrawHitDice()
        {
            this.HitDiceDisplay.DrawHitDice(Program.CcsFile.Character.Vitality.HitDice);
        }

        public void DrawWealth()
        {
            this.WealthDisplay.SetWealth(Program.CcsFile.Character.Wealth);
        }

        public void DrawWeight()
        {
            this.WeightDisplay.SetWeightValues(Program.CcsFile.Character, AppSettingsManager.UserSettings.UnitOfMeasurement);
            this.WeightDisplay.FormatCarryWeight(Program.CcsFile.Character);
        }

        public void Edit(object itemToEdit)
        {
            throw new NotImplementedException();
        }

        private void DrawDeathSavingThrows()
        {
            var deathSaves = Program.CcsFile.Character.Vitality.DeathSavingThrows;
            deathSaves.LazyInitialize();

            this.HealthDisplay.SetDeathSaveStyle(deathSaves);
        }

        private void TakeDamageButton_Click(object sender, RoutedEventArgs e)
        {
            var result = ConciergeWindowService.ShowDamage<Vitality>(
                Program.CcsFile.Character.Vitality,
                typeof(HpWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawHealth();
        }

        private void HealDamageButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowHeal<Vitality>(
                Program.CcsFile.Character.Vitality,
                typeof(HpWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawHealth();
        }

        private void SavingThrow_ToggleClicked(object sender, RoutedEventArgs e)
        {
            this.DrawSavingThrows();
        }

        private void Skill_ToggleClicked(object sender, RoutedEventArgs e)
        {
            this.DrawDetails();
            this.DrawSkills();
        }

        private void HitDiceDisplay_ValueChanged(object sender, RoutedEventArgs e)
        {
            this.DrawHitDice();
        }

        private void HealthDisplay_SaveClicked(object sender, RoutedEventArgs e)
        {
            this.DrawDeathSavingThrows();
        }

        private void EditHealth_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<Health>(
                Program.CcsFile.Character.Vitality.Health,
                typeof(HealthWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawHealth();
        }

        private void HitDiceDisplay_EditClicked(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<HitDice>(
                Program.CcsFile.Character.Vitality.HitDice,
                typeof(HitDiceWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawHitDice();
        }

        private void WealthDisplay_EditClicked(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<Wealth>(
                Program.CcsFile.Character.Wealth,
                typeof(WealthWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawWealth();
        }

        private void Grid_SavingThrowMouseUp(object sender, MouseButtonEventArgs e)
        {
            ConciergeWindowService.ShowEdit<SavingThrow>(
                Program.CcsFile.Character.SavingThrow,
                typeof(SavingThrowWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawSavingThrows();
        }

        private void Grid_SkillMouseUp(object sender, MouseButtonEventArgs e)
        {
            ConciergeWindowService.ShowEdit<Skill>(
                Program.CcsFile.Character.Skill,
                typeof(SkillWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawSkills();
        }

        private void Label_EditClicked(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<Senses>(
                Program.CcsFile.Character.Senses,
                typeof(SensesWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawDetails();
        }

        private void AttributeDisplay_EditClicked(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<Attributes>(
                Program.CcsFile.Character.Attributes,
                typeof(AttributesWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawAttributes();
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(AttributesWindow):
                    this.DrawAttributes();
                    break;
                case nameof(SensesWindow):
                    this.DrawDetails();
                    break;
                case nameof(HealthWindow):
                    this.DrawHealth();
                    break;
                case nameof(HitDiceWindow):
                    this.DrawHitDice();
                    break;
                case nameof(SkillWindow):
                    this.DrawSkills();
                    break;
                case nameof(SavingThrowWindow):
                    this.DrawSavingThrows();
                    break;
            }
        }
    }
}