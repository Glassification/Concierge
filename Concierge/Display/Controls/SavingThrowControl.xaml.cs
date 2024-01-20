// <copyright file="SavingThrowControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Character.AbilitySaves;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Common;
    using Concierge.Common.Extensions;
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

        public SavingThrowControl()
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

        public void SetStyle(SavingThrow savingThrow)
        {
            this.Tag = savingThrow;

            this.ProficiencyToggle.Foreground = savingThrow.Proficiency ? ConciergeBrushes.Mint : Brushes.SlateGray;
            this.ProficiencyToggle.Kind = savingThrow.Proficiency ? PackIconKind.RhombusSplit : PackIconKind.RhombusSplitOutline;

            SetTextStyleHelper(savingThrow.StatusChecks, this.SavingThrowNameField);
            SetTextStyleHelper(savingThrow.StatusChecks, this.SavingThrowBonusField);
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
            if (this.Tag is SavingThrow savingThrow)
            {
                ConciergeWindowService.ShowAbilityCheckWindow(typeof(AbilityCheckWindow), savingThrow, 0);
            }
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
            var save = Program.CcsFile.Character.SavingThrows.GetSavingThrow(this.SavingThrowName.Strip(" "));

            packIcon.Foreground = save.Proficiency ? ConciergeBrushes.Mint : Brushes.SlateGray;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void ProficiencyToggle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ConciergeSoundService.UpdateValue();

            var save = Program.CcsFile.Character.SavingThrows;
            var savelCopy = save.DeepCopy();

            var saveThrow = save.GetSavingThrow(this.SavingThrowName.Strip(" "));
            saveThrow.Proficiency = !saveThrow.Proficiency;

            this.RaiseEvent(new RoutedEventArgs(ToggleClickedEvent));

            Program.UndoRedoService.AddCommand(new EditCommand<SavingThrows>(save, savelCopy, ConciergePage.Overview));
        }
    }
}
