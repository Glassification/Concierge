// <copyright file="SpellSlotsControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.Magic;
    using Concierge.Commands;
    using Concierge.Common;
    using Concierge.Common.Utilities;
    using Concierge.Display.Enums;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for SpellSlotsControl.xaml.
    /// </summary>
    public partial class SpellSlotsControl : UserControl
    {
        public static readonly RoutedEvent EditClickedEvent =
            EventManager.RegisterRoutedEvent(
                "EditClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(SpellSlotsControl));

        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent(
                "ValueChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(SpellSlotsControl));

        public SpellSlotsControl()
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

        public void FillSpellSlots(SpellSlots spellSlots)
        {
            DrawSpellSlot(this.TotalFirstField, this.UsedFirstField, this.FirstGrid, this.FirstBorder, spellSlots.FirstUsed, spellSlots.FirstTotal);
            DrawSpellSlot(this.TotalSecondField, this.UsedSecondField, this.SecondGrid, this.SecondBorder, spellSlots.SecondUsed, spellSlots.SecondTotal);
            DrawSpellSlot(this.TotalThirdField, this.UsedThirdField, this.ThirdGrid, this.ThirdBorder, spellSlots.ThirdUsed, spellSlots.ThirdTotal);
            DrawSpellSlot(this.TotalFourthField, this.UsedFourthField, this.FourthGrid, this.FourthBorder, spellSlots.FourthUsed, spellSlots.FourthTotal);
            DrawSpellSlot(this.TotalFifthField, this.UsedFifthField, this.FifthGrid, this.FifthBorder, spellSlots.FifthUsed, spellSlots.FifthTotal);
            DrawSpellSlot(this.TotalSixthField, this.UsedSixthField, this.SixthGrid, this.SixthBorder, spellSlots.SixthUsed, spellSlots.SixthTotal);
            DrawSpellSlot(this.TotalSeventhField, this.UsedSeventhField, this.SeventhGrid, this.SeventhBorder, spellSlots.SeventhUsed, spellSlots.SeventhTotal);
            DrawSpellSlot(this.TotalEighthField, this.UsedEighthField, this.EighthGrid, this.EighthBorder, spellSlots.EighthUsed, spellSlots.EighthTotal);
            DrawSpellSlot(this.TotalNinethField, this.UsedNinethField, this.NinethGrid, this.NinethBorder, spellSlots.NinethUsed, spellSlots.NinethTotal);

            this.DrawSpellSlotHeader(this.FirstHeaderGrid, this.FirstHeaderBorder);
            this.DrawSpellSlotHeader(this.SecondHeaderGrid, this.SecondHeaderBorder);
            this.DrawSpellSlotHeader(this.ThirdHeaderGrid, this.ThirdHeaderBorder);
            this.DrawSpellSlotHeader(this.FourthHeaderGrid, this.FourthHeaderBorder);
            this.DrawSpellSlotHeader(this.FifthHeaderGrid, this.FifthHeaderBorder);
            this.DrawSpellSlotHeader(this.SixthHeaderGrid, this.SixthHeaderBorder);
            this.DrawSpellSlotHeader(this.SeventhHeaderGrid, this.SeventhHeaderBorder);
            this.DrawSpellSlotHeader(this.EighthHeaderGrid, this.EighthHeaderBorder);
            this.DrawSpellSlotHeader(this.NinethHeaderGrid, this.NinethHeaderBorder);
        }

        private static void DrawSpellSlot(TextBlock totalField, TextBlock spentField, Grid grid, Border border, int spent, int total)
        {
            totalField.Text = total.ToString();
            spentField.Text = spent.ToString();
            totalField.Foreground = spentField.Foreground = DisplayUtility.SetTotalTextStyle(total, spent);
            grid.Background = border.BorderBrush = DisplayUtility.SetTotalBoxStyle(total, spent);
        }

        private void DrawSpellSlotHeader(Grid header, Border headerBorder)
        {
            if (this.ShouldNotHighlight(header.Name))
            {
                headerBorder.BorderBrush = ConciergeBrushes.ControlForeBlue;
                header.Background = ConciergeBrushes.ControlForeBlue;
            }
        }

        private bool ShouldNotHighlight(string name)
        {
            return name switch
            {
                "FirstHeaderGrid" => this.TotalFirstField.Text.Equals(this.UsedFirstField.Text),
                "SecondHeaderGrid" => this.TotalSecondField.Text.Equals(this.UsedSecondField.Text),
                "ThirdHeaderGrid" => this.TotalThirdField.Text.Equals(this.UsedThirdField.Text),
                "FourthHeaderGrid" => this.TotalFourthField.Text.Equals(this.UsedFourthField.Text),
                "FifthHeaderGrid" => this.TotalFifthField.Text.Equals(this.UsedFifthField.Text),
                "SixthHeaderGrid" => this.TotalSixthField.Text.Equals(this.UsedSixthField.Text),
                "SeventhHeaderGrid" => this.TotalSeventhField.Text.Equals(this.UsedSeventhField.Text),
                "EighthHeaderGrid" => this.TotalEighthField.Text.Equals(this.UsedEighthField.Text),
                "NinethHeaderGrid" => this.TotalNinethField.Text.Equals(this.UsedNinethField.Text),
                _ => false,
            };
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Border border || this.ShouldNotHighlight(border.Name.Replace("Border", "Grid")))
            {
                return;
            }

            SoundService.PlayUpdateValue();
            var spellSlots = Program.CcsFile.Character.SpellCasting.SpellSlots;
            var oldItem = spellSlots.DeepCopy();

            var (used, total) = spellSlots.Increment(border.Name);
            if (used == 0)
            {
                return;
            }

            if (used == total)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }

            Program.UndoRedoService.AddCommand(new EditCommand<SpellSlots>(spellSlots, oldItem, ConciergePages.Spellcasting));

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
