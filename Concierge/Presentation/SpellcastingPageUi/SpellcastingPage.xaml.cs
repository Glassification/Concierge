// <copyright file="SpellcastingPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.SpellcastingPageUi
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Characters.Collections;
    using Concierge.Characters.Enums;
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

            this.InitializeClickEvents();
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
            this.TotalPactField.Text = Program.CcsFile.Character.SpellSlots.PactTotal.ToString();
            this.TotalPactField.Foreground = Utilities.SetTotalTextStyle(Program.CcsFile.Character.SpellSlots.PactTotal, Program.CcsFile.Character.SpellSlots.PactUsed);
            this.TotalPactBox.Background = Utilities.SetTotalBoxStyle(Program.CcsFile.Character.SpellSlots.PactTotal, Program.CcsFile.Character.SpellSlots.PactUsed);

            this.TotalFirstField.Text = Program.CcsFile.Character.SpellSlots.FirstTotal.ToString();
            this.TotalFirstField.Foreground = Utilities.SetTotalTextStyle(Program.CcsFile.Character.SpellSlots.FirstTotal, Program.CcsFile.Character.SpellSlots.FirstUsed);
            this.TotalFirstBox.Background = Utilities.SetTotalBoxStyle(Program.CcsFile.Character.SpellSlots.FirstTotal, Program.CcsFile.Character.SpellSlots.FirstUsed);

            this.TotalSecondField.Text = Program.CcsFile.Character.SpellSlots.SecondTotal.ToString();
            this.TotalSecondField.Foreground = Utilities.SetTotalTextStyle(Program.CcsFile.Character.SpellSlots.SecondTotal, Program.CcsFile.Character.SpellSlots.SecondUsed);
            this.TotalSecondBox.Background = Utilities.SetTotalBoxStyle(Program.CcsFile.Character.SpellSlots.SecondTotal, Program.CcsFile.Character.SpellSlots.SecondUsed);

            this.TotalThirdField.Text = Program.CcsFile.Character.SpellSlots.ThirdTotal.ToString();
            this.TotalThirdField.Foreground = Utilities.SetTotalTextStyle(Program.CcsFile.Character.SpellSlots.ThirdTotal, Program.CcsFile.Character.SpellSlots.ThirdUsed);
            this.TotalThirdBox.Background = Utilities.SetTotalBoxStyle(Program.CcsFile.Character.SpellSlots.ThirdTotal, Program.CcsFile.Character.SpellSlots.ThirdUsed);

            this.TotalFourthField.Text = Program.CcsFile.Character.SpellSlots.FourthTotal.ToString();
            this.TotalFourthField.Foreground = Utilities.SetTotalTextStyle(Program.CcsFile.Character.SpellSlots.FourthTotal, Program.CcsFile.Character.SpellSlots.FourthUsed);
            this.TotalFourthBox.Background = Utilities.SetTotalBoxStyle(Program.CcsFile.Character.SpellSlots.FourthTotal, Program.CcsFile.Character.SpellSlots.FourthUsed);

            this.TotalFifthField.Text = Program.CcsFile.Character.SpellSlots.FifthTotal.ToString();
            this.TotalFifthField.Foreground = Utilities.SetTotalTextStyle(Program.CcsFile.Character.SpellSlots.FifthTotal, Program.CcsFile.Character.SpellSlots.FifthUsed);
            this.TotalFifthBox.Background = Utilities.SetTotalBoxStyle(Program.CcsFile.Character.SpellSlots.FifthTotal, Program.CcsFile.Character.SpellSlots.FifthUsed);

            this.TotalSixthField.Text = Program.CcsFile.Character.SpellSlots.SixthTotal.ToString();
            this.TotalSixthField.Foreground = Utilities.SetTotalTextStyle(Program.CcsFile.Character.SpellSlots.SixthTotal, Program.CcsFile.Character.SpellSlots.SixthUsed);
            this.TotalSixthBox.Background = Utilities.SetTotalBoxStyle(Program.CcsFile.Character.SpellSlots.SixthTotal, Program.CcsFile.Character.SpellSlots.SixthUsed);

            this.TotalSeventhField.Text = Program.CcsFile.Character.SpellSlots.SeventhTotal.ToString();
            this.TotalSeventhField.Foreground = Utilities.SetTotalTextStyle(Program.CcsFile.Character.SpellSlots.SeventhTotal, Program.CcsFile.Character.SpellSlots.SeventhUsed);
            this.TotalSeventhBox.Background = Utilities.SetTotalBoxStyle(Program.CcsFile.Character.SpellSlots.SeventhTotal, Program.CcsFile.Character.SpellSlots.SeventhUsed);

            this.TotalEighthField.Text = Program.CcsFile.Character.SpellSlots.EighthTotal.ToString();
            this.TotalEighthField.Foreground = Utilities.SetTotalTextStyle(Program.CcsFile.Character.SpellSlots.EighthTotal, Program.CcsFile.Character.SpellSlots.EighthUsed);
            this.TotalEighthBox.Background = Utilities.SetTotalBoxStyle(Program.CcsFile.Character.SpellSlots.EighthTotal, Program.CcsFile.Character.SpellSlots.EighthUsed);

            this.TotalNinethField.Text = Program.CcsFile.Character.SpellSlots.NinethTotal.ToString();
            this.TotalNinethField.Foreground = Utilities.SetTotalTextStyle(Program.CcsFile.Character.SpellSlots.NinethTotal, Program.CcsFile.Character.SpellSlots.NinethUsed);
            this.TotalNinethBox.Background = Utilities.SetTotalBoxStyle(Program.CcsFile.Character.SpellSlots.NinethTotal, Program.CcsFile.Character.SpellSlots.NinethUsed);
        }

        private void FillUsedSpellSlots()
        {
            this.UsedPactField.Text = Program.CcsFile.Character.SpellSlots.PactUsed.ToString();
            this.UsedPactField.Foreground = Utilities.SetUsedTextStyle(Program.CcsFile.Character.SpellSlots.PactTotal, Program.CcsFile.Character.SpellSlots.PactUsed);
            this.UsedPactBox.Background = Utilities.SetUsedBoxStyle(Program.CcsFile.Character.SpellSlots.PactTotal, Program.CcsFile.Character.SpellSlots.PactUsed);

            this.UsedFirstField.Text = Program.CcsFile.Character.SpellSlots.FirstUsed.ToString();
            this.UsedFirstField.Foreground = Utilities.SetUsedTextStyle(Program.CcsFile.Character.SpellSlots.FirstTotal, Program.CcsFile.Character.SpellSlots.FirstUsed);
            this.UsedFirstBox.Background = Utilities.SetUsedBoxStyle(Program.CcsFile.Character.SpellSlots.FirstTotal, Program.CcsFile.Character.SpellSlots.FirstUsed);

            this.UsedSecondField.Text = Program.CcsFile.Character.SpellSlots.SecondUsed.ToString();
            this.UsedSecondField.Foreground = Utilities.SetUsedTextStyle(Program.CcsFile.Character.SpellSlots.SecondTotal, Program.CcsFile.Character.SpellSlots.SecondUsed);
            this.UsedSecondBox.Background = Utilities.SetUsedBoxStyle(Program.CcsFile.Character.SpellSlots.SecondTotal, Program.CcsFile.Character.SpellSlots.SecondUsed);

            this.UsedThirdField.Text = Program.CcsFile.Character.SpellSlots.ThirdUsed.ToString();
            this.UsedThirdField.Foreground = Utilities.SetUsedTextStyle(Program.CcsFile.Character.SpellSlots.ThirdTotal, Program.CcsFile.Character.SpellSlots.ThirdUsed);
            this.UsedThirdBox.Background = Utilities.SetUsedBoxStyle(Program.CcsFile.Character.SpellSlots.ThirdTotal, Program.CcsFile.Character.SpellSlots.ThirdUsed);

            this.UsedFourthField.Text = Program.CcsFile.Character.SpellSlots.FourthUsed.ToString();
            this.UsedFourthField.Foreground = Utilities.SetUsedTextStyle(Program.CcsFile.Character.SpellSlots.FourthTotal, Program.CcsFile.Character.SpellSlots.FourthUsed);
            this.UsedFourthBox.Background = Utilities.SetUsedBoxStyle(Program.CcsFile.Character.SpellSlots.FourthTotal, Program.CcsFile.Character.SpellSlots.FourthUsed);

            this.UsedFifthField.Text = Program.CcsFile.Character.SpellSlots.FifthUsed.ToString();
            this.UsedFifthField.Foreground = Utilities.SetUsedTextStyle(Program.CcsFile.Character.SpellSlots.FifthTotal, Program.CcsFile.Character.SpellSlots.FifthUsed);
            this.UsedFifthBox.Background = Utilities.SetUsedBoxStyle(Program.CcsFile.Character.SpellSlots.FifthTotal, Program.CcsFile.Character.SpellSlots.FifthUsed);

            this.UsedSixthField.Text = Program.CcsFile.Character.SpellSlots.SixthUsed.ToString();
            this.UsedSixthField.Foreground = Utilities.SetUsedTextStyle(Program.CcsFile.Character.SpellSlots.SixthTotal, Program.CcsFile.Character.SpellSlots.SixthUsed);
            this.UsedSixthBox.Background = Utilities.SetUsedBoxStyle(Program.CcsFile.Character.SpellSlots.SixthTotal, Program.CcsFile.Character.SpellSlots.SixthUsed);

            this.UsedSeventhField.Text = Program.CcsFile.Character.SpellSlots.SeventhUsed.ToString();
            this.UsedSeventhField.Foreground = Utilities.SetUsedTextStyle(Program.CcsFile.Character.SpellSlots.SeventhTotal, Program.CcsFile.Character.SpellSlots.SeventhUsed);
            this.UsedSeventhBox.Background = Utilities.SetUsedBoxStyle(Program.CcsFile.Character.SpellSlots.SeventhTotal, Program.CcsFile.Character.SpellSlots.SeventhUsed);

            this.UsedEighthField.Text = Program.CcsFile.Character.SpellSlots.EighthUsed.ToString();
            this.UsedEighthField.Foreground = Utilities.SetUsedTextStyle(Program.CcsFile.Character.SpellSlots.EighthTotal, Program.CcsFile.Character.SpellSlots.EighthUsed);
            this.UsedEighthBox.Background = Utilities.SetUsedBoxStyle(Program.CcsFile.Character.SpellSlots.EighthTotal, Program.CcsFile.Character.SpellSlots.EighthUsed);

            this.UsedNinethField.Text = Program.CcsFile.Character.SpellSlots.NinethUsed.ToString();
            this.UsedNinethField.Foreground = Utilities.SetUsedTextStyle(Program.CcsFile.Character.SpellSlots.NinethTotal, Program.CcsFile.Character.SpellSlots.NinethUsed);
            this.UsedNinethBox.Background = Utilities.SetUsedBoxStyle(Program.CcsFile.Character.SpellSlots.NinethTotal, Program.CcsFile.Character.SpellSlots.NinethUsed);
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

        private void InitializeClickEvents()
        {
            this.UsedPactBox.MouseDown += this.UsedSlot_MouseDown;
            this.UsedPactBox.MouseEnter += this.UsedSlot_MouseEnter;
            this.UsedPactBox.MouseLeave += this.UsedSlot_MouseLeave;

            this.UsedFirstBox.MouseDown += this.UsedSlot_MouseDown;
            this.UsedFirstBox.MouseEnter += this.UsedSlot_MouseEnter;
            this.UsedFirstBox.MouseLeave += this.UsedSlot_MouseLeave;

            this.UsedSecondBox.MouseDown += this.UsedSlot_MouseDown;
            this.UsedSecondBox.MouseEnter += this.UsedSlot_MouseEnter;
            this.UsedSecondBox.MouseLeave += this.UsedSlot_MouseLeave;

            this.UsedThirdBox.MouseDown += this.UsedSlot_MouseDown;
            this.UsedThirdBox.MouseEnter += this.UsedSlot_MouseEnter;
            this.UsedThirdBox.MouseLeave += this.UsedSlot_MouseLeave;

            this.UsedFourthBox.MouseDown += this.UsedSlot_MouseDown;
            this.UsedFourthBox.MouseEnter += this.UsedSlot_MouseEnter;
            this.UsedFourthBox.MouseLeave += this.UsedSlot_MouseLeave;

            this.UsedFifthBox.MouseDown += this.UsedSlot_MouseDown;
            this.UsedFifthBox.MouseEnter += this.UsedSlot_MouseEnter;
            this.UsedFifthBox.MouseLeave += this.UsedSlot_MouseLeave;

            this.UsedSixthBox.MouseDown += this.UsedSlot_MouseDown;
            this.UsedSixthBox.MouseEnter += this.UsedSlot_MouseEnter;
            this.UsedSixthBox.MouseLeave += this.UsedSlot_MouseLeave;

            this.UsedSeventhBox.MouseDown += this.UsedSlot_MouseDown;
            this.UsedSeventhBox.MouseEnter += this.UsedSlot_MouseEnter;
            this.UsedSeventhBox.MouseLeave += this.UsedSlot_MouseLeave;

            this.UsedEighthBox.MouseDown += this.UsedSlot_MouseDown;
            this.UsedEighthBox.MouseEnter += this.UsedSlot_MouseEnter;
            this.UsedEighthBox.MouseLeave += this.UsedSlot_MouseLeave;

            this.UsedNinethBox.MouseDown += this.UsedSlot_MouseDown;
            this.UsedNinethBox.MouseEnter += this.UsedSlot_MouseEnter;
            this.UsedNinethBox.MouseLeave += this.UsedSlot_MouseLeave;
        }

        private int IncrementUsedSpellSlots(int used, int total)
        {
            return used < total ? used + 1 : used;
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            int index;

            if (this.MagicClassDataGrid.SelectedItem != null)
            {
                var magicClass = (MagicClass)this.MagicClassDataGrid.SelectedItem;
                index = Program.CcsFile.Character.MagicClasses.IndexOf(magicClass);

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
                index = Program.CcsFile.Character.Spells.IndexOf(spell);

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
            int index;

            if (this.MagicClassDataGrid.SelectedItem != null)
            {
                var magicClass = (MagicClass)this.MagicClassDataGrid.SelectedItem;
                index = Program.CcsFile.Character.MagicClasses.IndexOf(magicClass);

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
                index = Program.CcsFile.Character.Spells.IndexOf(spell);

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
            Grid usedBox = sender as Grid;

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
