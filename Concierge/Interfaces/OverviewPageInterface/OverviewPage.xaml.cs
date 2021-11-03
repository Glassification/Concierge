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
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Commands.Enums;
    using Concierge.Interfaces.Enums;
    using Concierge.Tools.Interface;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for OverviewPage.xaml.
    /// </summary>
    public partial class OverviewPage : Page, IConciergePage
    {
        private readonly ModifyAttributesWindow modifyAttributesWindow = new ();
        private readonly ModifySensesWindow modifySensesWindow = new ();
        private readonly ModifyHealthWindow modifyHealthWindow = new ();
        private readonly ModifyHpWindow modifyHpWindow = new ();
        private readonly ModifyHitDiceWindow modifyHitDiceWindow = new ();
        private readonly ModifyWealthWindow modifyWealthWindow = new ();

        public OverviewPage()
        {
            this.InitializeComponent();

            this.DataContext = this;
            this.modifyAttributesWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifySensesWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyHealthWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifyHitDiceWindow.ApplyChanges += this.Window_ApplyChanges;

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

        private void InitializeToggleBox(Rectangle toggleBox, MouseButtonEventHandler mouseButtonEventHandler)
        {
            toggleBox.MouseDown += mouseButtonEventHandler;
            toggleBox.MouseEnter += this.ToggleBox_MouseEnter;
            toggleBox.MouseLeave += this.ToggleBox_MouseLeave;
        }

        private void DrawAttributes()
        {
            var attributes = Program.CcsFile.Character.Attributes;

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

            Utilities.SetTextStyle(savingThrow.Strength.StatusChecks, this.StrengthSavingThrowField);
            Utilities.SetTextStyle(savingThrow.Dexterity.StatusChecks, this.DexteritySavingThrowField);
            Utilities.SetTextStyle(savingThrow.Constitution.StatusChecks, this.ConstitutionSavingThrowField);
            Utilities.SetTextStyle(savingThrow.Intelligence.StatusChecks, this.IntelligenceSavingThrowField);
            Utilities.SetTextStyle(savingThrow.Wisdom.StatusChecks, this.WisdomSavingThrowField);
            Utilities.SetTextStyle(savingThrow.Charisma.StatusChecks, this.CharismaSavingThrowField);

            Utilities.SetTextStyle(savingThrow.Strength.StatusChecks, this.StrengthSavingThrowName);
            Utilities.SetTextStyle(savingThrow.Dexterity.StatusChecks, this.DexteritySavingThrowName);
            Utilities.SetTextStyle(savingThrow.Constitution.StatusChecks, this.ConstitutionSavingThrowName);
            Utilities.SetTextStyle(savingThrow.Intelligence.StatusChecks, this.IntelligenceSavingThrowName);
            Utilities.SetTextStyle(savingThrow.Wisdom.StatusChecks, this.WisdomSavingThrowName);
            Utilities.SetTextStyle(savingThrow.Charisma.StatusChecks, this.CharismaSavingThrowName);

            Utilities.SetProficiencyBoxStyle(savingThrow.Strength.Proficiency, this.StrengthProficiencyBox);
            Utilities.SetProficiencyBoxStyle(savingThrow.Dexterity.Proficiency, this.DexterityProficiencyBox);
            Utilities.SetProficiencyBoxStyle(savingThrow.Constitution.Proficiency, this.ConstitutionProficiencyBox);
            Utilities.SetProficiencyBoxStyle(savingThrow.Intelligence.Proficiency, this.IntelligenceProficiencyBox);
            Utilities.SetProficiencyBoxStyle(savingThrow.Wisdom.Proficiency, this.WisdomProficiencyBox);
            Utilities.SetProficiencyBoxStyle(savingThrow.Charisma.Proficiency, this.CharismaProficiencyBox);
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

            Utilities.SetTextStyle(skill.Athletics.Checks, this.AthleticsSkillField);
            Utilities.SetTextStyle(skill.Acrobatics.Checks, this.AcrobaticsSkillField);
            Utilities.SetTextStyle(skill.SleightOfHand.Checks, this.SleightOfHandSkillField);
            Utilities.SetTextStyle(skill.Stealth.Checks, this.StealthSkillField);
            Utilities.SetTextStyle(skill.Arcana.Checks, this.ArcanaSkillField);
            Utilities.SetTextStyle(skill.History.Checks, this.HistorySkillField);
            Utilities.SetTextStyle(skill.Investigation.Checks, this.InvestigationSkillField);
            Utilities.SetTextStyle(skill.Nature.Checks, this.NatureSkillField);
            Utilities.SetTextStyle(skill.Religion.Checks, this.ReligionSkillField);
            Utilities.SetTextStyle(skill.AnimalHandling.Checks, this.AnimalHandlingSkillField);
            Utilities.SetTextStyle(skill.Insight.Checks, this.InsightSkillField);
            Utilities.SetTextStyle(skill.Medicine.Checks, this.MedicineSkillField);
            Utilities.SetTextStyle(skill.Perception.Checks, this.PerceptionSkillField);
            Utilities.SetTextStyle(skill.Survival.Checks, this.SurvivalSkillField);
            Utilities.SetTextStyle(skill.Deception.Checks, this.DeceptionSkillField);
            Utilities.SetTextStyle(skill.Intimidation.Checks, this.IntimidationSkillField);
            Utilities.SetTextStyle(skill.Performance.Checks, this.PerformanceSkillField);
            Utilities.SetTextStyle(skill.Persuasion.Checks, this.PersuasionSkillField);

            Utilities.SetTextStyle(skill.Athletics.Checks, this.AthleticsSkillName);
            Utilities.SetTextStyle(skill.Acrobatics.Checks, this.AcrobaticsSkillName);
            Utilities.SetTextStyle(skill.SleightOfHand.Checks, this.SleightOfHandSkillName);
            Utilities.SetTextStyle(skill.Stealth.Checks, this.StealthSkillName);
            Utilities.SetTextStyle(skill.Arcana.Checks, this.ArcanaSkillName);
            Utilities.SetTextStyle(skill.History.Checks, this.HistorySkillName);
            Utilities.SetTextStyle(skill.Investigation.Checks, this.InvestigationSkillName);
            Utilities.SetTextStyle(skill.Nature.Checks, this.NatureSkillName);
            Utilities.SetTextStyle(skill.Religion.Checks, this.ReligionSkillName);
            Utilities.SetTextStyle(skill.AnimalHandling.Checks, this.AnimalHandlingSkillName);
            Utilities.SetTextStyle(skill.Insight.Checks, this.InsightSkillName);
            Utilities.SetTextStyle(skill.Medicine.Checks, this.MedicineSkillName);
            Utilities.SetTextStyle(skill.Perception.Checks, this.PerceptionSkillName);
            Utilities.SetTextStyle(skill.Survival.Checks, this.SurvivalSkillName);
            Utilities.SetTextStyle(skill.Deception.Checks, this.DeceptionSkillName);
            Utilities.SetTextStyle(skill.Intimidation.Checks, this.IntimidationSkillName);
            Utilities.SetTextStyle(skill.Performance.Checks, this.PerformanceSkillName);
            Utilities.SetTextStyle(skill.Persuasion.Checks, this.PersuasionSkillName);

            Utilities.SetProficiencyBoxStyle(skill.Athletics.Proficiency, this.AthleticsProficiencyBox);
            Utilities.SetProficiencyBoxStyle(skill.Acrobatics.Proficiency, this.AcrobaticsProficiencyBox);
            Utilities.SetProficiencyBoxStyle(skill.SleightOfHand.Proficiency, this.SleightOfHandProficiencyBox);
            Utilities.SetProficiencyBoxStyle(skill.Stealth.Proficiency, this.StealthProficiencyBox);
            Utilities.SetProficiencyBoxStyle(skill.Arcana.Proficiency, this.ArcanaProficiencyBox);
            Utilities.SetProficiencyBoxStyle(skill.History.Proficiency, this.HistoryProficiencyBox);
            Utilities.SetProficiencyBoxStyle(skill.Investigation.Proficiency, this.InvestigationProficiencyBox);
            Utilities.SetProficiencyBoxStyle(skill.Nature.Proficiency, this.NatureProficiencyBox);
            Utilities.SetProficiencyBoxStyle(skill.Religion.Proficiency, this.ReligionProficiencyBox);
            Utilities.SetProficiencyBoxStyle(skill.AnimalHandling.Proficiency, this.AnimalHandlingProficiencyBox);
            Utilities.SetProficiencyBoxStyle(skill.Insight.Proficiency, this.InsightProficiencyBox);
            Utilities.SetProficiencyBoxStyle(skill.Medicine.Proficiency, this.MedicineProficiencyBox);
            Utilities.SetProficiencyBoxStyle(skill.Perception.Proficiency, this.PerceptionProficiencyBox);
            Utilities.SetProficiencyBoxStyle(skill.Survival.Proficiency, this.SurvivalProficiencyBox);
            Utilities.SetProficiencyBoxStyle(skill.Deception.Proficiency, this.DeceptionProficiencyBox);
            Utilities.SetProficiencyBoxStyle(skill.Intimidation.Proficiency, this.IntimidationProficiencyBox);
            Utilities.SetProficiencyBoxStyle(skill.Performance.Proficiency, this.PerformanceProficiencyBox);
            Utilities.SetProficiencyBoxStyle(skill.Persuasion.Proficiency, this.PersuasionProficiencyBox);

            Utilities.SetProficiencyBoxStyle(skill.Athletics.Expertise, this.AthleticsExpertieseBox);
            Utilities.SetProficiencyBoxStyle(skill.Acrobatics.Expertise, this.AcrobaticsExpertieseBox);
            Utilities.SetProficiencyBoxStyle(skill.SleightOfHand.Expertise, this.SleightOfHandExpertieseBox);
            Utilities.SetProficiencyBoxStyle(skill.Stealth.Expertise, this.StealthExpertieseBox);
            Utilities.SetProficiencyBoxStyle(skill.Arcana.Expertise, this.ArcanaExpertieseBox);
            Utilities.SetProficiencyBoxStyle(skill.History.Expertise, this.HistoryExpertieseBox);
            Utilities.SetProficiencyBoxStyle(skill.Investigation.Expertise, this.InvestigationExpertieseBox);
            Utilities.SetProficiencyBoxStyle(skill.Nature.Expertise, this.NatureExpertieseBox);
            Utilities.SetProficiencyBoxStyle(skill.Religion.Expertise, this.ReligionExpertieseBox);
            Utilities.SetProficiencyBoxStyle(skill.AnimalHandling.Expertise, this.AnimalHandlingExpertieseBox);
            Utilities.SetProficiencyBoxStyle(skill.Insight.Expertise, this.InsightExpertieseBox);
            Utilities.SetProficiencyBoxStyle(skill.Medicine.Expertise, this.MedicineExpertieseBox);
            Utilities.SetProficiencyBoxStyle(skill.Perception.Expertise, this.PerceptionExpertieseBox);
            Utilities.SetProficiencyBoxStyle(skill.Survival.Expertise, this.SurvivalExpertieseBox);
            Utilities.SetProficiencyBoxStyle(skill.Deception.Expertise, this.DeceptionExpertieseBox);
            Utilities.SetProficiencyBoxStyle(skill.Intimidation.Expertise, this.IntimidationExpertieseBox);
            Utilities.SetProficiencyBoxStyle(skill.Performance.Expertise, this.PerformanceExpertieseBox);
            Utilities.SetProficiencyBoxStyle(skill.Persuasion.Expertise, this.PersuasionExpertieseBox);
        }

        private void DrawHealth()
        {
            var vitality = Program.CcsFile.Character.Vitality;

            this.CurrentHpField.Text = vitality.CurrentHealth.ToString();
            this.TotalHpField.Text = "/" + vitality.Health.MaxHealth.ToString();

            this.HpBackground.Foreground = Utilities.SetHealthStyle(vitality);
            this.TotalHpField.Foreground = Utilities.SetHealthStyle(vitality);
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

            Utilities.SetRectangleStyle(this.DeathSave1, deathSaves.DeathSaves[0]);
            Utilities.SetRectangleStyle(this.DeathSave2, deathSaves.DeathSaves[1]);
            Utilities.SetRectangleStyle(this.DeathSave3, deathSaves.DeathSaves[2]);
            Utilities.SetRectangleStyle(this.DeathSave4, deathSaves.DeathSaves[3]);
            Utilities.SetRectangleStyle(this.DeathSave5, deathSaves.DeathSaves[4]);
        }

        private void SavingThrows_MouseDown(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            ConciergeSound.UpdateValue();

            var rectangle = sender as Rectangle;
            var savingThrow = Program.CcsFile.Character.SavingThrow;

            switch (rectangle.Name)
            {
                case "StrengthProficiencyBox":
                    savingThrow.Strength.Proficiency = !savingThrow.Strength.Proficiency;
                    Program.UndoRedoService.AddCommand(new SavingThrowCommand(savingThrow.Strength, !savingThrow.Strength.Proficiency, savingThrow.Strength.Proficiency));
                    break;
                case "DexterityProficiencyBox":
                    savingThrow.Dexterity.Proficiency = !savingThrow.Dexterity.Proficiency;
                    Program.UndoRedoService.AddCommand(new SavingThrowCommand(savingThrow.Dexterity, !savingThrow.Dexterity.Proficiency, savingThrow.Dexterity.Proficiency));
                    break;
                case "ConstitutionProficiencyBox":
                    savingThrow.Constitution.Proficiency = !savingThrow.Constitution.Proficiency;
                    Program.UndoRedoService.AddCommand(new SavingThrowCommand(savingThrow.Constitution, !savingThrow.Constitution.Proficiency, savingThrow.Constitution.Proficiency));
                    break;
                case "IntelligenceProficiencyBox":
                    savingThrow.Intelligence.Proficiency = !savingThrow.Intelligence.Proficiency;
                    Program.UndoRedoService.AddCommand(new SavingThrowCommand(savingThrow.Intelligence, !savingThrow.Intelligence.Proficiency, savingThrow.Intelligence.Proficiency));
                    break;
                case "WisdomProficiencyBox":
                    savingThrow.Wisdom.Proficiency = !savingThrow.Wisdom.Proficiency;
                    Program.UndoRedoService.AddCommand(new SavingThrowCommand(savingThrow.Wisdom, !savingThrow.Wisdom.Proficiency, savingThrow.Wisdom.Proficiency));
                    break;
                case "CharismaProficiencyBox":
                    savingThrow.Charisma.Proficiency = !savingThrow.Charisma.Proficiency;
                    Program.UndoRedoService.AddCommand(new SavingThrowCommand(savingThrow.Charisma, !savingThrow.Charisma.Proficiency, savingThrow.Charisma.Proficiency));
                    break;
            }

            this.DrawSavingThrows();
        }

        private void SkillProficiency_MouseDown(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            ConciergeSound.UpdateValue();

            var rectangle = sender as Rectangle;
            var skill = Program.CcsFile.Character.Skill;

            switch (rectangle.Name)
            {
                case "AthleticsProficiencyBox":
                    skill.Athletics.Proficiency = !skill.Athletics.Proficiency;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Athletics, BonusType.Proficiency, !skill.Athletics.Proficiency, skill.Athletics.Proficiency));
                    break;
                case "AcrobaticsProficiencyBox":
                    skill.Acrobatics.Proficiency = !skill.Acrobatics.Proficiency;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Acrobatics, BonusType.Proficiency, !skill.Acrobatics.Proficiency, skill.Acrobatics.Proficiency));
                    break;
                case "SleightOfHandProficiencyBox":
                    skill.SleightOfHand.Proficiency = !skill.SleightOfHand.Proficiency;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.SleightOfHand, BonusType.Proficiency, !skill.SleightOfHand.Proficiency, skill.SleightOfHand.Proficiency));
                    break;
                case "StealthProficiencyBox":
                    skill.Stealth.Proficiency = !skill.Stealth.Proficiency;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Stealth, BonusType.Proficiency, !skill.Stealth.Proficiency, skill.Stealth.Proficiency));
                    break;
                case "ArcanaProficiencyBox":
                    skill.Arcana.Proficiency = !skill.Arcana.Proficiency;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Arcana, BonusType.Proficiency, !skill.Arcana.Proficiency, skill.Arcana.Proficiency));
                    break;
                case "HistoryProficiencyBox":
                    skill.History.Proficiency = !skill.History.Proficiency;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.History, BonusType.Proficiency, !skill.History.Proficiency, skill.History.Proficiency));
                    break;
                case "InvestigationProficiencyBox":
                    skill.Investigation.Proficiency = !skill.Investigation.Proficiency;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Investigation, BonusType.Proficiency, !skill.Investigation.Proficiency, skill.Investigation.Proficiency));
                    break;
                case "NatureProficiencyBox":
                    skill.Nature.Proficiency = !skill.Nature.Proficiency;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Nature, BonusType.Proficiency, !skill.Nature.Proficiency, skill.Nature.Proficiency));
                    break;
                case "ReligionProficiencyBox":
                    skill.Religion.Proficiency = !skill.Religion.Proficiency;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Religion, BonusType.Proficiency, !skill.Religion.Proficiency, skill.Religion.Proficiency));
                    break;
                case "AnimalHandlingProficiencyBox":
                    skill.AnimalHandling.Proficiency = !skill.AnimalHandling.Proficiency;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.AnimalHandling, BonusType.Proficiency, !skill.AnimalHandling.Proficiency, skill.AnimalHandling.Proficiency));

                    break;
                case "InsightProficiencyBox":
                    skill.Insight.Proficiency = !skill.Insight.Proficiency;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Insight, BonusType.Proficiency, !skill.Insight.Proficiency, skill.Insight.Proficiency));
                    break;
                case "MedicineProficiencyBox":
                    skill.Medicine.Proficiency = !skill.Medicine.Proficiency;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Medicine, BonusType.Proficiency, !skill.Medicine.Proficiency, skill.Medicine.Proficiency));
                    break;
                case "PerceptionProficiencyBox":
                    skill.Perception.Proficiency = !skill.Perception.Proficiency;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Perception, BonusType.Proficiency, !skill.Perception.Proficiency, skill.Perception.Proficiency));
                    break;
                case "SurvivalProficiencyBox":
                    skill.Survival.Proficiency = !skill.Survival.Proficiency;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Survival, BonusType.Proficiency, !skill.Survival.Proficiency, skill.Survival.Proficiency));
                    break;
                case "DeceptionProficiencyBox":
                    skill.Deception.Proficiency = !skill.Deception.Proficiency;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Deception, BonusType.Proficiency, !skill.Deception.Proficiency, skill.Deception.Proficiency));
                    break;
                case "IntimidationProficiencyBox":
                    skill.Intimidation.Proficiency = !skill.Intimidation.Proficiency;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Intimidation, BonusType.Proficiency, !skill.Intimidation.Proficiency, skill.Intimidation.Proficiency));
                    break;
                case "PerformanceProficiencyBox":
                    skill.Performance.Proficiency = !skill.Performance.Proficiency;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Performance, BonusType.Proficiency, !skill.Performance.Proficiency, skill.Performance.Proficiency));
                    break;
                case "PersuasionProficiencyBox":
                    skill.Persuasion.Proficiency = !skill.Persuasion.Proficiency;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Persuasion, BonusType.Proficiency, !skill.Persuasion.Proficiency, skill.Persuasion.Proficiency));
                    break;
            }

            this.DrawSkills();
        }

        private void SkillExpertise_MouseDown(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            ConciergeSound.UpdateValue();

            Rectangle rectangle = sender as Rectangle;
            var skill = Program.CcsFile.Character.Skill;

            switch (rectangle.Name)
            {
                case "AthleticsExpertieseBox":
                    skill.Athletics.Expertise = !skill.Athletics.Expertise;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Athletics, BonusType.Expertise, !skill.Athletics.Expertise, skill.Athletics.Expertise));
                    break;
                case "AcrobaticsExpertieseBox":
                    skill.Acrobatics.Expertise = !skill.Acrobatics.Expertise;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Acrobatics, BonusType.Expertise, !skill.Acrobatics.Expertise, skill.Acrobatics.Expertise));
                    break;
                case "SleightOfHandExpertieseBox":
                    skill.SleightOfHand.Expertise = !skill.SleightOfHand.Expertise;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.SleightOfHand, BonusType.Expertise, !skill.SleightOfHand.Expertise, skill.SleightOfHand.Expertise));
                    break;
                case "StealthExpertieseBox":
                    skill.Stealth.Expertise = !skill.Stealth.Expertise;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Stealth, BonusType.Expertise, !skill.Stealth.Expertise, skill.Stealth.Expertise));
                    break;
                case "ArcanaExpertieseBox":
                    skill.Arcana.Expertise = !skill.Arcana.Expertise;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Arcana, BonusType.Expertise, !skill.Arcana.Expertise, skill.Arcana.Expertise));
                    break;
                case "HistoryExpertieseBox":
                    skill.History.Expertise = !skill.History.Expertise;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.History, BonusType.Expertise, !skill.History.Expertise, skill.History.Expertise));
                    break;
                case "InvestigationExpertieseBox":
                    skill.Investigation.Expertise = !skill.Investigation.Expertise;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Investigation, BonusType.Expertise, !skill.Investigation.Expertise, skill.Investigation.Expertise));
                    break;
                case "NatureExpertieseBox":
                    skill.Nature.Expertise = !skill.Nature.Expertise;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Nature, BonusType.Expertise, !skill.Nature.Expertise, skill.Nature.Expertise));
                    break;
                case "ReligionExpertieseBox":
                    skill.Religion.Expertise = !skill.Religion.Expertise;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Religion, BonusType.Expertise, !skill.Religion.Expertise, skill.Religion.Expertise));
                    break;
                case "AnimalHandlingExpertieseBox":
                    skill.AnimalHandling.Expertise = !skill.AnimalHandling.Expertise;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.AnimalHandling, BonusType.Expertise, !skill.AnimalHandling.Expertise, skill.AnimalHandling.Expertise));
                    break;
                case "InsightExpertieseBox":
                    skill.Insight.Expertise = !skill.Insight.Expertise;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Insight, BonusType.Expertise, !skill.Insight.Expertise, skill.Insight.Expertise));
                    break;
                case "MedicineExpertieseBox":
                    skill.Medicine.Expertise = !skill.Medicine.Expertise;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Medicine, BonusType.Expertise, !skill.Medicine.Expertise, skill.Medicine.Expertise));
                    break;
                case "PerceptionExpertieseBox":
                    skill.Perception.Expertise = !skill.Perception.Expertise;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Perception, BonusType.Expertise, !skill.Perception.Expertise, skill.Perception.Expertise));
                    break;
                case "SurvivalExpertieseBox":
                    skill.Survival.Expertise = !skill.Survival.Expertise;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Survival, BonusType.Expertise, !skill.Survival.Expertise, skill.Survival.Expertise));
                    break;
                case "DeceptionExpertieseBox":
                    skill.Deception.Expertise = !skill.Deception.Expertise;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Deception, BonusType.Expertise, !skill.Deception.Expertise, skill.Deception.Expertise));
                    break;
                case "IntimidationExpertieseBox":
                    skill.Intimidation.Expertise = !skill.Intimidation.Expertise;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Intimidation, BonusType.Expertise, !skill.Intimidation.Expertise, skill.Intimidation.Expertise));
                    break;
                case "PerformanceExpertieseBox":
                    skill.Performance.Expertise = !skill.Performance.Expertise;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Performance, BonusType.Expertise, !skill.Performance.Expertise, skill.Performance.Expertise));
                    break;
                case "PersuasionExpertieseBox":
                    skill.Persuasion.Expertise = !skill.Persuasion.Expertise;
                    Program.UndoRedoService.AddCommand(new SkillCommand(skill.Persuasion, BonusType.Expertise, !skill.Persuasion.Expertise, skill.Persuasion.Expertise));
                    break;
            }

            this.DrawSkills();
        }

        private void ToggleBox_MouseEnter(object sender, MouseEventArgs e)
        {
            var rectangle = sender as Rectangle;
            rectangle.Stroke = Colours.RectangleBorderHighlight;
            rectangle.StrokeThickness = 1;

            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void ToggleBox_MouseLeave(object sender, MouseEventArgs e)
        {
            var rectangle = sender as Rectangle;
            rectangle.Stroke = Colours.RectangleBorder;
            rectangle.StrokeThickness = 1;

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void EditAttributesButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifyAttributesWindow.EditAttributes(Program.CcsFile.Character.Attributes);
            this.DrawAttributes();
            this.DrawSavingThrows();
            this.DrawSkills();
        }

        private void EditSensesButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifySensesWindow.EditSenses();
            this.DrawDetails();
        }

        private void EditHealthButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifyHealthWindow.EditHealth(Program.CcsFile.Character.Vitality.Health);
            this.DrawHealth();
        }

        private void EditHitDiceButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifyHitDiceWindow.ShowEdit(Program.CcsFile.Character.Vitality.HitDice);
            this.DrawHitDice();
        }

        private void TakeDamageButton_Click(object sender, RoutedEventArgs e)
        {
            var character = Program.CcsFile.Character;

            this.modifyHpWindow.ShowDamage(character.Vitality);
            this.DrawHealth();

            if (character.Vitality.IsDead)
            {
                DisplayCharacterDeathWindow(character);
            }
        }

        private void HealDamageButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifyHpWindow.ShowHeal(Program.CcsFile.Character.Vitality);
            this.DrawHealth();
        }

        private void SpentBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2)
            {
                return;
            }

            var hitDice = Program.CcsFile.Character.Vitality.HitDice;
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
            var hitDice = Program.CcsFile.Character.Vitality.HitDice;

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

        private void EditWealthButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifyWealthWindow.ShowEdit();
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
            }
        }

        private void PassSave_Click(object sender, RoutedEventArgs e)
        {
            var character = Program.CcsFile.Character;

            if (character.Vitality.DeathSavingThrows.DeathSaveStatus != DeathSave.None)
            {
                return;
            }

            Program.Modify();

            character.Vitality.DeathSavingThrows.MakeDeathSave(DeathSave.Success);
            this.DrawDeathSavingThrows();
        }

        private void FailSave_Click(object sender, RoutedEventArgs e)
        {
            var character = Program.CcsFile.Character;

            if (character.Vitality.DeathSavingThrows.DeathSaveStatus != DeathSave.None)
            {
                return;
            }

            Program.Modify();

            character.Vitality.DeathSavingThrows.MakeDeathSave(DeathSave.Failure);
            this.DrawDeathSavingThrows();

            if (character.Vitality.DeathSavingThrows.DeathSaveStatus == DeathSave.Failure && !this.DeathScreenShown)
            {
                DisplayCharacterDeathWindow(character);
                this.DeathScreenShown = true;
            }
        }

        private void ResetSaves_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            Program.CcsFile.Character.Vitality.DeathSavingThrows.ResetDeathSaves();
            this.DrawDeathSavingThrows();

            this.DeathScreenShown = false;
        }
    }
}