// <copyright file="ModifyWealthWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.DetailsPageUi
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ModifyWealthWindow.xaml.
    /// </summary>
    public partial class ModifyWealthWindow : Window
    {
        public ModifyWealthWindow()
        {
            this.InitializeComponent();
        }

        private int CP { get; set; }

        private int SP { get; set; }

        private int EP { get; set; }

        private int GP { get; set; }

        private int PP { get; set; }

        public void ShowWindow()
        {
            this.ClearFields();

            this.CP = Program.CcsFile.Character.Wealth.Copper;
            this.SP = Program.CcsFile.Character.Wealth.Silver;
            this.EP = Program.CcsFile.Character.Wealth.Electrum;
            this.GP = Program.CcsFile.Character.Wealth.Gold;
            this.PP = Program.CcsFile.Character.Wealth.Platinum;

            this.FillFields();

            this.ShowDialog();
        }

        private void ClearFields()
        {
            this.AddRadioButton.IsChecked = true;
            this.CpRadioButton.IsChecked = true;
            this.AmountUpDown.Value = 0;
        }

        private void FillFields()
        {
            this.CopperField.Text = this.CP.ToString();
            this.SilverField.Text = this.SP.ToString();
            this.ElectrumField.Text = this.EP.ToString();
            this.GoldField.Text = this.GP.ToString();
            this.PlatinumField.Text = this.PP.ToString();
        }

        private int GetAmount()
        {
            return this.AddRadioButton.IsChecked ?? false
                ? this.AmountUpDown.Value ?? 0
                : this.SubtractRadioButton.IsChecked ?? false ? (this.AmountUpDown.Value ?? 0) * -1 : 0;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.CcsFile.Character.Wealth.Copper = this.CP;
            Program.CcsFile.Character.Wealth.Silver = this.SP;
            Program.CcsFile.Character.Wealth.Electrum = this.EP;
            Program.CcsFile.Character.Wealth.Gold = this.GP;
            Program.CcsFile.Character.Wealth.Platinum = this.PP;

            Program.Modified = true;
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.CpRadioButton.IsChecked ?? false)
            {
                this.CP += this.GetAmount();
            }
            else if (this.SpRadioButton.IsChecked ?? false)
            {
                this.SP += this.GetAmount();
            }
            else if (this.EpRadioButton.IsChecked ?? false)
            {
                this.EP += this.GetAmount();
            }
            else if (this.GpRadioButton.IsChecked ?? false)
            {
                this.GP += this.GetAmount();
            }
            else if (this.PpRadioButton.IsChecked ?? false)
            {
                this.PP += this.GetAmount();
            }

            this.FillFields();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
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
