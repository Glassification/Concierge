// <copyright file="SkillWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;

    using Concierge.Character.AbilitySkills;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for SkillWindow.xaml.
    /// </summary>
    public partial class SkillWindow : ConciergeWindow
    {
        public SkillWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.AthleticsComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.AcrobaticsComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.SleightOfHandComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.StealthComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.ArcanaComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.HistoryComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.InvestigationComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.NatureComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.ReligionComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.AnimalHandlingComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.InsightComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.MedicineComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.PerceptionComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.SurvivalComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.DeceptionComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.IntimidationComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.PerformanceComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();
            this.PersuasionComboBox.ItemsSource = ComboBoxGenerator.StatusChecksComboBox();

            this.ConciergePage = ConciergePage.None;
            this.Skill = new Skills();
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.AthleticsComboBox);
            this.SetMouseOverEvents(this.AcrobaticsComboBox);
            this.SetMouseOverEvents(this.SleightOfHandComboBox);
            this.SetMouseOverEvents(this.StealthComboBox);
            this.SetMouseOverEvents(this.ArcanaComboBox);
            this.SetMouseOverEvents(this.HistoryComboBox);
            this.SetMouseOverEvents(this.InvestigationComboBox);
            this.SetMouseOverEvents(this.NatureComboBox);
            this.SetMouseOverEvents(this.ReligionComboBox);
            this.SetMouseOverEvents(this.AnimalHandlingComboBox);
            this.SetMouseOverEvents(this.InsightComboBox);
            this.SetMouseOverEvents(this.MedicineComboBox);
            this.SetMouseOverEvents(this.PerceptionComboBox);
            this.SetMouseOverEvents(this.SurvivalComboBox);
            this.SetMouseOverEvents(this.DeceptionComboBox);
            this.SetMouseOverEvents(this.IntimidationComboBox);
            this.SetMouseOverEvents(this.PerformanceComboBox);
            this.SetMouseOverEvents(this.PersuasionComboBox);
        }

        public override string HeaderText => "Edit Skill Checks";

        public override string WindowName => nameof(SkillWindow);

        private Skills Skill { get; set; }

        public override void ShowEdit<T>(T skill)
        {
            if (skill is not Skills castItem)
            {
                return;
            }

            this.Skill = castItem;
            this.FillFields();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            this.UpdateSkills();
            this.CloseConciergeWindow();
        }

        private void FillFields()
        {
            this.AthleticsComboBox.Text = this.Skill.Athletics.CheckOverride.ToString();
            this.AcrobaticsComboBox.Text = this.Skill.Acrobatics.CheckOverride.ToString();
            this.SleightOfHandComboBox.Text = this.Skill.SleightOfHand.CheckOverride.ToString();
            this.StealthComboBox.Text = this.Skill.Stealth.CheckOverride.ToString();
            this.ArcanaComboBox.Text = this.Skill.Arcana.CheckOverride.ToString();
            this.HistoryComboBox.Text = this.Skill.History.CheckOverride.ToString();
            this.InvestigationComboBox.Text = this.Skill.Investigation.CheckOverride.ToString();
            this.NatureComboBox.Text = this.Skill.Nature.CheckOverride.ToString();
            this.ReligionComboBox.Text = this.Skill.Religion.CheckOverride.ToString();
            this.AnimalHandlingComboBox.Text = this.Skill.AnimalHandling.CheckOverride.ToString();
            this.InsightComboBox.Text = this.Skill.Insight.CheckOverride.ToString();
            this.MedicineComboBox.Text = this.Skill.Medicine.CheckOverride.ToString();
            this.PerceptionComboBox.Text = this.Skill.Perception.CheckOverride.ToString();
            this.SurvivalComboBox.Text = this.Skill.Survival.CheckOverride.ToString();
            this.DeceptionComboBox.Text = this.Skill.Deception.CheckOverride.ToString();
            this.IntimidationComboBox.Text = this.Skill.Intimidation.CheckOverride.ToString();
            this.PerformanceComboBox.Text = this.Skill.Performance.CheckOverride.ToString();
            this.PersuasionComboBox.Text = this.Skill.Persuasion.CheckOverride.ToString();
        }

        private void UpdateSkills()
        {
            var oldSkill = this.Skill.DeepCopy();

            this.Skill.Athletics.CheckOverride = this.AthleticsComboBox.Text.ToEnum<StatusChecks>();
            this.Skill.Acrobatics.CheckOverride = this.AcrobaticsComboBox.Text.ToEnum<StatusChecks>();
            this.Skill.SleightOfHand.CheckOverride = this.SleightOfHandComboBox.Text.ToEnum<StatusChecks>();
            this.Skill.Stealth.CheckOverride = this.StealthComboBox.Text.ToEnum<StatusChecks>();
            this.Skill.Arcana.CheckOverride = this.ArcanaComboBox.Text.ToEnum<StatusChecks>();
            this.Skill.History.CheckOverride = this.HistoryComboBox.Text.ToEnum<StatusChecks>();
            this.Skill.Investigation.CheckOverride = this.InvestigationComboBox.Text.ToEnum<StatusChecks>();
            this.Skill.Nature.CheckOverride = this.NatureComboBox.Text.ToEnum<StatusChecks>();
            this.Skill.Religion.CheckOverride = this.ReligionComboBox.Text.ToEnum<StatusChecks>();
            this.Skill.AnimalHandling.CheckOverride = this.AnimalHandlingComboBox.Text.ToEnum<StatusChecks>();
            this.Skill.Insight.CheckOverride = this.InsightComboBox.Text.ToEnum<StatusChecks>();
            this.Skill.Medicine.CheckOverride = this.MedicineComboBox.Text.ToEnum<StatusChecks>();
            this.Skill.Perception.CheckOverride = this.PerceptionComboBox.Text.ToEnum<StatusChecks>();
            this.Skill.Survival.CheckOverride = this.SurvivalComboBox.Text.ToEnum<StatusChecks>();
            this.Skill.Deception.CheckOverride = this.DeceptionComboBox.Text.ToEnum<StatusChecks>();
            this.Skill.Intimidation.CheckOverride = this.IntimidationComboBox.Text.ToEnum<StatusChecks>();
            this.Skill.Performance.CheckOverride = this.PerformanceComboBox.Text.ToEnum<StatusChecks>();
            this.Skill.Persuasion.CheckOverride = this.PersuasionComboBox.Text.ToEnum<StatusChecks>();

            Program.UndoRedoService.AddCommand(new EditCommand<Skills>(this.Skill, oldSkill, this.ConciergePage));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateSkills();
            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }
    }
}
