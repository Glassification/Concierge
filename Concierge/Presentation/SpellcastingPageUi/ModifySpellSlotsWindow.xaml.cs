using System;
using System.Windows;
using System.Windows.Input;

namespace Concierge.Presentation.SpellcastingPageUi
{
    /// <summary>
    /// Interaction logic for ModifySpellSlotsWindow.xaml
    /// </summary>
    public partial class ModifySpellSlotsWindow : Window
    {
        public ModifySpellSlotsWindow()
        {
            InitializeComponent();
        }

        public void EditSpellSlots()
        {
            FillFields();
            ShowDialog();
        }

        private void FillFields()
        {
            UsedPactUpDown.Value = Program.Character.SpellSlots.PactUsed;
            Used1UpDown.Value = Program.Character.SpellSlots.FirstUsed;
            Used2UpDown.Value = Program.Character.SpellSlots.SecondUsed;
            Used3UpDown.Value = Program.Character.SpellSlots.ThirdUsed;
            Used4UpDown.Value = Program.Character.SpellSlots.FourthUsed;
            Used5UpDown.Value = Program.Character.SpellSlots.FifthUsed;
            Used6UpDown.Value = Program.Character.SpellSlots.SixthUsed;
            Used7UpDown.Value = Program.Character.SpellSlots.SeventhUsed;
            Used8UpDown.Value = Program.Character.SpellSlots.EighthUsed;
            Used9UpDown.Value = Program.Character.SpellSlots.NinethUsed;

            TotalPactUpDown.Value = Program.Character.SpellSlots.PactTotal;
            Total1UpDown.Value = Program.Character.SpellSlots.FirstTotal;
            Total2UpDown.Value = Program.Character.SpellSlots.SecondTotal;
            Total3UpDown.Value = Program.Character.SpellSlots.ThirdTotal;
            Total4UpDown.Value = Program.Character.SpellSlots.FourthTotal;
            Total5UpDown.Value = Program.Character.SpellSlots.FifthTotal;
            Total6UpDown.Value = Program.Character.SpellSlots.SixthTotal;
            Total7UpDown.Value = Program.Character.SpellSlots.SeventhTotal;
            Total8UpDown.Value = Program.Character.SpellSlots.EighthTotal;
            Total9UpDown.Value = Program.Character.SpellSlots.NinethTotal;
        }

        private void UpdateSpellSlots()
        {
            Program.Character.SpellSlots.PactUsed = UsedPactUpDown.Value ?? 0;
            Program.Character.SpellSlots.FirstUsed = Used1UpDown.Value ?? 0;
            Program.Character.SpellSlots.SecondUsed = Used2UpDown.Value ?? 0;
            Program.Character.SpellSlots.ThirdUsed = Used3UpDown.Value ?? 0;
            Program.Character.SpellSlots.FourthUsed = Used4UpDown.Value ?? 0;
            Program.Character.SpellSlots.FifthUsed = Used5UpDown.Value ?? 0;
            Program.Character.SpellSlots.SixthUsed = UsedPactUpDown.Value ?? 0;
            Program.Character.SpellSlots.SeventhUsed = Used7UpDown.Value ?? 0;
            Program.Character.SpellSlots.EighthUsed = Used8UpDown.Value ?? 0;
            Program.Character.SpellSlots.NinethUsed = Used9UpDown.Value ?? 0;

            Program.Character.SpellSlots.PactTotal = TotalPactUpDown.Value ?? 0;
            Program.Character.SpellSlots.FirstTotal = Total1UpDown.Value ?? 0;
            Program.Character.SpellSlots.SecondTotal = Total2UpDown.Value ?? 0;
            Program.Character.SpellSlots.ThirdTotal = Total3UpDown.Value ?? 0;
            Program.Character.SpellSlots.FourthTotal = Total4UpDown.Value ?? 0;
            Program.Character.SpellSlots.FifthTotal = Total5UpDown.Value ?? 0;
            Program.Character.SpellSlots.SixthTotal = Total6UpDown.Value ?? 0;
            Program.Character.SpellSlots.SeventhTotal = Total7UpDown.Value ?? 0;
            Program.Character.SpellSlots.EighthTotal = Total8UpDown.Value ?? 0;
            Program.Character.SpellSlots.NinethTotal = Total9UpDown.Value ?? 0;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateSpellSlots();
            Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateSpellSlots();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
