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
            this.StrengthBonusField.Text = Utilities.CalculateBonus(Program.Character.Attributes.Strength).ToString();
            this.DexterityBonusField.Text = Utilities.CalculateBonus(Program.Character.Attributes.Dexterity).ToString();
            this.ConstitutionBonusField.Text = Utilities.CalculateBonus(Program.Character.Attributes.Constitution).ToString();
            this.IntelligenceBonusField.Text = Utilities.CalculateBonus(Program.Character.Attributes.Intelligence).ToString();
            this.WisdomBonusField.Text = Utilities.CalculateBonus(Program.Character.Attributes.Wisdom).ToString();
            this.CharismaBonusField.Text = Utilities.CalculateBonus(Program.Character.Attributes.Charisma).ToString();

            this.StrengthScoreField.Text = Program.Character.Attributes.Strength.ToString();
            this.DexterityScoreField.Text = Program.Character.Attributes.Dexterity.ToString();
            this.ConstitutionScoreField.Text = Program.Character.Attributes.Constitution.ToString();
            this.IntelligenceScoreField.Text = Program.Character.Attributes.Intelligence.ToString();
            this.WisdomScoreField.Text = Program.Character.Attributes.Wisdom.ToString();
            this.CharismaScoreField.Text = Program.Character.Attributes.Charisma.ToString();
        }

        private void DrawDetails()
        {
            this.InitiativeField.Text = Program.Character.Initiative.ToString();
            this.PassivePerceptionField.Text = Program.Character.PassivePerception.ToString();
            this.VisionField.Text = Program.Character.Details.Vision.ToString();
            this.MovementSpeedField.Text = Program.Character.Details.Movement.ToString();
        }

        private void DrawSavingThrows()
        {
            this.StrengthSavingThrowField.Text = Program.Character.SavingThrow.Strength.Bonus.ToString();
            this.DexteritySavingThrowField.Text = Program.Character.SavingThrow.Dexterity.Bonus.ToString();
            this.ConstitutionSavingThrowField.Text = Program.Character.SavingThrow.Constitution.Bonus.ToString();
            this.IntelligenceSavingThrowField.Text = Program.Character.SavingThrow.Intelligence.Bonus.ToString();
            this.WisdomSavingThrowField.Text = Program.Character.SavingThrow.Wisdom.Bonus.ToString();
            this.CharismaSavingThrowField.Text = Program.Character.SavingThrow.Charisma.Bonus.ToString();

            this.SetTextStyle(Program.Character.SavingThrow.Strength.StatusChecks, this.StrengthSavingThrowField);
            this.SetTextStyle(Program.Character.SavingThrow.Dexterity.StatusChecks, this.DexteritySavingThrowField);
            this.SetTextStyle(Program.Character.SavingThrow.Constitution.StatusChecks, this.ConstitutionSavingThrowField);
            this.SetTextStyle(Program.Character.SavingThrow.Intelligence.StatusChecks, this.IntelligenceSavingThrowField);
            this.SetTextStyle(Program.Character.SavingThrow.Wisdom.StatusChecks, this.WisdomSavingThrowField);
            this.SetTextStyle(Program.Character.SavingThrow.Charisma.StatusChecks, this.CharismaSavingThrowField);

            this.SetTextStyle(Program.Character.SavingThrow.Strength.StatusChecks, this.StrengthSavingThrowName);
            this.SetTextStyle(Program.Character.SavingThrow.Dexterity.StatusChecks, this.DexteritySavingThrowName);
            this.SetTextStyle(Program.Character.SavingThrow.Constitution.StatusChecks, this.ConstitutionSavingThrowName);
            this.SetTextStyle(Program.Character.SavingThrow.Intelligence.StatusChecks, this.IntelligenceSavingThrowName);
            this.SetTextStyle(Program.Character.SavingThrow.Wisdom.StatusChecks, this.WisdomSavingThrowName);
            this.SetTextStyle(Program.Character.SavingThrow.Charisma.StatusChecks, this.CharismaSavingThrowName);

            this.SetBoxStyle(Program.Character.SavingThrow.Strength.Proficiency, this.StrengthProficiencyBox);
            this.SetBoxStyle(Program.Character.SavingThrow.Dexterity.Proficiency, this.DexterityProficiencyBox);
            this.SetBoxStyle(Program.Character.SavingThrow.Constitution.Proficiency, this.ConstitutionProficiencyBox);
            this.SetBoxStyle(Program.Character.SavingThrow.Intelligence.Proficiency, this.IntelligenceProficiencyBox);
            this.SetBoxStyle(Program.Character.SavingThrow.Wisdom.Proficiency, this.WisdomProficiencyBox);
            this.SetBoxStyle(Program.Character.SavingThrow.Charisma.Proficiency, this.CharismaProficiencyBox);
        }

        private void DrawSkills()
        {
            this.AthleticsSkillField.Text = Program.Character.Skill.Athletics.Bonus.ToString();
            this.AcrobaticsSkillField.Text = Program.Character.Skill.Acrobatics.Bonus.ToString();
            this.SleightOfHandSkillField.Text = Program.Character.Skill.SleightOfHand.Bonus.ToString();
            this.StealthSkillField.Text = Program.Character.Skill.Stealth.Bonus.ToString();
            this.ArcanaSkillField.Text = Program.Character.Skill.Arcana.Bonus.ToString();
            this.HistorySkillField.Text = Program.Character.Skill.History.Bonus.ToString();
            this.InvestigationSkillField.Text = Program.Character.Skill.Investigation.Bonus.ToString();
            this.NatureSkillField.Text = Program.Character.Skill.Nature.Bonus.ToString();
            this.ReligionSkillField.Text = Program.Character.Skill.Religion.Bonus.ToString();
            this.AnimalHandlingSkillField.Text = Program.Character.Skill.AnimalHandling.Bonus.ToString();
            this.InsightSkillField.Text = Program.Character.Skill.Insight.Bonus.ToString();
            this.MedicineSkillField.Text = Program.Character.Skill.Medicine.Bonus.ToString();
            this.PerceptionSkillField.Text = Program.Character.Skill.Perception.Bonus.ToString();
            this.SurvivalSkillField.Text = Program.Character.Skill.Survival.Bonus.ToString();
            this.DeceptionSkillField.Text = Program.Character.Skill.Deception.Bonus.ToString();
            this.IntimidationSkillField.Text = Program.Character.Skill.Intimidation.Bonus.ToString();
            this.PerformanceSkillField.Text = Program.Character.Skill.Performance.Bonus.ToString();
            this.PersuasionSkillField.Text = Program.Character.Skill.Persuasion.Bonus.ToString();

            this.SetTextStyle(Program.Character.Skill.Athletics.Checks, this.AthleticsSkillField);
            this.SetTextStyle(Program.Character.Skill.Acrobatics.Checks, this.AcrobaticsSkillField);
            this.SetTextStyle(Program.Character.Skill.SleightOfHand.Checks, this.SleightOfHandSkillField);
            this.SetTextStyle(Program.Character.Skill.Stealth.Checks, this.StealthSkillField);
            this.SetTextStyle(Program.Character.Skill.Arcana.Checks, this.ArcanaSkillField);
            this.SetTextStyle(Program.Character.Skill.History.Checks, this.HistorySkillField);
            this.SetTextStyle(Program.Character.Skill.Investigation.Checks, this.InvestigationSkillField);
            this.SetTextStyle(Program.Character.Skill.Nature.Checks, this.NatureSkillField);
            this.SetTextStyle(Program.Character.Skill.Religion.Checks, this.ReligionSkillField);
            this.SetTextStyle(Program.Character.Skill.AnimalHandling.Checks, this.AnimalHandlingSkillField);
            this.SetTextStyle(Program.Character.Skill.Insight.Checks, this.InsightSkillField);
            this.SetTextStyle(Program.Character.Skill.Medicine.Checks, this.MedicineSkillField);
            this.SetTextStyle(Program.Character.Skill.Perception.Checks, this.PerceptionSkillField);
            this.SetTextStyle(Program.Character.Skill.Survival.Checks, this.SurvivalSkillField);
            this.SetTextStyle(Program.Character.Skill.Deception.Checks, this.DeceptionSkillField);
            this.SetTextStyle(Program.Character.Skill.Intimidation.Checks, this.IntimidationSkillField);
            this.SetTextStyle(Program.Character.Skill.Performance.Checks, this.PerformanceSkillField);
            this.SetTextStyle(Program.Character.Skill.Persuasion.Checks, this.PersuasionSkillField);

            this.SetTextStyle(Program.Character.Skill.Athletics.Checks, this.AthleticsSkillName);
            this.SetTextStyle(Program.Character.Skill.Acrobatics.Checks, this.AcrobaticsSkillName);
            this.SetTextStyle(Program.Character.Skill.SleightOfHand.Checks, this.SleightOfHandSkillName);
            this.SetTextStyle(Program.Character.Skill.Stealth.Checks, this.StealthSkillName);
            this.SetTextStyle(Program.Character.Skill.Arcana.Checks, this.ArcanaSkillName);
            this.SetTextStyle(Program.Character.Skill.History.Checks, this.HistorySkillName);
            this.SetTextStyle(Program.Character.Skill.Investigation.Checks, this.InvestigationSkillName);
            this.SetTextStyle(Program.Character.Skill.Nature.Checks, this.NatureSkillName);
            this.SetTextStyle(Program.Character.Skill.Religion.Checks, this.ReligionSkillName);
            this.SetTextStyle(Program.Character.Skill.AnimalHandling.Checks, this.AnimalHandlingSkillName);
            this.SetTextStyle(Program.Character.Skill.Insight.Checks, this.InsightSkillName);
            this.SetTextStyle(Program.Character.Skill.Medicine.Checks, this.MedicineSkillName);
            this.SetTextStyle(Program.Character.Skill.Perception.Checks, this.PerceptionSkillName);
            this.SetTextStyle(Program.Character.Skill.Survival.Checks, this.SurvivalSkillName);
            this.SetTextStyle(Program.Character.Skill.Deception.Checks, this.DeceptionSkillName);
            this.SetTextStyle(Program.Character.Skill.Intimidation.Checks, this.IntimidationSkillName);
            this.SetTextStyle(Program.Character.Skill.Performance.Checks, this.PerformanceSkillName);
            this.SetTextStyle(Program.Character.Skill.Persuasion.Checks, this.PersuasionSkillName);

            this.SetBoxStyle(Program.Character.Skill.Athletics.Proficiency, this.AthleticsProficiencyBox);
            this.SetBoxStyle(Program.Character.Skill.Acrobatics.Proficiency, this.AcrobaticsProficiencyBox);
            this.SetBoxStyle(Program.Character.Skill.SleightOfHand.Proficiency, this.SleightOfHandProficiencyBox);
            this.SetBoxStyle(Program.Character.Skill.Stealth.Proficiency, this.StealthProficiencyBox);
            this.SetBoxStyle(Program.Character.Skill.Arcana.Proficiency, this.ArcanaProficiencyBox);
            this.SetBoxStyle(Program.Character.Skill.History.Proficiency, this.HistoryProficiencyBox);
            this.SetBoxStyle(Program.Character.Skill.Investigation.Proficiency, this.InvestigationProficiencyBox);
            this.SetBoxStyle(Program.Character.Skill.Nature.Proficiency, this.NatureProficiencyBox);
            this.SetBoxStyle(Program.Character.Skill.Religion.Proficiency, this.ReligionProficiencyBox);
            this.SetBoxStyle(Program.Character.Skill.AnimalHandling.Proficiency, this.AnimalHandlingProficiencyBox);
            this.SetBoxStyle(Program.Character.Skill.Insight.Proficiency, this.InsightProficiencyBox);
            this.SetBoxStyle(Program.Character.Skill.Medicine.Proficiency, this.MedicineProficiencyBox);
            this.SetBoxStyle(Program.Character.Skill.Perception.Proficiency, this.PerceptionProficiencyBox);
            this.SetBoxStyle(Program.Character.Skill.Survival.Proficiency, this.SurvivalProficiencyBox);
            this.SetBoxStyle(Program.Character.Skill.Deception.Proficiency, this.DeceptionProficiencyBox);
            this.SetBoxStyle(Program.Character.Skill.Intimidation.Proficiency, this.IntimidationProficiencyBox);
            this.SetBoxStyle(Program.Character.Skill.Performance.Proficiency, this.PerformanceProficiencyBox);
            this.SetBoxStyle(Program.Character.Skill.Persuasion.Proficiency, this.PersuasionProficiencyBox);

            this.SetBoxStyle(Program.Character.Skill.Athletics.Expertise, this.AthleticsExpertieseBox);
            this.SetBoxStyle(Program.Character.Skill.Acrobatics.Expertise, this.AcrobaticsExpertieseBox);
            this.SetBoxStyle(Program.Character.Skill.SleightOfHand.Expertise, this.SleightOfHandExpertieseBox);
            this.SetBoxStyle(Program.Character.Skill.Stealth.Expertise, this.StealthExpertieseBox);
            this.SetBoxStyle(Program.Character.Skill.Arcana.Expertise, this.ArcanaExpertieseBox);
            this.SetBoxStyle(Program.Character.Skill.History.Expertise, this.HistoryExpertieseBox);
            this.SetBoxStyle(Program.Character.Skill.Investigation.Expertise, this.InvestigationExpertieseBox);
            this.SetBoxStyle(Program.Character.Skill.Nature.Expertise, this.NatureExpertieseBox);
            this.SetBoxStyle(Program.Character.Skill.Religion.Expertise, this.ReligionExpertieseBox);
            this.SetBoxStyle(Program.Character.Skill.AnimalHandling.Expertise, this.AnimalHandlingExpertieseBox);
            this.SetBoxStyle(Program.Character.Skill.Insight.Expertise, this.InsightExpertieseBox);
            this.SetBoxStyle(Program.Character.Skill.Medicine.Expertise, this.MedicineExpertieseBox);
            this.SetBoxStyle(Program.Character.Skill.Perception.Expertise, this.PerceptionExpertieseBox);
            this.SetBoxStyle(Program.Character.Skill.Survival.Expertise, this.SurvivalExpertieseBox);
            this.SetBoxStyle(Program.Character.Skill.Deception.Expertise, this.DeceptionExpertieseBox);
            this.SetBoxStyle(Program.Character.Skill.Intimidation.Expertise, this.IntimidationExpertieseBox);
            this.SetBoxStyle(Program.Character.Skill.Performance.Expertise, this.PerformanceExpertieseBox);
            this.SetBoxStyle(Program.Character.Skill.Persuasion.Expertise, this.PersuasionExpertieseBox);
        }

        private void DrawHealth()
        {
            this.CurrentHpField.Text = Program.Character.Vitality.CurrentHealth.ToString();
            this.TotalHpField.Text = "/" + Program.Character.Vitality.MaxHealth.ToString();

            this.CurrentHpField.Foreground = this.SetHealthStyle();
            this.TotalHpField.Foreground = this.SetHealthStyle();
        }

        private void DrawArmorClass()
        {
            this.ArmorClassField.Text = Program.Character.Armor.TotalArmorClass.ToString();
        }

        private void DrawHitDice()
        {
            this.D6TotalField.Text = Program.Character.Vitality.HitDice.TotalD6.ToString();
            this.D6TotalField.Foreground = Utilities.SetTotalTextStyle(Program.Character.Vitality.HitDice.TotalD6, Program.Character.Vitality.HitDice.SpentD6);
            this.D6TotalBox.Background = Utilities.SetTotalBoxStyle(Program.Character.Vitality.HitDice.TotalD6, Program.Character.Vitality.HitDice.SpentD6);
            this.D6SpentField.Text = Program.Character.Vitality.HitDice.SpentD6.ToString();
            this.D6SpentField.Foreground = Utilities.SetUsedTextStyle(Program.Character.Vitality.HitDice.TotalD6, Program.Character.Vitality.HitDice.SpentD6);
            this.D6SpentBox.Background = Utilities.SetUsedBoxStyle(Program.Character.Vitality.HitDice.TotalD6, Program.Character.Vitality.HitDice.SpentD6);

            this.D8TotalField.Text = Program.Character.Vitality.HitDice.TotalD8.ToString();
            this.D8TotalField.Foreground = Utilities.SetTotalTextStyle(Program.Character.Vitality.HitDice.TotalD8, Program.Character.Vitality.HitDice.SpentD8);
            this.D8SpentField.Text = Program.Character.Vitality.HitDice.SpentD8.ToString();
            this.D8SpentField.Foreground = Utilities.SetUsedTextStyle(Program.Character.Vitality.HitDice.TotalD8, Program.Character.Vitality.HitDice.SpentD8);
            this.D8SpentBox.Background = Utilities.SetUsedBoxStyle(Program.Character.Vitality.HitDice.TotalD8, Program.Character.Vitality.HitDice.SpentD8);
            this.D8TotalBox.Background = Utilities.SetTotalBoxStyle(Program.Character.Vitality.HitDice.TotalD8, Program.Character.Vitality.HitDice.SpentD8);

            this.D10TotalField.Text = Program.Character.Vitality.HitDice.TotalD10.ToString();
            this.D10TotalField.Foreground = Utilities.SetTotalTextStyle(Program.Character.Vitality.HitDice.TotalD10, Program.Character.Vitality.HitDice.SpentD10);
            this.D10SpentField.Text = Program.Character.Vitality.HitDice.SpentD10.ToString();
            this.D10SpentField.Foreground = Utilities.SetUsedTextStyle(Program.Character.Vitality.HitDice.TotalD10, Program.Character.Vitality.HitDice.SpentD10);
            this.D10SpentBox.Background = Utilities.SetUsedBoxStyle(Program.Character.Vitality.HitDice.TotalD10, Program.Character.Vitality.HitDice.SpentD10);
            this.D10TotalBox.Background = Utilities.SetTotalBoxStyle(Program.Character.Vitality.HitDice.TotalD10, Program.Character.Vitality.HitDice.SpentD10);

            this.D12TotalField.Text = Program.Character.Vitality.HitDice.TotalD12.ToString();
            this.D12TotalField.Foreground = Utilities.SetTotalTextStyle(Program.Character.Vitality.HitDice.TotalD12, Program.Character.Vitality.HitDice.SpentD12);
            this.D12SpentField.Text = Program.Character.Vitality.HitDice.SpentD12.ToString();
            this.D12SpentField.Foreground = Utilities.SetUsedTextStyle(Program.Character.Vitality.HitDice.TotalD12, Program.Character.Vitality.HitDice.SpentD12);
            this.D12SpentBox.Background = Utilities.SetUsedBoxStyle(Program.Character.Vitality.HitDice.TotalD12, Program.Character.Vitality.HitDice.SpentD12);
            this.D12TotalBox.Background = Utilities.SetTotalBoxStyle(Program.Character.Vitality.HitDice.TotalD12, Program.Character.Vitality.HitDice.SpentD12);
        }

        private void DrawResourcePool()
        {
            this.SetArrowStyle();

            if (Program.Character.ClassResources.Count > 0)
            {
                ClassResource classResource = Program.Character.ClassResources[this.ResourceIndex];

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

            this.RightResourceButton.Foreground = this.ResourceIndex == Program.Character.ClassResources.Count - 1 || Program.Character.ClassResources.Count == 0
                ? Brushes.DimGray
                : Brushes.White;
        }

        private Brush SetHealthStyle()
        {
            int third = Program.Character.Vitality.MaxHealth / 3;
            int hp = Program.Character.Vitality.CurrentHealth;

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
            if (this.ResourceIndex < Program.Character.ClassResources.Count - 1)
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
                    Program.Character.SavingThrow.Strength.Proficiency = !Program.Character.SavingThrow.Strength.Proficiency;
                    break;
                case "DexterityProficiencyBox":
                    Program.Character.SavingThrow.Dexterity.Proficiency = !Program.Character.SavingThrow.Dexterity.Proficiency;
                    break;
                case "ConstitutionProficiencyBox":
                    Program.Character.SavingThrow.Constitution.Proficiency = !Program.Character.SavingThrow.Constitution.Proficiency;
                    break;
                case "IntelligenceProficiencyBox":
                    Program.Character.SavingThrow.Intelligence.Proficiency = !Program.Character.SavingThrow.Intelligence.Proficiency;
                    break;
                case "WisdomProficiencyBox":
                    Program.Character.SavingThrow.Wisdom.Proficiency = !Program.Character.SavingThrow.Wisdom.Proficiency;
                    break;
                case "CharismaProficiencyBox":
                    Program.Character.SavingThrow.Charisma.Proficiency = !Program.Character.SavingThrow.Charisma.Proficiency;
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
                    Program.Character.Skill.Athletics.Proficiency = !Program.Character.Skill.Athletics.Proficiency;
                    break;
                case "AcrobaticsProficiencyBox":
                    Program.Character.Skill.Acrobatics.Proficiency = !Program.Character.Skill.Acrobatics.Proficiency;
                    break;
                case "SleightOfHandProficiencyBox":
                    Program.Character.Skill.SleightOfHand.Proficiency = !Program.Character.Skill.SleightOfHand.Proficiency;
                    break;
                case "StealthProficiencyBox":
                    Program.Character.Skill.Stealth.Proficiency = !Program.Character.Skill.Stealth.Proficiency;
                    break;
                case "ArcanaProficiencyBox":
                    Program.Character.Skill.Arcana.Proficiency = !Program.Character.Skill.Arcana.Proficiency;
                    break;
                case "HistoryProficiencyBox":
                    Program.Character.Skill.History.Proficiency = !Program.Character.Skill.History.Proficiency;
                    break;
                case "InvestigationProficiencyBox":
                    Program.Character.Skill.Investigation.Proficiency = !Program.Character.Skill.Investigation.Proficiency;
                    break;
                case "NatureProficiencyBox":
                    Program.Character.Skill.Nature.Proficiency = !Program.Character.Skill.Nature.Proficiency;
                    break;
                case "ReligionProficiencyBox":
                    Program.Character.Skill.Religion.Proficiency = !Program.Character.Skill.Religion.Proficiency;
                    break;
                case "AnimalHandlingProficiencyBox":
                    Program.Character.Skill.AnimalHandling.Proficiency = !Program.Character.Skill.AnimalHandling.Proficiency;
                    break;
                case "InsightProficiencyBox":
                    Program.Character.Skill.Insight.Proficiency = !Program.Character.Skill.Insight.Proficiency;
                    break;
                case "MedicineProficiencyBox":
                    Program.Character.Skill.Medicine.Proficiency = !Program.Character.Skill.Medicine.Proficiency;
                    break;
                case "PerceptionProficiencyBox":
                    Program.Character.Skill.Perception.Proficiency = !Program.Character.Skill.Perception.Proficiency;
                    break;
                case "SurvivalProficiencyBox":
                    Program.Character.Skill.Survival.Proficiency = !Program.Character.Skill.Survival.Proficiency;
                    break;
                case "DeceptionProficiencyBox":
                    Program.Character.Skill.Deception.Proficiency = !Program.Character.Skill.Deception.Proficiency;
                    break;
                case "IntimidationProficiencyBox":
                    Program.Character.Skill.Intimidation.Proficiency = !Program.Character.Skill.Intimidation.Proficiency;
                    break;
                case "PerformanceProficiencyBox":
                    Program.Character.Skill.Performance.Proficiency = !Program.Character.Skill.Performance.Proficiency;
                    break;
                case "PersuasionProficiencyBox":
                    Program.Character.Skill.Persuasion.Proficiency = !Program.Character.Skill.Persuasion.Proficiency;
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
                    Program.Character.Skill.Athletics.Expertise = !Program.Character.Skill.Athletics.Expertise;
                    break;
                case "AcrobaticsExpertieseBox":
                    Program.Character.Skill.Acrobatics.Expertise = !Program.Character.Skill.Acrobatics.Expertise;
                    break;
                case "SleightOfHandExpertieseBox":
                    Program.Character.Skill.SleightOfHand.Expertise = !Program.Character.Skill.SleightOfHand.Expertise;
                    break;
                case "StealthExpertieseBox":
                    Program.Character.Skill.Stealth.Expertise = !Program.Character.Skill.Stealth.Expertise;
                    break;
                case "ArcanaExpertieseBox":
                    Program.Character.Skill.Arcana.Expertise = !Program.Character.Skill.Arcana.Expertise;
                    break;
                case "HistoryExpertieseBox":
                    Program.Character.Skill.History.Expertise = !Program.Character.Skill.History.Expertise;
                    break;
                case "InvestigationExpertieseBox":
                    Program.Character.Skill.Investigation.Expertise = !Program.Character.Skill.Investigation.Expertise;
                    break;
                case "NatureExpertieseBox":
                    Program.Character.Skill.Nature.Expertise = !Program.Character.Skill.Nature.Expertise;
                    break;
                case "ReligionExpertieseBox":
                    Program.Character.Skill.Religion.Expertise = !Program.Character.Skill.Religion.Expertise;
                    break;
                case "AnimalHandlingExpertieseBox":
                    Program.Character.Skill.AnimalHandling.Expertise = !Program.Character.Skill.AnimalHandling.Expertise;
                    break;
                case "InsightExpertieseBox":
                    Program.Character.Skill.Insight.Expertise = !Program.Character.Skill.Insight.Expertise;
                    break;
                case "MedicineExpertieseBox":
                    Program.Character.Skill.Medicine.Expertise = !Program.Character.Skill.Medicine.Expertise;
                    break;
                case "PerceptionExpertieseBox":
                    Program.Character.Skill.Perception.Expertise = !Program.Character.Skill.Perception.Expertise;
                    break;
                case "SurvivalExpertieseBox":
                    Program.Character.Skill.Survival.Expertise = !Program.Character.Skill.Survival.Expertise;
                    break;
                case "DeceptionExpertieseBox":
                    Program.Character.Skill.Deception.Expertise = !Program.Character.Skill.Deception.Expertise;
                    break;
                case "IntimidationExpertieseBox":
                    Program.Character.Skill.Intimidation.Expertise = !Program.Character.Skill.Intimidation.Expertise;
                    break;
                case "PerformanceExpertieseBox":
                    Program.Character.Skill.Performance.Expertise = !Program.Character.Skill.Performance.Expertise;
                    break;
                case "PersuasionExpertieseBox":
                    Program.Character.Skill.Persuasion.Expertise = !Program.Character.Skill.Persuasion.Expertise;
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
            Program.Character.Vitality.HitDice.SpentD6++;
            this.DrawHitDice();

            if (Program.Character.Vitality.HitDice.SpentD6 == Program.Character.Vitality.HitDice.TotalD6)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void D8SpentBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Program.Character.Vitality.HitDice.SpentD8++;
            this.DrawHitDice();

            if (Program.Character.Vitality.HitDice.SpentD8 == Program.Character.Vitality.HitDice.TotalD8)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void D10SpentBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Program.Character.Vitality.HitDice.SpentD10++;
            this.DrawHitDice();

            if (Program.Character.Vitality.HitDice.SpentD10 == Program.Character.Vitality.HitDice.TotalD10)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void D12SpentBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Program.Character.Vitality.HitDice.SpentD12++;
            this.DrawHitDice();

            if (Program.Character.Vitality.HitDice.SpentD12 == Program.Character.Vitality.HitDice.TotalD12)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void D6SpentBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Program.Character.Vitality.HitDice.SpentD6 != Program.Character.Vitality.HitDice.TotalD6)
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
            if (Program.Character.Vitality.HitDice.SpentD8 != Program.Character.Vitality.HitDice.TotalD8)
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
            if (Program.Character.Vitality.HitDice.SpentD10 != Program.Character.Vitality.HitDice.TotalD10)
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
            if (Program.Character.Vitality.HitDice.SpentD12 != Program.Character.Vitality.HitDice.TotalD12)
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
