// <copyright file="OverviewPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.OverviewPageInterface
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.AbilitySavingThrows;
    using Concierge.Character.AbilitySkills;
    using Concierge.Character.Characteristics;
    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Interfaces.Controls;
    using Concierge.Interfaces.Enums;
    using Concierge.Services;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for OverviewPage.xaml.
    /// </summary>
    public partial class OverviewPage : Page, IConciergePage
    {
        public OverviewPage()
        {
            this.InitializeComponent();

            this.StrengthAttributeDisplay.InitializeFontSize();
            this.DexterityAttributeDisplay.InitializeFontSize();
            this.ConstitutionAttributeDisplay.InitializeFontSize();
            this.IntelligenceAttributeDisplay.InitializeFontSize();
            this.WisdomAttributeDisplay.InitializeFontSize();
            this.CharismaAttributeDisplay.InitializeFontSize();

            this.DataContext = this;
            this.CurrentHitDiceBox = string.Empty;
        }

        public ConciergePage ConciergePage => ConciergePage.Overview;

        public bool HasEditableDataGrid => false;

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

        private void DrawAttributes()
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

        private void DrawSkills()
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

        private void DrawHealth()
        {
            var vitality = Program.CcsFile.Character.Vitality;

            this.HealthDisplay.CurrentHealth = vitality.CurrentHealth;
            this.HealthDisplay.TotalHealth = vitality.Health.MaxHealth;
            this.HealthDisplay.SetHealthStyle(vitality);
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

            this.HealthDisplay.SetDeathSaveStyle(deathSaves);
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
            var result = ConciergeWindowService.ShowDamage<Vitality>(
                Program.CcsFile.Character.Vitality,
                typeof(ModifyHpWindow),
                this.Window_ApplyChanges,
                ConciergePage.Overview);
            this.DrawHealth();

            if (Program.CcsFile.Character.Vitality.IsDead && result == ConciergeWindowResult.OK)
            {
                HealthDisplay.DisplayCharacterDeathWindow(Program.CcsFile.Character.Properties.Name);
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
            if (e.ClickCount != 2 || sender is not Grid usedBox)
            {
                return;
            }

            var hitDice = Program.CcsFile.Character.Vitality.HitDice;
            var oldItem = hitDice.DeepCopy();
            switch (usedBox.Name)
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
            if (sender is not Grid grid)
            {
                return;
            }

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
            if (sender is not Grid grid)
            {
                return;
            }

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

        private void SavingThrow_ToggleClicked(object sender, RoutedEventArgs e)
        {
            this.DrawSavingThrows();
        }

        private void Skill_ToggleClicked(object sender, RoutedEventArgs e)
        {
            this.DrawSkills();
        }

        private void HealthDisplay_SaveClicked(object sender, RoutedEventArgs e)
        {
            this.DrawDeathSavingThrows();
        }
    }
}