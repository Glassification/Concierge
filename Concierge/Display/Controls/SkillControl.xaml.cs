// <copyright file="SkillControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Character.Aspects;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Common;
    using Concierge.Common.Utilities;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows.Utility;
    using Concierge.Services;
    using MaterialDesignThemes.Wpf;

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

        private Skill skill;

        public SkillControl()
        {
            this.InitializeComponent();

            this.skill = Skill.Default;
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

        public string ProficiencyToolTip => $"Toggle {this.SkillName} Proficiency";

        public string ExpertiseToolTip => $"Toggle {this.SkillName} Expertise";

        public void Draw(Attributes attributes, Skill skill)
        {
            this.skill = skill;

            this.SkillBonus = attributes.GetSkillBonus(this.skill, Program.CcsFile.Character.ProficiencyBonus).ToString();
            this.ProficiencyToggle.Foreground = this.skill.Proficiency ? ConciergeBrushes.Mint : Brushes.SlateGray;
            this.ProficiencyToggle.Kind = this.skill.Proficiency ? PackIconKind.RhombusSplit : PackIconKind.RhombusSplitOutline;

            this.ExpertiseToggle.Foreground = this.skill.Expertise ? ConciergeBrushes.Mint : Brushes.SlateGray;
            this.ExpertiseToggle.Kind = this.skill.Expertise ? PackIconKind.RhombusSplit : PackIconKind.RhombusSplitOutline;

            var status = this.skill.GetStatus(Program.CcsFile.Character.Vitality);
            SetTextStyleHelper(status, this.SkillNameField);
            SetTextStyleHelper(status, this.SkillBonusField);
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
                    textBlock.TextDecorations = [];
                    textBlock.Foreground = Brushes.Green;
                    textBlock.ToolTip = "Advantage";
                    break;
                case StatusChecks.Disadvantage:
                    textBlock.TextDecorations = [];
                    textBlock.Foreground = Brushes.IndianRed;
                    textBlock.ToolTip = "Disadvantage";
                    break;
                case StatusChecks.Normal:
                default:
                    textBlock.TextDecorations = [];
                    textBlock.Foreground = Brushes.White;
                    textBlock.ToolTip = null;
                    break;
            }
        }

        private void SkillField_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ConciergeWindowService.ShowAbilityCheckWindow(typeof(AbilityCheckWindow), this.skill, 0);
        }

        private void Toggle_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is not Grid grid)
            {
                return;
            }

            var packIcon = DisplayUtility.FindVisualChildren<PackIcon>(grid).First();
            packIcon.Foreground = ConciergeBrushes.BorderHighlight;
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Toggle_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is not Grid grid)
            {
                return;
            }

            var packIcon = DisplayUtility.FindVisualChildren<PackIcon>(grid).First();
            var check = packIcon.Name.Contains("Proficiency", StringComparison.InvariantCultureIgnoreCase) ? this.skill.Proficiency : this.skill.Expertise;

            packIcon.Foreground = check ? ConciergeBrushes.Mint : Brushes.SlateGray;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void ProficiencyToggle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SoundService.PlayUpdateValue();

            var skillCopy = this.skill.DeepCopy();
            this.skill.Proficiency = !this.skill.Proficiency;
            this.RaiseEvent(new RoutedEventArgs(ToggleClickedEvent));

            Program.UndoRedoService.AddCommand(new EditCommand<Skill>(this.skill, skillCopy, ConciergePages.Overview));
        }

        private void ExpertiseToggle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SoundService.PlayUpdateValue();

            var skillCopy = this.skill.DeepCopy();
            this.skill.Expertise = !this.skill.Expertise;
            this.RaiseEvent(new RoutedEventArgs(ToggleClickedEvent));

            Program.UndoRedoService.AddCommand(new EditCommand<Skill>(this.skill, skillCopy, ConciergePages.Overview));
        }
    }
}
