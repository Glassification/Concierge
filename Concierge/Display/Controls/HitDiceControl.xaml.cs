// <copyright file="HitDiceControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Vitals;
    using Concierge.Commands;
    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Common.Utilities;
    using Concierge.Display.Enums;
    using Concierge.Services;
    using Microsoft.VisualBasic.ApplicationServices;

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
            DrawHitDie(this.D6TotalField, this.D6SpentField, this.D6Grid, this.D6Border, hitDice.SpentD6, hitDice.TotalD6);
            DrawHitDie(this.D8TotalField, this.D8SpentField, this.D8Grid, this.D8Border, hitDice.SpentD8, hitDice.TotalD8);
            DrawHitDie(this.D10TotalField, this.D10SpentField, this.D10Grid, this.D10Border, hitDice.SpentD10, hitDice.TotalD10);
            DrawHitDie(this.D12TotalField, this.D12SpentField, this.D12Grid, this.D12Border, hitDice.SpentD12, hitDice.TotalD12);

            this.DrawHitDieHeader(this.D6HeaderGrid, this.D6HeaderBorder);
            this.DrawHitDieHeader(this.D8HeaderGrid, this.D8HeaderBorder);
            this.DrawHitDieHeader(this.D10HeaderGrid, this.D10HeaderBorder);
            this.DrawHitDieHeader(this.D12HeaderGrid, this.D12HeaderBorder);
        }

        private static void DrawHitDie(TextBlock totalField, TextBlock spentField, Grid grid, Border border, int spent, int total)
        {
            totalField.Text = total.ToString();
            spentField.Text = spent.ToString();
            totalField.Foreground = spentField.Foreground = DisplayUtility.SetTotalTextStyle(total, spent);
            grid.Background = border.BorderBrush = DisplayUtility.SetTotalBoxStyle(total, spent);
        }

        private void DrawHitDieHeader(Grid header, Border headerBorder)
        {
            if (this.ShouldNotHighlight(header.Name))
            {
                headerBorder.BorderBrush = ConciergeBrushes.ControlForeBlue;
                header.Background = ConciergeBrushes.ControlForeBlue;
            }
        }

        private Vitality GetVitality()
        {
            return this.ConciergePage switch
            {
                ConciergePage.Companion => Program.CcsFile.Character.Companion.Vitality,
                _ => Program.CcsFile.Character.Vitality,
            };
        }

        private Attributes GetAttributes()
        {
            return this.ConciergePage switch
            {
                ConciergePage.Companion => Program.CcsFile.Character.Companion.Characteristic.Attributes,
                _ => Program.CcsFile.Character.Characteristic.Attributes,
            };
        }

        private bool ShouldNotHighlight(string name)
        {
            return name switch
            {
                "D6HeaderGrid" => this.D6TotalField.Text.Equals(this.D6SpentField.Text),
                "D8HeaderGrid" => this.D8TotalField.Text.Equals(this.D8SpentField.Text),
                "D10HeaderGrid" => this.D10TotalField.Text.Equals(this.D10SpentField.Text),
                "D12HeaderGrid" => this.D12TotalField.Text.Equals(this.D12SpentField.Text),
                _ => false,
            };
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Border border || this.ShouldNotHighlight(border.Name.Replace("Border", "Grid")))
            {
                return;
            }

            ConciergeSoundService.UpdateValue();
            var vitality = this.GetVitality();
            if (vitality.Health.IsFull)
            {
                return;
            }

            var oldItem = vitality.DeepCopy();

            var (dice, used, total) = vitality.HitDice.Increment(border.Name);
            if (dice == Dice.None)
            {
                return;
            }

            if (used == total)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }

            var attributes = this.GetAttributes();
            var roll = vitality.RollHitDice(dice, attributes);

            Program.MainWindow?.DisplayStatusText($"Rolled Hit Die: {roll}");
            Program.UndoRedoService.AddCommand(new EditCommand<Vitality>(vitality, oldItem, this.ConciergePage));

            this.RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
        }

        private void Header_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is not Border border)
            {
                return;
            }

            var grid = DisplayUtility.FindVisualChildren<Grid>(border).FirstOrDefault(x => x.Name.Contains("HeaderGrid"));
            if (grid is null || this.ShouldNotHighlight(grid.Name))
            {
                return;
            }

            border.BorderBrush = ConciergeBrushes.BorderHighlight;
            grid.Background = ConciergeBrushes.BorderHighlight;
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Header_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is not Border border)
            {
                return;
            }

            var grid = DisplayUtility.FindVisualChildren<Grid>(border).FirstOrDefault(x => x.Name.Contains("HeaderGrid"));
            if (grid is null || this.ShouldNotHighlight(grid.Name))
            {
                return;
            }

            border.BorderBrush = ConciergeBrushes.ControlForeBlue;
            grid.Background = ConciergeBrushes.ControlForeBlue;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(EditClickedEvent));
        }
    }
}
