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
    using Concierge.Interfaces.Components;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for SpellcastingPage.xaml.
    /// </summary>
    public partial class SpellcastingPage : Page, IConciergePage
    {
        private readonly SpellcastingSelectionWindow spellcastingSelectionWindow = new ();
        private readonly ModifySpellWindow modifySpellWindow = new ();
        private readonly ModifySpellClassWindow modifySpellClassWindow = new ();
        private readonly ModifySpellSlotsWindow modifySpellSlotsWindow = new ();

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
        }

        private delegate void DrawList();

        public void Draw()
        {
            this.DrawSpellList();
            this.DrawMagicClasses();
            this.DrawTotalSpellSlots();
            this.DrawUsedSpellSlots();
        }

        private static void FillTotalSpellSlot(TextBlock totalField, Grid totalBox, int usedSpells, int totalSpells)
        {
            totalField.Text = totalSpells.ToString();
            totalField.Foreground = Utilities.SetTotalTextStyle(totalSpells, usedSpells);
            totalBox.Background = Utilities.SetTotalBoxStyle(totalSpells, usedSpells);
        }

        private static void FillUsedSpellSlot(TextBlock usedField, Grid usedBox, int usedSpells, int totalSpells)
        {
            usedField.Text = usedSpells.ToString();
            usedField.Foreground = Utilities.SetUsedTextStyle(totalSpells, usedSpells);
            usedBox.Background = Utilities.SetUsedBoxStyle(totalSpells, usedSpells);
        }

        private static bool NextItem<T>(ConciergeDataGrid dataGrid, DrawList drawList, List<T> list, int limit, int increment)
        {
            var index = dataGrid.NextItem(list, limit, increment);

            if (index != -1)
            {
                drawList();
                dataGrid.SelectedIndex = index;

                return true;
            }

            return false;
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

            FillUsedSpellSlot(this.UsedPactField, this.UsedPactBox, spellSlots.PactUsed, spellSlots.PactTotal);
            FillUsedSpellSlot(this.UsedFirstField, this.UsedFirstBox, spellSlots.FirstUsed, spellSlots.FirstTotal);
            FillUsedSpellSlot(this.UsedSecondField, this.UsedSecondBox, spellSlots.SecondUsed, spellSlots.SecondTotal);
            FillUsedSpellSlot(this.UsedThirdField, this.UsedThirdBox, spellSlots.ThirdUsed, spellSlots.ThirdTotal);
            FillUsedSpellSlot(this.UsedFourthField, this.UsedFourthBox, spellSlots.FourthUsed, spellSlots.FourthTotal);
            FillUsedSpellSlot(this.UsedFifthField, this.UsedFifthBox, spellSlots.FifthUsed, spellSlots.FifthTotal);
            FillUsedSpellSlot(this.UsedSixthField, this.UsedSixthBox, spellSlots.SixthUsed, spellSlots.SixthTotal);
            FillUsedSpellSlot(this.UsedSeventhField, this.UsedSeventhBox, spellSlots.SeventhUsed, spellSlots.SeventhTotal);
            FillUsedSpellSlot(this.UsedEighthField, this.UsedEighthBox, spellSlots.EighthUsed, spellSlots.EighthTotal);
            FillUsedSpellSlot(this.UsedNinethField, this.UsedNinethBox, spellSlots.NinethUsed, spellSlots.NinethTotal);
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
            if (!NextItem(this.MagicClassDataGrid, this.DrawMagicClasses, Program.CcsFile.Character.MagicClasses, 0, -1))
            {
                NextItem(this.SpellListDataGrid, this.DrawSpellList, Program.CcsFile.Character.Spells, 0, -1);
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            if (!NextItem(this.MagicClassDataGrid, this.DrawMagicClasses, Program.CcsFile.Character.MagicClasses, Program.CcsFile.Character.MagicClasses.Count - 1, 1))
            {
                NextItem(this.SpellListDataGrid, this.DrawSpellList, Program.CcsFile.Character.Spells, Program.CcsFile.Character.Spells.Count - 1, 1);
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
                    this.modifySpellClassWindow.AddClass();
                    this.DrawMagicClasses();
                    break;
                case PopupButtons.AddSpell:
                    this.modifySpellWindow.AddSpell();
                    this.DrawSpellList();
                    break;
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.MagicClassDataGrid.SelectedItem != null)
            {
                var magicClass = (MagicClass)this.MagicClassDataGrid.SelectedItem;
                this.modifySpellClassWindow.EditClass(magicClass);
                this.DrawMagicClasses();
            }
            else if (this.SpellListDataGrid.SelectedItem != null)
            {
                var spell = (Spell)this.SpellListDataGrid.SelectedItem;
                this.modifySpellWindow.EditSpell(spell);
                this.DrawSpellList();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.MagicClassDataGrid.SelectedItem != null)
            {
                Program.Modify();

                MagicClass magicClass = (MagicClass)this.MagicClassDataGrid.SelectedItem;
                Program.CcsFile.Character.MagicClasses.Remove(magicClass);
                this.DrawMagicClasses();
            }
            else if (this.SpellListDataGrid.SelectedItem != null)
            {
                Program.Modify();

                Spell spell = (Spell)this.SpellListDataGrid.SelectedItem;
                Program.CcsFile.Character.Spells.Remove(spell);
                this.DrawSpellList();
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
            this.modifySpellSlotsWindow.EditSpellSlots();
            this.DrawTotalSpellSlots();
            this.DrawUsedSpellSlots();
        }

        private void UsedSlot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var spellSlots = Program.CcsFile.Character.SpellSlots;
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

            this.DrawTotalSpellSlots();
            this.DrawUsedSpellSlots();
        }

        private void UsedSlot_MouseEnter(object sender, MouseEventArgs e)
        {
            var spellSlots = Program.CcsFile.Character.SpellSlots;
            switch ((sender as Grid).Name)
            {
                case "UsedPactBox":
                    Utilities.SetCursor(spellSlots.PactUsed, spellSlots.PactTotal, (x, y) => x != y, Cursors.Hand);
                    break;
                case "UsedFirstBox":
                    Utilities.SetCursor(spellSlots.FirstUsed, spellSlots.FirstTotal, (x, y) => x != y, Cursors.Hand);
                    break;
                case "UsedSecondBox":
                    Utilities.SetCursor(spellSlots.SecondUsed, spellSlots.SecondTotal, (x, y) => x != y, Cursors.Hand);
                    break;
                case "UsedThirdBox":
                    Utilities.SetCursor(spellSlots.ThirdUsed, spellSlots.ThirdTotal, (x, y) => x != y, Cursors.Hand);
                    break;
                case "UsedFourthBox":
                    Utilities.SetCursor(spellSlots.FourthUsed, spellSlots.FourthTotal, (x, y) => x != y, Cursors.Hand);
                    break;
                case "UsedFifthBox":
                    Utilities.SetCursor(spellSlots.FifthUsed, spellSlots.FifthTotal, (x, y) => x != y, Cursors.Hand);
                    break;
                case "UsedSixthBox":
                    Utilities.SetCursor(spellSlots.SixthUsed, spellSlots.SixthTotal, (x, y) => x != y, Cursors.Hand);
                    break;
                case "UsedSeventhBox":
                    Utilities.SetCursor(spellSlots.SeventhUsed, spellSlots.SeventhTotal, (x, y) => x != y, Cursors.Hand);
                    break;
                case "UsedEighthBox":
                    Utilities.SetCursor(spellSlots.EighthUsed, spellSlots.EighthTotal, (x, y) => x != y, Cursors.Hand);
                    break;
                case "UsedNinethBox":
                    Utilities.SetCursor(spellSlots.NinethUsed, spellSlots.NinethTotal, (x, y) => x != y, Cursors.Hand);
                    break;
            }
        }

        private void UsedSlot_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void SpellListDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            Program.CcsFile.Character.Spells.Clear();

            foreach (var spell in this.SpellListDataGrid.Items)
            {
                Program.CcsFile.Character.Spells.Add(spell as Spell);
            }
        }

        private void MagicClassDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            Program.CcsFile.Character.MagicClasses.Clear();

            foreach (var magicClass in this.SpellListDataGrid.Items)
            {
                Program.CcsFile.Character.MagicClasses.Add(magicClass as MagicClass);
            }
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
                    break;
                case "ModifySpellSlotsWindow":
                    this.DrawUsedSpellSlots();
                    this.DrawTotalSpellSlots();
                    break;
            }
        }
    }
}
