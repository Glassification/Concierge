// <copyright file="ModifySpellSlotsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.SpellcastingPageUi
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for ModifySpellSlotsWindow.xaml.
    /// </summary>
    public partial class ModifySpellSlotsWindow : Window
    {
        public ModifySpellSlotsWindow()
        {
            this.InitializeComponent();
        }

        public void EditSpellSlots()
        {
            this.FillFields();
            this.ShowDialog();
        }

        private void FillFields()
        {
            this.UsedPactUpDown.Value = Program.Character.SpellSlots.PactUsed;
            this.Used1UpDown.Value = Program.Character.SpellSlots.FirstUsed;
            this.Used2UpDown.Value = Program.Character.SpellSlots.SecondUsed;
            this.Used3UpDown.Value = Program.Character.SpellSlots.ThirdUsed;
            this.Used4UpDown.Value = Program.Character.SpellSlots.FourthUsed;
            this.Used5UpDown.Value = Program.Character.SpellSlots.FifthUsed;
            this.Used6UpDown.Value = Program.Character.SpellSlots.SixthUsed;
            this.Used7UpDown.Value = Program.Character.SpellSlots.SeventhUsed;
            this.Used8UpDown.Value = Program.Character.SpellSlots.EighthUsed;
            this.Used9UpDown.Value = Program.Character.SpellSlots.NinethUsed;

            this.TotalPactUpDown.Value = Program.Character.SpellSlots.PactTotal;
            this.Total1UpDown.Value = Program.Character.SpellSlots.FirstTotal;
            this.Total2UpDown.Value = Program.Character.SpellSlots.SecondTotal;
            this.Total3UpDown.Value = Program.Character.SpellSlots.ThirdTotal;
            this.Total4UpDown.Value = Program.Character.SpellSlots.FourthTotal;
            this.Total5UpDown.Value = Program.Character.SpellSlots.FifthTotal;
            this.Total6UpDown.Value = Program.Character.SpellSlots.SixthTotal;
            this.Total7UpDown.Value = Program.Character.SpellSlots.SeventhTotal;
            this.Total8UpDown.Value = Program.Character.SpellSlots.EighthTotal;
            this.Total9UpDown.Value = Program.Character.SpellSlots.NinethTotal;
        }

        private void UpdateSpellSlots()
        {
            Program.Character.SpellSlots.PactUsed = this.UsedPactUpDown.Value ?? 0;
            Program.Character.SpellSlots.FirstUsed = this.Used1UpDown.Value ?? 0;
            Program.Character.SpellSlots.SecondUsed = this.Used2UpDown.Value ?? 0;
            Program.Character.SpellSlots.ThirdUsed = this.Used3UpDown.Value ?? 0;
            Program.Character.SpellSlots.FourthUsed = this.Used4UpDown.Value ?? 0;
            Program.Character.SpellSlots.FifthUsed = this.Used5UpDown.Value ?? 0;
            Program.Character.SpellSlots.SixthUsed = this.UsedPactUpDown.Value ?? 0;
            Program.Character.SpellSlots.SeventhUsed = this.Used7UpDown.Value ?? 0;
            Program.Character.SpellSlots.EighthUsed = this.Used8UpDown.Value ?? 0;
            Program.Character.SpellSlots.NinethUsed = this.Used9UpDown.Value ?? 0;

            Program.Character.SpellSlots.PactTotal = this.TotalPactUpDown.Value ?? 0;
            Program.Character.SpellSlots.FirstTotal = this.Total1UpDown.Value ?? 0;
            Program.Character.SpellSlots.SecondTotal = this.Total2UpDown.Value ?? 0;
            Program.Character.SpellSlots.ThirdTotal = this.Total3UpDown.Value ?? 0;
            Program.Character.SpellSlots.FourthTotal = this.Total4UpDown.Value ?? 0;
            Program.Character.SpellSlots.FifthTotal = this.Total5UpDown.Value ?? 0;
            Program.Character.SpellSlots.SixthTotal = this.Total6UpDown.Value ?? 0;
            Program.Character.SpellSlots.SeventhTotal = this.Total7UpDown.Value ?? 0;
            Program.Character.SpellSlots.EighthTotal = this.Total8UpDown.Value ?? 0;
            Program.Character.SpellSlots.NinethTotal = this.Total9UpDown.Value ?? 0;

            Program.Modified = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateSpellSlots();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateSpellSlots();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
