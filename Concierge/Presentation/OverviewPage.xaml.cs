using Concierge.Characters;
using Concierge.Characters.Collections;
using Concierge.Utility;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Concierge.Presentation
{
    /// <summary>
    /// Interaction logic for OverviewPage.xaml
    /// </summary>
    public partial class OverviewPage : Page
    {

        #region Constructor

        public OverviewPage()
        {
            InitializeComponent();
            DataContext = this;
            ResourceIndex = 0;

            StrengthProficiencyBox.MouseDown += SavingThrows_MouseDown;
            DexterityProficiencyBox.MouseDown += SavingThrows_MouseDown;
            ConstitutionProficiencyBox.MouseDown += SavingThrows_MouseDown;
            IntelligenceProficiencyBox.MouseDown += SavingThrows_MouseDown;
            WisdomProficiencyBox.MouseDown += SavingThrows_MouseDown;
            CharismaProficiencyBox.MouseDown += SavingThrows_MouseDown;

            AthleticsProficiencyBox.MouseDown += SkillProficiency_MouseDown;
            AcrobaticsProficiencyBox.MouseDown += SkillProficiency_MouseDown;
            SleightOfHandProficiencyBox.MouseDown += SkillProficiency_MouseDown;
            StealthProficiencyBox.MouseDown += SkillProficiency_MouseDown;
            ArcanaProficiencyBox.MouseDown += SkillProficiency_MouseDown;
            HistoryProficiencyBox.MouseDown += SkillProficiency_MouseDown;
            InvestigationProficiencyBox.MouseDown += SkillProficiency_MouseDown;
            NatureProficiencyBox.MouseDown += SkillProficiency_MouseDown;
            ReligionProficiencyBox.MouseDown += SkillProficiency_MouseDown;
            AnimalHandlingProficiencyBox.MouseDown += SkillProficiency_MouseDown;
            InsightProficiencyBox.MouseDown += SkillProficiency_MouseDown;
            MedicineProficiencyBox.MouseDown += SkillProficiency_MouseDown;
            PerceptionProficiencyBox.MouseDown += SkillProficiency_MouseDown;
            SurvivalProficiencyBox.MouseDown += SkillProficiency_MouseDown;
            DeceptionProficiencyBox.MouseDown += SkillProficiency_MouseDown;
            IntimidationProficiencyBox.MouseDown += SkillProficiency_MouseDown;
            PerformanceProficiencyBox.MouseDown += SkillProficiency_MouseDown;
            PersuasionProficiencyBox.MouseDown += SkillProficiency_MouseDown;

            AthleticsExpertieseBox.MouseDown += SkillExpertise_MouseDown;
            AcrobaticsExpertieseBox.MouseDown += SkillExpertise_MouseDown;
            SleightOfHandExpertieseBox.MouseDown += SkillExpertise_MouseDown;
            StealthExpertieseBox.MouseDown += SkillExpertise_MouseDown;
            ArcanaExpertieseBox.MouseDown += SkillExpertise_MouseDown;
            HistoryExpertieseBox.MouseDown += SkillExpertise_MouseDown;
            InvestigationExpertieseBox.MouseDown += SkillExpertise_MouseDown;
            NatureExpertieseBox.MouseDown += SkillExpertise_MouseDown;
            ReligionExpertieseBox.MouseDown += SkillExpertise_MouseDown;
            AnimalHandlingExpertieseBox.MouseDown += SkillExpertise_MouseDown;
            InsightExpertieseBox.MouseDown += SkillExpertise_MouseDown;
            MedicineExpertieseBox.MouseDown += SkillExpertise_MouseDown;
            PerceptionExpertieseBox.MouseDown += SkillExpertise_MouseDown;
            SurvivalExpertieseBox.MouseDown += SkillExpertise_MouseDown;
            DeceptionExpertieseBox.MouseDown += SkillExpertise_MouseDown;
            IntimidationExpertieseBox.MouseDown += SkillExpertise_MouseDown;
            PerformanceExpertieseBox.MouseDown += SkillExpertise_MouseDown;
            PersuasionExpertieseBox.MouseDown += SkillExpertise_MouseDown;
        }

        #endregion

        #region Methods

        #region Drawing

        public void Draw()
        {
            DrawAttributes();
            DrawDetails();
            DrawSavingThrows();
            DrawSkills();
            DrawHealth();
            DrawArmorClass();
            DrawHitDice();
            DrawResourcePool();
        }

        #region Attributes

        private void DrawAttributes()
        {
            StrengthBonusField.Text = Constants.CalculateBonus(Program.Character.Attributes.Strength).ToString();
            DexterityBonusField.Text = Constants.CalculateBonus(Program.Character.Attributes.Dexterity).ToString();
            ConstitutionBonusField.Text = Constants.CalculateBonus(Program.Character.Attributes.Constitution).ToString();
            IntelligenceBonusField.Text = Constants.CalculateBonus(Program.Character.Attributes.Intelligence).ToString();
            WisdomBonusField.Text = Constants.CalculateBonus(Program.Character.Attributes.Wisdom).ToString();
            CharismaBonusField.Text = Constants.CalculateBonus(Program.Character.Attributes.Charisma).ToString();

            StrengthScoreField.Text = Program.Character.Attributes.Strength.ToString();
            DexterityScoreField.Text = Program.Character.Attributes.Dexterity.ToString();
            ConstitutionScoreField.Text = Program.Character.Attributes.Constitution.ToString();
            IntelligenceScoreField.Text = Program.Character.Attributes.Intelligence.ToString();
            WisdomScoreField.Text = Program.Character.Attributes.Wisdom.ToString();
            CharismaScoreField.Text = Program.Character.Attributes.Charisma.ToString();
        }

        #endregion

        #region Details

        private void DrawDetails()
        {
            InitiativeField.Text = Program.Character.Initiative.ToString();
            PassivePerceptionField.Text = Program.Character.PassivePerception.ToString();
            VisionField.Text = Program.Character.Details.Vision;
            MovementSpeedField.Text = Program.Character.Details.Movement.ToString();
        }

        #endregion

        #region Saving Throws

        private void DrawSavingThrows()
        {
            StrengthSavingThrowField.Text = Program.Character.SavingThrow.Strength.Bonus.ToString();
            DexteritySavingThrowField.Text = Program.Character.SavingThrow.Dexterity.Bonus.ToString();
            ConstitutionSavingThrowField.Text = Program.Character.SavingThrow.Constitution.Bonus.ToString();
            IntelligenceSavingThrowField.Text = Program.Character.SavingThrow.Intelligence.Bonus.ToString();
            WisdomSavingThrowField.Text = Program.Character.SavingThrow.Wisdom.Bonus.ToString();
            CharismaSavingThrowField.Text = Program.Character.SavingThrow.Charisma.Bonus.ToString();

            SetTextStyle(Program.Character.SavingThrow.Strength.Checks, StrengthSavingThrowField);
            SetTextStyle(Program.Character.SavingThrow.Dexterity.Checks, DexteritySavingThrowField);
            SetTextStyle(Program.Character.SavingThrow.Constitution.Checks, ConstitutionSavingThrowField);
            SetTextStyle(Program.Character.SavingThrow.Intelligence.Checks, IntelligenceSavingThrowField);
            SetTextStyle(Program.Character.SavingThrow.Wisdom.Checks, WisdomSavingThrowField);
            SetTextStyle(Program.Character.SavingThrow.Charisma.Checks, CharismaSavingThrowField);

            SetTextStyle(Program.Character.SavingThrow.Strength.Checks, StrengthSavingThrowName);
            SetTextStyle(Program.Character.SavingThrow.Dexterity.Checks, DexteritySavingThrowName);
            SetTextStyle(Program.Character.SavingThrow.Constitution.Checks, ConstitutionSavingThrowName);
            SetTextStyle(Program.Character.SavingThrow.Intelligence.Checks, IntelligenceSavingThrowName);
            SetTextStyle(Program.Character.SavingThrow.Wisdom.Checks, WisdomSavingThrowName);
            SetTextStyle(Program.Character.SavingThrow.Charisma.Checks, CharismaSavingThrowName);

            SetBoxStyle(Program.Character.SavingThrow.Strength.Proficiency, StrengthProficiencyBox);
            SetBoxStyle(Program.Character.SavingThrow.Dexterity.Proficiency, DexterityProficiencyBox);
            SetBoxStyle(Program.Character.SavingThrow.Constitution.Proficiency, ConstitutionProficiencyBox);
            SetBoxStyle(Program.Character.SavingThrow.Intelligence.Proficiency, IntelligenceProficiencyBox);
            SetBoxStyle(Program.Character.SavingThrow.Wisdom.Proficiency, WisdomProficiencyBox);
            SetBoxStyle(Program.Character.SavingThrow.Charisma.Proficiency, CharismaProficiencyBox);
        }

        #endregion

        #region Skills

        private void DrawSkills()
        {
            AthleticsSkillField.Text = Program.Character.Skill.Athletics.Bonus.ToString();
            AcrobaticsSkillField.Text = Program.Character.Skill.Acrobatics.Bonus.ToString();
            SleightOfHandSkillField.Text = Program.Character.Skill.SleightOfHand.Bonus.ToString();
            StealthSkillField.Text = Program.Character.Skill.Stealth.Bonus.ToString();
            ArcanaSkillField.Text = Program.Character.Skill.Arcana.Bonus.ToString();
            HistorySkillField.Text = Program.Character.Skill.History.Bonus.ToString();
            InvestigationSkillField.Text = Program.Character.Skill.Investigation.Bonus.ToString();
            NatureSkillField.Text = Program.Character.Skill.Nature.Bonus.ToString();
            ReligionSkillField.Text = Program.Character.Skill.Religion.Bonus.ToString();
            AnimalHandlingSkillField.Text = Program.Character.Skill.AnimalHandling.Bonus.ToString();
            InsightSkillField.Text = Program.Character.Skill.Insight.Bonus.ToString();
            MedicineSkillField.Text = Program.Character.Skill.Medicine.Bonus.ToString();
            PerceptionSkillField.Text = Program.Character.Skill.Perception.Bonus.ToString();
            SurvivalSkillField.Text = Program.Character.Skill.Survival.Bonus.ToString();
            DeceptionSkillField.Text = Program.Character.Skill.Deception.Bonus.ToString();
            IntimidationSkillField.Text = Program.Character.Skill.Intimidation.Bonus.ToString();
            PerformanceSkillField.Text = Program.Character.Skill.Performance.Bonus.ToString();
            PersuasionSkillField.Text = Program.Character.Skill.Persuasion.Bonus.ToString();

            SetTextStyle(Program.Character.Skill.Athletics.Checks, AthleticsSkillField);
            SetTextStyle(Program.Character.Skill.Acrobatics.Checks, AcrobaticsSkillField);
            SetTextStyle(Program.Character.Skill.SleightOfHand.Checks, SleightOfHandSkillField);
            SetTextStyle(Program.Character.Skill.Stealth.Checks, StealthSkillField);
            SetTextStyle(Program.Character.Skill.Arcana.Checks, ArcanaSkillField);
            SetTextStyle(Program.Character.Skill.History.Checks, HistorySkillField);
            SetTextStyle(Program.Character.Skill.Investigation.Checks, InvestigationSkillField);
            SetTextStyle(Program.Character.Skill.Nature.Checks, NatureSkillField);
            SetTextStyle(Program.Character.Skill.Religion.Checks, ReligionSkillField);
            SetTextStyle(Program.Character.Skill.AnimalHandling.Checks, AnimalHandlingSkillField);
            SetTextStyle(Program.Character.Skill.Insight.Checks, InsightSkillField);
            SetTextStyle(Program.Character.Skill.Medicine.Checks, MedicineSkillField);
            SetTextStyle(Program.Character.Skill.Perception.Checks, PerceptionSkillField);
            SetTextStyle(Program.Character.Skill.Survival.Checks, SurvivalSkillField);
            SetTextStyle(Program.Character.Skill.Deception.Checks, DeceptionSkillField);
            SetTextStyle(Program.Character.Skill.Intimidation.Checks, IntimidationSkillField);
            SetTextStyle(Program.Character.Skill.Performance.Checks, PerformanceSkillField);
            SetTextStyle(Program.Character.Skill.Persuasion.Checks, PersuasionSkillField);
            
            SetTextStyle(Program.Character.Skill.Athletics.Checks, AthleticsSkillName);
            SetTextStyle(Program.Character.Skill.Acrobatics.Checks, AcrobaticsSkillName);
            SetTextStyle(Program.Character.Skill.SleightOfHand.Checks, SleightOfHandSkillName);
            SetTextStyle(Program.Character.Skill.Stealth.Checks, StealthSkillName);
            SetTextStyle(Program.Character.Skill.Arcana.Checks, ArcanaSkillName);
            SetTextStyle(Program.Character.Skill.History.Checks, HistorySkillName);
            SetTextStyle(Program.Character.Skill.Investigation.Checks, InvestigationSkillName);
            SetTextStyle(Program.Character.Skill.Nature.Checks, NatureSkillName);
            SetTextStyle(Program.Character.Skill.Religion.Checks, ReligionSkillName);
            SetTextStyle(Program.Character.Skill.AnimalHandling.Checks, AnimalHandlingSkillName);
            SetTextStyle(Program.Character.Skill.Insight.Checks, InsightSkillName);
            SetTextStyle(Program.Character.Skill.Medicine.Checks, MedicineSkillName);
            SetTextStyle(Program.Character.Skill.Perception.Checks, PerceptionSkillName);
            SetTextStyle(Program.Character.Skill.Survival.Checks, SurvivalSkillName);
            SetTextStyle(Program.Character.Skill.Deception.Checks, DeceptionSkillName);
            SetTextStyle(Program.Character.Skill.Intimidation.Checks, IntimidationSkillName);
            SetTextStyle(Program.Character.Skill.Performance.Checks, PerformanceSkillName);
            SetTextStyle(Program.Character.Skill.Persuasion.Checks, PersuasionSkillName);

            SetBoxStyle(Program.Character.Skill.Athletics.Proficiency, AthleticsProficiencyBox);
            SetBoxStyle(Program.Character.Skill.Acrobatics.Proficiency, AcrobaticsProficiencyBox);
            SetBoxStyle(Program.Character.Skill.SleightOfHand.Proficiency, SleightOfHandProficiencyBox);
            SetBoxStyle(Program.Character.Skill.Stealth.Proficiency, StealthProficiencyBox);
            SetBoxStyle(Program.Character.Skill.Arcana.Proficiency, ArcanaProficiencyBox);
            SetBoxStyle(Program.Character.Skill.History.Proficiency, HistoryProficiencyBox);
            SetBoxStyle(Program.Character.Skill.Investigation.Proficiency, InvestigationProficiencyBox);
            SetBoxStyle(Program.Character.Skill.Nature.Proficiency, NatureProficiencyBox);
            SetBoxStyle(Program.Character.Skill.Religion.Proficiency, ReligionProficiencyBox);
            SetBoxStyle(Program.Character.Skill.AnimalHandling.Proficiency, AnimalHandlingProficiencyBox);
            SetBoxStyle(Program.Character.Skill.Insight.Proficiency, InsightProficiencyBox);
            SetBoxStyle(Program.Character.Skill.Medicine.Proficiency, MedicineProficiencyBox);
            SetBoxStyle(Program.Character.Skill.Perception.Proficiency, PerceptionProficiencyBox);
            SetBoxStyle(Program.Character.Skill.Survival.Proficiency, SurvivalProficiencyBox);
            SetBoxStyle(Program.Character.Skill.Deception.Proficiency, DeceptionProficiencyBox);
            SetBoxStyle(Program.Character.Skill.Intimidation.Proficiency, IntimidationProficiencyBox);
            SetBoxStyle(Program.Character.Skill.Performance.Proficiency, PerformanceProficiencyBox);
            SetBoxStyle(Program.Character.Skill.Persuasion.Proficiency, PersuasionProficiencyBox);

            SetBoxStyle(Program.Character.Skill.Athletics.Expertise, AthleticsExpertieseBox);
            SetBoxStyle(Program.Character.Skill.Acrobatics.Expertise, AcrobaticsExpertieseBox);
            SetBoxStyle(Program.Character.Skill.SleightOfHand.Expertise, SleightOfHandExpertieseBox);
            SetBoxStyle(Program.Character.Skill.Stealth.Expertise, StealthExpertieseBox);
            SetBoxStyle(Program.Character.Skill.Arcana.Expertise, ArcanaExpertieseBox);
            SetBoxStyle(Program.Character.Skill.History.Expertise, HistoryExpertieseBox);
            SetBoxStyle(Program.Character.Skill.Investigation.Expertise, InvestigationExpertieseBox);
            SetBoxStyle(Program.Character.Skill.Nature.Expertise, NatureExpertieseBox);
            SetBoxStyle(Program.Character.Skill.Religion.Expertise, ReligionExpertieseBox);
            SetBoxStyle(Program.Character.Skill.AnimalHandling.Expertise, AnimalHandlingExpertieseBox);
            SetBoxStyle(Program.Character.Skill.Insight.Expertise, InsightExpertieseBox);
            SetBoxStyle(Program.Character.Skill.Medicine.Expertise, MedicineExpertieseBox);
            SetBoxStyle(Program.Character.Skill.Perception.Expertise, PerceptionExpertieseBox);
            SetBoxStyle(Program.Character.Skill.Survival.Expertise, SurvivalExpertieseBox);
            SetBoxStyle(Program.Character.Skill.Deception.Expertise, DeceptionExpertieseBox);
            SetBoxStyle(Program.Character.Skill.Intimidation.Expertise, IntimidationExpertieseBox);
            SetBoxStyle(Program.Character.Skill.Performance.Expertise, PerformanceExpertieseBox);
            SetBoxStyle(Program.Character.Skill.Persuasion.Expertise, PersuasionExpertieseBox);
        }

        #endregion

        #region AC HP

        private void DrawHealth()
        {
            CurrentHpField.Text = Program.Character.Vitality.CurrentHealth.ToString();
            TotalHpField.Text = "/" + Program.Character.Vitality.MaxHealth.ToString();

            CurrentHpField.Foreground = SetHealthStyle();
            TotalHpField.Foreground = SetHealthStyle();
        }

        private void DrawArmorClass()
        {
            ArmorClassField.Text = Program.Character.Armor.TotalArmorClass.ToString();
        }

        #endregion

        #region HitDice

        private void DrawHitDice()
        {
            D6TotalField.Text = Program.Character.Vitality.HitDice.TotalD6.ToString();
            D6TotalField.Foreground = Constants.SetTotalTextStyle(Program.Character.Vitality.HitDice.TotalD6, Program.Character.Vitality.HitDice.SpentD6);
            D6TotalBox.Background = Constants.SetTotalBoxStyle(Program.Character.Vitality.HitDice.TotalD6, Program.Character.Vitality.HitDice.SpentD6);
            D6SpentField.Text = Program.Character.Vitality.HitDice.SpentD6.ToString();
            D6SpentField.Foreground = Constants.SetUsedTextStyle(Program.Character.Vitality.HitDice.TotalD6, Program.Character.Vitality.HitDice.SpentD6);
            D6SpentBox.Background = Constants.SetUsedBoxStyle(Program.Character.Vitality.HitDice.TotalD6, Program.Character.Vitality.HitDice.SpentD6);

            D8TotalField.Text = Program.Character.Vitality.HitDice.TotalD8.ToString();
            D8TotalField.Foreground = Constants.SetTotalTextStyle(Program.Character.Vitality.HitDice.TotalD8, Program.Character.Vitality.HitDice.SpentD8);
            D8SpentField.Text = Program.Character.Vitality.HitDice.SpentD8.ToString();
            D8SpentField.Foreground = Constants.SetUsedTextStyle(Program.Character.Vitality.HitDice.TotalD8, Program.Character.Vitality.HitDice.SpentD8);
            D8SpentBox.Background = Constants.SetUsedBoxStyle(Program.Character.Vitality.HitDice.TotalD8, Program.Character.Vitality.HitDice.SpentD8);
            D8TotalBox.Background = Constants.SetTotalBoxStyle(Program.Character.Vitality.HitDice.TotalD8, Program.Character.Vitality.HitDice.SpentD8);

            D10TotalField.Text = Program.Character.Vitality.HitDice.TotalD10.ToString();
            D10TotalField.Foreground = Constants.SetTotalTextStyle(Program.Character.Vitality.HitDice.TotalD10, Program.Character.Vitality.HitDice.SpentD10);
            D10SpentField.Text = Program.Character.Vitality.HitDice.SpentD10.ToString();
            D10SpentField.Foreground = Constants.SetUsedTextStyle(Program.Character.Vitality.HitDice.TotalD10, Program.Character.Vitality.HitDice.SpentD10);
            D10SpentBox.Background = Constants.SetUsedBoxStyle(Program.Character.Vitality.HitDice.TotalD10, Program.Character.Vitality.HitDice.SpentD10);
            D10TotalBox.Background = Constants.SetTotalBoxStyle(Program.Character.Vitality.HitDice.TotalD10, Program.Character.Vitality.HitDice.SpentD10);

            D12TotalField.Text = Program.Character.Vitality.HitDice.TotalD12.ToString();
            D12TotalField.Foreground = Constants.SetTotalTextStyle(Program.Character.Vitality.HitDice.TotalD12, Program.Character.Vitality.HitDice.SpentD12);
            D12SpentField.Text = Program.Character.Vitality.HitDice.SpentD12.ToString();
            D12SpentField.Foreground = Constants.SetUsedTextStyle(Program.Character.Vitality.HitDice.TotalD12, Program.Character.Vitality.HitDice.SpentD12);
            D12SpentBox.Background = Constants.SetUsedBoxStyle(Program.Character.Vitality.HitDice.TotalD12, Program.Character.Vitality.HitDice.SpentD12);
            D12TotalBox.Background = Constants.SetTotalBoxStyle(Program.Character.Vitality.HitDice.TotalD12, Program.Character.Vitality.HitDice.SpentD12);
        }

        #endregion

        #region Resource Pool

        private void DrawResourcePool()
        {
            SetArrowStyle();

            if (Program.Character.ClassResources.Count > 0)
            {
                ClassResource classResource = Program.Character.ClassResources[ResourceIndex];

                ResourceTypeField.Text = classResource.Type;

                ResourcePoolField.Text = classResource.Total.ToString();
                ResourcePoolBox.Background = Constants.SetTotalBoxStyle(classResource.Total, classResource.Spent);

                ResourceSpentField.Text = classResource.Total.ToString();
                ResourceSpentBox.Background = Constants.SetUsedBoxStyle(classResource.Total, classResource.Spent);
            }
            else
            {
                ResourceTypeField.Text = "None";

                ResourcePoolField.Text = "0";
                ResourcePoolBox.Background = Constants.SetTotalBoxStyle(0, 0);

                ResourceSpentField.Text = "0";
                ResourceSpentBox.Background = Constants.SetUsedBoxStyle(0, 0);
            }
        }

        #endregion

        #endregion

        #region Helpers

        private void SetArrowStyle()
        {
            if (ResourceIndex == 0)
                LeftResourceButton.Foreground = Brushes.DimGray;
            else
                LeftResourceButton.Foreground = Brushes.White;

            if (ResourceIndex == Program.Character.ClassResources.Count - 1 || Program.Character.ClassResources.Count == 0)
                RightResourceButton.Foreground = Brushes.DimGray;
            else
                RightResourceButton.Foreground = Brushes.White;
        }

        private Brush SetHealthStyle()
        {
            int third = Program.Character.Vitality.MaxHealth / 3;
            int hp = Program.Character.Vitality.CurrentHealth;

            if (hp < third && hp > 0)
                return Brushes.IndianRed;
            else if (hp >= third * 2)
                return Brushes.DarkGreen;
            else if (hp <= 0)
                return Brushes.DarkGray;
            else
                return Brushes.DarkOrange;
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

        private void SetTextStyle(Constants.Checks check, TextBlock textBlock)
        {
            switch (check)
            {
                case Constants.Checks.Fail:
                    textBlock.TextDecorations = TextDecorations.Strikethrough;
                    textBlock.Foreground = Brushes.DarkGray;
                    break;
                case Constants.Checks.Advantage:
                    textBlock.Foreground = Brushes.Green;
                    break;
                case Constants.Checks.Disadvantage:
                    textBlock.Foreground = Brushes.IndianRed;
                    break;
                case Constants.Checks.Normal:
                default:
                    textBlock.TextDecorations.Clear();
                    textBlock.Foreground = Brushes.White;
                    break;
            }
        }

        #endregion

        #endregion

        #region Accessors

        public int HeartWidth
        {
            get
            {
                return (int)HealthBox.RenderSize.Width;
            }
        }

        public int HeartHeight
        {
            get
            {
                return (int)HealthBox.RenderSize.Height;
            }
        }

        public int ShieldWidth
        {
            get
            {
                return (int)HealthBox.RenderSize.Width;
            }
        }

        public int ShieldHeight
        {
            get
            {
                return (int)HealthBox.RenderSize.Height;
            }
        }

        public int ResourceIndex { get; private set; }

        #endregion

        #region Events

        private void LeftResourceButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceIndex > 0)
            {
                ResourceIndex--;
                DrawResourcePool();
            }
        }

        private void RightResourceButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceIndex < Program.Character.ClassResources.Count - 1)
            {
                ResourceIndex++;
                DrawResourcePool();
            }
        }

        private void SavingThrows_MouseDown(object sender, RoutedEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;

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

            DrawSavingThrows();
        }

        private void SkillProficiency_MouseDown(object sender, RoutedEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;

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

            DrawSkills();
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

            DrawSkills();
        }

        #endregion

    }
}
