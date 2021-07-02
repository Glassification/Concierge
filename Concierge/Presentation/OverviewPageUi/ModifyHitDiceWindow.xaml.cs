// <copyright file="ModifyHitDiceWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.OverviewPageUi
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ModifyHitDiceWindow.xaml.
    /// </summary>
    public partial class ModifyHitDiceWindow : Window
    {
        public ModifyHitDiceWindow()
        {
            this.InitializeComponent();
        }

        public void ModifyHitDice()
        {
            this.SetHitDice();

            this.ShowDialog();
        }

        private void SetHitDice()
        {
            this.TotalD6UpDown.Value = Program.Character.Vitality.HitDice.TotalD6;
            this.TotalD8UpDown.Value = Program.Character.Vitality.HitDice.TotalD8;
            this.TotalD10UpDown.Value = Program.Character.Vitality.HitDice.TotalD10;
            this.TotalD12UpDown.Value = Program.Character.Vitality.HitDice.TotalD12;

            this.UsedD6UpDown.Value = Program.Character.Vitality.HitDice.SpentD6;
            this.UsedD8UpDown.Value = Program.Character.Vitality.HitDice.SpentD8;
            this.UsedD10UpDown.Value = Program.Character.Vitality.HitDice.SpentD10;
            this.UsedD12UpDown.Value = Program.Character.Vitality.HitDice.SpentD12;
        }

        private void GetHitDice()
        {
            Program.Character.Vitality.HitDice.TotalD6 = this.TotalD6UpDown.Value ?? 0;
            Program.Character.Vitality.HitDice.TotalD8 = this.TotalD8UpDown.Value ?? 0;
            Program.Character.Vitality.HitDice.TotalD10 = this.TotalD10UpDown.Value ?? 0;
            Program.Character.Vitality.HitDice.TotalD12 = this.TotalD12UpDown.Value ?? 0;

            Program.Character.Vitality.HitDice.SpentD6 = this.UsedD6UpDown.Value ?? 0;
            Program.Character.Vitality.HitDice.SpentD8 = this.UsedD8UpDown.Value ?? 0;
            Program.Character.Vitality.HitDice.SpentD10 = this.UsedD10UpDown.Value ?? 0;
            Program.Character.Vitality.HitDice.SpentD12 = this.UsedD12UpDown.Value ?? 0;

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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.GetHitDice();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.GetHitDice();
            this.Hide();
        }

        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            this.CloseButton.Foreground = Brushes.Black;
        }

        private void CloseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            this.CloseButton.Foreground = Brushes.White;
        }
    }
}
