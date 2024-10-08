﻿// <copyright file="SavingThrowControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
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
    using Concierge.Display.Pages;
    using Concierge.Display.Windows.Utility;
    using Concierge.Services;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for SavingThrowControl.xaml.
    /// </summary>
    public partial class SavingThrowControl : UserControl
    {
        public static readonly DependencyProperty SavingThrowNameProperty =
            DependencyProperty.Register(
                "SavingThrowName",
                typeof(string),
                typeof(SavingThrowControl),
                new UIPropertyMetadata("Saving Throw Name"));

        public static readonly DependencyProperty SavingThrowBonusProperty =
            DependencyProperty.Register(
                "SavingThrowBonus",
                typeof(string),
                typeof(SavingThrowControl),
                new UIPropertyMetadata("0"));

        public static readonly RoutedEvent ToggleClickedEvent =
            EventManager.RegisterRoutedEvent(
                "ToggleClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(SavingThrowControl));

        public static readonly DependencyProperty FillBrushProperty =
            DependencyProperty.Register(
                "FillBrush",
                typeof(Brush),
                typeof(OverviewPage));

        private Attribute attribute;

        public SavingThrowControl()
        {
            this.InitializeComponent();

            this.attribute = Attribute.Default;
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Stretch;
        }

        public event RoutedEventHandler ToggleClicked
        {
            add { this.AddHandler(ToggleClickedEvent, value); }
            remove { this.RemoveHandler(ToggleClickedEvent, value); }
        }

        public string SavingThrowName
        {
            get { return (string)this.GetValue(SavingThrowNameProperty); }
            set { this.SetValue(SavingThrowNameProperty, value); }
        }

        public string SavingThrowBonus
        {
            get
            {
                return (string)this.GetValue(SavingThrowBonusProperty);
            }

            set
            {
                this.SetValue(SavingThrowBonusProperty, value);
                this.SavingThrowBonusField.Text = value;
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
                this.SavingThrowGrid.Background = value;
                this.SavingThrowBorder.BorderBrush = value;
            }
        }

        public string ProficiencyToolTip => $"Toggle {this.SavingThrowName} Proficiency";

        public void Draw(Attribute attribute)
        {
            this.attribute = attribute;

            this.SavingThrowBonus = attribute.GetSaveBonus(Program.CcsFile.Character.ProficiencyBonus).ToString();
            this.ProficiencyToggle.Foreground = attribute.Proficiency ? ConciergeBrushes.Mint : Brushes.SlateGray;
            this.ProficiencyToggle.Kind = attribute.Proficiency ? PackIconKind.RhombusSplit : PackIconKind.RhombusSplitOutline;

            var status = attribute.GetSaveStatus(Program.CcsFile.Character.Vitality);
            SetTextStyleHelper(status, this.SavingThrowNameField);
            SetTextStyleHelper(status, this.SavingThrowBonusField);
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

        private void SavingThrowField_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowService.ShowAbilityCheckWindow(typeof(AbilityCheckWindow), this.attribute, 0);
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
            packIcon.Foreground = this.attribute.Proficiency ? ConciergeBrushes.Mint : Brushes.SlateGray;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void ProficiencyToggle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SoundService.PlayUpdateValue();

            var attributeCopy = this.attribute.DeepCopy();
            this.attribute.Proficiency = !this.attribute.Proficiency;

            this.RaiseEvent(new RoutedEventArgs(ToggleClickedEvent));

            Program.UndoRedoService.AddCommand(new EditCommand<Attribute>(this.attribute, attributeCopy, ConciergePages.Overview));
        }
    }
}
