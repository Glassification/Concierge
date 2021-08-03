// <copyright file="SpellcastingPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.SpellcastingPageUi
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Characters.Enums;
    using Concierge.Characters.Spellcasting;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for SpellcastingPage.xaml.
    /// </summary>
    public partial class SpellcastingPage : Page
    {
        private readonly SpellcastingSelectionWindow spellcastingSelectionWindow = new SpellcastingSelectionWindow();
        private readonly ModifySpellWindow modifySpellWindow = new ModifySpellWindow();
        private readonly ModifySpellClassWindow modifySpellClassWindow = new ModifySpellClassWindow();
        private readonly ModifySpellSlotsWindow modifySpellSlotsWindow = new ModifySpellSlotsWindow();

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

        public void Draw()
        {
            this.DrawSpellList();
            this.DrawMagicClasses();
            this.DrawTotalSpellSlots();
            this.DrawUsedSpellSlots();
        }

        private static int IncrementUsedSpellSlots(int used, int total)
        {
            return used < total ? used + 1 : used;
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
            if (this.MagicClassDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var magicClass = (MagicClass)this.MagicClassDataGrid.SelectedItem;
                var index = Program.CcsFile.Character.MagicClasses.IndexOf(magicClass);

                if (index != 0)
                {
                    Utilities.Swap(Program.CcsFile.Character.MagicClasses, index, index - 1);
                    this.DrawMagicClasses();
                    this.MagicClassDataGrid.SelectedIndex = index - 1;
                }
            }
            else if (this.SpellListDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var spell = (Spell)this.SpellListDataGrid.SelectedItem;
                var index = Program.CcsFile.Character.Spells.IndexOf(spell);

                if (index != 0)
                {
                    Utilities.Swap(Program.CcsFile.Character.Spells, index, index - 1);
                    this.DrawSpellList();
                    this.SpellListDataGrid.SelectedIndex = index - 1;
                }
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            if (this.MagicClassDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var magicClass = (MagicClass)this.MagicClassDataGrid.SelectedItem;
                var index = Program.CcsFile.Character.MagicClasses.IndexOf(magicClass);

                if (index != Program.CcsFile.Character.MagicClasses.Count - 1)
                {
                    Utilities.Swap(Program.CcsFile.Character.MagicClasses, index, index + 1);
                    this.DrawMagicClasses();
                    this.MagicClassDataGrid.SelectedIndex = index + 1;
                }
            }
            else if (this.SpellListDataGrid.SelectedItem != null)
            {
                Program.Modify();

                var spell = (Spell)this.SpellListDataGrid.SelectedItem;
                var index = Program.CcsFile.Character.Spells.IndexOf(spell);

                if (index != Program.CcsFile.Character.Spells.Count - 1)
                {
                    Utilities.Swap(Program.CcsFile.Character.Spells, index, index + 1);
                    this.DrawSpellList();
                    this.SpellListDataGrid.SelectedIndex = index + 1;
                }
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
            Program.Modify();

            var usedBox = sender as Grid;

            switch (usedBox.Name)
            {
                case "UsedPactBox":
                    Program.CcsFile.Character.SpellSlots.PactUsed = IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.PactUsed, Program.CcsFile.Character.SpellSlots.PactTotal);
                    break;
                case "UsedFirstBox":
                    Program.CcsFile.Character.SpellSlots.FirstUsed = IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.FirstUsed, Program.CcsFile.Character.SpellSlots.FirstTotal);
                    break;
                case "UsedSecondBox":
                    Program.CcsFile.Character.SpellSlots.SecondUsed = IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.SecondUsed, Program.CcsFile.Character.SpellSlots.SecondTotal);
                    break;
                case "UsedThirdBox":
                    Program.CcsFile.Character.SpellSlots.ThirdUsed = IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.ThirdUsed, Program.CcsFile.Character.SpellSlots.ThirdTotal);
                    break;
                case "UsedFourthBox":
                    Program.CcsFile.Character.SpellSlots.FourthUsed = IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.FourthUsed, Program.CcsFile.Character.SpellSlots.FourthTotal);
                    break;
                case "UsedFifthBox":
                    Program.CcsFile.Character.SpellSlots.FifthUsed = IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.FifthUsed, Program.CcsFile.Character.SpellSlots.FifthTotal);
                    break;
                case "UsedSixthBox":
                    Program.CcsFile.Character.SpellSlots.SixthUsed = IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.SixthUsed, Program.CcsFile.Character.SpellSlots.SixthTotal);
                    break;
                case "UsedSeventhBox":
                    Program.CcsFile.Character.SpellSlots.SeventhUsed = IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.SeventhUsed, Program.CcsFile.Character.SpellSlots.SeventhTotal);
                    break;
                case "UsedEighthBox":
                    Program.CcsFile.Character.SpellSlots.EighthUsed = IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.EighthUsed, Program.CcsFile.Character.SpellSlots.EighthTotal);
                    break;
                case "UsedNinethBox":
                    Program.CcsFile.Character.SpellSlots.NinethUsed = IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.NinethUsed, Program.CcsFile.Character.SpellSlots.NinethTotal);
                    break;
            }

            this.DrawTotalSpellSlots();
            this.DrawUsedSpellSlots();
        }

        private void UsedSlot_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
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
