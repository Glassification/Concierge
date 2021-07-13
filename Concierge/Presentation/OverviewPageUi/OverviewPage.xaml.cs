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
            this.VisionField.Text = Program.CcsFile.Character.Details.Vision.ToString();
            this.MovementSpeedField.Text = Program.CcsFile.Character.Details.Movement.ToString();
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

            this.SetTextStyle(savingThrow.Strength.StatusChecks, this.StrengthSavingThrowField);
            this.SetTextStyle(savingThrow.Dexterity.StatusChecks, this.DexteritySavingThrowField);
            this.SetTextStyle(savingThrow.Constitution.StatusChecks, this.ConstitutionSavingThrowField);
            this.SetTextStyle(savingThrow.Intelligence.StatusChecks, this.IntelligenceSavingThrowField);
            this.SetTextStyle(savingThrow.Wisdom.StatusChecks, this.WisdomSavingThrowField);
            this.SetTextStyle(savingThrow.Charisma.StatusChecks, this.CharismaSavingThrowField);

            this.SetTextStyle(savingThrow.Strength.StatusChecks, this.StrengthSavingThrowName);
            this.SetTextStyle(savingThrow.Dexterity.StatusChecks, this.DexteritySavingThrowName);
            this.SetTextStyle(savingThrow.Constitution.StatusChecks, this.ConstitutionSavingThrowName);
            this.SetTextStyle(savingThrow.Intelligence.StatusChecks, this.IntelligenceSavingThrowName);
            this.SetTextStyle(savingThrow.Wisdom.StatusChecks, this.WisdomSavingThrowName);
            this.SetTextStyle(savingThrow.Charisma.StatusChecks, this.CharismaSavingThrowName);

            this.SetBoxStyle(savingThrow.Strength.Proficiency, this.StrengthProficiencyBox);
            this.SetBoxStyle(savingThrow.Dexterity.Proficiency, this.DexterityProficiencyBox);
            this.SetBoxStyle(savingThrow.Constitution.Proficiency, this.ConstitutionProficiencyBox);
            this.SetBoxStyle(savingThrow.Intelligence.Proficiency, this.IntelligenceProficiencyBox);
            this.SetBoxStyle(savingThrow.Wisdom.Proficiency, this.WisdomProficiencyBox);
            this.SetBoxStyle(savingThrow.Charisma.Proficiency, this.CharismaProficiencyBox);
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

            this.SetTextStyle(skill.Athletics.Checks, this.AthleticsSkillField);
            this.SetTextStyle(skill.Acrobatics.Checks, this.AcrobaticsSkillField);
            this.SetTextStyle(skill.SleightOfHand.Checks, this.SleightOfHandSkillField);
            this.SetTextStyle(skill.Stealth.Checks, this.StealthSkillField);
            this.SetTextStyle(skill.Arcana.Checks, this.ArcanaSkillField);
            this.SetTextStyle(skill.History.Checks, this.HistorySkillField);
            this.SetTextStyle(skill.Investigation.Checks, this.InvestigationSkillField);
            this.SetTextStyle(skill.Nature.Checks, this.NatureSkillField);
            this.SetTextStyle(skill.Religion.Checks, this.ReligionSkillField);
            this.SetTextStyle(skill.AnimalHandling.Checks, this.AnimalHandlingSkillField);
            this.SetTextStyle(skill.Insight.Checks, this.InsightSkillField);
            this.SetTextStyle(skill.Medicine.Checks, this.MedicineSkillField);
            this.SetTextStyle(skill.Perception.Checks, this.PerceptionSkillField);
            this.SetTextStyle(skill.Survival.Checks, this.SurvivalSkillField);
            this.SetTextStyle(skill.Deception.Checks, this.DeceptionSkillField);
            this.SetTextStyle(skill.Intimidation.Checks, this.IntimidationSkillField);
            this.SetTextStyle(skill.Performance.Checks, this.PerformanceSkillField);
            this.SetTextStyle(skill.Persuasion.Checks, this.PersuasionSkillField);

            this.SetTextStyle(skill.Athletics.Checks, this.AthleticsSkillName);
            this.SetTextStyle(skill.Acrobatics.Checks, this.AcrobaticsSkillName);
            this.SetTextStyle(skill.SleightOfHand.Checks, this.SleightOfHandSkillName);
            this.SetTextStyle(skill.Stealth.Checks, this.StealthSkillName);
            this.SetTextStyle(skill.Arcana.Checks, this.ArcanaSkillName);
            this.SetTextStyle(skill.History.Checks, this.HistorySkillName);
            this.SetTextStyle(skill.Investigation.Checks, this.InvestigationSkillName);
            this.SetTextStyle(skill.Nature.Checks, this.NatureSkillName);
            this.SetTextStyle(skill.Religion.Checks, this.ReligionSkillName);
            this.SetTextStyle(skill.AnimalHandling.Checks, this.AnimalHandlingSkillName);
            this.SetTextStyle(skill.Insight.Checks, this.InsightSkillName);
            this.SetTextStyle(skill.Medicine.Checks, this.MedicineSkillName);
            this.SetTextStyle(skill.Perception.Checks, this.PerceptionSkillName);
            this.SetTextStyle(skill.Survival.Checks, this.SurvivalSkillName);
            this.SetTextStyle(skill.Deception.Checks, this.DeceptionSkillName);
            this.SetTextStyle(skill.Intimidation.Checks, this.IntimidationSkillName);
            this.SetTextStyle(skill.Performance.Checks, this.PerformanceSkillName);
            this.SetTextStyle(skill.Persuasion.Checks, this.PersuasionSkillName);

            this.SetBoxStyle(skill.Athletics.Proficiency, this.AthleticsProficiencyBox);
            this.SetBoxStyle(skill.Acrobatics.Proficiency, this.AcrobaticsProficiencyBox);
            this.SetBoxStyle(skill.SleightOfHand.Proficiency, this.SleightOfHandProficiencyBox);
            this.SetBoxStyle(skill.Stealth.Proficiency, this.StealthProficiencyBox);
            this.SetBoxStyle(skill.Arcana.Proficiency, this.ArcanaProficiencyBox);
            this.SetBoxStyle(skill.History.Proficiency, this.HistoryProficiencyBox);
            this.SetBoxStyle(skill.Investigation.Proficiency, this.InvestigationProficiencyBox);
            this.SetBoxStyle(skill.Nature.Proficiency, this.NatureProficiencyBox);
            this.SetBoxStyle(skill.Religion.Proficiency, this.ReligionProficiencyBox);
            this.SetBoxStyle(skill.AnimalHandling.Proficiency, this.AnimalHandlingProficiencyBox);
            this.SetBoxStyle(skill.Insight.Proficiency, this.InsightProficiencyBox);
            this.SetBoxStyle(skill.Medicine.Proficiency, this.MedicineProficiencyBox);
            this.SetBoxStyle(skill.Perception.Proficiency, this.PerceptionProficiencyBox);
            this.SetBoxStyle(skill.Survival.Proficiency, this.SurvivalProficiencyBox);
            this.SetBoxStyle(skill.Deception.Proficiency, this.DeceptionProficiencyBox);
            this.SetBoxStyle(skill.Intimidation.Proficiency, this.IntimidationProficiencyBox);
            this.SetBoxStyle(skill.Performance.Proficiency, this.PerformanceProficiencyBox);
            this.SetBoxStyle(skill.Persuasion.Proficiency, this.PersuasionProficiencyBox);

            this.SetBoxStyle(skill.Athletics.Expertise, this.AthleticsExpertieseBox);
            this.SetBoxStyle(skill.Acrobatics.Expertise, this.AcrobaticsExpertieseBox);
            this.SetBoxStyle(skill.SleightOfHand.Expertise, this.SleightOfHandExpertieseBox);
            this.SetBoxStyle(skill.Stealth.Expertise, this.StealthExpertieseBox);
            this.SetBoxStyle(skill.Arcana.Expertise, this.ArcanaExpertieseBox);
            this.SetBoxStyle(skill.History.Expertise, this.HistoryExpertieseBox);
            this.SetBoxStyle(skill.Investigation.Expertise, this.InvestigationExpertieseBox);
            this.SetBoxStyle(skill.Nature.Expertise, this.NatureExpertieseBox);
            this.SetBoxStyle(skill.Religion.Expertise, this.ReligionExpertieseBox);
            this.SetBoxStyle(skill.AnimalHandling.Expertise, this.AnimalHandlingExpertieseBox);
            this.SetBoxStyle(skill.Insight.Expertise, this.InsightExpertieseBox);
            this.SetBoxStyle(skill.Medicine.Expertise, this.MedicineExpertieseBox);
            this.SetBoxStyle(skill.Perception.Expertise, this.PerceptionExpertieseBox);
            this.SetBoxStyle(skill.Survival.Expertise, this.SurvivalExpertieseBox);
            this.SetBoxStyle(skill.Deception.Expertise, this.DeceptionExpertieseBox);
            this.SetBoxStyle(skill.Intimidation.Expertise, this.IntimidationExpertieseBox);
            this.SetBoxStyle(skill.Performance.Expertise, this.PerformanceExpertieseBox);
            this.SetBoxStyle(skill.Persuasion.Expertise, this.PersuasionExpertieseBox);
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
            var hitDice = Program.CcsFile.Character.Vitality.HitDice;

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
