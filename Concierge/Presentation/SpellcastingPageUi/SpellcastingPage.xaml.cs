using Concierge.Characters.Collections;
using Concierge.Utility;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Concierge.Presentation.SpellcastingPageUi
{
    /// <summary>
    /// Interaction logic for SpellcastingPage.xaml
    /// </summary>
    public partial class SpellcastingPage : Page
    {

        #region Constructor

        public SpellcastingPage()
        {
            InitializeComponent();
            SpellcastingSelectionWindow = new SpellcastingSelectionWindow();
            ModifySpellWindow = new ModifySpellWindow();
            ModifySpellClassWindow = new ModifySpellClassWindow();
            ModifySpellSlotsWindow = new ModifySpellSlotsWindow();

            InitializeClickEvents();
        }

        #endregion

        #region Methods

        public void Draw()
        {
            FillSpellList();
            FillMagicClassList();
            FillTotalSpellSlots();
            FillUsedSpellSlots();
        }

        private void FillTotalSpellSlots()
        {
            TotalPactField.Text = Program.Character.SpellSlots.PactTotal.ToString();
            TotalPactField.Foreground = Constants.SetTotalTextStyle(Program.Character.SpellSlots.PactTotal, Program.Character.SpellSlots.PactUsed);
            TotalPactBox.Background = Constants.SetTotalBoxStyle(Program.Character.SpellSlots.PactTotal, Program.Character.SpellSlots.PactUsed);

            TotalFirstField.Text = Program.Character.SpellSlots.FirstTotal.ToString();
            TotalFirstField.Foreground = Constants.SetTotalTextStyle(Program.Character.SpellSlots.FirstTotal, Program.Character.SpellSlots.FirstUsed);
            TotalFirstBox.Background = Constants.SetTotalBoxStyle(Program.Character.SpellSlots.FirstTotal, Program.Character.SpellSlots.FirstUsed);

            TotalSecondField.Text = Program.Character.SpellSlots.SecondTotal.ToString();
            TotalSecondField.Foreground = Constants.SetTotalTextStyle(Program.Character.SpellSlots.SecondTotal, Program.Character.SpellSlots.SecondUsed);
            TotalSecondBox.Background = Constants.SetTotalBoxStyle(Program.Character.SpellSlots.SecondTotal, Program.Character.SpellSlots.SecondUsed);

            TotalThirdField.Text = Program.Character.SpellSlots.ThirdTotal.ToString();
            TotalThirdField.Foreground = Constants.SetTotalTextStyle(Program.Character.SpellSlots.ThirdTotal, Program.Character.SpellSlots.ThirdUsed);
            TotalThirdBox.Background = Constants.SetTotalBoxStyle(Program.Character.SpellSlots.ThirdTotal, Program.Character.SpellSlots.ThirdUsed);

            TotalFourthField.Text = Program.Character.SpellSlots.FourthTotal.ToString();
            TotalFourthField.Foreground = Constants.SetTotalTextStyle(Program.Character.SpellSlots.FourthTotal, Program.Character.SpellSlots.FourthUsed);
            TotalFourthBox.Background = Constants.SetTotalBoxStyle(Program.Character.SpellSlots.FourthTotal, Program.Character.SpellSlots.FourthUsed);

            TotalFifthField.Text = Program.Character.SpellSlots.FifthTotal.ToString();
            TotalFifthField.Foreground = Constants.SetTotalTextStyle(Program.Character.SpellSlots.FifthTotal, Program.Character.SpellSlots.FifthUsed);
            TotalFifthBox.Background = Constants.SetTotalBoxStyle(Program.Character.SpellSlots.FifthTotal, Program.Character.SpellSlots.FifthUsed);

            TotalSixthField.Text = Program.Character.SpellSlots.SixthTotal.ToString();
            TotalSixthField.Foreground = Constants.SetTotalTextStyle(Program.Character.SpellSlots.SixthTotal, Program.Character.SpellSlots.SixthUsed);
            TotalSixthBox.Background = Constants.SetTotalBoxStyle(Program.Character.SpellSlots.SixthTotal, Program.Character.SpellSlots.SixthUsed);

            TotalSeventhField.Text = Program.Character.SpellSlots.SeventhTotal.ToString();
            TotalSeventhField.Foreground = Constants.SetTotalTextStyle(Program.Character.SpellSlots.SeventhTotal, Program.Character.SpellSlots.SeventhUsed);
            TotalSeventhBox.Background = Constants.SetTotalBoxStyle(Program.Character.SpellSlots.SeventhTotal, Program.Character.SpellSlots.SeventhUsed);

            TotalEighthField.Text = Program.Character.SpellSlots.EighthTotal.ToString();
            TotalEighthField.Foreground = Constants.SetTotalTextStyle(Program.Character.SpellSlots.EighthTotal, Program.Character.SpellSlots.EighthUsed);
            TotalEighthBox.Background = Constants.SetTotalBoxStyle(Program.Character.SpellSlots.EighthTotal, Program.Character.SpellSlots.EighthUsed);

            TotalNinethField.Text = Program.Character.SpellSlots.NinethTotal.ToString();
            TotalNinethField.Foreground = Constants.SetTotalTextStyle(Program.Character.SpellSlots.NinethTotal, Program.Character.SpellSlots.NinethUsed);
            TotalNinethBox.Background = Constants.SetTotalBoxStyle(Program.Character.SpellSlots.NinethTotal, Program.Character.SpellSlots.NinethUsed);
        }

        private void FillUsedSpellSlots()
        {
            UsedPactField.Text = Program.Character.SpellSlots.PactUsed.ToString();
            UsedPactField.Foreground = Constants.SetUsedTextStyle(Program.Character.SpellSlots.PactTotal, Program.Character.SpellSlots.PactUsed);
            UsedPactBox.Background = Constants.SetUsedBoxStyle(Program.Character.SpellSlots.PactTotal, Program.Character.SpellSlots.PactUsed);

            UsedFirstField.Text = Program.Character.SpellSlots.FirstUsed.ToString();
            UsedFirstField.Foreground = Constants.SetUsedTextStyle(Program.Character.SpellSlots.FirstTotal, Program.Character.SpellSlots.FirstUsed);
            UsedFirstBox.Background = Constants.SetUsedBoxStyle(Program.Character.SpellSlots.FirstTotal, Program.Character.SpellSlots.FirstUsed);

            UsedSecondField.Text = Program.Character.SpellSlots.SecondUsed.ToString();
            UsedSecondField.Foreground = Constants.SetUsedTextStyle(Program.Character.SpellSlots.SecondTotal, Program.Character.SpellSlots.SecondUsed);
            UsedSecondBox.Background = Constants.SetUsedBoxStyle(Program.Character.SpellSlots.SecondTotal, Program.Character.SpellSlots.SecondUsed);

            UsedThirdField.Text = Program.Character.SpellSlots.ThirdUsed.ToString();
            UsedThirdField.Foreground = Constants.SetUsedTextStyle(Program.Character.SpellSlots.ThirdTotal, Program.Character.SpellSlots.ThirdUsed);
            UsedThirdBox.Background = Constants.SetUsedBoxStyle(Program.Character.SpellSlots.ThirdTotal, Program.Character.SpellSlots.ThirdUsed);

            UsedFourthField.Text = Program.Character.SpellSlots.FourthUsed.ToString();
            UsedFourthField.Foreground = Constants.SetUsedTextStyle(Program.Character.SpellSlots.FourthTotal, Program.Character.SpellSlots.FourthUsed);
            UsedFourthBox.Background = Constants.SetUsedBoxStyle(Program.Character.SpellSlots.FourthTotal, Program.Character.SpellSlots.FourthUsed);

            UsedFifthField.Text = Program.Character.SpellSlots.FifthUsed.ToString();
            UsedFifthField.Foreground = Constants.SetUsedTextStyle(Program.Character.SpellSlots.FifthTotal, Program.Character.SpellSlots.FifthUsed);
            UsedFifthBox.Background = Constants.SetUsedBoxStyle(Program.Character.SpellSlots.FifthTotal, Program.Character.SpellSlots.FifthUsed);

            UsedSixthField.Text = Program.Character.SpellSlots.SixthUsed.ToString();
            UsedSixthField.Foreground = Constants.SetUsedTextStyle(Program.Character.SpellSlots.SixthTotal, Program.Character.SpellSlots.SixthUsed);
            UsedSixthBox.Background = Constants.SetUsedBoxStyle(Program.Character.SpellSlots.SixthTotal, Program.Character.SpellSlots.SixthUsed);

            UsedSeventhField.Text = Program.Character.SpellSlots.SeventhUsed.ToString();
            UsedSeventhField.Foreground = Constants.SetUsedTextStyle(Program.Character.SpellSlots.SeventhTotal, Program.Character.SpellSlots.SeventhUsed);
            UsedSeventhBox.Background = Constants.SetUsedBoxStyle(Program.Character.SpellSlots.SeventhTotal, Program.Character.SpellSlots.SeventhUsed);

            UsedEighthField.Text = Program.Character.SpellSlots.EighthUsed.ToString();
            UsedEighthField.Foreground = Constants.SetUsedTextStyle(Program.Character.SpellSlots.EighthTotal, Program.Character.SpellSlots.EighthUsed);
            UsedEighthBox.Background = Constants.SetUsedBoxStyle(Program.Character.SpellSlots.EighthTotal, Program.Character.SpellSlots.EighthUsed);

            UsedNinethField.Text = Program.Character.SpellSlots.NinethUsed.ToString();
            UsedNinethField.Foreground = Constants.SetUsedTextStyle(Program.Character.SpellSlots.NinethTotal, Program.Character.SpellSlots.NinethUsed);
            UsedNinethBox.Background = Constants.SetUsedBoxStyle(Program.Character.SpellSlots.NinethTotal, Program.Character.SpellSlots.NinethUsed);
        }

        private void FillSpellList()
        {
            SpellListDataGrid.Items.Clear();

            foreach (var spell in Program.Character.Spells)
            {
                SpellListDataGrid.Items.Add(spell);
            }
        }

        private void FillMagicClassList()
        {
            MagicClassDataGrid.Items.Clear();

            foreach (var magicClass in Program.Character.MagicClasses)
            {
                MagicClassDataGrid.Items.Add(magicClass);
            }

            CasterLevelField.Text = Program.Character.CasterLevel.ToString();
        }

        private void InitializeClickEvents()
        {
            UsedPactBox.MouseDown += UsedSlot_MouseDown;
            UsedPactBox.MouseEnter += UsedSlot_MouseEnter;
            UsedPactBox.MouseLeave += UsedSlot_MouseLeave;

            UsedFirstBox.MouseDown += UsedSlot_MouseDown;
            UsedFirstBox.MouseEnter += UsedSlot_MouseEnter;
            UsedFirstBox.MouseLeave += UsedSlot_MouseLeave;

            UsedSecondBox.MouseDown += UsedSlot_MouseDown;
            UsedSecondBox.MouseEnter += UsedSlot_MouseEnter;
            UsedSecondBox.MouseLeave += UsedSlot_MouseLeave;

            UsedThirdBox.MouseDown += UsedSlot_MouseDown;
            UsedThirdBox.MouseEnter += UsedSlot_MouseEnter;
            UsedThirdBox.MouseLeave += UsedSlot_MouseLeave;

            UsedFourthBox.MouseDown += UsedSlot_MouseDown;
            UsedFourthBox.MouseEnter += UsedSlot_MouseEnter;
            UsedFourthBox.MouseLeave += UsedSlot_MouseLeave;

            UsedFifthBox.MouseDown += UsedSlot_MouseDown;
            UsedFifthBox.MouseEnter += UsedSlot_MouseEnter;
            UsedFifthBox.MouseLeave += UsedSlot_MouseLeave;

            UsedSixthBox.MouseDown += UsedSlot_MouseDown;
            UsedSixthBox.MouseEnter += UsedSlot_MouseEnter;
            UsedSixthBox.MouseLeave += UsedSlot_MouseLeave;

            UsedSeventhBox.MouseDown += UsedSlot_MouseDown;
            UsedSeventhBox.MouseEnter += UsedSlot_MouseEnter;
            UsedSeventhBox.MouseLeave += UsedSlot_MouseLeave;

            UsedEighthBox.MouseDown += UsedSlot_MouseDown;
            UsedEighthBox.MouseEnter += UsedSlot_MouseEnter;
            UsedEighthBox.MouseLeave += UsedSlot_MouseLeave;

            UsedNinethBox.MouseDown += UsedSlot_MouseDown;
            UsedNinethBox.MouseEnter += UsedSlot_MouseEnter;
            UsedNinethBox.MouseLeave += UsedSlot_MouseLeave;

        }

        private int IncrementUsedSpellSlots(int used, int total)
        {
            if (used < total)
            {
                return used + 1;
            }

            return used;
        }

        #endregion

        #region Accessors

        private SpellcastingSelectionWindow SpellcastingSelectionWindow { get; }
        private ModifySpellWindow ModifySpellWindow { get; }
        private ModifySpellClassWindow ModifySpellClassWindow { get; }
        private ModifySpellSlotsWindow ModifySpellSlotsWindow { get; }

        #endregion

        #region Events

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            int index;

            if (MagicClassDataGrid.SelectedItem != null)
            {
                MagicClass magicClass = (MagicClass)MagicClassDataGrid.SelectedItem;
                index = Program.Character.MagicClasses.IndexOf(magicClass);

                if (index != 0)
                {
                    Constants.Swap(Program.Character.MagicClasses, index, index - 1);
                    FillMagicClassList();
                    MagicClassDataGrid.SelectedIndex = index - 1;
                }
            }
            else if (SpellListDataGrid.SelectedItem != null)
            {
                Spell spell = (Spell)SpellListDataGrid.SelectedItem;
                index = Program.Character.Spells.IndexOf(spell);

                if (index != 0)
                {
                    Constants.Swap(Program.Character.Spells, index, index - 1);
                    FillSpellList();
                    SpellListDataGrid.SelectedIndex = index - 1;
                }
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            int index;

            if (MagicClassDataGrid.SelectedItem != null)
            {
                MagicClass magicClass = (MagicClass)MagicClassDataGrid.SelectedItem;
                index = Program.Character.MagicClasses.IndexOf(magicClass);

                if (index != Program.Character.MagicClasses.Count - 1)
                {
                    Constants.Swap(Program.Character.MagicClasses, index, index + 1);
                    FillMagicClassList();
                    MagicClassDataGrid.SelectedIndex = index + 1;
                }
            }
            else if (SpellListDataGrid.SelectedItem != null)
            {
                Spell spell = (Spell)SpellListDataGrid.SelectedItem;
                index = Program.Character.Spells.IndexOf(spell);

                if (index != Program.Character.Spells.Count - 1)
                {
                    Constants.Swap(Program.Character.Spells, index, index + 1);
                    FillSpellList();
                    SpellListDataGrid.SelectedIndex = index + 1;
                }
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            MagicClassDataGrid.UnselectAll();
            SpellListDataGrid.UnselectAll();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Constants.PopupButtons popupButton;

            popupButton = SpellcastingSelectionWindow.ShowPopup();

            switch (popupButton)
            {
                case Constants.PopupButtons.AddMagicClass:
                    ModifySpellClassWindow.AddClass();
                    FillMagicClassList();
                    break;
                case Constants.PopupButtons.AddSpell:
                    ModifySpellWindow.AddSpell();
                    FillSpellList();
                    break;
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (MagicClassDataGrid.SelectedItem != null)
            {
                MagicClass magicClass = (MagicClass)MagicClassDataGrid.SelectedItem;
                ModifySpellClassWindow.EditClass(magicClass);
                FillMagicClassList();
            }
            else if (SpellListDataGrid.SelectedItem != null)
            {
                Spell spell = (Spell)SpellListDataGrid.SelectedItem;
                ModifySpellWindow.EditSpell(spell);
                FillSpellList();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MagicClassDataGrid.SelectedItem != null)
            {
                MagicClass magicClass = (MagicClass)MagicClassDataGrid.SelectedItem;
                Program.Character.MagicClasses.Remove(magicClass);
                FillMagicClassList();
            }
            else if (SpellListDataGrid.SelectedItem != null)
            {
                Spell spell = (Spell)SpellListDataGrid.SelectedItem;
                Program.Character.Spells.Remove(spell);
                FillSpellList();
            }
        }

        private void MagicClassDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MagicClassDataGrid.SelectedItem != null)
            {
                SpellListDataGrid.UnselectAll();
            }
        }

        private void SpellListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SpellListDataGrid.SelectedItem != null)
            {
                MagicClassDataGrid.UnselectAll();
            }
        }

        private void LevelEditButton_Click(object sender, RoutedEventArgs e)
        {
            ModifySpellSlotsWindow.EditSpellSlots();
            FillTotalSpellSlots();
            FillUsedSpellSlots();
        }

        private void UsedSlot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid usedBox = sender as Grid;

            switch (usedBox.Name)
            {
                case "UsedPactBox":
                    Program.Character.SpellSlots.PactUsed = IncrementUsedSpellSlots(Program.Character.SpellSlots.PactUsed, Program.Character.SpellSlots.PactTotal);
                    break;
                case "UsedFirstBox":
                    Program.Character.SpellSlots.FirstUsed = IncrementUsedSpellSlots(Program.Character.SpellSlots.FirstUsed, Program.Character.SpellSlots.FirstTotal);
                    break;
                case "UsedSecondBox":
                    Program.Character.SpellSlots.SecondUsed = IncrementUsedSpellSlots(Program.Character.SpellSlots.SecondUsed, Program.Character.SpellSlots.SecondTotal);
                    break;
                case "UsedThirdBox":
                    Program.Character.SpellSlots.ThirdUsed = IncrementUsedSpellSlots(Program.Character.SpellSlots.ThirdUsed, Program.Character.SpellSlots.ThirdTotal);
                    break;
                case "UsedFourthBox":
                    Program.Character.SpellSlots.FourthUsed = IncrementUsedSpellSlots(Program.Character.SpellSlots.FourthUsed, Program.Character.SpellSlots.FourthTotal);
                    break;
                case "UsedFifthBox":
                    Program.Character.SpellSlots.FifthUsed = IncrementUsedSpellSlots(Program.Character.SpellSlots.FifthUsed, Program.Character.SpellSlots.FifthTotal);
                    break;
                case "UsedSixthBox":
                    Program.Character.SpellSlots.SixthUsed = IncrementUsedSpellSlots(Program.Character.SpellSlots.SixthUsed, Program.Character.SpellSlots.SixthTotal);
                    break;
                case "UsedSeventhBox":
                    Program.Character.SpellSlots.SeventhUsed = IncrementUsedSpellSlots(Program.Character.SpellSlots.SeventhUsed, Program.Character.SpellSlots.SeventhTotal);
                    break;
                case "UsedEighthBox":
                    Program.Character.SpellSlots.EighthUsed = IncrementUsedSpellSlots(Program.Character.SpellSlots.EighthUsed, Program.Character.SpellSlots.EighthTotal);
                    break;
                case "UsedNinethBox":
                    Program.Character.SpellSlots.NinethUsed = IncrementUsedSpellSlots(Program.Character.SpellSlots.NinethUsed, Program.Character.SpellSlots.NinethTotal);
                    break;
            }

            FillTotalSpellSlots();
            FillUsedSpellSlots();
        }

        private void UsedSlot_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void UsedSlot_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion
    }
}
