// <copyright file="SkillWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.Linq;
    using System.Windows;

    using Concierge.Character.AbilitySkills;
    using Concierge.Character.Enums;
    using Concierge.Commands;
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

            this.AthleticsComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.AcrobaticsComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.SleightOfHandComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.StealthComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.ArcanaComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.HistoryComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.InvestigationComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.NatureComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.ReligionComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.AnimalHandlingComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.InsightComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.MedicineComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.PerceptionComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.SurvivalComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.DeceptionComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.IntimidationComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.PerformanceComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();
            this.PersuasionComboBox.ItemsSource = Enum.GetValues(typeof(StatusChecks)).Cast<StatusChecks>();

            this.ConciergePage = ConciergePage.None;
            this.Skill = new Skills();
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetFocusEvents(this.AthleticsComboBox);
            this.SetFocusEvents(this.AcrobaticsComboBox);
            this.SetFocusEvents(this.SleightOfHandComboBox);
            this.SetFocusEvents(this.StealthComboBox);
            this.SetFocusEvents(this.ArcanaComboBox);
            this.SetFocusEvents(this.HistoryComboBox);
            this.SetFocusEvents(this.InvestigationComboBox);
            this.SetFocusEvents(this.NatureComboBox);
            this.SetFocusEvents(this.ReligionComboBox);
            this.SetFocusEvents(this.AnimalHandlingComboBox);
            this.SetFocusEvents(this.InsightComboBox);
            this.SetFocusEvents(this.MedicineComboBox);
            this.SetFocusEvents(this.PerceptionComboBox);
            this.SetFocusEvents(this.SurvivalComboBox);
            this.SetFocusEvents(this.DeceptionComboBox);
            this.SetFocusEvents(this.IntimidationComboBox);
            this.SetFocusEvents(this.PerformanceComboBox);
            this.SetFocusEvents(this.PersuasionComboBox);
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

            this.Skill.Athletics.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.AthleticsComboBox.Text);
            this.Skill.Acrobatics.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.AcrobaticsComboBox.Text);
            this.Skill.SleightOfHand.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.SleightOfHandComboBox.Text);
            this.Skill.Stealth.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.StealthComboBox.Text);
            this.Skill.Arcana.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.ArcanaComboBox.Text);
            this.Skill.History.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.HistoryComboBox.Text);
            this.Skill.Investigation.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.InvestigationComboBox.Text);
            this.Skill.Nature.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.NatureComboBox.Text);
            this.Skill.Religion.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.ReligionComboBox.Text);
            this.Skill.AnimalHandling.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.AnimalHandlingComboBox.Text);
            this.Skill.Insight.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.InsightComboBox.Text);
            this.Skill.Medicine.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.MedicineComboBox.Text);
            this.Skill.Perception.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.PerceptionComboBox.Text);
            this.Skill.Survival.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.SurvivalComboBox.Text);
            this.Skill.Deception.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.DeceptionComboBox.Text);
            this.Skill.Intimidation.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.IntimidationComboBox.Text);
            this.Skill.Performance.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.PerformanceComboBox.Text);
            this.Skill.Persuasion.CheckOverride = (StatusChecks)Enum.Parse(typeof(StatusChecks), this.PersuasionComboBox.Text);

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
