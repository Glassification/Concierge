// <copyright file="OverviewPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.OverviewPageUi
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using Concierge.Characters.Collections;
    using Concierge.Characters.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for OverviewPage.xaml.
    /// </summary>
    public partial class OverviewPage : Page
    {
        public OverviewPage()
        {
            this.InitializeComponent();

            this.DataContext = this;
            this.ResourceIndex = 0;
            this.ModifyAttributesWindow = new ModifyAttributesWindow();
            this.ModifySensesWindow = new ModifySensesWindow();
            this.ModifyHealthWindow = new ModifyHealthWindow();
            this.ModifyHpWindow = new ModifyHpWindow();
            this.ModifyHitDiceWindow = new ModifyHitDiceWindow();
            this.StrengthProficiencyBox.MouseDown += this.SavingThrows_MouseDown;
            this.StrengthProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.StrengthProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.DexterityProficiencyBox.MouseDown += this.SavingThrows_MouseDown;
            this.DexterityProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.DexterityProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.ConstitutionProficiencyBox.MouseDown += this.SavingThrows_MouseDown;
            this.ConstitutionProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.ConstitutionProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.IntelligenceProficiencyBox.MouseDown += this.SavingThrows_MouseDown;
            this.IntelligenceProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.IntelligenceProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.WisdomProficiencyBox.MouseDown += this.SavingThrows_MouseDown;
            this.WisdomProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.WisdomProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.CharismaProficiencyBox.MouseDown += this.SavingThrows_MouseDown;
            this.CharismaProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.CharismaProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            //------------------------------------------------------------------
            this.AthleticsProficiencyBox.MouseDown += this.SkillProficiency_MouseDown;
            this.AthleticsProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.AthleticsProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.AcrobaticsProficiencyBox.MouseDown += this.SkillProficiency_MouseDown;
            this.AcrobaticsProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.AcrobaticsProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.SleightOfHandProficiencyBox.MouseDown += this.SkillProficiency_MouseDown;
            this.SleightOfHandProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.SleightOfHandProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.StealthProficiencyBox.MouseDown += this.SkillProficiency_MouseDown;
            this.StealthProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.StealthProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.ArcanaProficiencyBox.MouseDown += this.SkillProficiency_MouseDown;
            this.ArcanaProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.ArcanaProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.HistoryProficiencyBox.MouseDown += this.SkillProficiency_MouseDown;
            this.HistoryProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.HistoryProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.InvestigationProficiencyBox.MouseDown += this.SkillProficiency_MouseDown;
            this.InvestigationProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.InvestigationProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.NatureProficiencyBox.MouseDown += this.SkillProficiency_MouseDown;
            this.NatureProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.NatureProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.ReligionProficiencyBox.MouseDown += this.SkillProficiency_MouseDown;
            this.NatureProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.NatureProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.AnimalHandlingProficiencyBox.MouseDown += this.SkillProficiency_MouseDown;
            this.AnimalHandlingProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.AnimalHandlingProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.InsightProficiencyBox.MouseDown += this.SkillProficiency_MouseDown;
            this.InsightProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.InsightProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.MedicineProficiencyBox.MouseDown += this.SkillProficiency_MouseDown;
            this.MedicineProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.MedicineProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.PerceptionProficiencyBox.MouseDown += this.SkillProficiency_MouseDown;
            this.PerceptionProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.PerceptionProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.SurvivalProficiencyBox.MouseDown += this.SkillProficiency_MouseDown;
            this.SurvivalProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.SurvivalProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.DeceptionProficiencyBox.MouseDown += this.SkillProficiency_MouseDown;
            this.DeceptionProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.DeceptionProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.IntimidationProficiencyBox.MouseDown += this.SkillProficiency_MouseDown;
            this.IntimidationProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.IntimidationProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.PerformanceProficiencyBox.MouseDown += this.SkillProficiency_MouseDown;
            this.PerformanceProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.PerformanceProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.PersuasionProficiencyBox.MouseDown += this.SkillProficiency_MouseDown;
            this.PersuasionProficiencyBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.PersuasionProficiencyBox.MouseLeave += this.ToggleBox_MouseLeave;

            //------------------------------------------------------------------
            this.AthleticsExpertieseBox.MouseDown += this.SkillExpertise_MouseDown;
            this.AthleticsExpertieseBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.AthleticsExpertieseBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.AcrobaticsExpertieseBox.MouseDown += this.SkillExpertise_MouseDown;
            this.AcrobaticsExpertieseBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.AcrobaticsExpertieseBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.SleightOfHandExpertieseBox.MouseDown += this.SkillExpertise_MouseDown;
            this.SleightOfHandExpertieseBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.SleightOfHandExpertieseBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.StealthExpertieseBox.MouseDown += this.SkillExpertise_MouseDown;
            this.StealthExpertieseBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.StealthExpertieseBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.ArcanaExpertieseBox.MouseDown += this.SkillExpertise_MouseDown;
            this.ArcanaExpertieseBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.ArcanaExpertieseBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.HistoryExpertieseBox.MouseDown += this.SkillExpertise_MouseDown;
            this.HistoryExpertieseBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.HistoryExpertieseBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.InvestigationExpertieseBox.MouseDown += this.SkillExpertise_MouseDown;
            this.InvestigationExpertieseBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.InvestigationExpertieseBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.NatureExpertieseBox.MouseDown += this.SkillExpertise_MouseDown;
            this.NatureExpertieseBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.NatureExpertieseBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.ReligionExpertieseBox.MouseDown += this.SkillExpertise_MouseDown;
            this.ReligionExpertieseBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.ReligionExpertieseBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.AnimalHandlingExpertieseBox.MouseDown += this.SkillExpertise_MouseDown;
            this.AnimalHandlingExpertieseBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.AnimalHandlingExpertieseBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.InsightExpertieseBox.MouseDown += this.SkillExpertise_MouseDown;
            this.InsightExpertieseBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.InsightExpertieseBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.MedicineExpertieseBox.MouseDown += this.SkillExpertise_MouseDown;
            this.MedicineExpertieseBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.MedicineExpertieseBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.PerceptionExpertieseBox.MouseDown += this.SkillExpertise_MouseDown;
            this.PerceptionExpertieseBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.PerceptionExpertieseBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.SurvivalExpertieseBox.MouseDown += this.SkillExpertise_MouseDown;
            this.SurvivalExpertieseBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.SurvivalExpertieseBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.DeceptionExpertieseBox.MouseDown += this.SkillExpertise_MouseDown;
            this.DeceptionExpertieseBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.DeceptionExpertieseBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.IntimidationExpertieseBox.MouseDown += this.SkillExpertise_MouseDown;
            this.IntimidationExpertieseBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.IntimidationExpertieseBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.PerformanceExpertieseBox.MouseDown += this.SkillExpertise_MouseDown;
            this.PerformanceExpertieseBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.PerformanceExpertieseBox.MouseLeave += this.ToggleBox_MouseLeave;

            this.PersuasionExpertieseBox.MouseDown += this.SkillExpertise_MouseDown;
            this.PersuasionExpertieseBox.MouseEnter += this.ToggleBox_MouseEnter;
            this.PersuasionExpertieseBox.MouseLeave += this.ToggleBox_MouseLeave;
        }

        public int HeartWidth => (int)this.HealthBox.RenderSize.Width;

        public int HeartHeight => (int)this.HealthBox.RenderSize.Height;

        public int ShieldWidth => (int)this.HealthBox.RenderSize.Width;

        public int ShieldHeight => (int)this.HealthBox.RenderSize.Height;

        public int ResourceIndex { get; private set; }

        private ModifyAttributesWindow ModifyAttributesWindow { get; }

        private ModifySensesWindow ModifySensesWindow { get; }

        private ModifyHealthWindow ModifyHealthWindow { get; }

        private ModifyHpWindow ModifyHpWindow { get; }

        private ModifyHitDiceWindow ModifyHitDiceWindow { get; }

        public void Draw()
        {
            this.DrawAttributes();
            this.DrawDetails();
            this.DrawSavingThrows();
            this.DrawSkills();
            this.DrawHealth();
            this.DrawArmorClass();
            this.DrawHitDice();
            this.DrawResourcePool();
        }

        private void DrawAttributes()
        {
            this.StrengthBonusField.Text = Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Strength).ToString();
            this.DexterityBonusField.Text = Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Dexterity).ToString();
            this.ConstitutionBonusField.Text = Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Constitution).ToString();
            this.IntelligenceBonusField.Text = Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Intelligence).ToString();
            this.WisdomBonusField.Text = Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Wisdom).ToString();
            this.CharismaBonusField.Text = Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Charisma).ToString();

            this.StrengthScoreField.Text = Program.CcsFile.Character.Attributes.Strength.ToString();
            this.DexterityScoreField.Text = Program.CcsFile.Character.Attributes.Dexterity.ToString();
            this.ConstitutionScoreField.Text = Program.CcsFile.Character.Attributes.Constitution.ToString();
            this.IntelligenceScoreField.Text = Program.CcsFile.Character.Attributes.Intelligence.ToString();
            this.WisdomScoreField.Text = Program.CcsFile.Character.Attributes.Wisdom.ToString();
            this.CharismaScoreField.Text = Program.CcsFile.Character.Attributes.Charisma.ToString();
        }

        private void DrawDetails()
        {
            this.InitiativeField.Text = Program.CcsFile.Character.Initiative.ToString();
            this.PassivePerceptionField.Text = Program.CcsFile.Character.PassivePerception.ToString();
            this.VisionField.Text = Program.CcsFile.Character.Details.Vision.ToString();
            this.MovementSpeedField.Text = Program.CcsFile.Character.Details.Movement.ToString();
        }

        private void DrawSavingThrows()
        {
            this.StrengthSavingThrowField.Text = Program.CcsFile.Character.SavingThrow.Strength.Bonus.ToString();
            this.DexteritySavingThrowField.Text = Program.CcsFile.Character.SavingThrow.Dexterity.Bonus.ToString();
            this.ConstitutionSavingThrowField.Text = Program.CcsFile.Character.SavingThrow.Constitution.Bonus.ToString();
            this.IntelligenceSavingThrowField.Text = Program.CcsFile.Character.SavingThrow.Intelligence.Bonus.ToString();
            this.WisdomSavingThrowField.Text = Program.CcsFile.Character.SavingThrow.Wisdom.Bonus.ToString();
            this.CharismaSavingThrowField.Text = Program.CcsFile.Character.SavingThrow.Charisma.Bonus.ToString();

            this.SetTextStyle(Program.CcsFile.Character.SavingThrow.Strength.StatusChecks, this.StrengthSavingThrowField);
            this.SetTextStyle(Program.CcsFile.Character.SavingThrow.Dexterity.StatusChecks, this.DexteritySavingThrowField);
            this.SetTextStyle(Program.CcsFile.Character.SavingThrow.Constitution.StatusChecks, this.ConstitutionSavingThrowField);
            this.SetTextStyle(Program.CcsFile.Character.SavingThrow.Intelligence.StatusChecks, this.IntelligenceSavingThrowField);
            this.SetTextStyle(Program.CcsFile.Character.SavingThrow.Wisdom.StatusChecks, this.WisdomSavingThrowField);
            this.SetTextStyle(Program.CcsFile.Character.SavingThrow.Charisma.StatusChecks, this.CharismaSavingThrowField);

            this.SetTextStyle(Program.CcsFile.Character.SavingThrow.Strength.StatusChecks, this.StrengthSavingThrowName);
            this.SetTextStyle(Program.CcsFile.Character.SavingThrow.Dexterity.StatusChecks, this.DexteritySavingThrowName);
            this.SetTextStyle(Program.CcsFile.Character.SavingThrow.Constitution.StatusChecks, this.ConstitutionSavingThrowName);
            this.SetTextStyle(Program.CcsFile.Character.SavingThrow.Intelligence.StatusChecks, this.IntelligenceSavingThrowName);
            this.SetTextStyle(Program.CcsFile.Character.SavingThrow.Wisdom.StatusChecks, this.WisdomSavingThrowName);
            this.SetTextStyle(Program.CcsFile.Character.SavingThrow.Charisma.StatusChecks, this.CharismaSavingThrowName);

            this.SetBoxStyle(Program.CcsFile.Character.SavingThrow.Strength.Proficiency, this.StrengthProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.SavingThrow.Dexterity.Proficiency, this.DexterityProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.SavingThrow.Constitution.Proficiency, this.ConstitutionProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.SavingThrow.Intelligence.Proficiency, this.IntelligenceProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.SavingThrow.Wisdom.Proficiency, this.WisdomProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.SavingThrow.Charisma.Proficiency, this.CharismaProficiencyBox);
        }

        private void DrawSkills()
        {
            this.AthleticsSkillField.Text = Program.CcsFile.Character.Skill.Athletics.Bonus.ToString();
            this.AcrobaticsSkillField.Text = Program.CcsFile.Character.Skill.Acrobatics.Bonus.ToString();
            this.SleightOfHandSkillField.Text = Program.CcsFile.Character.Skill.SleightOfHand.Bonus.ToString();
            this.StealthSkillField.Text = Program.CcsFile.Character.Skill.Stealth.Bonus.ToString();
            this.ArcanaSkillField.Text = Program.CcsFile.Character.Skill.Arcana.Bonus.ToString();
            this.HistorySkillField.Text = Program.CcsFile.Character.Skill.History.Bonus.ToString();
            this.InvestigationSkillField.Text = Program.CcsFile.Character.Skill.Investigation.Bonus.ToString();
            this.NatureSkillField.Text = Program.CcsFile.Character.Skill.Nature.Bonus.ToString();
            this.ReligionSkillField.Text = Program.CcsFile.Character.Skill.Religion.Bonus.ToString();
            this.AnimalHandlingSkillField.Text = Program.CcsFile.Character.Skill.AnimalHandling.Bonus.ToString();
            this.InsightSkillField.Text = Program.CcsFile.Character.Skill.Insight.Bonus.ToString();
            this.MedicineSkillField.Text = Program.CcsFile.Character.Skill.Medicine.Bonus.ToString();
            this.PerceptionSkillField.Text = Program.CcsFile.Character.Skill.Perception.Bonus.ToString();
            this.SurvivalSkillField.Text = Program.CcsFile.Character.Skill.Survival.Bonus.ToString();
            this.DeceptionSkillField.Text = Program.CcsFile.Character.Skill.Deception.Bonus.ToString();
            this.IntimidationSkillField.Text = Program.CcsFile.Character.Skill.Intimidation.Bonus.ToString();
            this.PerformanceSkillField.Text = Program.CcsFile.Character.Skill.Performance.Bonus.ToString();
            this.PersuasionSkillField.Text = Program.CcsFile.Character.Skill.Persuasion.Bonus.ToString();

            this.SetTextStyle(Program.CcsFile.Character.Skill.Athletics.Checks, this.AthleticsSkillField);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Acrobatics.Checks, this.AcrobaticsSkillField);
            this.SetTextStyle(Program.CcsFile.Character.Skill.SleightOfHand.Checks, this.SleightOfHandSkillField);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Stealth.Checks, this.StealthSkillField);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Arcana.Checks, this.ArcanaSkillField);
            this.SetTextStyle(Program.CcsFile.Character.Skill.History.Checks, this.HistorySkillField);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Investigation.Checks, this.InvestigationSkillField);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Nature.Checks, this.NatureSkillField);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Religion.Checks, this.ReligionSkillField);
            this.SetTextStyle(Program.CcsFile.Character.Skill.AnimalHandling.Checks, this.AnimalHandlingSkillField);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Insight.Checks, this.InsightSkillField);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Medicine.Checks, this.MedicineSkillField);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Perception.Checks, this.PerceptionSkillField);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Survival.Checks, this.SurvivalSkillField);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Deception.Checks, this.DeceptionSkillField);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Intimidation.Checks, this.IntimidationSkillField);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Performance.Checks, this.PerformanceSkillField);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Persuasion.Checks, this.PersuasionSkillField);

            this.SetTextStyle(Program.CcsFile.Character.Skill.Athletics.Checks, this.AthleticsSkillName);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Acrobatics.Checks, this.AcrobaticsSkillName);
            this.SetTextStyle(Program.CcsFile.Character.Skill.SleightOfHand.Checks, this.SleightOfHandSkillName);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Stealth.Checks, this.StealthSkillName);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Arcana.Checks, this.ArcanaSkillName);
            this.SetTextStyle(Program.CcsFile.Character.Skill.History.Checks, this.HistorySkillName);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Investigation.Checks, this.InvestigationSkillName);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Nature.Checks, this.NatureSkillName);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Religion.Checks, this.ReligionSkillName);
            this.SetTextStyle(Program.CcsFile.Character.Skill.AnimalHandling.Checks, this.AnimalHandlingSkillName);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Insight.Checks, this.InsightSkillName);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Medicine.Checks, this.MedicineSkillName);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Perception.Checks, this.PerceptionSkillName);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Survival.Checks, this.SurvivalSkillName);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Deception.Checks, this.DeceptionSkillName);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Intimidation.Checks, this.IntimidationSkillName);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Performance.Checks, this.PerformanceSkillName);
            this.SetTextStyle(Program.CcsFile.Character.Skill.Persuasion.Checks, this.PersuasionSkillName);

            this.SetBoxStyle(Program.CcsFile.Character.Skill.Athletics.Proficiency, this.AthleticsProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Acrobatics.Proficiency, this.AcrobaticsProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.SleightOfHand.Proficiency, this.SleightOfHandProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Stealth.Proficiency, this.StealthProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Arcana.Proficiency, this.ArcanaProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.History.Proficiency, this.HistoryProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Investigation.Proficiency, this.InvestigationProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Nature.Proficiency, this.NatureProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Religion.Proficiency, this.ReligionProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.AnimalHandling.Proficiency, this.AnimalHandlingProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Insight.Proficiency, this.InsightProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Medicine.Proficiency, this.MedicineProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Perception.Proficiency, this.PerceptionProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Survival.Proficiency, this.SurvivalProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Deception.Proficiency, this.DeceptionProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Intimidation.Proficiency, this.IntimidationProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Performance.Proficiency, this.PerformanceProficiencyBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Persuasion.Proficiency, this.PersuasionProficiencyBox);

            this.SetBoxStyle(Program.CcsFile.Character.Skill.Athletics.Expertise, this.AthleticsExpertieseBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Acrobatics.Expertise, this.AcrobaticsExpertieseBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.SleightOfHand.Expertise, this.SleightOfHandExpertieseBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Stealth.Expertise, this.StealthExpertieseBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Arcana.Expertise, this.ArcanaExpertieseBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.History.Expertise, this.HistoryExpertieseBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Investigation.Expertise, this.InvestigationExpertieseBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Nature.Expertise, this.NatureExpertieseBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Religion.Expertise, this.ReligionExpertieseBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.AnimalHandling.Expertise, this.AnimalHandlingExpertieseBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Insight.Expertise, this.InsightExpertieseBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Medicine.Expertise, this.MedicineExpertieseBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Perception.Expertise, this.PerceptionExpertieseBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Survival.Expertise, this.SurvivalExpertieseBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Deception.Expertise, this.DeceptionExpertieseBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Intimidation.Expertise, this.IntimidationExpertieseBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Performance.Expertise, this.PerformanceExpertieseBox);
            this.SetBoxStyle(Program.CcsFile.Character.Skill.Persuasion.Expertise, this.PersuasionExpertieseBox);
        }

        private void DrawHealth()
        {
            this.CurrentHpField.Text = Program.CcsFile.Character.Vitality.CurrentHealth.ToString();
            this.TotalHpField.Text = "/" + Program.CcsFile.Character.Vitality.MaxHealth.ToString();

            this.CurrentHpField.Foreground = this.SetHealthStyle();
            this.TotalHpField.Foreground = this.SetHealthStyle();
        }

        private void DrawArmorClass()
        {
            this.ArmorClassField.Text = Program.CcsFile.Character.Armor.TotalArmorClass.ToString();
        }

        private void DrawHitDice()
        {
            this.D6TotalField.Text = Program.CcsFile.Character.Vitality.HitDice.TotalD6.ToString();
            this.D6TotalField.Foreground = Utilities.SetTotalTextStyle(Program.CcsFile.Character.Vitality.HitDice.TotalD6, Program.CcsFile.Character.Vitality.HitDice.SpentD6);
            this.D6TotalBox.Background = Utilities.SetTotalBoxStyle(Program.CcsFile.Character.Vitality.HitDice.TotalD6, Program.CcsFile.Character.Vitality.HitDice.SpentD6);
            this.D6SpentField.Text = Program.CcsFile.Character.Vitality.HitDice.SpentD6.ToString();
            this.D6SpentField.Foreground = Utilities.SetUsedTextStyle(Program.CcsFile.Character.Vitality.HitDice.TotalD6, Program.CcsFile.Character.Vitality.HitDice.SpentD6);
            this.D6SpentBox.Background = Utilities.SetUsedBoxStyle(Program.CcsFile.Character.Vitality.HitDice.TotalD6, Program.CcsFile.Character.Vitality.HitDice.SpentD6);

            this.D8TotalField.Text = Program.CcsFile.Character.Vitality.HitDice.TotalD8.ToString();
            this.D8TotalField.Foreground = Utilities.SetTotalTextStyle(Program.CcsFile.Character.Vitality.HitDice.TotalD8, Program.CcsFile.Character.Vitality.HitDice.SpentD8);
            this.D8SpentField.Text = Program.CcsFile.Character.Vitality.HitDice.SpentD8.ToString();
            this.D8SpentField.Foreground = Utilities.SetUsedTextStyle(Program.CcsFile.Character.Vitality.HitDice.TotalD8, Program.CcsFile.Character.Vitality.HitDice.SpentD8);
            this.D8SpentBox.Background = Utilities.SetUsedBoxStyle(Program.CcsFile.Character.Vitality.HitDice.TotalD8, Program.CcsFile.Character.Vitality.HitDice.SpentD8);
            this.D8TotalBox.Background = Utilities.SetTotalBoxStyle(Program.CcsFile.Character.Vitality.HitDice.TotalD8, Program.CcsFile.Character.Vitality.HitDice.SpentD8);

            this.D10TotalField.Text = Program.CcsFile.Character.Vitality.HitDice.TotalD10.ToString();
            this.D10TotalField.Foreground = Utilities.SetTotalTextStyle(Program.CcsFile.Character.Vitality.HitDice.TotalD10, Program.CcsFile.Character.Vitality.HitDice.SpentD10);
            this.D10SpentField.Text = Program.CcsFile.Character.Vitality.HitDice.SpentD10.ToString();
            this.D10SpentField.Foreground = Utilities.SetUsedTextStyle(Program.CcsFile.Character.Vitality.HitDice.TotalD10, Program.CcsFile.Character.Vitality.HitDice.SpentD10);
            this.D10SpentBox.Background = Utilities.SetUsedBoxStyle(Program.CcsFile.Character.Vitality.HitDice.TotalD10, Program.CcsFile.Character.Vitality.HitDice.SpentD10);
            this.D10TotalBox.Background = Utilities.SetTotalBoxStyle(Program.CcsFile.Character.Vitality.HitDice.TotalD10, Program.CcsFile.Character.Vitality.HitDice.SpentD10);

            this.D12TotalField.Text = Program.CcsFile.Character.Vitality.HitDice.TotalD12.ToString();
            this.D12TotalField.Foreground = Utilities.SetTotalTextStyle(Program.CcsFile.Character.Vitality.HitDice.TotalD12, Program.CcsFile.Character.Vitality.HitDice.SpentD12);
            this.D12SpentField.Text = Program.CcsFile.Character.Vitality.HitDice.SpentD12.ToString();
            this.D12SpentField.Foreground = Utilities.SetUsedTextStyle(Program.CcsFile.Character.Vitality.HitDice.TotalD12, Program.CcsFile.Character.Vitality.HitDice.SpentD12);
            this.D12SpentBox.Background = Utilities.SetUsedBoxStyle(Program.CcsFile.Character.Vitality.HitDice.TotalD12, Program.CcsFile.Character.Vitality.HitDice.SpentD12);
            this.D12TotalBox.Background = Utilities.SetTotalBoxStyle(Program.CcsFile.Character.Vitality.HitDice.TotalD12, Program.CcsFile.Character.Vitality.HitDice.SpentD12);
        }

        private void DrawResourcePool()
        {
            this.SetArrowStyle();

            if (Program.CcsFile.Character.ClassResources.Count > 0)
            {
                ClassResource classResource = Program.CcsFile.Character.ClassResources[this.ResourceIndex];

                this.ResourceTypeField.Text = classResource.Type;

                this.ResourcePoolField.Text = classResource.Total.ToString();
                this.ResourcePoolBox.Background = Utilities.SetTotalBoxStyle(classResource.Total, classResource.Spent);

                this.ResourceSpentField.Text = classResource.Total.ToString();
                this.ResourceSpentBox.Background = Utilities.SetUsedBoxStyle(classResource.Total, classResource.Spent);
            }
            else
            {
                this.ResourceTypeField.Text = "None";

                this.ResourcePoolField.Text = "0";
                this.ResourcePoolBox.Background = Utilities.SetTotalBoxStyle(0, 0);

                this.ResourceSpentField.Text = "0";
                this.ResourceSpentBox.Background = Utilities.SetUsedBoxStyle(0, 0);
            }
        }

        private void SetArrowStyle()
        {
            this.LeftResourceButton.Foreground = this.ResourceIndex == 0 ? Brushes.DimGray : Brushes.White;

            this.RightResourceButton.Foreground = this.ResourceIndex == Program.CcsFile.Character.ClassResources.Count - 1 || Program.CcsFile.Character.ClassResources.Count == 0
                ? Brushes.DimGray
                : Brushes.White;
        }

        private Brush SetHealthStyle()
        {
            int third = Program.CcsFile.Character.Vitality.MaxHealth / 3;
            int hp = Program.CcsFile.Character.Vitality.CurrentHealth;

            return hp < third && hp > 0
                ? Brushes.IndianRed
                : hp >= third * 2 ? Brushes.DarkGreen : hp <= 0 ? Brushes.DarkGray : Brushes.DarkOrange;
        }

        private void SetBoxStyle(bool flag, Rectangle rectangle)
        {
            if (flag)
            {
                rectangle.Fill = new SolidColorBrush(Color.FromArgb(255, 6, 1, 31));
                rectangle.Stroke = Brushes.Transparent;
            }
            else
            {
                rectangle.Stroke = new SolidColorBrush(Color.FromArgb(255, 6, 1, 31));
                rectangle.Fill = Brushes.Transparent;
            }
        }

        private void SetTextStyle(StatusChecks check, TextBlock textBlock)
        {
            switch (check)
            {
                case StatusChecks.Fail:
                    textBlock.TextDecorations = TextDecorations.Strikethrough;
                    textBlock.Foreground = Brushes.DarkGray;
                    textBlock.ToolTip = "Automatic Fail";
                    break;
                case StatusChecks.Advantage:
                    textBlock.TextDecorations = new TextDecorationCollection();
                    textBlock.Foreground = Brushes.Green;
                    textBlock.ToolTip = "Advantage";
                    break;
                case StatusChecks.Disadvantage:
                    textBlock.TextDecorations = new TextDecorationCollection();
                    textBlock.Foreground = Brushes.IndianRed;
                    textBlock.ToolTip = "Disadvantage";
                    break;
                case StatusChecks.Normal:
                default:
                    textBlock.TextDecorations = new TextDecorationCollection();
                    textBlock.Foreground = Brushes.White;
                    textBlock.ToolTip = null;
                    break;
            }
        }

        private void LeftResourceButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ResourceIndex > 0)
            {
                this.ResourceIndex--;
                this.DrawResourcePool();
            }
        }

        private void RightResourceButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ResourceIndex < Program.CcsFile.Character.ClassResources.Count - 1)
            {
                this.ResourceIndex++;
                this.DrawResourcePool();
            }
        }

        private void SavingThrows_MouseDown(object sender, RoutedEventArgs e)
        {
            var rectangle = sender as Rectangle;

            switch (rectangle.Name)
            {
                case "StrengthProficiencyBox":
                    Program.CcsFile.Character.SavingThrow.Strength.Proficiency = !Program.CcsFile.Character.SavingThrow.Strength.Proficiency;
                    break;
                case "DexterityProficiencyBox":
                    Program.CcsFile.Character.SavingThrow.Dexterity.Proficiency = !Program.CcsFile.Character.SavingThrow.Dexterity.Proficiency;
                    break;
                case "ConstitutionProficiencyBox":
                    Program.CcsFile.Character.SavingThrow.Constitution.Proficiency = !Program.CcsFile.Character.SavingThrow.Constitution.Proficiency;
                    break;
                case "IntelligenceProficiencyBox":
                    Program.CcsFile.Character.SavingThrow.Intelligence.Proficiency = !Program.CcsFile.Character.SavingThrow.Intelligence.Proficiency;
                    break;
                case "WisdomProficiencyBox":
                    Program.CcsFile.Character.SavingThrow.Wisdom.Proficiency = !Program.CcsFile.Character.SavingThrow.Wisdom.Proficiency;
                    break;
                case "CharismaProficiencyBox":
                    Program.CcsFile.Character.SavingThrow.Charisma.Proficiency = !Program.CcsFile.Character.SavingThrow.Charisma.Proficiency;
                    break;
            }

            this.DrawSavingThrows();
        }

        private void SkillProficiency_MouseDown(object sender, RoutedEventArgs e)
        {
            var rectangle = sender as Rectangle;

            switch (rectangle.Name)
            {
                case "AthleticsProficiencyBox":
                    Program.CcsFile.Character.Skill.Athletics.Proficiency = !Program.CcsFile.Character.Skill.Athletics.Proficiency;
                    break;
                case "AcrobaticsProficiencyBox":
                    Program.CcsFile.Character.Skill.Acrobatics.Proficiency = !Program.CcsFile.Character.Skill.Acrobatics.Proficiency;
                    break;
                case "SleightOfHandProficiencyBox":
                    Program.CcsFile.Character.Skill.SleightOfHand.Proficiency = !Program.CcsFile.Character.Skill.SleightOfHand.Proficiency;
                    break;
                case "StealthProficiencyBox":
                    Program.CcsFile.Character.Skill.Stealth.Proficiency = !Program.CcsFile.Character.Skill.Stealth.Proficiency;
                    break;
                case "ArcanaProficiencyBox":
                    Program.CcsFile.Character.Skill.Arcana.Proficiency = !Program.CcsFile.Character.Skill.Arcana.Proficiency;
                    break;
                case "HistoryProficiencyBox":
                    Program.CcsFile.Character.Skill.History.Proficiency = !Program.CcsFile.Character.Skill.History.Proficiency;
                    break;
                case "InvestigationProficiencyBox":
                    Program.CcsFile.Character.Skill.Investigation.Proficiency = !Program.CcsFile.Character.Skill.Investigation.Proficiency;
                    break;
                case "NatureProficiencyBox":
                    Program.CcsFile.Character.Skill.Nature.Proficiency = !Program.CcsFile.Character.Skill.Nature.Proficiency;
                    break;
                case "ReligionProficiencyBox":
                    Program.CcsFile.Character.Skill.Religion.Proficiency = !Program.CcsFile.Character.Skill.Religion.Proficiency;
                    break;
                case "AnimalHandlingProficiencyBox":
                    Program.CcsFile.Character.Skill.AnimalHandling.Proficiency = !Program.CcsFile.Character.Skill.AnimalHandling.Proficiency;
                    break;
                case "InsightProficiencyBox":
                    Program.CcsFile.Character.Skill.Insight.Proficiency = !Program.CcsFile.Character.Skill.Insight.Proficiency;
                    break;
                case "MedicineProficiencyBox":
                    Program.CcsFile.Character.Skill.Medicine.Proficiency = !Program.CcsFile.Character.Skill.Medicine.Proficiency;
                    break;
                case "PerceptionProficiencyBox":
                    Program.CcsFile.Character.Skill.Perception.Proficiency = !Program.CcsFile.Character.Skill.Perception.Proficiency;
                    break;
                case "SurvivalProficiencyBox":
                    Program.CcsFile.Character.Skill.Survival.Proficiency = !Program.CcsFile.Character.Skill.Survival.Proficiency;
                    break;
                case "DeceptionProficiencyBox":
                    Program.CcsFile.Character.Skill.Deception.Proficiency = !Program.CcsFile.Character.Skill.Deception.Proficiency;
                    break;
                case "IntimidationProficiencyBox":
                    Program.CcsFile.Character.Skill.Intimidation.Proficiency = !Program.CcsFile.Character.Skill.Intimidation.Proficiency;
                    break;
                case "PerformanceProficiencyBox":
                    Program.CcsFile.Character.Skill.Performance.Proficiency = !Program.CcsFile.Character.Skill.Performance.Proficiency;
                    break;
                case "PersuasionProficiencyBox":
                    Program.CcsFile.Character.Skill.Persuasion.Proficiency = !Program.CcsFile.Character.Skill.Persuasion.Proficiency;
                    break;
            }

            this.DrawSkills();
        }

        private void SkillExpertise_MouseDown(object sender, RoutedEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;

            switch (rectangle.Name)
            {
                case "AthleticsExpertieseBox":
                    Program.CcsFile.Character.Skill.Athletics.Expertise = !Program.CcsFile.Character.Skill.Athletics.Expertise;
                    break;
                case "AcrobaticsExpertieseBox":
                    Program.CcsFile.Character.Skill.Acrobatics.Expertise = !Program.CcsFile.Character.Skill.Acrobatics.Expertise;
                    break;
                case "SleightOfHandExpertieseBox":
                    Program.CcsFile.Character.Skill.SleightOfHand.Expertise = !Program.CcsFile.Character.Skill.SleightOfHand.Expertise;
                    break;
                case "StealthExpertieseBox":
                    Program.CcsFile.Character.Skill.Stealth.Expertise = !Program.CcsFile.Character.Skill.Stealth.Expertise;
                    break;
                case "ArcanaExpertieseBox":
                    Program.CcsFile.Character.Skill.Arcana.Expertise = !Program.CcsFile.Character.Skill.Arcana.Expertise;
                    break;
                case "HistoryExpertieseBox":
                    Program.CcsFile.Character.Skill.History.Expertise = !Program.CcsFile.Character.Skill.History.Expertise;
                    break;
                case "InvestigationExpertieseBox":
                    Program.CcsFile.Character.Skill.Investigation.Expertise = !Program.CcsFile.Character.Skill.Investigation.Expertise;
                    break;
                case "NatureExpertieseBox":
                    Program.CcsFile.Character.Skill.Nature.Expertise = !Program.CcsFile.Character.Skill.Nature.Expertise;
                    break;
                case "ReligionExpertieseBox":
                    Program.CcsFile.Character.Skill.Religion.Expertise = !Program.CcsFile.Character.Skill.Religion.Expertise;
                    break;
                case "AnimalHandlingExpertieseBox":
                    Program.CcsFile.Character.Skill.AnimalHandling.Expertise = !Program.CcsFile.Character.Skill.AnimalHandling.Expertise;
                    break;
                case "InsightExpertieseBox":
                    Program.CcsFile.Character.Skill.Insight.Expertise = !Program.CcsFile.Character.Skill.Insight.Expertise;
                    break;
                case "MedicineExpertieseBox":
                    Program.CcsFile.Character.Skill.Medicine.Expertise = !Program.CcsFile.Character.Skill.Medicine.Expertise;
                    break;
                case "PerceptionExpertieseBox":
                    Program.CcsFile.Character.Skill.Perception.Expertise = !Program.CcsFile.Character.Skill.Perception.Expertise;
                    break;
                case "SurvivalExpertieseBox":
                    Program.CcsFile.Character.Skill.Survival.Expertise = !Program.CcsFile.Character.Skill.Survival.Expertise;
                    break;
                case "DeceptionExpertieseBox":
                    Program.CcsFile.Character.Skill.Deception.Expertise = !Program.CcsFile.Character.Skill.Deception.Expertise;
                    break;
                case "IntimidationExpertieseBox":
                    Program.CcsFile.Character.Skill.Intimidation.Expertise = !Program.CcsFile.Character.Skill.Intimidation.Expertise;
                    break;
                case "PerformanceExpertieseBox":
                    Program.CcsFile.Character.Skill.Performance.Expertise = !Program.CcsFile.Character.Skill.Performance.Expertise;
                    break;
                case "PersuasionExpertieseBox":
                    Program.CcsFile.Character.Skill.Persuasion.Expertise = !Program.CcsFile.Character.Skill.Persuasion.Expertise;
                    break;
            }

            this.DrawSkills();
        }

        private void ToggleBox_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void ToggleBox_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void EditAttributesButton_Click(object sender, RoutedEventArgs e)
        {
            this.ModifyAttributesWindow.EditAttributes();
            this.DrawAttributes();
            this.DrawSavingThrows();
            this.DrawSkills();
        }

        private void EditSensesButton_Click(object sender, RoutedEventArgs e)
        {
            this.ModifySensesWindow.EditSenses();
            this.DrawDetails();
        }

        private void EditHealthButton_Click(object sender, RoutedEventArgs e)
        {
            this.ModifyHealthWindow.EditHealth();
            this.DrawHealth();
        }

        private void EditHitDiceButton_Click(object sender, RoutedEventArgs e)
        {
            this.ModifyHitDiceWindow.ModifyHitDice();
            this.DrawHitDice();
        }

        private void EditResourceButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void AddResourceButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void TakeDamageButton_Click(object sender, RoutedEventArgs e)
        {
            this.ModifyHpWindow.SubtractHP();
            this.DrawHealth();
        }

        private void HealDamageButton_Click(object sender, RoutedEventArgs e)
        {
            this.ModifyHpWindow.AddHP();
            this.DrawHealth();
        }

        private void D6SpentBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Program.CcsFile.Character.Vitality.HitDice.SpentD6++;
            this.DrawHitDice();

            if (Program.CcsFile.Character.Vitality.HitDice.SpentD6 == Program.CcsFile.Character.Vitality.HitDice.TotalD6)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void D8SpentBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Program.CcsFile.Character.Vitality.HitDice.SpentD8++;
            this.DrawHitDice();

            if (Program.CcsFile.Character.Vitality.HitDice.SpentD8 == Program.CcsFile.Character.Vitality.HitDice.TotalD8)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void D10SpentBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Program.CcsFile.Character.Vitality.HitDice.SpentD10++;
            this.DrawHitDice();

            if (Program.CcsFile.Character.Vitality.HitDice.SpentD10 == Program.CcsFile.Character.Vitality.HitDice.TotalD10)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void D12SpentBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Program.CcsFile.Character.Vitality.HitDice.SpentD12++;
            this.DrawHitDice();

            if (Program.CcsFile.Character.Vitality.HitDice.SpentD12 == Program.CcsFile.Character.Vitality.HitDice.TotalD12)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void D6SpentBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Program.CcsFile.Character.Vitality.HitDice.SpentD6 != Program.CcsFile.Character.Vitality.HitDice.TotalD6)
            {
                Mouse.OverrideCursor = Cursors.Hand;
            }
        }

        private void D6SpentBox_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void D8SpentBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Program.CcsFile.Character.Vitality.HitDice.SpentD8 != Program.CcsFile.Character.Vitality.HitDice.TotalD8)
            {
                Mouse.OverrideCursor = Cursors.Hand;
            }
        }

        private void D8SpentBox_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void D10SpentBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Program.CcsFile.Character.Vitality.HitDice.SpentD10 != Program.CcsFile.Character.Vitality.HitDice.TotalD10)
            {
                Mouse.OverrideCursor = Cursors.Hand;
            }
        }

        private void D10SpentBox_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void D12SpentBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Program.CcsFile.Character.Vitality.HitDice.SpentD12 != Program.CcsFile.Character.Vitality.HitDice.TotalD12)
            {
                Mouse.OverrideCursor = Cursors.Hand;
            }
        }

        private void D12SpentBox_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
