// <copyright file="SkillControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using Concierge.Character.AbilitySkills;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Display.Enums;
    using Concierge.Persistence;

    /// <summary>
    /// Interaction logic for SkillControl.xaml.
    /// </summary>
    public partial class SkillControl : UserControl
    {
        public static readonly DependencyProperty SkillNameProperty =
            DependencyProperty.Register(
                "SkillName",
                typeof(string),
                typeof(SkillControl),
                new UIPropertyMetadata("Skill Name"));

        public static readonly DependencyProperty SkillBonusProperty =
            DependencyProperty.Register(
                "SkillBonus",
                typeof(string),
                typeof(SkillControl),
                new UIPropertyMetadata("0"));

        public static readonly RoutedEvent ToggleClickedEvent =
            EventManager.RegisterRoutedEvent(
                "ToggleClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(SkillControl));

        public static readonly DependencyProperty FillBrushProperty =
            DependencyProperty.Register(
                "FillBrush",
                typeof(Brush),
                typeof(AttributeControl), // No idea why it only works this way
                new UIPropertyMetadata(Brushes.Transparent));

        public SkillControl()
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

        public Brush FillBrush
        {
            get
            {
                return (Brush)this.GetValue(FillBrushProperty);
            }

            set
            {
                this.SetValue(FillBrushProperty, value);
                this.SkillGrid.Background = value;
                this.SkillBorder.BorderBrush = value;
            }
        }

        public void SetStyle(bool proficiencyFlag, bool expertiseFlag, StatusChecks check)
        {
            this.ProficiencyBox.Fill = proficiencyFlag ? Brushes.SteelBlue : Brushes.Transparent;
            this.ExpertiseBox.Fill = expertiseFlag ? Brushes.SteelBlue : Brushes.Transparent;

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
            if (sender is not Ellipse ellipse)
            {
                return;
            }

            ellipse.Stroke = ConciergeBrushes.BorderHighlight;
            ellipse.Fill = ConciergeBrushes.BorderHighlight;
            ellipse.StrokeThickness = 1;

            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void ToggleBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is not Ellipse ellipse)
            {
                return;
            }

            var save = Program.CcsFile.Character.Skill.GetSkill(this.SkillName.Strip(" "));
            var check = ellipse.Name.Contains("Proficiency", StringComparison.InvariantCultureIgnoreCase) ? save.Proficiency : save.Expertise;

            ellipse.Stroke = Brushes.SteelBlue;
            ellipse.Fill = check ? Brushes.SteelBlue : Brushes.Transparent;
            ellipse.StrokeThickness = 1;

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void SkillProficiency_MouseDown(object sender, RoutedEventArgs e)
        {
            ConciergeSound.UpdateValue();

            var skill = Program.CcsFile.Character.Skill;
            var skillCopy = skill.DeepCopy();

            var save = skill.GetSkill(this.SkillName.Strip(" "));
            save.Proficiency = !save.Proficiency;

            this.RaiseEvent(new RoutedEventArgs(ToggleClickedEvent));

            Program.UndoRedoService.AddCommand(new EditCommand<Skill>(skill, skillCopy, ConciergePage.Overview));
            Program.Modify();
        }

        private void SkillExpertise_MouseDown(object sender, RoutedEventArgs e)
        {
            ConciergeSound.UpdateValue();

            var skill = Program.CcsFile.Character.Skill;
            var skillCopy = skill.DeepCopy();

            var save = skill.GetSkill(this.SkillName.Strip(" "));
            save.Expertise = !save.Expertise;

            this.RaiseEvent(new RoutedEventArgs(ToggleClickedEvent));

            Program.UndoRedoService.AddCommand(new EditCommand<Skill>(skill, skillCopy, ConciergePage.Overview));
            Program.Modify();
        }
    }
}
