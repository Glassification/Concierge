// <copyright file="SpellSlotsDisplay.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.Spellcasting;
    using Concierge.Commands;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for SpellSlotsDisplay.xaml.
    /// </summary>
    public partial class SpellSlotsDisplay : UserControl
    {
        public static readonly RoutedEvent EditClickedEvent =
            EventManager.RegisterRoutedEvent(
                "EditClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(SpellSlotsDisplay));

        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent(
                "ValueChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(SpellSlotsDisplay));

        public SpellSlotsDisplay()
        {
            this.InitializeComponent();
            this.CurrentSpellBox = string.Empty;
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

        private string CurrentSpellBox { get; set; }

        public void InitializeUsedSlot()
        {
            this.InitializeUsedSlotHelper(this.UsedPactBox);
            this.InitializeUsedSlotHelper(this.UsedFirstBox);
            this.InitializeUsedSlotHelper(this.UsedSecondBox);
            this.InitializeUsedSlotHelper(this.UsedThirdBox);
            this.InitializeUsedSlotHelper(this.UsedFourthBox);
            this.InitializeUsedSlotHelper(this.UsedFifthBox);
            this.InitializeUsedSlotHelper(this.UsedSixthBox);
            this.InitializeUsedSlotHelper(this.UsedSeventhBox);
            this.InitializeUsedSlotHelper(this.UsedEighthBox);
            this.InitializeUsedSlotHelper(this.UsedNinethBox);
        }

        public void FillTotalSpellSlot(SpellSlots spellSlots)
        {
            this.FillTotalSpellSlotHelper(this.TotalPactField, this.TotalPactBox, spellSlots.PactUsed, spellSlots.PactTotal);
            this.FillTotalSpellSlotHelper(this.TotalFirstField, this.TotalFirstBox, spellSlots.FirstUsed, spellSlots.FirstTotal);
            this.FillTotalSpellSlotHelper(this.TotalSecondField, this.TotalSecondBox, spellSlots.SecondUsed, spellSlots.SecondTotal);
            this.FillTotalSpellSlotHelper(this.TotalThirdField, this.TotalThirdBox, spellSlots.ThirdUsed, spellSlots.ThirdTotal);
            this.FillTotalSpellSlotHelper(this.TotalFourthField, this.TotalFourthBox, spellSlots.FourthUsed, spellSlots.FourthTotal);
            this.FillTotalSpellSlotHelper(this.TotalFifthField, this.TotalFifthBox, spellSlots.FifthUsed, spellSlots.FifthTotal);
            this.FillTotalSpellSlotHelper(this.TotalSixthField, this.TotalSixthBox, spellSlots.SixthUsed, spellSlots.SixthTotal);
            this.FillTotalSpellSlotHelper(this.TotalSeventhField, this.TotalSeventhBox, spellSlots.SeventhUsed, spellSlots.SeventhTotal);
            this.FillTotalSpellSlotHelper(this.TotalEighthField, this.TotalEighthBox, spellSlots.EighthUsed, spellSlots.EighthTotal);
            this.FillTotalSpellSlotHelper(this.TotalNinethField, this.TotalNinethBox, spellSlots.NinethUsed, spellSlots.NinethTotal);
        }

        public void FillUsedSpellSlot(SpellSlots spellSlots)
        {
            this.FillUsedSpellSlotHelper(this.UsedPactField, this.UsedPactBox, this.UsedPactBorder, spellSlots.PactUsed, spellSlots.PactTotal);
            this.FillUsedSpellSlotHelper(this.UsedFirstField, this.UsedFirstBox, this.UsedFirstBorder, spellSlots.FirstUsed, spellSlots.FirstTotal);
            this.FillUsedSpellSlotHelper(this.UsedSecondField, this.UsedSecondBox, this.UsedSecondBorder, spellSlots.SecondUsed, spellSlots.SecondTotal);
            this.FillUsedSpellSlotHelper(this.UsedThirdField, this.UsedThirdBox, this.UsedThirdBorder, spellSlots.ThirdUsed, spellSlots.ThirdTotal);
            this.FillUsedSpellSlotHelper(this.UsedFourthField, this.UsedFourthBox, this.UsedFourthBorder, spellSlots.FourthUsed, spellSlots.FourthTotal);
            this.FillUsedSpellSlotHelper(this.UsedFifthField, this.UsedFifthBox, this.UsedFifthBorder, spellSlots.FifthUsed, spellSlots.FifthTotal);
            this.FillUsedSpellSlotHelper(this.UsedSixthField, this.UsedSixthBox, this.UsedSixthBorder, spellSlots.SixthUsed, spellSlots.SixthTotal);
            this.FillUsedSpellSlotHelper(this.UsedSeventhField, this.UsedSeventhBox, this.UsedSeventhBorder, spellSlots.SeventhUsed, spellSlots.SeventhTotal);
            this.FillUsedSpellSlotHelper(this.UsedEighthField, this.UsedEighthBox, this.UsedEighthBorder, spellSlots.EighthUsed, spellSlots.EighthTotal);
            this.FillUsedSpellSlotHelper(this.UsedNinethField, this.UsedNinethBox, this.UsedNinethBorder, spellSlots.NinethUsed, spellSlots.NinethTotal);
        }

        private void InitializeUsedSlotHelper(Grid usedSlot)
        {
            usedSlot.MouseDown += this.UsedSlot_MouseDown;
            usedSlot.MouseEnter += this.UsedSlot_MouseEnter;
            usedSlot.MouseLeave += this.UsedSlot_MouseLeave;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "The way she goes.")]
        private void FillTotalSpellSlotHelper(TextBlock totalField, Grid totalBox, int usedSpells, int totalSpells)
        {
            totalField.Text = totalSpells.ToString();
            totalField.Foreground = DisplayUtility.SetTotalTextStyle(totalSpells, usedSpells);
            totalBox.Background = DisplayUtility.SetTotalBoxStyle(totalSpells, usedSpells);
        }

        private void FillUsedSpellSlotHelper(TextBlock usedField, Grid usedBox, Border border, int usedSpells, int totalSpells)
        {
            usedField.Text = usedSpells.ToString();
            usedField.Foreground = DisplayUtility.SetUsedTextStyle(totalSpells, usedSpells);
            usedBox.Background = DisplayUtility.SetUsedBoxStyle(totalSpells, usedSpells);
            DisplayUtility.SetBorderColour(usedSpells, totalSpells, usedBox, border, this.CurrentSpellBox);
        }

        private void UsedSlot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2 || sender is not Grid usedBox)
            {
                return;
            }

            var spellSlots = Program.CcsFile.Character.SpellSlots;
            var oldItem = spellSlots.DeepCopy();
            switch (usedBox.Name)
            {
                case "UsedPactBox":
                    spellSlots.PactUsed = DisplayUtility.IncrementUsedSlots(spellSlots.PactUsed, spellSlots.PactTotal);
                    DisplayUtility.SetCursor(spellSlots.PactUsed, spellSlots.PactTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
                case "UsedFirstBox":
                    spellSlots.FirstUsed = DisplayUtility.IncrementUsedSlots(spellSlots.FirstUsed, spellSlots.FirstTotal);
                    DisplayUtility.SetCursor(spellSlots.FirstUsed, spellSlots.FirstTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
                case "UsedSecondBox":
                    spellSlots.SecondUsed = DisplayUtility.IncrementUsedSlots(spellSlots.SecondUsed, spellSlots.SecondTotal);
                    DisplayUtility.SetCursor(spellSlots.SecondUsed, spellSlots.SecondTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
                case "UsedThirdBox":
                    spellSlots.ThirdUsed = DisplayUtility.IncrementUsedSlots(spellSlots.ThirdUsed, spellSlots.ThirdTotal);
                    DisplayUtility.SetCursor(spellSlots.ThirdUsed, spellSlots.ThirdTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
                case "UsedFourthBox":
                    spellSlots.FourthUsed = DisplayUtility.IncrementUsedSlots(spellSlots.FourthUsed, spellSlots.FourthTotal);
                    DisplayUtility.SetCursor(spellSlots.FourthUsed, spellSlots.FourthTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
                case "UsedFifthBox":
                    spellSlots.FifthUsed = DisplayUtility.IncrementUsedSlots(spellSlots.FifthUsed, spellSlots.FifthTotal);
                    DisplayUtility.SetCursor(spellSlots.FifthUsed, spellSlots.FifthTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
                case "UsedSixthBox":
                    spellSlots.SixthUsed = DisplayUtility.IncrementUsedSlots(spellSlots.SixthUsed, spellSlots.SixthTotal);
                    DisplayUtility.SetCursor(spellSlots.SixthUsed, spellSlots.SixthTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
                case "UsedSeventhBox":
                    spellSlots.SeventhUsed = DisplayUtility.IncrementUsedSlots(spellSlots.SeventhUsed, spellSlots.SeventhTotal);
                    DisplayUtility.SetCursor(spellSlots.SeventhUsed, spellSlots.SeventhTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
                case "UsedEighthBox":
                    spellSlots.EighthUsed = DisplayUtility.IncrementUsedSlots(spellSlots.EighthUsed, spellSlots.EighthTotal);
                    DisplayUtility.SetCursor(spellSlots.EighthUsed, spellSlots.EighthTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
                case "UsedNinethBox":
                    spellSlots.NinethUsed = DisplayUtility.IncrementUsedSlots(spellSlots.NinethUsed, spellSlots.NinethTotal);
                    DisplayUtility.SetCursor(spellSlots.NinethUsed, spellSlots.NinethTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
            }

            Program.UndoRedoService.AddCommand(new EditCommand<SpellSlots>(spellSlots, oldItem, ConciergePage.Spellcasting));
            Program.Modify();

            this.RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
        }

        private void UsedSlot_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is not Grid grid)
            {
                return;
            }

            var spellSlots = Program.CcsFile.Character.SpellSlots;
            this.CurrentSpellBox = grid.Name;
            switch (grid.Name)
            {
                case "UsedPactBox":
                    DisplayUtility.SetCursor(spellSlots.PactUsed, spellSlots.PactTotal, (x, y) => x != y, Cursors.Hand);
                    DisplayUtility.SetBorderColour(spellSlots.PactUsed, spellSlots.PactTotal, grid, this.UsedPactBorder, this.CurrentSpellBox);
                    break;
                case "UsedFirstBox":
                    DisplayUtility.SetCursor(spellSlots.FirstUsed, spellSlots.FirstTotal, (x, y) => x != y, Cursors.Hand);
                    DisplayUtility.SetBorderColour(spellSlots.FirstUsed, spellSlots.FirstTotal, grid, this.UsedFirstBorder, this.CurrentSpellBox);
                    break;
                case "UsedSecondBox":
                    DisplayUtility.SetCursor(spellSlots.SecondUsed, spellSlots.SecondTotal, (x, y) => x != y, Cursors.Hand);
                    DisplayUtility.SetBorderColour(spellSlots.SecondUsed, spellSlots.SecondTotal, grid, this.UsedSecondBorder, this.CurrentSpellBox);
                    break;
                case "UsedThirdBox":
                    DisplayUtility.SetCursor(spellSlots.ThirdUsed, spellSlots.ThirdTotal, (x, y) => x != y, Cursors.Hand);
                    DisplayUtility.SetBorderColour(spellSlots.ThirdUsed, spellSlots.ThirdTotal, grid, this.UsedThirdBorder, this.CurrentSpellBox);
                    break;
                case "UsedFourthBox":
                    DisplayUtility.SetCursor(spellSlots.FourthUsed, spellSlots.FourthTotal, (x, y) => x != y, Cursors.Hand);
                    DisplayUtility.SetBorderColour(spellSlots.FourthUsed, spellSlots.FourthTotal, grid, this.UsedFourthBorder, this.CurrentSpellBox);
                    break;
                case "UsedFifthBox":
                    DisplayUtility.SetCursor(spellSlots.FifthUsed, spellSlots.FifthTotal, (x, y) => x != y, Cursors.Hand);
                    DisplayUtility.SetBorderColour(spellSlots.FifthUsed, spellSlots.FifthTotal, grid, this.UsedFifthBorder, this.CurrentSpellBox);
                    break;
                case "UsedSixthBox":
                    DisplayUtility.SetCursor(spellSlots.SixthUsed, spellSlots.SixthTotal, (x, y) => x != y, Cursors.Hand);
                    DisplayUtility.SetBorderColour(spellSlots.SixthUsed, spellSlots.SixthTotal, grid, this.UsedSixthBorder, this.CurrentSpellBox);
                    break;
                case "UsedSeventhBox":
                    DisplayUtility.SetCursor(spellSlots.SeventhUsed, spellSlots.SeventhTotal, (x, y) => x != y, Cursors.Hand);
                    DisplayUtility.SetBorderColour(spellSlots.SeventhUsed, spellSlots.SeventhTotal, grid, this.UsedSeventhBorder, this.CurrentSpellBox);
                    break;
                case "UsedEighthBox":
                    DisplayUtility.SetCursor(spellSlots.EighthUsed, spellSlots.EighthTotal, (x, y) => x != y, Cursors.Hand);
                    DisplayUtility.SetBorderColour(spellSlots.EighthUsed, spellSlots.EighthTotal, grid, this.UsedEighthBorder, this.CurrentSpellBox);
                    break;
                case "UsedNinethBox":
                    DisplayUtility.SetCursor(spellSlots.NinethUsed, spellSlots.NinethTotal, (x, y) => x != y, Cursors.Hand);
                    DisplayUtility.SetBorderColour(spellSlots.NinethUsed, spellSlots.NinethTotal, grid, this.UsedNinethBorder, this.CurrentSpellBox);
                    break;
            }
        }

        private void UsedSlot_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is not Grid grid)
            {
                return;
            }

            switch (grid.Name)
            {
                case "UsedPactBox":
                    this.UsedPactBorder.BorderBrush = grid.Background;
                    break;
                case "UsedFirstBox":
                    this.UsedFirstBorder.BorderBrush = grid.Background;
                    break;
                case "UsedSecondBox":
                    this.UsedSecondBorder.BorderBrush = grid.Background;
                    break;
                case "UsedThirdBox":
                    this.UsedThirdBorder.BorderBrush = grid.Background;
                    break;
                case "UsedFourthBox":
                    this.UsedFourthBorder.BorderBrush = grid.Background;
                    break;
                case "UsedFifthBox":
                    this.UsedFifthBorder.BorderBrush = grid.Background;
                    break;
                case "UsedSixthBox":
                    this.UsedSixthBorder.BorderBrush = grid.Background;
                    break;
                case "UsedSeventhBox":
                    this.UsedSeventhBorder.BorderBrush = grid.Background;
                    break;
                case "UsedEighthBox":
                    this.UsedEighthBorder.BorderBrush = grid.Background;
                    break;
                case "UsedNinethBox":
                    this.UsedNinethBorder.BorderBrush = grid.Background;
                    break;
            }

            Mouse.OverrideCursor = Cursors.Arrow;
            this.CurrentSpellBox = string.Empty;
        }

        private void LevelEditButton_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(EditClickedEvent));
        }
    }
}
