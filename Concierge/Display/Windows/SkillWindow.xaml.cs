// <copyright file="SkillWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;

    using Concierge.Character.Aspects;
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
        private Attributes attributes = new ();

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

        public override void ShowEdit<T>(T skill)
        {
            if (skill is not Attributes castItem)
            {
                return;
            }

            this.attributes = castItem;
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
            this.AthleticsComboBox.Text = this.attributes.Athletics.SkillOverride.ToString();
            this.AcrobaticsComboBox.Text = this.attributes.Acrobatics.SkillOverride.ToString();
            this.SleightOfHandComboBox.Text = this.attributes.SleightOfHand.SkillOverride.ToString();
            this.StealthComboBox.Text = this.attributes.Stealth.SkillOverride.ToString();
            this.ArcanaComboBox.Text = this.attributes.Arcana.SkillOverride.ToString();
            this.HistoryComboBox.Text = this.attributes.History.SkillOverride.ToString();
            this.InvestigationComboBox.Text = this.attributes.Investigation.SkillOverride.ToString();
            this.NatureComboBox.Text = this.attributes.Nature.SkillOverride.ToString();
            this.ReligionComboBox.Text = this.attributes.Religion.SkillOverride.ToString();
            this.AnimalHandlingComboBox.Text = this.attributes.AnimalHandling.SkillOverride.ToString();
            this.InsightComboBox.Text = this.attributes.Insight.SkillOverride.ToString();
            this.MedicineComboBox.Text = this.attributes.Medicine.SkillOverride.ToString();
            this.PerceptionComboBox.Text = this.attributes.Perception.SkillOverride.ToString();
            this.SurvivalComboBox.Text = this.attributes.Survival.SkillOverride.ToString();
            this.DeceptionComboBox.Text = this.attributes.Deception.SkillOverride.ToString();
            this.IntimidationComboBox.Text = this.attributes.Intimidation.SkillOverride.ToString();
            this.PerformanceComboBox.Text = this.attributes.Performance.SkillOverride.ToString();
            this.PersuasionComboBox.Text = this.attributes.Persuasion.SkillOverride.ToString();
        }

        private void UpdateSkills()
        {
            var oldSkill = this.attributes.DeepCopy();

            this.attributes.Athletics.SkillOverride = this.AthleticsComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Acrobatics.SkillOverride = this.AcrobaticsComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.SleightOfHand.SkillOverride = this.SleightOfHandComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Stealth.SkillOverride = this.StealthComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Arcana.SkillOverride = this.ArcanaComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.History.SkillOverride = this.HistoryComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Investigation.SkillOverride = this.InvestigationComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Nature.SkillOverride = this.NatureComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Religion.SkillOverride = this.ReligionComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.AnimalHandling.SkillOverride = this.AnimalHandlingComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Insight.SkillOverride = this.InsightComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Medicine.SkillOverride = this.MedicineComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Perception.SkillOverride = this.PerceptionComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Survival.SkillOverride = this.SurvivalComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Deception.SkillOverride = this.DeceptionComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Intimidation.SkillOverride = this.IntimidationComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Performance.SkillOverride = this.PerformanceComboBox.Text.ToEnum<StatusChecks>();
            this.attributes.Persuasion.SkillOverride = this.PersuasionComboBox.Text.ToEnum<StatusChecks>();

            Program.UndoRedoService.AddCommand(new EditCommand<Attributes>(this.attributes, oldSkill, this.ConciergePage));
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
