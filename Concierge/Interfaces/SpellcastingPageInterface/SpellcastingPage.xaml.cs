// <copyright file="SpellcastingPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.SpellcastingPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Character.Enums;
    using Concierge.Character.Spellcasting;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for SpellcastingPage.xaml.
    /// </summary>
    public partial class SpellcastingPage : Page, IConciergePage
    {
        private readonly SpellcastingSelectionWindow spellcastingSelectionWindow = new ();
        private readonly ModifySpellWindow modifySpellWindow = new (ConciergePage.Spellcasting);
        private readonly ModifySpellClassWindow modifySpellClassWindow = new (ConciergePage.Spellcasting);
        private readonly ModifySpellSlotsWindow modifySpellSlotsWindow = new (ConciergePage.Spellcasting);

        public SpellcastingPage()
        {
            this.InitializeComponent();

            this.modifySpellWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifySpellClassWindow.ApplyChanges += this.Window_ApplyChanges;
            this.modifySpellSlotsWindow.ApplyChanges += this.Window_ApplyChanges;

            this.InitializeUsedSlot(this.UsedPactBox);
            this.InitializeUsedSlot(this.UsedFirstBox);
            this.InitializeUsedSlot(this.UsedSecondBox);
            this.InitializeUsedSlot(this.UsedThirdBox);
            this.InitializeUsedSlot(this.UsedFourthBox);
            this.InitializeUsedSlot(this.UsedFifthBox);
            this.InitializeUsedSlot(this.UsedSixthBox);
            this.InitializeUsedSlot(this.UsedSeventhBox);
            this.InitializeUsedSlot(this.UsedEighthBox);
            this.InitializeUsedSlot(this.UsedNinethBox);

            this.CurrentSpellBox = string.Empty;
        }

        private delegate void DrawList();

        public ConciergePage ConciergePage => ConciergePage.Spellcasting;

        private string CurrentSpellBox { get; set; }

        public void Draw()
        {
            this.DrawSpellList();
            this.DrawMagicClasses();
            this.DrawTotalSpellSlots();
            this.DrawUsedSpellSlots();
        }

        public void Edit(object itemToEdit)
        {
            if (itemToEdit is Spell)
            {
                var index = this.SpellListDataGrid.SelectedIndex;
                this.modifySpellWindow.EditSpell(itemToEdit as Spell);
                this.DrawSpellList();
                this.DrawMagicClasses();
                this.SpellListDataGrid.SetSelectedIndex(index);
            }
            else if (itemToEdit is MagicClass)
            {
                var index = this.MagicClassDataGrid.SelectedIndex;
                this.modifySpellClassWindow.EditClass(itemToEdit as MagicClass);
                this.DrawMagicClasses();
                this.MagicClassDataGrid.SetSelectedIndex(index);
            }
        }

        private static void FillTotalSpellSlot(TextBlock totalField, Grid totalBox, int usedSpells, int totalSpells)
        {
            totalField.Text = totalSpells.ToString();
            totalField.Foreground = Utilities.SetTotalTextStyle(totalSpells, usedSpells);
            totalBox.Background = Utilities.SetTotalBoxStyle(totalSpells, usedSpells);
        }

        private bool NextItem<T>(ConciergeDataGrid dataGrid, DrawList drawList, List<T> list, int limit, int increment)
        {
            var index = dataGrid.NextItem(list, limit, increment, this.ConciergePage);

            if (index != -1)
            {
                drawList();
                dataGrid.SetSelectedIndex(index);

                return true;
            }

            return false;
        }

        private void FillUsedSpellSlot(TextBlock usedField, Grid usedBox, Border border, int usedSpells, int totalSpells)
        {
            usedField.Text = usedSpells.ToString();
            usedField.Foreground = Utilities.SetUsedTextStyle(totalSpells, usedSpells);
            usedBox.Background = Utilities.SetUsedBoxStyle(totalSpells, usedSpells);
            Utilities.SetBorderColour(usedSpells, totalSpells, usedBox, border, this.CurrentSpellBox);
        }

        private void DrawTotalSpellSlots()
        {
            var spellSlots = Program.CcsFile.Character.SpellSlots;

            FillTotalSpellSlot(this.TotalPactField, this.TotalPactBox, spellSlots.PactUsed, spellSlots.PactTotal);
            FillTotalSpellSlot(this.TotalFirstField, this.TotalFirstBox, spellSlots.FirstUsed, spellSlots.FirstTotal);
            FillTotalSpellSlot(this.TotalSecondField, this.TotalSecondBox, spellSlots.SecondUsed, spellSlots.SecondTotal);
            FillTotalSpellSlot(this.TotalThirdField, this.TotalThirdBox, spellSlots.ThirdUsed, spellSlots.ThirdTotal);
            FillTotalSpellSlot(this.TotalFourthField, this.TotalFourthBox, spellSlots.FourthUsed, spellSlots.FourthTotal);
            FillTotalSpellSlot(this.TotalFifthField, this.TotalFifthBox, spellSlots.FifthUsed, spellSlots.FifthTotal);
            FillTotalSpellSlot(this.TotalSixthField, this.TotalSixthBox, spellSlots.SixthUsed, spellSlots.SixthTotal);
            FillTotalSpellSlot(this.TotalSeventhField, this.TotalSeventhBox, spellSlots.SeventhUsed, spellSlots.SeventhTotal);
            FillTotalSpellSlot(this.TotalEighthField, this.TotalEighthBox, spellSlots.EighthUsed, spellSlots.EighthTotal);
            FillTotalSpellSlot(this.TotalNinethField, this.TotalNinethBox, spellSlots.NinethUsed, spellSlots.NinethTotal);
        }

        private void DrawUsedSpellSlots()
        {
            var spellSlots = Program.CcsFile.Character.SpellSlots;

            this.FillUsedSpellSlot(this.UsedPactField, this.UsedPactBox, this.UsedPactBorder, spellSlots.PactUsed, spellSlots.PactTotal);
            this.FillUsedSpellSlot(this.UsedFirstField, this.UsedFirstBox, this.UsedFirstBorder, spellSlots.FirstUsed, spellSlots.FirstTotal);
            this.FillUsedSpellSlot(this.UsedSecondField, this.UsedSecondBox, this.UsedSecondBorder, spellSlots.SecondUsed, spellSlots.SecondTotal);
            this.FillUsedSpellSlot(this.UsedThirdField, this.UsedThirdBox, this.UsedThirdBorder, spellSlots.ThirdUsed, spellSlots.ThirdTotal);
            this.FillUsedSpellSlot(this.UsedFourthField, this.UsedFourthBox, this.UsedFourthBorder, spellSlots.FourthUsed, spellSlots.FourthTotal);
            this.FillUsedSpellSlot(this.UsedFifthField, this.UsedFifthBox, this.UsedFifthBorder, spellSlots.FifthUsed, spellSlots.FifthTotal);
            this.FillUsedSpellSlot(this.UsedSixthField, this.UsedSixthBox, this.UsedSixthBorder, spellSlots.SixthUsed, spellSlots.SixthTotal);
            this.FillUsedSpellSlot(this.UsedSeventhField, this.UsedSeventhBox, this.UsedSeventhBorder, spellSlots.SeventhUsed, spellSlots.SeventhTotal);
            this.FillUsedSpellSlot(this.UsedEighthField, this.UsedEighthBox, this.UsedEighthBorder, spellSlots.EighthUsed, spellSlots.EighthTotal);
            this.FillUsedSpellSlot(this.UsedNinethField, this.UsedNinethBox, this.UsedNinethBorder, spellSlots.NinethUsed, spellSlots.NinethTotal);
        }

        private void DrawSpellList()
        {
            this.SpellListDataGrid.Items.Clear();

            foreach (var spell in Program.CcsFile.Character.Spells)
            {
                this.SpellListDataGrid.Items.Add(spell);
            }
        }

        private void DrawMagicClasses()
        {
            this.MagicClassDataGrid.Items.Clear();

            foreach (var magicClass in Program.CcsFile.Character.MagicClasses)
            {
                this.MagicClassDataGrid.Items.Add(magicClass);
            }

            this.CasterLevelField.Text = Program.CcsFile.Character.CasterLevel.ToString();
        }

        private void InitializeUsedSlot(Grid usedSlot)
        {
            usedSlot.MouseDown += this.UsedSlot_MouseDown;
            usedSlot.MouseEnter += this.UsedSlot_MouseEnter;
            usedSlot.MouseLeave += this.UsedSlot_MouseLeave;
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            if (!this.NextItem(this.MagicClassDataGrid, this.DrawMagicClasses, Program.CcsFile.Character.MagicClasses, 0, -1))
            {
                this.NextItem(this.SpellListDataGrid, this.DrawSpellList, Program.CcsFile.Character.Spells, 0, -1);
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            if (!this.NextItem(this.MagicClassDataGrid, this.DrawMagicClasses, Program.CcsFile.Character.MagicClasses, Program.CcsFile.Character.MagicClasses.Count - 1, 1))
            {
                this.NextItem(this.SpellListDataGrid, this.DrawSpellList, Program.CcsFile.Character.Spells, Program.CcsFile.Character.Spells.Count - 1, 1);
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this.MagicClassDataGrid.UnselectAll();
            this.SpellListDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var popupButton = this.spellcastingSelectionWindow.ShowPopup();

            switch (popupButton)
            {
                case PopupButtons.AddMagicClass:
                    this.modifySpellClassWindow.AddClass(Program.CcsFile.Character.MagicClasses);
                    this.DrawMagicClasses();
                    if (this.modifySpellClassWindow.ItemsAdded)
                    {
                        this.MagicClassDataGrid.SetSelectedIndex(this.MagicClassDataGrid.LastIndex);
                    }

                    break;
                case PopupButtons.AddSpell:
                    this.modifySpellWindow.AddSpell(Program.CcsFile.Character.Spells);
                    this.DrawSpellList();
                    this.DrawMagicClasses();
                    if (this.modifySpellWindow.ItemsAdded)
                    {
                        this.SpellListDataGrid.SetSelectedIndex(this.SpellListDataGrid.LastIndex);
                    }

                    break;
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.MagicClassDataGrid.SelectedItem != null)
            {
                this.Edit(this.MagicClassDataGrid.SelectedItem);
            }
            else if (this.SpellListDataGrid.SelectedItem != null)
            {
                this.Edit(this.SpellListDataGrid.SelectedItem);
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.MagicClassDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var magicClass = (MagicClass)this.MagicClassDataGrid.SelectedItem;
                var index = this.MagicClassDataGrid.SelectedIndex;

                Program.UndoRedoService.AddCommand(new DeleteCommand<MagicClass>(Program.CcsFile.Character.MagicClasses, magicClass, index, this.ConciergePage));
                Program.CcsFile.Character.MagicClasses.Remove(magicClass);
                this.DrawMagicClasses();
                this.MagicClassDataGrid.SetSelectedIndex(index);
            }
            else if (this.SpellListDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var spell = (Spell)this.SpellListDataGrid.SelectedItem;
                var index = this.SpellListDataGrid.SelectedIndex;

                Program.UndoRedoService.AddCommand(new DeleteCommand<Spell>(Program.CcsFile.Character.Spells, spell, index, this.ConciergePage));
                Program.CcsFile.Character.Spells.Remove(spell);
                this.DrawSpellList();
                this.SpellListDataGrid.SetSelectedIndex(index);
            }
        }

        private void MagicClassDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.MagicClassDataGrid.SelectedItem != null)
            {
                this.SpellListDataGrid.UnselectAll();
            }
        }

        private void SpellListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SpellListDataGrid.SelectedItem != null)
            {
                this.MagicClassDataGrid.UnselectAll();
            }
        }

        private void LevelEditButton_Click(object sender, RoutedEventArgs e)
        {
            this.modifySpellSlotsWindow.ShowEdit(Program.CcsFile.Character.SpellSlots);
            this.DrawTotalSpellSlots();
            this.DrawUsedSpellSlots();
        }

        private void UsedSlot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2)
            {
                return;
            }

            var spellSlots = Program.CcsFile.Character.SpellSlots;
            var oldItem = spellSlots.DeepCopy() as SpellSlots;
            var usedBox = sender as Grid;

            switch (usedBox.Name)
            {
                case "UsedPactBox":
                    spellSlots.PactUsed = Utilities.IncrementUsedSlots(spellSlots.PactUsed, spellSlots.PactTotal);
                    Utilities.SetCursor(spellSlots.PactUsed, spellSlots.PactTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
                case "UsedFirstBox":
                    spellSlots.FirstUsed = Utilities.IncrementUsedSlots(spellSlots.FirstUsed, spellSlots.FirstTotal);
                    Utilities.SetCursor(spellSlots.FirstUsed, spellSlots.FirstTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
                case "UsedSecondBox":
                    spellSlots.SecondUsed = Utilities.IncrementUsedSlots(spellSlots.SecondUsed, spellSlots.SecondTotal);
                    Utilities.SetCursor(spellSlots.SecondUsed, spellSlots.SecondTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
                case "UsedThirdBox":
                    spellSlots.ThirdUsed = Utilities.IncrementUsedSlots(spellSlots.ThirdUsed, spellSlots.ThirdTotal);
                    Utilities.SetCursor(spellSlots.ThirdUsed, spellSlots.ThirdTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
                case "UsedFourthBox":
                    spellSlots.FourthUsed = Utilities.IncrementUsedSlots(spellSlots.FourthUsed, spellSlots.FourthTotal);
                    Utilities.SetCursor(spellSlots.FourthUsed, spellSlots.FourthTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
                case "UsedFifthBox":
                    spellSlots.FifthUsed = Utilities.IncrementUsedSlots(spellSlots.FifthUsed, spellSlots.FifthTotal);
                    Utilities.SetCursor(spellSlots.FifthUsed, spellSlots.FifthTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
                case "UsedSixthBox":
                    spellSlots.SixthUsed = Utilities.IncrementUsedSlots(spellSlots.SixthUsed, spellSlots.SixthTotal);
                    Utilities.SetCursor(spellSlots.SixthUsed, spellSlots.SixthTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
                case "UsedSeventhBox":
                    spellSlots.SeventhUsed = Utilities.IncrementUsedSlots(spellSlots.SeventhUsed, spellSlots.SeventhTotal);
                    Utilities.SetCursor(spellSlots.SeventhUsed, spellSlots.SeventhTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
                case "UsedEighthBox":
                    spellSlots.EighthUsed = Utilities.IncrementUsedSlots(spellSlots.EighthUsed, spellSlots.EighthTotal);
                    Utilities.SetCursor(spellSlots.EighthUsed, spellSlots.EighthTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
                case "UsedNinethBox":
                    spellSlots.NinethUsed = Utilities.IncrementUsedSlots(spellSlots.NinethUsed, spellSlots.NinethTotal);
                    Utilities.SetCursor(spellSlots.NinethUsed, spellSlots.NinethTotal, (x, y) => x >= y, Cursors.Arrow);
                    break;
            }

            Program.UndoRedoService.AddCommand(new EditCommand<SpellSlots>(spellSlots, oldItem, this.ConciergePage));

            this.DrawTotalSpellSlots();
            this.DrawUsedSpellSlots();
        }

        private void UsedSlot_MouseEnter(object sender, MouseEventArgs e)
        {
            var grid = sender as Grid;
            var spellSlots = Program.CcsFile.Character.SpellSlots;

            this.CurrentSpellBox = grid.Name;

            switch (grid.Name)
            {
                case "UsedPactBox":
                    Utilities.SetCursor(spellSlots.PactUsed, spellSlots.PactTotal, (x, y) => x != y, Cursors.Hand);
                    Utilities.SetBorderColour(spellSlots.PactUsed, spellSlots.PactTotal, grid, this.UsedPactBorder, this.CurrentSpellBox);
                    break;
                case "UsedFirstBox":
                    Utilities.SetCursor(spellSlots.FirstUsed, spellSlots.FirstTotal, (x, y) => x != y, Cursors.Hand);
                    Utilities.SetBorderColour(spellSlots.FirstUsed, spellSlots.FirstTotal, grid, this.UsedFirstBorder, this.CurrentSpellBox);
                    break;
                case "UsedSecondBox":
                    Utilities.SetCursor(spellSlots.SecondUsed, spellSlots.SecondTotal, (x, y) => x != y, Cursors.Hand);
                    Utilities.SetBorderColour(spellSlots.SecondUsed, spellSlots.SecondTotal, grid, this.UsedSecondBorder, this.CurrentSpellBox);
                    break;
                case "UsedThirdBox":
                    Utilities.SetCursor(spellSlots.ThirdUsed, spellSlots.ThirdTotal, (x, y) => x != y, Cursors.Hand);
                    Utilities.SetBorderColour(spellSlots.ThirdUsed, spellSlots.ThirdTotal, grid, this.UsedThirdBorder, this.CurrentSpellBox);
                    break;
                case "UsedFourthBox":
                    Utilities.SetCursor(spellSlots.FourthUsed, spellSlots.FourthTotal, (x, y) => x != y, Cursors.Hand);
                    Utilities.SetBorderColour(spellSlots.FourthUsed, spellSlots.FourthTotal, grid, this.UsedFourthBorder, this.CurrentSpellBox);
                    break;
                case "UsedFifthBox":
                    Utilities.SetCursor(spellSlots.FifthUsed, spellSlots.FifthTotal, (x, y) => x != y, Cursors.Hand);
                    Utilities.SetBorderColour(spellSlots.FifthUsed, spellSlots.FifthTotal, grid, this.UsedFifthBorder, this.CurrentSpellBox);
                    break;
                case "UsedSixthBox":
                    Utilities.SetCursor(spellSlots.SixthUsed, spellSlots.SixthTotal, (x, y) => x != y, Cursors.Hand);
                    Utilities.SetBorderColour(spellSlots.SixthUsed, spellSlots.SixthTotal, grid, this.UsedSixthBorder, this.CurrentSpellBox);
                    break;
                case "UsedSeventhBox":
                    Utilities.SetCursor(spellSlots.SeventhUsed, spellSlots.SeventhTotal, (x, y) => x != y, Cursors.Hand);
                    Utilities.SetBorderColour(spellSlots.SeventhUsed, spellSlots.SeventhTotal, grid, this.UsedSeventhBorder, this.CurrentSpellBox);
                    break;
                case "UsedEighthBox":
                    Utilities.SetCursor(spellSlots.EighthUsed, spellSlots.EighthTotal, (x, y) => x != y, Cursors.Hand);
                    Utilities.SetBorderColour(spellSlots.EighthUsed, spellSlots.EighthTotal, grid, this.UsedEighthBorder, this.CurrentSpellBox);
                    break;
                case "UsedNinethBox":
                    Utilities.SetCursor(spellSlots.NinethUsed, spellSlots.NinethTotal, (x, y) => x != y, Cursors.Hand);
                    Utilities.SetBorderColour(spellSlots.NinethUsed, spellSlots.NinethTotal, grid, this.UsedNinethBorder, this.CurrentSpellBox);
                    break;
            }
        }

        private void UsedSlot_MouseLeave(object sender, MouseEventArgs e)
        {
            var grid = sender as Grid;

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

        private void SpellListDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Utilities.SortListFromDataGrid(this.SpellListDataGrid, Program.CcsFile.Character.Spells, this.ConciergePage);
        }

        private void MagicClassDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Utilities.SortListFromDataGrid(this.MagicClassDataGrid, Program.CcsFile.Character.MagicClasses, this.ConciergePage);
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case "ModifySpellClassWindow":
                    this.DrawMagicClasses();
                    break;
                case "ModifySpellWindow":
                    this.DrawSpellList();
                    this.DrawMagicClasses();
                    break;
                case "ModifySpellSlotsWindow":
                    this.DrawUsedSpellSlots();
                    this.DrawTotalSpellSlots();
                    break;
            }
        }
    }
}
