// <copyright file="ModifyWealthWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.OverviewPageInterface
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Interfaces.Enums;

    /// <summary>
    /// Interaction logic for ModifyWealthWindow.xaml.
    /// </summary>
    public partial class ModifyWealthWindow : Window, IConciergeWindow
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

        private ConciergeWindowResult Result { get; set; }

        public ConciergeWindowResult ShowWizardSetup()
        {
            this.ClearFields();
            this.FillFields();
            this.ShowDialog();

            return this.Result;
        }

        public void ShowWindow()
        {
            this.ClearFields();
            this.FillFields();
            this.ShowDialog();
        }

        public void UpdateCancelButton(string text)
        {
            this.CancelButton.Content = text;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void ClearFields()
        {
            this.AmountUpDown.UpdatingValue();

            this.AddRadioButton.IsChecked = true;
            this.CpRadioButton.IsChecked = true;
            this.AmountUpDown.Value = 0;

            this.CP = Program.CcsFile.Character.Wealth.Copper;
            this.SP = Program.CcsFile.Character.Wealth.Silver;
            this.EP = Program.CcsFile.Character.Wealth.Electrum;
            this.GP = Program.CcsFile.Character.Wealth.Gold;
            this.PP = Program.CcsFile.Character.Wealth.Platinum;
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
            this.Result = ConciergeWindowResult.Exit;
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Result = ConciergeWindowResult.OK;

            Program.CcsFile.Character.Wealth.Copper = this.CP;
            Program.CcsFile.Character.Wealth.Silver = this.SP;
            Program.CcsFile.Character.Wealth.Electrum = this.EP;
            Program.CcsFile.Character.Wealth.Gold = this.GP;
            Program.CcsFile.Character.Wealth.Platinum = this.PP;

            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();

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
            this.Result = ConciergeWindowResult.Cancel;
            this.Hide();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Result = ConciergeWindowResult.Exit;
                    this.Hide();
                    break;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
