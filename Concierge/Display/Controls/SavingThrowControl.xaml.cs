// <copyright file="SavingThrowControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using Concierge.Character.AbilitySavingThrows;
    using Concierge.Character.Enums;
    using Concierge.Commands;
    using Concierge.Display.Enums;
    using Concierge.Display.Pages;
    using Concierge.Utility;

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

        public void SetStyle(bool proficiencyFlag, StatusChecks check)
        {
            this.ProficiencyBox.Fill = proficiencyFlag ? Brushes.SteelBlue : Brushes.Transparent;

            SetTextStyleHelper(check, this.SavingThrowNameField);
            SetTextStyleHelper(check, this.SavingThrowBonusField);
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

            var save = Program.CcsFile.Character.SavingThrow.GetSavingThrow(this.SavingThrowName);

            ellipse.Stroke = Brushes.SteelBlue;
            ellipse.Fill = save.Proficiency ? Brushes.SteelBlue : Brushes.Transparent;
            ellipse.StrokeThickness = 1;

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void SavingThrowProficiency_MouseDown(object sender, RoutedEventArgs e)
        {
            ConciergeSound.UpdateValue();

            var savingThrow = Program.CcsFile.Character.SavingThrow;
            var savingThrowCopy = savingThrow.DeepCopy();

            var save = savingThrow.GetSavingThrow(this.SavingThrowName);
            save.Proficiency = !save.Proficiency;

            this.RaiseEvent(new RoutedEventArgs(ToggleClickedEvent));

            Program.UndoRedoService.AddCommand(new EditCommand<SavingThrow>(savingThrow, savingThrowCopy, ConciergePage.Overview));
            Program.Modify();
        }
    }
}
