// <copyright file="SpellSlotsControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.Spellcasting;
    using Concierge.Character.Statuses;
    using Concierge.Commands;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Utilities;

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

        public void FillSpellSlot(SpellSlots spellSlots)
        {
            DrawSpellSlotsHelper(this.TotalPactField, this.UsedPactField, this.PactGrid, this.PactBorder, spellSlots.PactUsed, spellSlots.PactTotal);
            DrawSpellSlotsHelper(this.TotalFirstField, this.UsedFirstField, this.FirstGrid, this.FirstBorder, spellSlots.FirstUsed, spellSlots.FirstTotal);
            DrawSpellSlotsHelper(this.TotalSecondField, this.UsedSecondField, this.SecondGrid, this.SecondBorder, spellSlots.SecondUsed, spellSlots.SecondTotal);
            DrawSpellSlotsHelper(this.TotalThirdField, this.UsedThirdField, this.ThirdGrid, this.ThirdBorder, spellSlots.ThirdUsed, spellSlots.ThirdTotal);
            DrawSpellSlotsHelper(this.TotalFourthField, this.UsedFourthField, this.FourthGrid, this.FourthBorder, spellSlots.FourthUsed, spellSlots.FourthTotal);
            DrawSpellSlotsHelper(this.TotalFifthField, this.UsedFifthField, this.FifthGrid, this.FifthBorder, spellSlots.FifthUsed, spellSlots.FifthTotal);
            DrawSpellSlotsHelper(this.TotalSixthField, this.UsedSixthField, this.SixthGrid, this.SixthBorder, spellSlots.SixthUsed, spellSlots.SixthTotal);
            DrawSpellSlotsHelper(this.TotalSeventhField, this.UsedSeventhField, this.SeventhGrid, this.SeventhBorder, spellSlots.SeventhUsed, spellSlots.SeventhTotal);
            DrawSpellSlotsHelper(this.TotalEighthField, this.UsedEighthField, this.EighthGrid, this.EighthBorder, spellSlots.EighthUsed, spellSlots.EighthTotal);
            DrawSpellSlotsHelper(this.TotalNinethField, this.UsedNinethField, this.NinethGrid, this.NinethBorder, spellSlots.NinethUsed, spellSlots.NinethTotal);
        }

        private static void DrawSpellSlotsHelper(TextBlock totalField, TextBlock spentField, Grid grid, Border border, int spent, int total)
        {
            totalField.Text = total.ToString();
            spentField.Text = spent.ToString();
            totalField.Foreground = spentField.Foreground = DisplayUtility.SetTotalTextStyle(total, spent);
            grid.Background = border.BorderBrush = DisplayUtility.SetTotalBoxStyle(total, spent);
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Border border)
            {
                return;
            }

            var spellSlots = Program.CcsFile.Character.SpellSlots;
            var oldItem = spellSlots.DeepCopy();

            var result = spellSlots.Increment(border.Name);
            if (result == 0)
            {
                return;
            }

            Program.UndoRedoService.AddCommand(new EditCommand<SpellSlots>(spellSlots, oldItem, ConciergePage.Spellcasting));
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

        private void LevelEditButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(EditClickedEvent));
        }
    }
}
