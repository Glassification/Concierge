// <copyright file="SettingsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.HelperUi
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Presentation.Enums;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for SettingsWindow.xaml.
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            this.InitializeComponent();
        }

        private string FormattedInterval => $"Autosave Interval: {Constants.AutosaveIntervals[(int)this.AutosaveInterval.Value]} minute{((int)this.AutosaveInterval.Value > 0 ? "s" : string.Empty)}";

        public void ShowWindow()
        {
            this.Read();
            this.ShowDialog();
        }

        private void Read()
        {
            this.AutosaveCheckBox.IsChecked = Settings.AutosaveEnable;
            this.AutosaveInterval.Value = Settings.AutosaveInterval;
            this.CoinWeightCheckBox.IsChecked = Settings.UseCoinWeight;
            this.EncumbranceCheckBox.IsChecked = Settings.UseEncumbrance;
            this.IntervalTextBox.Text = this.FormattedInterval;

            if (Settings.AutosaveEnable)
            {
                this.IntervalTextBox.IsEnabled = true;
                this.AutosaveInterval.IsEnabled = true;
            }
            else
            {
                this.IntervalTextBox.IsEnabled = false;
                this.AutosaveInterval.IsEnabled = false;
            }
        }

        private void Write()
        {
            if (Program.CcsFile == null && (this.AutosaveCheckBox.IsChecked ?? false))
            {
                Program.ConciergeMessageWindow.ShowWindow(
                    "You must save this sheet before enabling autosave.",
                    MessageWindowButtons.Ok);
            }
            else
            {
                Settings.AutosaveEnable = this.AutosaveCheckBox.IsChecked ?? false;
            }

            Settings.AutosaveInterval = (int)this.AutosaveInterval.Value;
            Settings.UseCoinWeight = this.CoinWeightCheckBox.IsChecked ?? false;
            Settings.UseEncumbrance = this.EncumbranceCheckBox.IsChecked ?? false;
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
            this.Write();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Write();
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

        private void AutosaveInterval_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.IntervalTextBox.Text = this.FormattedInterval;
        }

        private void AutosaveCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.IntervalTextBox.IsEnabled = true;
            this.AutosaveInterval.IsEnabled = true;
        }

        private void AutosaveCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.IntervalTextBox.IsEnabled = false;
            this.AutosaveInterval.IsEnabled = false;
        }
    }
}
