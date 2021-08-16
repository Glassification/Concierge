// <copyright file="SettingsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.HelperUi
{
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Interface.Enums;
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
            this.AutosaveCheckBox.UpdatingValue();
            this.CoinWeightCheckBox.UpdatingValue();
            this.EncumbranceCheckBox.UpdatingValue();
            this.MuteCheckBox.UpdatingValue();

            this.AutosaveCheckBox.IsChecked = Program.CcsFile.AutosaveEnable;
            this.AutosaveInterval.Value = Program.CcsFile.AutosaveInterval;
            this.CoinWeightCheckBox.IsChecked = Program.CcsFile.UseCoinWeight;
            this.EncumbranceCheckBox.IsChecked = Program.CcsFile.UseEncumbrance;
            this.IntervalTextBox.Text = this.FormattedInterval;
            this.MuteCheckBox.IsChecked = Program.CcsFile.MuteSound;

            if (Program.CcsFile.AutosaveEnable)
            {
                this.IntervalTextBox.IsEnabled = true;
                this.AutosaveInterval.IsEnabled = true;
            }
            else
            {
                this.IntervalTextBox.IsEnabled = false;
                this.AutosaveInterval.IsEnabled = false;
            }

            this.AutosaveCheckBox.UpdatedValue();
            this.CoinWeightCheckBox.UpdatedValue();
            this.EncumbranceCheckBox.UpdatedValue();
            this.MuteCheckBox.UpdatedValue();
        }

        private void Write()
        {
            if (Program.CcsFile == null && (this.AutosaveCheckBox.IsChecked ?? false))
            {
                Program.ConciergeMessageWindow.ShowWindow(
                    "You must save this sheet before enabling autosave.",
                    "Warning",
                    MessageWindowButtons.Ok,
                    MessageWindowIcons.Alert);
            }
            else
            {
                Program.CcsFile.AutosaveEnable = this.AutosaveCheckBox.IsChecked ?? false;
            }

            Program.CcsFile.AutosaveInterval = (int)this.AutosaveInterval.Value;
            Program.CcsFile.UseCoinWeight = this.CoinWeightCheckBox.IsChecked ?? false;
            Program.CcsFile.UseEncumbrance = this.EncumbranceCheckBox.IsChecked ?? false;
            Program.CcsFile.MuteSound = this.MuteCheckBox.IsChecked ?? false;
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
            Program.Modify();
            this.Write();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Program.Modify();
            this.Write();
            this.Hide();
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
