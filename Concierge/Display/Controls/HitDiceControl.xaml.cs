﻿// <copyright file="HitDiceControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Display.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for HitDiceDisplay.xaml.
    /// </summary>
    public partial class HitDiceControl : UserControl
    {
        public static readonly DependencyProperty ConciergePageProperty =
            DependencyProperty.Register(
                "ConciergePage",
                typeof(ConciergePage),
                typeof(HitDiceControl),
                new UIPropertyMetadata(ConciergePage.Overview));

        public static readonly RoutedEvent EditClickedEvent =
            EventManager.RegisterRoutedEvent(
                "EditClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(HitDiceControl));

        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent(
                "ValueChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(HitDiceControl));

        public HitDiceControl()
        {
            this.InitializeComponent();
        }

        public event RoutedEventHandler EditClicked
        {
            add { this.AddHandler(EditClickedEvent, value); }
            remove { this.RemoveHandler(EditClickedEvent, value); }
        }

        public event RoutedEventHandler ValueChanged
        {
            add { this.AddHandler(ValueChangedEvent, value); }
            remove { this.RemoveHandler(ValueChangedEvent, value); }
        }

        public ConciergePage ConciergePage
        {
            get { return (ConciergePage)this.GetValue(ConciergePageProperty); }
            set { this.SetValue(ConciergePageProperty, value); }
        }

        public void DrawHitDice(HitDice hitDice)
        {
            DrawHitDiceHelper(this.D6TotalField, this.D6SpentField, this.D6Grid, this.D6Border, hitDice.SpentD6, hitDice.TotalD6);
            DrawHitDiceHelper(this.D8TotalField, this.D8SpentField, this.D8Grid, this.D8Border, hitDice.SpentD8, hitDice.TotalD8);
            DrawHitDiceHelper(this.D10TotalField, this.D10SpentField, this.D10Grid, this.D10Border, hitDice.SpentD10, hitDice.TotalD10);
            DrawHitDiceHelper(this.D12TotalField, this.D12SpentField, this.D12Grid, this.D12Border, hitDice.SpentD12, hitDice.TotalD12);
        }

        private static void DrawHitDiceHelper(TextBlock totalField, TextBlock spentField, Grid grid, Border border, int spent, int total)
        {
            totalField.Text = total.ToString();
            spentField.Text = spent.ToString();
            totalField.Foreground = spentField.Foreground = DisplayUtility.SetTotalTextStyle(total, spent);
            grid.Background = border.BorderBrush = DisplayUtility.SetTotalBoxStyle(total, spent);
        }

        private HitDice GetHitDice()
        {
            return this.ConciergePage switch
            {
                ConciergePage.Companion => Program.CcsFile.Character.Companion.Vitality.HitDice,
                _ => Program.CcsFile.Character.Vitality.HitDice,
            };
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Border border)
            {
                return;
            }

            var hitDice = this.GetHitDice();
            var oldItem = hitDice.DeepCopy();

            var result = hitDice.Increment(border.Name);
            if (result == 0)
            {
                return;
            }

            Program.UndoRedoService.AddCommand(new EditCommand<HitDice>(hitDice, oldItem, this.ConciergePage));
            Program.Modify();

            this.RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
        }

        private void Header_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is not Border border)
            {
                return;
            }

            var grid = DisplayUtility.FindVisualChildren<Grid>(border).FirstOrDefault(x => x.Name.Contains("HeaderGrid"));
            if (grid is null)
            {
                return;
            }

            border.BorderBrush = ConciergeColors.RectangleBorderHighlight;
            grid.Background = ConciergeColors.RectangleBorderHighlight;
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Header_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is not Border border)
            {
                return;
            }

            var grid = DisplayUtility.FindVisualChildren<Grid>(border).FirstOrDefault(x => x.Name.Contains("HeaderGrid"));
            if (grid is null)
            {
                return;
            }

            border.BorderBrush = ConciergeColors.ControlForeGray;
            grid.Background = ConciergeColors.ControlForeGray;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(EditClickedEvent));
        }
    }
}