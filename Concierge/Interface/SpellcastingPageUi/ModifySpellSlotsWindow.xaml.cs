// <copyright file="ModifySpellSlotsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.SpellcastingPageUi
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ModifySpellSlotsWindow.xaml.
    /// </summary>
    public partial class ModifySpellSlotsWindow : Window
    {
        public ModifySpellSlotsWindow()
        {
            this.InitializeComponent();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        public void EditSpellSlots()
        {
            this.FillFields();
            this.ShowDialog();
        }

        private void FillFields()
        {
            this.UsedPactUpDown.Value = Program.CcsFile.Character.SpellSlots.PactUsed;
            this.Used1UpDown.Value = Program.CcsFile.Character.SpellSlots.FirstUsed;
            this.Used2UpDown.Value = Program.CcsFile.Character.SpellSlots.SecondUsed;
            this.Used3UpDown.Value = Program.CcsFile.Character.SpellSlots.ThirdUsed;
            this.Used4UpDown.Value = Program.CcsFile.Character.SpellSlots.FourthUsed;
            this.Used5UpDown.Value = Program.CcsFile.Character.SpellSlots.FifthUsed;
            this.Used6UpDown.Value = Program.CcsFile.Character.SpellSlots.SixthUsed;
            this.Used7UpDown.Value = Program.CcsFile.Character.SpellSlots.SeventhUsed;
            this.Used8UpDown.Value = Program.CcsFile.Character.SpellSlots.EighthUsed;
            this.Used9UpDown.Value = Program.CcsFile.Character.SpellSlots.NinethUsed;

            this.TotalPactUpDown.Value = Program.CcsFile.Character.SpellSlots.PactTotal;
            this.Total1UpDown.Value = Program.CcsFile.Character.SpellSlots.FirstTotal;
            this.Total2UpDown.Value = Program.CcsFile.Character.SpellSlots.SecondTotal;
            this.Total3UpDown.Value = Program.CcsFile.Character.SpellSlots.ThirdTotal;
            this.Total4UpDown.Value = Program.CcsFile.Character.SpellSlots.FourthTotal;
            this.Total5UpDown.Value = Program.CcsFile.Character.SpellSlots.FifthTotal;
            this.Total6UpDown.Value = Program.CcsFile.Character.SpellSlots.SixthTotal;
            this.Total7UpDown.Value = Program.CcsFile.Character.SpellSlots.SeventhTotal;
            this.Total8UpDown.Value = Program.CcsFile.Character.SpellSlots.EighthTotal;
            this.Total9UpDown.Value = Program.CcsFile.Character.SpellSlots.NinethTotal;
        }

        private void UpdateSpellSlots()
        {
            Program.CcsFile.Character.SpellSlots.PactUsed = this.UsedPactUpDown.Value ?? 0;
            Program.CcsFile.Character.SpellSlots.FirstUsed = this.Used1UpDown.Value ?? 0;
            Program.CcsFile.Character.SpellSlots.SecondUsed = this.Used2UpDown.Value ?? 0;
            Program.CcsFile.Character.SpellSlots.ThirdUsed = this.Used3UpDown.Value ?? 0;
            Program.CcsFile.Character.SpellSlots.FourthUsed = this.Used4UpDown.Value ?? 0;
            Program.CcsFile.Character.SpellSlots.FifthUsed = this.Used5UpDown.Value ?? 0;
            Program.CcsFile.Character.SpellSlots.SixthUsed = this.UsedPactUpDown.Value ?? 0;
            Program.CcsFile.Character.SpellSlots.SeventhUsed = this.Used7UpDown.Value ?? 0;
            Program.CcsFile.Character.SpellSlots.EighthUsed = this.Used8UpDown.Value ?? 0;
            Program.CcsFile.Character.SpellSlots.NinethUsed = this.Used9UpDown.Value ?? 0;

            Program.CcsFile.Character.SpellSlots.PactTotal = this.TotalPactUpDown.Value ?? 0;
            Program.CcsFile.Character.SpellSlots.FirstTotal = this.Total1UpDown.Value ?? 0;
            Program.CcsFile.Character.SpellSlots.SecondTotal = this.Total2UpDown.Value ?? 0;
            Program.CcsFile.Character.SpellSlots.ThirdTotal = this.Total3UpDown.Value ?? 0;
            Program.CcsFile.Character.SpellSlots.FourthTotal = this.Total4UpDown.Value ?? 0;
            Program.CcsFile.Character.SpellSlots.FifthTotal = this.Total5UpDown.Value ?? 0;
            Program.CcsFile.Character.SpellSlots.SixthTotal = this.Total6UpDown.Value ?? 0;
            Program.CcsFile.Character.SpellSlots.SeventhTotal = this.Total7UpDown.Value ?? 0;
            Program.CcsFile.Character.SpellSlots.EighthTotal = this.Total8UpDown.Value ?? 0;
            Program.CcsFile.Character.SpellSlots.NinethTotal = this.Total9UpDown.Value ?? 0;
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
            Program.Modify();

            this.UpdateSpellSlots();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

            this.UpdateSpellSlots();

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Button_MouseEnter(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.Black;
        }

        private void Button_MouseLeave(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.White;
        }
    }
}
