// <copyright file="OverviewPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.OverviewPageInterface
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Shapes;

    using Concierge.Character;
    using Concierge.Character.AbilitySavingThrows;
    using Concierge.Character.AbilitySkills;
    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Interfaces.Enums;
    using Concierge.Services;
    using Concierge.Tools.Interface;
    using Concierge.Utility;
    using Concierge.Utility.Colors;
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

            this.InitializeToggleBox(this.StrengthProficiencyBox, this.SavingThrows_MouseDown);
            this.InitializeToggleBox(this.DexterityProficiencyBox, this.SavingThrows_MouseDown);
            this.InitializeToggleBox(this.ConstitutionProficiencyBox, this.SavingThrows_MouseDown);
            this.InitializeToggleBox(this.IntelligenceProficiencyBox, this.SavingThrows_MouseDown);
            this.InitializeToggleBox(this.WisdomProficiencyBox, this.SavingThrows_MouseDown);
            this.InitializeToggleBox(this.CharismaProficiencyBox, this.SavingThrows_MouseDown);

            this.InitializeToggleBox(this.AthleticsProficiencyBox, this.SkillProficiency_MouseDown);
            this.InitializeToggleBox(this.AcrobaticsProficiencyBox, this.SkillProficiency_MouseDown);
            this.InitializeToggleBox(this.SleightOfHandProficiencyBox, this.SkillProficiency_MouseDown);
            this.InitializeToggleBox(this.StealthProficiencyBox, this.SkillProficiency_MouseDown);
            this.InitializeToggleBox(this.ArcanaProficiencyBox, this.SkillProficiency_MouseDown);
            this.InitializeToggleBox(this.HistoryProficiencyBox, this.SkillProficiency_MouseDown);
            this.InitializeToggleBox(this.InvestigationProficiencyBox, this.SkillProficiency_MouseDown);
            this.InitializeToggleBox(this.NatureProficiencyBox, this.SkillProficiency_MouseDown);
            this.InitializeToggleBox(this.ReligionProficiencyBox, this.SkillProficiency_MouseDown);
            this.InitializeToggleBox(this.AnimalHandlingProficiencyBox, this.SkillProficiency_MouseDown);
            this.InitializeToggleBox(this.InsightProficiencyBox, this.SkillProficiency_MouseDown);
            this.InitializeToggleBox(this.MedicineProficiencyBox, this.SkillProficiency_MouseDown);
            this.InitializeToggleBox(this.PerceptionProficiencyBox, this.SkillProficiency_MouseDown);
            this.InitializeToggleBox(this.SurvivalProficiencyBox, this.SkillProficiency_MouseDown);
            this.InitializeToggleBox(this.DeceptionProficiencyBox, this.SkillProficiency_MouseDown);
            this.InitializeToggleBox(this.IntimidationProficiencyBox, this.SkillProficiency_MouseDown);
            this.InitializeToggleBox(this.PerformanceProficiencyBox, this.SkillProficiency_MouseDown);
            this.InitializeToggleBox(this.PersuasionProficiencyBox, this.SkillProficiency_MouseDown);

            this.InitializeToggleBox(this.AthleticsExpertieseBox, this.SkillExpertise_MouseDown);
            this.InitializeToggleBox(this.AcrobaticsExpertieseBox, this.SkillExpertise_MouseDown);
            this.InitializeToggleBox(this.SleightOfHandExpertieseBox, this.SkillExpertise_MouseDown);
            this.InitializeToggleBox(this.StealthExpertieseBox, this.SkillExpertise_MouseDown);
            this.InitializeToggleBox(this.ArcanaExpertieseBox, this.SkillExpertise_MouseDown);
            this.InitializeToggleBox(this.HistoryExpertieseBox, this.SkillExpertise_MouseDown);
            this.InitializeToggleBox(this.InvestigationExpertieseBox, this.SkillExpertise_MouseDown);
            this.InitializeToggleBox(this.NatureExpertieseBox, this.SkillExpertise_MouseDown);
            this.InitializeToggleBox(this.ReligionExpertieseBox, this.SkillExpertise_MouseDown);
            this.InitializeToggleBox(this.AnimalHandlingExpertieseBox, this.SkillExpertise_MouseDown);
            this.InitializeToggleBox(this.InsightExpertieseBox, this.SkillExpertise_MouseDown);
            this.InitializeToggleBox(this.MedicineExpertieseBox, this.SkillExpertise_MouseDown);
            this.InitializeToggleBox(this.PerceptionExpertieseBox, this.SkillExpertise_MouseDown);
            this.InitializeToggleBox(this.SurvivalExpertieseBox, this.SkillExpertise_MouseDown);
            this.InitializeToggleBox(this.DeceptionExpertieseBox, this.SkillExpertise_MouseDown);
            this.InitializeToggleBox(this.IntimidationExpertieseBox, this.SkillExpertise_MouseDown);
            this.InitializeToggleBox(this.PerformanceExpertieseBox, this.SkillExpertise_MouseDown);
            this.InitializeToggleBox(this.PersuasionExpertieseBox, this.SkillExpertise_MouseDown);

            this.DataContext = this;
            this.CurrentHitDiceBox = string.Empty;
        }

        public ConciergePage ConciergePage => ConciergePage.Overview;

        private bool DeathScreenShown { get; set; }

        private string CurrentHitDiceBox { get; set; }

        public void Draw()
        {
            this.DrawAttributes();
            this.DrawDetails();
            this.DrawSavingThrows();
            this.DrawSkills();
            this.DrawHealth();
            this.DrawArmorClass();
            this.DrawHitDice();
            this.DrawWealth();
            this.DrawDeathSavingThrows();
        }

        public void Edit(object itemToEdit)
        {
            throw new NotImplementedException();
        }

        private static void DisplayCharacterDeathWindow(ConciergeCharacter character)
        {
            ConciergeMessageBox.Show(
                    $"{(character.Properties.Name.IsNullOrWhiteSpace() ? "Your character" : character.Properties.Name)} has died.",
                    "Player Death",
                    ConciergeWindowButtons.Ok,
                    ConciergeWindowIcons.Alert);
        }

        private void DrawSpentHitDice(TextBlock spentField, Grid spentBox, Border border, int spent, int total)
        {
            spentField.Text = spent.ToString();
            spentField.Foreground = DisplayUtility.SetUsedTextStyle(total, spent);
            spentBox.Background = DisplayUtility.SetUsedBoxStyle(total, spent);
            DisplayUtility.SetBorderColour(spent, total, spentBox, border, this.CurrentHitDiceBox);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Consistency")]
        private void DrawTotalHitDice(TextBlock totalField, Grid totalBox, int spent, int total)
        {
            totalField.Text = total.ToString();
            totalField.Foreground = DisplayUtility.SetTotalTextStyle(total, spent);
            totalBox.Background = DisplayUtility.SetTotalBoxStyle(total, spent);
        }

        private void InitializeToggleBox(Rectangle toggleBox, MouseButtonEventHandler mouseButtonEventHandler)
        {
            toggleBox.MouseDown += mouseButtonEventHandler;
            toggleBox.MouseEnter += this.ToggleBox_MouseEnter;
            toggleBox.MouseLeave += this.ToggleBox_MouseLeave;
        }

        private void DrawAttributes()
        {
            var attributes = Program.CcsFile.Character.Attributes;

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
            this.InitiativeField.Text = Program.CcsFile.Character.Initiative.ToString();
            this.PassivePerceptionField.Text = Program.CcsFile.Character.PassivePerception.ToString();
            this.VisionField.Text = Program.CcsFile.Character.Senses.Vision.ToString();
            this.MovementSpeedField.Text = Program.CcsFile.Character.Senses.Movement.ToString();
        }

        private void DrawSavingThrows()
        {
            var savingThrow = Program.CcsFile.Character.SavingThrow;

            this.StrengthSavingThrowField.Text = savingThrow.Strength.Bonus.ToString();
            this.DexteritySavingThrowField.Text = savingThrow.Dexterity.Bonus.ToString();
            this.ConstitutionSavingThrowField.Text = savingThrow.Constitution.Bonus.ToString();
            this.IntelligenceSavingThrowField.Text = savingThrow.Intelligence.Bonus.ToString();
            this.WisdomSavingThrowField.Text = savingThrow.Wisdom.Bonus.ToString();
            this.CharismaSavingThrowField.Text = savingThrow.Charisma.Bonus.ToString();

            DisplayUtility.SetTextStyle(savingThrow.Strength.StatusChecks, this.StrengthSavingThrowField);
            DisplayUtility.SetTextStyle(savingThrow.Dexterity.StatusChecks, this.DexteritySavingThrowField);
            DisplayUtility.SetTextStyle(savingThrow.Constitution.StatusChecks, this.ConstitutionSavingThrowField);
            DisplayUtility.SetTextStyle(savingThrow.Intelligence.StatusChecks, this.IntelligenceSavingThrowField);
            DisplayUtility.SetTextStyle(savingThrow.Wisdom.StatusChecks, this.WisdomSavingThrowField);
            DisplayUtility.SetTextStyle(savingThrow.Charisma.StatusChecks, this.CharismaSavingThrowField);

            DisplayUtility.SetTextStyle(savingThrow.Strength.StatusChecks, this.StrengthSavingThrowName);
            DisplayUtility.SetTextStyle(savingThrow.Dexterity.StatusChecks, this.DexteritySavingThrowName);
            DisplayUtility.SetTextStyle(savingThrow.Constitution.StatusChecks, this.ConstitutionSavingThrowName);
            DisplayUtility.SetTextStyle(savingThrow.Intelligence.StatusChecks, this.IntelligenceSavingThrowName);
            DisplayUtility.SetTextStyle(savingThrow.Wisdom.StatusChecks, this.WisdomSavingThrowName);
            DisplayUtility.SetTextStyle(savingThrow.Charisma.StatusChecks, this.CharismaSavingThrowName);

            DisplayUtility.SetProficiencyBoxStyle(savingThrow.Strength.Proficiency, this.StrengthProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(savingThrow.Dexterity.Proficiency, this.DexterityProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(savingThrow.Constitution.Proficiency, this.ConstitutionProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(savingThrow.Intelligence.Proficiency, this.IntelligenceProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(savingThrow.Wisdom.Proficiency, this.WisdomProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(savingThrow.Charisma.Proficiency, this.CharismaProficiencyBox);
        }

        private void DrawSkills()
        {
            var skill = Program.CcsFile.Character.Skill;

            this.AthleticsSkillField.Text = skill.Athletics.Bonus.ToString();
            this.AcrobaticsSkillField.Text = skill.Acrobatics.Bonus.ToString();
            this.SleightOfHandSkillField.Text = skill.SleightOfHand.Bonus.ToString();
            this.StealthSkillField.Text = skill.Stealth.Bonus.ToString();
            this.ArcanaSkillField.Text = skill.Arcana.Bonus.ToString();
            this.HistorySkillField.Text = skill.History.Bonus.ToString();
            this.InvestigationSkillField.Text = skill.Investigation.Bonus.ToString();
            this.NatureSkillField.Text = skill.Nature.Bonus.ToString();
            this.ReligionSkillField.Text = skill.Religion.Bonus.ToString();
            this.AnimalHandlingSkillField.Text = skill.AnimalHandling.Bonus.ToString();
            this.InsightSkillField.Text = skill.Insight.Bonus.ToString();
            this.MedicineSkillField.Text = skill.Medicine.Bonus.ToString();
            this.PerceptionSkillField.Text = skill.Perception.Bonus.ToString();
            this.SurvivalSkillField.Text = skill.Survival.Bonus.ToString();
            this.DeceptionSkillField.Text = skill.Deception.Bonus.ToString();
            this.IntimidationSkillField.Text = skill.Intimidation.Bonus.ToString();
            this.PerformanceSkillField.Text = skill.Performance.Bonus.ToString();
            this.PersuasionSkillField.Text = skill.Persuasion.Bonus.ToString();

            DisplayUtility.SetTextStyle(skill.Athletics.Checks, this.AthleticsSkillField);
            DisplayUtility.SetTextStyle(skill.Acrobatics.Checks, this.AcrobaticsSkillField);
            DisplayUtility.SetTextStyle(skill.SleightOfHand.Checks, this.SleightOfHandSkillField);
            DisplayUtility.SetTextStyle(skill.Stealth.Checks, this.StealthSkillField);
            DisplayUtility.SetTextStyle(skill.Arcana.Checks, this.ArcanaSkillField);
            DisplayUtility.SetTextStyle(skill.History.Checks, this.HistorySkillField);
            DisplayUtility.SetTextStyle(skill.Investigation.Checks, this.InvestigationSkillField);
            DisplayUtility.SetTextStyle(skill.Nature.Checks, this.NatureSkillField);
            DisplayUtility.SetTextStyle(skill.Religion.Checks, this.ReligionSkillField);
            DisplayUtility.SetTextStyle(skill.AnimalHandling.Checks, this.AnimalHandlingSkillField);
            DisplayUtility.SetTextStyle(skill.Insight.Checks, this.InsightSkillField);
            DisplayUtility.SetTextStyle(skill.Medicine.Checks, this.MedicineSkillField);
            DisplayUtility.SetTextStyle(skill.Perception.Checks, this.PerceptionSkillField);
            DisplayUtility.SetTextStyle(skill.Survival.Checks, this.SurvivalSkillField);
            DisplayUtility.SetTextStyle(skill.Deception.Checks, this.DeceptionSkillField);
            DisplayUtility.SetTextStyle(skill.Intimidation.Checks, this.IntimidationSkillField);
            DisplayUtility.SetTextStyle(skill.Performance.Checks, this.PerformanceSkillField);
            DisplayUtility.SetTextStyle(skill.Persuasion.Checks, this.PersuasionSkillField);

            DisplayUtility.SetTextStyle(skill.Athletics.Checks, this.AthleticsSkillName);
            DisplayUtility.SetTextStyle(skill.Acrobatics.Checks, this.AcrobaticsSkillName);
            DisplayUtility.SetTextStyle(skill.SleightOfHand.Checks, this.SleightOfHandSkillName);
            DisplayUtility.SetTextStyle(skill.Stealth.Checks, this.StealthSkillName);
            DisplayUtility.SetTextStyle(skill.Arcana.Checks, this.ArcanaSkillName);
            DisplayUtility.SetTextStyle(skill.History.Checks, this.HistorySkillName);
            DisplayUtility.SetTextStyle(skill.Investigation.Checks, this.InvestigationSkillName);
            DisplayUtility.SetTextStyle(skill.Nature.Checks, this.NatureSkillName);
            DisplayUtility.SetTextStyle(skill.Religion.Checks, this.ReligionSkillName);
            DisplayUtility.SetTextStyle(skill.AnimalHandling.Checks, this.AnimalHandlingSkillName);
            DisplayUtility.SetTextStyle(skill.Insight.Checks, this.InsightSkillName);
            DisplayUtility.SetTextStyle(skill.Medicine.Checks, this.MedicineSkillName);
            DisplayUtility.SetTextStyle(skill.Perception.Checks, this.PerceptionSkillName);
            DisplayUtility.SetTextStyle(skill.Survival.Checks, this.SurvivalSkillName);
            DisplayUtility.SetTextStyle(skill.Deception.Checks, this.DeceptionSkillName);
            DisplayUtility.SetTextStyle(skill.Intimidation.Checks, this.IntimidationSkillName);
            DisplayUtility.SetTextStyle(skill.Performance.Checks, this.PerformanceSkillName);
            DisplayUtility.SetTextStyle(skill.Persuasion.Checks, this.PersuasionSkillName);

            DisplayUtility.SetProficiencyBoxStyle(skill.Athletics.Proficiency, this.AthleticsProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Acrobatics.Proficiency, this.AcrobaticsProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.SleightOfHand.Proficiency, this.SleightOfHandProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Stealth.Proficiency, this.StealthProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Arcana.Proficiency, this.ArcanaProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.History.Proficiency, this.HistoryProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Investigation.Proficiency, this.InvestigationProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Nature.Proficiency, this.NatureProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Religion.Proficiency, this.ReligionProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.AnimalHandling.Proficiency, this.AnimalHandlingProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Insight.Proficiency, this.InsightProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Medicine.Proficiency, this.MedicineProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Perception.Proficiency, this.PerceptionProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Survival.Proficiency, this.SurvivalProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Deception.Proficiency, this.DeceptionProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Intimidation.Proficiency, this.IntimidationProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Performance.Proficiency, this.PerformanceProficiencyBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Persuasion.Proficiency, this.PersuasionProficiencyBox);

            DisplayUtility.SetProficiencyBoxStyle(skill.Athletics.Expertise, this.AthleticsExpertieseBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Acrobatics.Expertise, this.AcrobaticsExpertieseBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.SleightOfHand.Expertise, this.SleightOfHandExpertieseBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Stealth.Expertise, this.StealthExpertieseBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Arcana.Expertise, this.ArcanaExpertieseBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.History.Expertise, this.HistoryExpertieseBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Investigation.Expertise, this.InvestigationExpertieseBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Nature.Expertise, this.NatureExpertieseBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Religion.Expertise, this.ReligionExpertieseBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.AnimalHandling.Expertise, this.AnimalHandlingExpertieseBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Insight.Expertise, this.InsightExpertieseBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Medicine.Expertise, this.MedicineExpertieseBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Perception.Expertise, this.PerceptionExpertieseBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Survival.Expertise, this.SurvivalExpertieseBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Deception.Expertise, this.DeceptionExpertieseBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Intimidation.Expertise, this.IntimidationExpertieseBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Performance.Expertise, this.PerformanceExpertieseBox);
            DisplayUtility.SetProficiencyBoxStyle(skill.Persuasion.Expertise, this.PersuasionExpertieseBox);
        }

        private void DrawHealth()
        {
            var vitality = Program.CcsFile.Character.Vitality;

            this.CurrentHpField.Text = vitality.CurrentHealth.ToString();
            this.TotalHpField.Text = "/" + vitality.Health.MaxHealth.ToString();

            this.HpBackground.Foreground = DisplayUtility.SetHealthStyle(vitality);
            this.TotalHpField.Foreground = DisplayUtility.SetHealthStyle(vitality);
        }

        private void DrawArmorClass()
        {
            this.ArmorClassField.Text = Program.CcsFile.Character.Armor.TotalArmorClass.ToString();
        }

        private void DrawHitDice()
        {
            var hitDice = Program.CcsFile.Character.Vitality.HitDice;

            this.DrawSpentHitDice(this.D6SpentField, this.D6SpentBox, this.D6Border, hitDice.SpentD6, hitDice.TotalD6);
            this.DrawTotalHitDice(this.D6TotalField, this.D6TotalBox, hitDice.SpentD6, hitDice.TotalD6);

            this.DrawSpentHitDice(this.D8SpentField, this.D8SpentBox, this.D8Border, hitDice.SpentD8, hitDice.TotalD8);
            this.DrawTotalHitDice(this.D8TotalField, this.D8TotalBox, hitDice.SpentD8, hitDice.TotalD8);

            this.DrawSpentHitDice(this.D10SpentField, this.D10SpentBox, this.D10Border, hitDice.SpentD10, hitDice.TotalD10);
            this.DrawTotalHitDice(this.D10TotalField, this.D10TotalBox, hitDice.SpentD10, hitDice.TotalD10);

            this.DrawSpentHitDice(this.D12SpentField, this.D12SpentBox, this.D12Border, hitDice.SpentD12, hitDice.TotalD12);
            this.DrawTotalHitDice(this.D12TotalField, this.D12TotalBox, hitDice.SpentD12, hitDice.TotalD12);
        }

        private void DrawWealth()
        {
            var wealth = Program.CcsFile.Character.Wealth;

            this.TotalWealthField.Text = $"¤ {string.Format("{0:0.00}", wealth.TotalValue)}";

            this.CopperField.Text = wealth.Copper.ToString();
            this.SilverField.Text = wealth.Silver.ToString();
            this.ElectrumField.Text = wealth.Electrum.ToString();
            this.GoldField.Text = wealth.Gold.ToString();
            this.PlatinumField.Text = wealth.Platinum.ToString();
        }

        private void DrawDeathSavingThrows()
        {
            var deathSaves = Program.CcsFile.Character.Vitality.DeathSavingThrows;
            deathSaves.LazyInitialize();

            DisplayUtility.SetRectangleStyle(this.DeathSave1, deathSaves.DeathSaves[0]);
            DisplayUtility.SetRectangleStyle(this.DeathSave2, deathSaves.DeathSaves[1]);
            DisplayUtility.SetRectangleStyle(this.DeathSave3, deathSaves.DeathSaves[2]);
            DisplayUtility.SetRectangleStyle(this.DeathSave4, deathSaves.DeathSaves[3]);
            DisplayUtility.SetRectangleStyle(this.DeathSave5, deathSaves.DeathSaves[4]);
        }

        private void SavingThrows_MouseDown(object sender, RoutedEventArgs e)
        {
            ConciergeSound.UpdateValue();

            var rectangle = sender as Rectangle;
            var savingThrow = Program.CcsFile.Character.SavingThrow;
            var savingThrowCopy = savingThrow.DeepCopy();

            switch (rectangle.Name)
            {
                case "StrengthProficiencyBox":
                    savingThrow.Strength.Proficiency = !savingThrow.Strength.Proficiency;
                    break;
                case "DexterityProficiencyBox":
                    savingThrow.Dexterity.Proficiency = !savingThrow.Dexterity.Proficiency;
                    break;
                case "ConstitutionProficiencyBox":
                    savingThrow.Constitution.Proficiency = !savingThrow.Constitution.Proficiency;
                    break;
                case "IntelligenceProficiencyBox":
                    savingThrow.Intelligence.Proficiency = !savingThrow.Intelligence.Proficiency;
                    break;
                case "WisdomProficiencyBox":
                    savingThrow.Wisdom.Proficiency = !savingThrow.Wisdom.Proficiency;
                    break;
                case "CharismaProficiencyBox":
                    savingThrow.Charisma.Proficiency = !savingThrow.Charisma.Proficiency;
                    break;
            }

            this.DrawSavingThrows();
            this.Draw();

            Program.UndoRedoService.AddCommand(new EditCommand<SavingThrow>(savingThrow, savingThrowCopy, this.ConciergePage));
            Program.Modify();
        }

        private void SkillProficiency_MouseDown(object sender, RoutedEventArgs e)
        {
            ConciergeSound.UpdateValue();

            var rectangle = sender as Rectangle;
            var skill = Program.CcsFile.Character.Skill;
            var skillCopy = skill.DeepCopy();

            switch (rectangle.Name)
            {
                case "AthleticsProficiencyBox":
                    skill.Athletics.Proficiency = !skill.Athletics.Proficiency;
                    break;
                case "AcrobaticsProficiencyBox":
                    skill.Acrobatics.Proficiency = !skill.Acrobatics.Proficiency;
                    break;
                case "SleightOfHandProficiencyBox":
                    skill.SleightOfHand.Proficiency = !skill.SleightOfHand.Proficiency;
                    break;
                case "StealthProficiencyBox":
                    skill.Stealth.Proficiency = !skill.Stealth.Proficiency;
                    break;
                case "ArcanaProficiencyBox":
                    skill.Arcana.Proficiency = !skill.Arcana.Proficiency;
                    break;
                case "HistoryProficiencyBox":
                    skill.History.Proficiency = !skill.History.Proficiency;
                    break;
                case "InvestigationProficiencyBox":
                    skill.Investigation.Proficiency = !skill.Investigation.Proficiency;
                    break;
                case "NatureProficiencyBox":
                    skill.Nature.Proficiency = !skill.Nature.Proficiency;
                    break;
                case "ReligionProficiencyBox":
                    skill.Religion.Proficiency = !skill.Religion.Proficiency;
                    break;
                case "AnimalHandlingProficiencyBox":
                    skill.AnimalHandling.Proficiency = !skill.AnimalHandling.Proficiency;
                    break;
                case "InsightProficiencyBox":
                    skill.Insight.Proficiency = !skill.Insight.Proficiency;
                    break;
                case "MedicineProficiencyBox":
                    skill.Medicine.Proficiency = !skill.Medicine.Proficiency;
                    break;
                case "PerceptionProficiencyBox":
                    skill.Perception.Proficiency = !skill.Perception.Proficiency;
                    break;
                case "SurvivalProficiencyBox":
                    skill.Survival.Proficiency = !skill.Survival.Proficiency;
                    break;
                case "DeceptionProficiencyBox":
                    skill.Deception.Proficiency = !skill.Deception.Proficiency;
                    break;
                case "IntimidationProficiencyBox":
                    skill.Intimidation.Proficiency = !skill.Intimidation.Proficiency;
                    break;
                case "PerformanceProficiencyBox":
                    skill.Performance.Proficiency = !skill.Performance.Proficiency;
                    break;
                case "PersuasionProficiencyBox":
                    skill.Persuasion.Proficiency = !skill.Persuasion.Proficiency;
                    break;
            }

            this.DrawSkills();

            Program.UndoRedoService.AddCommand(new EditCommand<Skill>(skill, skillCopy, this.ConciergePage));
            Program.Modify();
        }

        private void SkillExpertise_MouseDown(object sender, RoutedEventArgs e)
        {
            ConciergeSound.UpdateValue();

            var rectangle = sender as Rectangle;
            var skill = Program.CcsFile.Character.Skill;
            var skillCopy = skill.DeepCopy();

            switch (rectangle.Name)
            {
                case "AthleticsExpertieseBox":
                    skill.Athletics.Expertise = !skill.Athletics.Expertise;
                    break;
                case "AcrobaticsExpertieseBox":
                    skill.Acrobatics.Expertise = !skill.Acrobatics.Expertise;
                    break;
                case "SleightOfHandExpertieseBox":
                    skill.SleightOfHand.Expertise = !skill.SleightOfHand.Expertise;
                    break;
                case "StealthExpertieseBox":
                    skill.Stealth.Expertise = !skill.Stealth.Expertise;
                    break;
                case "ArcanaExpertieseBox":
                    skill.Arcana.Expertise = !skill.Arcana.Expertise;
                    break;
                case "HistoryExpertieseBox":
                    skill.History.Expertise = !skill.History.Expertise;
                    break;
                case "InvestigationExpertieseBox":
                    skill.Investigation.Expertise = !skill.Investigation.Expertise;
                    break;
                case "NatureExpertieseBox":
                    skill.Nature.Expertise = !skill.Nature.Expertise;
                    break;
                case "ReligionExpertieseBox":
                    skill.Religion.Expertise = !skill.Religion.Expertise;
                    break;
                case "AnimalHandlingExpertieseBox":
                    skill.AnimalHandling.Expertise = !skill.AnimalHandling.Expertise;
                    break;
                case "InsightExpertieseBox":
                    skill.Insight.Expertise = !skill.Insight.Expertise;
                    break;
                case "MedicineExpertieseBox":
                    skill.Medicine.Expertise = !skill.Medicine.Expertise;
                    break;
                case "PerceptionExpertieseBox":
                    skill.Perception.Expertise = !skill.Perception.Expertise;
                    break;
                case "SurvivalExpertieseBox":
                    skill.Survival.Expertise = !skill.Survival.Expertise;
                    break;
                case "DeceptionExpertieseBox":
                    skill.Deception.Expertise = !skill.Deception.Expertise;
                    break;
                case "IntimidationExpertieseBox":
                    skill.Intimidation.Expertise = !skill.Intimidation.Expertise;
                    break;
                case "PerformanceExpertieseBox":
                    skill.Performance.Expertise = !skill.Performance.Expertise;
                    break;
                case "PersuasionExpertieseBox":
                    skill.Persuasion.Expertise = !skill.Persuasion.Expertise;
                    break;
            }

            this.DrawSkills();

            Program.UndoRedoService.AddCommand(new EditCommand<Skill>(skill, skillCopy, this.ConciergePage));
            Program.Modify();
        }

        private void ToggleBox_MouseEnter(object sender, MouseEventArgs e)
        {
            var rectangle = sender as Rectangle;
            rectangle.Stroke = ConciergeColors.RectangleBorderHighlight;
            rectangle.StrokeThickness = 1;

            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void ToggleBox_MouseLeave(object sender, MouseEventArgs e)
        {
            var rectangle = sender as Rectangle;
            rectangle.Stroke = ConciergeColors.RectangleBorder;
            rectangle.StrokeThickness = 1;

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void EditAttributesButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<Attributes>(
                Program.CcsFile.Character.Attributes,
                typeof(ModifyAttributesWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawAttributes();
            this.DrawSavingThrows();
            this.DrawSkills();
        }

        private void EditSensesButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<Senses>(
                Program.CcsFile.Character.Senses,
                typeof(ModifySensesWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawDetails();
        }

        private void EditHealthButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<Health>(
                Program.CcsFile.Character.Vitality.Health,
                typeof(ModifyHealthWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawHealth();
        }

        private void EditHitDiceButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<HitDice>(
                Program.CcsFile.Character.Vitality.HitDice,
                typeof(ModifyHitDiceWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawHitDice();
        }

        private void TakeDamageButton_Click(object sender, RoutedEventArgs e)
        {
            var character = Program.CcsFile.Character;

            ConciergeWindowService.ShowDamage<Vitality>(
                Program.CcsFile.Character.Vitality,
                typeof(ModifyHpWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawHealth();

            if (character.Vitality.IsDead)
            {
                DisplayCharacterDeathWindow(character);
            }
        }

        private void HealDamageButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowHeal<Vitality>(
                Program.CcsFile.Character.Vitality,
                typeof(ModifyHpWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawHealth();
        }

        private void SpentBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2)
            {
                return;
            }

            var hitDice = Program.CcsFile.Character.Vitality.HitDice;
            var oldItem = hitDice.DeepCopy();
            switch ((sender as Grid).Name)
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
            var grid = sender as Grid;
            var hitDice = Program.CcsFile.Character.Vitality.HitDice;

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

        private void EditWealthButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<Wealth>(
                Program.CcsFile.Character.Wealth,
                typeof(ModifyWealthWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawWealth();
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case "ModifyAttributesWindow":
                    this.DrawAttributes();
                    break;
                case "ModifySensesWindow":
                    this.DrawDetails();
                    break;
                case "ModifyHealthWindow":
                    this.DrawHealth();
                    break;
                case "ModifyHitDiceWindow":
                    this.DrawHitDice();
                    break;
                case "ModifySkillCheckWindow":
                    this.DrawSkills();
                    break;
                case "ModifySavingThrowCheckWindow":
                    this.DrawSavingThrows();
                    break;
            }
        }

        private void PassSave_Click(object sender, RoutedEventArgs e)
        {
            var character = Program.CcsFile.Character;

            if (character.Vitality.DeathSavingThrows.DeathSaveStatus != DeathSave.None)
            {
                return;
            }

            var oldItem = character.Vitality.DeathSavingThrows.DeepCopy();
            character.Vitality.DeathSavingThrows.MakeDeathSave(DeathSave.Success);
            Program.UndoRedoService.AddCommand(new EditCommand<DeathSavingThrows>(character.Vitality.DeathSavingThrows, oldItem, this.ConciergePage));

            this.DrawDeathSavingThrows();

            Program.Modify();
        }

        private void FailSave_Click(object sender, RoutedEventArgs e)
        {
            var character = Program.CcsFile.Character;

            if (character.Vitality.DeathSavingThrows.DeathSaveStatus != DeathSave.None)
            {
                return;
            }

            var oldItem = character.Vitality.DeathSavingThrows.DeepCopy();
            character.Vitality.DeathSavingThrows.MakeDeathSave(DeathSave.Failure);
            Program.UndoRedoService.AddCommand(new EditCommand<DeathSavingThrows>(character.Vitality.DeathSavingThrows, oldItem, this.ConciergePage));

            this.DrawDeathSavingThrows();

            if (character.Vitality.DeathSavingThrows.DeathSaveStatus == DeathSave.Failure && !this.DeathScreenShown)
            {
                DisplayCharacterDeathWindow(character);
                this.DeathScreenShown = true;
            }

            Program.Modify();
        }

        private void ResetSaves_Click(object sender, RoutedEventArgs e)
        {
            var oldItem = Program.CcsFile.Character.Vitality.DeathSavingThrows.DeepCopy();
            Program.CcsFile.Character.Vitality.DeathSavingThrows.ResetDeathSaves();
            Program.UndoRedoService.AddCommand(new EditCommand<DeathSavingThrows>(Program.CcsFile.Character.Vitality.DeathSavingThrows, oldItem, this.ConciergePage));

            this.DrawDeathSavingThrows();
            this.DeathScreenShown = false;

            Program.Modify();
        }

        private void SkillCheckButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<Skill>(
                Program.CcsFile.Character.Skill,
                typeof(ModifySkillCheckWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawSkills();
        }

        private void SavingThrowCheckButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeWindowService.ShowEdit<SavingThrow>(
                Program.CcsFile.Character.SavingThrow,
                typeof(ModifySavingThrowCheckWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawSavingThrows();
        }
    }
}