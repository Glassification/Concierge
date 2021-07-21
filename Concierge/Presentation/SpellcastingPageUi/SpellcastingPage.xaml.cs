// <copyright file="SpellcastingPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.SpellcastingPageUi
{
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
        public SpellcastingPage()
        {
            this.InitializeComponent();
            this.SpellcastingSelectionWindow = new SpellcastingSelectionWindow();
            this.ModifySpellWindow = new ModifySpellWindow();
            this.ModifySpellClassWindow = new ModifySpellClassWindow();
            this.ModifySpellSlotsWindow = new ModifySpellSlotsWindow();

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

        private SpellcastingSelectionWindow SpellcastingSelectionWindow { get; }

        private ModifySpellWindow ModifySpellWindow { get; }

        private ModifySpellClassWindow ModifySpellClassWindow { get; }

        private ModifySpellSlotsWindow ModifySpellSlotsWindow { get; }

        public void Draw()
        {
            this.FillSpellList();
            this.FillMagicClassList();
            this.FillTotalSpellSlots();
            this.FillUsedSpellSlots();
        }

        private void FillTotalSpellSlots()
        {
            var spellSlots = Program.CcsFile.Character.SpellSlots;

            this.FillTotalSpellSlot(this.TotalPactField, this.TotalPactBox, spellSlots.PactUsed, spellSlots.PactTotal);
            this.FillTotalSpellSlot(this.TotalFirstField, this.TotalFirstBox, spellSlots.FirstUsed, spellSlots.FirstTotal);
            this.FillTotalSpellSlot(this.TotalSecondField, this.TotalSecondBox, spellSlots.SecondUsed, spellSlots.SecondTotal);
            this.FillTotalSpellSlot(this.TotalThirdField, this.TotalThirdBox, spellSlots.ThirdUsed, spellSlots.ThirdTotal);
            this.FillTotalSpellSlot(this.TotalFourthField, this.TotalFourthBox, spellSlots.FourthUsed, spellSlots.FourthTotal);
            this.FillTotalSpellSlot(this.TotalFifthField, this.TotalFifthBox, spellSlots.FifthUsed, spellSlots.FifthTotal);
            this.FillTotalSpellSlot(this.TotalSixthField, this.TotalSixthBox, spellSlots.SixthUsed, spellSlots.SixthTotal);
            this.FillTotalSpellSlot(this.TotalSeventhField, this.TotalSeventhBox, spellSlots.SeventhUsed, spellSlots.SeventhTotal);
            this.FillTotalSpellSlot(this.TotalEighthField, this.TotalEighthBox, spellSlots.EighthUsed, spellSlots.EighthTotal);
            this.FillTotalSpellSlot(this.TotalNinethField, this.TotalNinethBox, spellSlots.NinethUsed, spellSlots.NinethTotal);
        }

        private void FillUsedSpellSlots()
        {
            var spellSlots = Program.CcsFile.Character.SpellSlots;

            this.FillUsedSpellSlot(this.UsedPactField, this.UsedPactBox, spellSlots.PactUsed, spellSlots.PactTotal);
            this.FillUsedSpellSlot(this.UsedFirstField, this.UsedFirstBox, spellSlots.FirstUsed, spellSlots.FirstTotal);
            this.FillUsedSpellSlot(this.UsedSecondField, this.UsedSecondBox, spellSlots.SecondUsed, spellSlots.SecondTotal);
            this.FillUsedSpellSlot(this.UsedThirdField, this.UsedThirdBox, spellSlots.ThirdUsed, spellSlots.ThirdTotal);
            this.FillUsedSpellSlot(this.UsedFourthField, this.UsedFourthBox, spellSlots.FourthUsed, spellSlots.FourthTotal);
            this.FillUsedSpellSlot(this.UsedFifthField, this.UsedFifthBox, spellSlots.FifthUsed, spellSlots.FifthTotal);
            this.FillUsedSpellSlot(this.UsedSixthField, this.UsedSixthBox, spellSlots.SixthUsed, spellSlots.SixthTotal);
            this.FillUsedSpellSlot(this.UsedSeventhField, this.UsedSeventhBox, spellSlots.SeventhUsed, spellSlots.SeventhTotal);
            this.FillUsedSpellSlot(this.UsedEighthField, this.UsedEighthBox, spellSlots.EighthUsed, spellSlots.EighthTotal);
            this.FillUsedSpellSlot(this.UsedNinethField, this.UsedNinethBox, spellSlots.NinethUsed, spellSlots.NinethTotal);
        }

        private void FillTotalSpellSlot(TextBlock usedField, Grid usedBox, int usedSpells, int totalSpells)
        {
            usedField.Text = usedSpells.ToString();
            usedField.Foreground = Utilities.SetTotalTextStyle(totalSpells, usedSpells);
            usedBox.Background = Utilities.SetTotalBoxStyle(totalSpells, usedSpells);
        }

        private void FillUsedSpellSlot(TextBlock usedField, Grid usedBox, int usedSpells, int totalSpells)
        {
            usedField.Text = usedSpells.ToString();
            usedField.Foreground = Utilities.SetUsedTextStyle(totalSpells, usedSpells);
            usedBox.Background = Utilities.SetUsedBoxStyle(totalSpells, usedSpells);
        }

        private void FillSpellList()
        {
            this.SpellListDataGrid.Items.Clear();

            foreach (var spell in Program.CcsFile.Character.Spells)
            {
                this.SpellListDataGrid.Items.Add(spell);
            }
        }

        private void FillMagicClassList()
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

        private int IncrementUsedSpellSlots(int used, int total)
        {
            return used < total ? used + 1 : used;
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            if (this.MagicClassDataGrid.SelectedItem != null)
            {
                var magicClass = (MagicClass)this.MagicClassDataGrid.SelectedItem;
                var index = Program.CcsFile.Character.MagicClasses.IndexOf(magicClass);

                if (index != 0)
                {
                    Utilities.Swap(Program.CcsFile.Character.MagicClasses, index, index - 1);
                    this.FillMagicClassList();
                    this.MagicClassDataGrid.SelectedIndex = index - 1;
                }
            }
            else if (this.SpellListDataGrid.SelectedItem != null)
            {
                var spell = (Spell)this.SpellListDataGrid.SelectedItem;
                var index = Program.CcsFile.Character.Spells.IndexOf(spell);

                if (index != 0)
                {
                    Utilities.Swap(Program.CcsFile.Character.Spells, index, index - 1);
                    this.FillSpellList();
                    this.SpellListDataGrid.SelectedIndex = index - 1;
                }
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            if (this.MagicClassDataGrid.SelectedItem != null)
            {
                var magicClass = (MagicClass)this.MagicClassDataGrid.SelectedItem;
                var index = Program.CcsFile.Character.MagicClasses.IndexOf(magicClass);

                if (index != Program.CcsFile.Character.MagicClasses.Count - 1)
                {
                    Utilities.Swap(Program.CcsFile.Character.MagicClasses, index, index + 1);
                    this.FillMagicClassList();
                    this.MagicClassDataGrid.SelectedIndex = index + 1;
                }
            }
            else if (this.SpellListDataGrid.SelectedItem != null)
            {
                var spell = (Spell)this.SpellListDataGrid.SelectedItem;
                var index = Program.CcsFile.Character.Spells.IndexOf(spell);

                if (index != Program.CcsFile.Character.Spells.Count - 1)
                {
                    Utilities.Swap(Program.CcsFile.Character.Spells, index, index + 1);
                    this.FillSpellList();
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
            var popupButton = this.SpellcastingSelectionWindow.ShowPopup();

            switch (popupButton)
            {
                case PopupButtons.AddMagicClass:
                    this.ModifySpellClassWindow.AddClass();
                    this.FillMagicClassList();
                    break;
                case PopupButtons.AddSpell:
                    this.ModifySpellWindow.AddSpell();
                    this.FillSpellList();
                    break;
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.MagicClassDataGrid.SelectedItem != null)
            {
                var magicClass = (MagicClass)this.MagicClassDataGrid.SelectedItem;
                this.ModifySpellClassWindow.EditClass(magicClass);
                this.FillMagicClassList();
            }
            else if (this.SpellListDataGrid.SelectedItem != null)
            {
                var spell = (Spell)this.SpellListDataGrid.SelectedItem;
                this.ModifySpellWindow.EditSpell(spell);
                this.FillSpellList();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.MagicClassDataGrid.SelectedItem != null)
            {
                MagicClass magicClass = (MagicClass)this.MagicClassDataGrid.SelectedItem;
                Program.CcsFile.Character.MagicClasses.Remove(magicClass);
                this.FillMagicClassList();
            }
            else if (this.SpellListDataGrid.SelectedItem != null)
            {
                Spell spell = (Spell)this.SpellListDataGrid.SelectedItem;
                Program.CcsFile.Character.Spells.Remove(spell);
                this.FillSpellList();
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
            this.ModifySpellSlotsWindow.EditSpellSlots();
            this.FillTotalSpellSlots();
            this.FillUsedSpellSlots();
        }

        private void UsedSlot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var usedBox = sender as Grid;

            switch (usedBox.Name)
            {
                case "UsedPactBox":
                    Program.CcsFile.Character.SpellSlots.PactUsed = this.IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.PactUsed, Program.CcsFile.Character.SpellSlots.PactTotal);
                    break;
                case "UsedFirstBox":
                    Program.CcsFile.Character.SpellSlots.FirstUsed = this.IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.FirstUsed, Program.CcsFile.Character.SpellSlots.FirstTotal);
                    break;
                case "UsedSecondBox":
                    Program.CcsFile.Character.SpellSlots.SecondUsed = this.IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.SecondUsed, Program.CcsFile.Character.SpellSlots.SecondTotal);
                    break;
                case "UsedThirdBox":
                    Program.CcsFile.Character.SpellSlots.ThirdUsed = this.IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.ThirdUsed, Program.CcsFile.Character.SpellSlots.ThirdTotal);
                    break;
                case "UsedFourthBox":
                    Program.CcsFile.Character.SpellSlots.FourthUsed = this.IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.FourthUsed, Program.CcsFile.Character.SpellSlots.FourthTotal);
                    break;
                case "UsedFifthBox":
                    Program.CcsFile.Character.SpellSlots.FifthUsed = this.IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.FifthUsed, Program.CcsFile.Character.SpellSlots.FifthTotal);
                    break;
                case "UsedSixthBox":
                    Program.CcsFile.Character.SpellSlots.SixthUsed = this.IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.SixthUsed, Program.CcsFile.Character.SpellSlots.SixthTotal);
                    break;
                case "UsedSeventhBox":
                    Program.CcsFile.Character.SpellSlots.SeventhUsed = this.IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.SeventhUsed, Program.CcsFile.Character.SpellSlots.SeventhTotal);
                    break;
                case "UsedEighthBox":
                    Program.CcsFile.Character.SpellSlots.EighthUsed = this.IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.EighthUsed, Program.CcsFile.Character.SpellSlots.EighthTotal);
                    break;
                case "UsedNinethBox":
                    Program.CcsFile.Character.SpellSlots.NinethUsed = this.IncrementUsedSpellSlots(Program.CcsFile.Character.SpellSlots.NinethUsed, Program.CcsFile.Character.SpellSlots.NinethTotal);
                    break;
            }

            this.FillTotalSpellSlots();
            this.FillUsedSpellSlots();
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
            Program.CcsFile.Character.Spells.Clear();

            foreach (var spell in this.SpellListDataGrid.Items)
            {
                Program.CcsFile.Character.Spells.Add(spell as Spell);
            }
        }

        private void MagicClassDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            Program.CcsFile.Character.MagicClasses.Clear();

            foreach (var magicClass in this.SpellListDataGrid.Items)
            {
                Program.CcsFile.Character.MagicClasses.Add(magicClass as MagicClass);
            }
        }
    }
}
