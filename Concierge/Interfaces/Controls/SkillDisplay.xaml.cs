// <copyright file="SkillDisplay.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using Concierge.Character.AbilitySkills;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for SkillDisplay.xaml.
    /// </summary>
    public partial class SkillDisplay : UserControl
    {
        public static readonly DependencyProperty SkillNameProperty =
            DependencyProperty.Register(
                "SkillName",
                typeof(string),
                typeof(AttributeDisplay),
                new UIPropertyMetadata("Skill Name"));

        public static readonly DependencyProperty SkillBonusProperty =
            DependencyProperty.Register(
                "SkillBonus",
                typeof(string),
                typeof(AttributeDisplay),
                new UIPropertyMetadata("0"));

        public static readonly RoutedEvent ToggleClickedEvent =
            EventManager.RegisterRoutedEvent(
                "ToggleClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(SkillDisplay));

        public SkillDisplay()
        {
            this.InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Stretch;
        }

        public event RoutedEventHandler ToggleClicked
        {
            add { this.AddHandler(ToggleClickedEvent, value); }
            remove { this.RemoveHandler(ToggleClickedEvent, value); }
        }

        public string SkillName
        {
            get { return (string)this.GetValue(SkillNameProperty); }
            set { this.SetValue(SkillNameProperty, value); }
        }

        public string SkillBonus
        {
            get
            {
                return (string)this.GetValue(SkillBonusProperty);
            }

            set
            {
                this.SetValue(SkillBonusProperty, value);
                this.SkillBonusField.Text = value;
            }
        }

        public void SetStyle(bool proficiencyFlag, bool expertiseFlag, StatusChecks check)
        {
            this.ProficiencyBox.Fill = proficiencyFlag ? ConciergeColors.ProficiencyBrush : Brushes.Transparent;
            this.ExpertiseBox.Fill = expertiseFlag ? ConciergeColors.ProficiencyBrush : Brushes.Transparent;

            SetTextStyleHelper(check, this.SkillNameField);
            SetTextStyleHelper(check, this.SkillBonusField);
        }

        private static void SetTextStyleHelper(StatusChecks check, TextBlock textBlock)
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

        private void ToggleBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is not Rectangle rectangle)
            {
                return;
            }

            rectangle.Stroke = ConciergeColors.RectangleBorderHighlight;
            rectangle.StrokeThickness = 1;

            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void ToggleBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is not Rectangle rectangle)
            {
                return;
            }

            rectangle.Stroke = ConciergeColors.RectangleBorder;
            rectangle.StrokeThickness = 1;

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void SkillProficiency_MouseDown(object sender, RoutedEventArgs e)
        {
            ConciergeSound.UpdateValue();

            var skill = Program.CcsFile.Character.Skill;
            var skillCopy = skill.DeepCopy();

            switch (this.SkillName.Strip(" "))
            {
                case "Athletics":
                    skill.Athletics.Proficiency = !skill.Athletics.Proficiency;
                    break;
                case "Acrobatics":
                    skill.Acrobatics.Proficiency = !skill.Acrobatics.Proficiency;
                    break;
                case "SleightOfHand":
                    skill.SleightOfHand.Proficiency = !skill.SleightOfHand.Proficiency;
                    break;
                case "Stealth":
                    skill.Stealth.Proficiency = !skill.Stealth.Proficiency;
                    break;
                case "Arcana":
                    skill.Arcana.Proficiency = !skill.Arcana.Proficiency;
                    break;
                case "History":
                    skill.History.Proficiency = !skill.History.Proficiency;
                    break;
                case "Investigation":
                    skill.Investigation.Proficiency = !skill.Investigation.Proficiency;
                    break;
                case "Nature":
                    skill.Nature.Proficiency = !skill.Nature.Proficiency;
                    break;
                case "Religion":
                    skill.Religion.Proficiency = !skill.Religion.Proficiency;
                    break;
                case "AnimalHandling":
                    skill.AnimalHandling.Proficiency = !skill.AnimalHandling.Proficiency;
                    break;
                case "Insight":
                    skill.Insight.Proficiency = !skill.Insight.Proficiency;
                    break;
                case "Medicine":
                    skill.Medicine.Proficiency = !skill.Medicine.Proficiency;
                    break;
                case "Perception":
                    skill.Perception.Proficiency = !skill.Perception.Proficiency;
                    break;
                case "Survival":
                    skill.Survival.Proficiency = !skill.Survival.Proficiency;
                    break;
                case "Deception":
                    skill.Deception.Proficiency = !skill.Deception.Proficiency;
                    break;
                case "Intimidation":
                    skill.Intimidation.Proficiency = !skill.Intimidation.Proficiency;
                    break;
                case "Performance":
                    skill.Performance.Proficiency = !skill.Performance.Proficiency;
                    break;
                case "Persuasion":
                    skill.Persuasion.Proficiency = !skill.Persuasion.Proficiency;
                    break;
            }

            this.RaiseEvent(new RoutedEventArgs(ToggleClickedEvent));

            Program.UndoRedoService.AddCommand(new EditCommand<Skill>(skill, skillCopy, ConciergePage.Overview));
            Program.Modify();
        }

        private void SkillExpertise_MouseDown(object sender, RoutedEventArgs e)
        {
            ConciergeSound.UpdateValue();

            var skill = Program.CcsFile.Character.Skill;
            var skillCopy = skill.DeepCopy();

            switch (this.SkillName.Strip(" "))
            {
                case "Athletics":
                    skill.Athletics.Expertise = !skill.Athletics.Expertise;
                    break;
                case "Acrobatics":
                    skill.Acrobatics.Expertise = !skill.Acrobatics.Expertise;
                    break;
                case "SleightOfHand":
                    skill.SleightOfHand.Expertise = !skill.SleightOfHand.Expertise;
                    break;
                case "Stealth":
                    skill.Stealth.Expertise = !skill.Stealth.Expertise;
                    break;
                case "Arcana":
                    skill.Arcana.Expertise = !skill.Arcana.Expertise;
                    break;
                case "History":
                    skill.History.Expertise = !skill.History.Expertise;
                    break;
                case "Investigation":
                    skill.Investigation.Expertise = !skill.Investigation.Expertise;
                    break;
                case "Nature":
                    skill.Nature.Expertise = !skill.Nature.Expertise;
                    break;
                case "Religion":
                    skill.Religion.Expertise = !skill.Religion.Expertise;
                    break;
                case "AnimalHandling":
                    skill.AnimalHandling.Expertise = !skill.AnimalHandling.Expertise;
                    break;
                case "Insight":
                    skill.Insight.Expertise = !skill.Insight.Expertise;
                    break;
                case "Medicine":
                    skill.Medicine.Expertise = !skill.Medicine.Expertise;
                    break;
                case "Perception":
                    skill.Perception.Expertise = !skill.Perception.Expertise;
                    break;
                case "Survival":
                    skill.Survival.Expertise = !skill.Survival.Expertise;
                    break;
                case "Deception":
                    skill.Deception.Expertise = !skill.Deception.Expertise;
                    break;
                case "Intimidation":
                    skill.Intimidation.Expertise = !skill.Intimidation.Expertise;
                    break;
                case "Performance":
                    skill.Performance.Expertise = !skill.Performance.Expertise;
                    break;
                case "Persuasion":
                    skill.Persuasion.Expertise = !skill.Persuasion.Expertise;
                    break;
            }

            this.RaiseEvent(new RoutedEventArgs(ToggleClickedEvent));

            Program.UndoRedoService.AddCommand(new EditCommand<Skill>(skill, skillCopy, ConciergePage.Overview));
            Program.Modify();
        }
    }
}
