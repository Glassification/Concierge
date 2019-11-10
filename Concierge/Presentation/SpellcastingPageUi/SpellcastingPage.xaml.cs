using Concierge.Characters.Collections;
using Concierge.Utility;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Concierge.Presentation.SpellcastingPageUi
{
    /// <summary>
    /// Interaction logic for SpellcastingPage.xaml
    /// </summary>
    public partial class SpellcastingPage : Page
    {
        public SpellcastingPage()
        {
            InitializeComponent();
        }

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
            int level = 0;
            MagicClassDataGrid.Items.Clear();

            foreach (var magicClass in Program.Character.MagicClasses)
            {
                MagicClassDataGrid.Items.Add(magicClass);
                level += magicClass.Level;
            }

            CasterLevelField.Text = level.ToString();
        }

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

        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
