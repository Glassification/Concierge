// <copyright file="SettingsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.HelperInterface
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Interfaces.Enums;
    using Concierge.Tools;
    using Concierge.Utility;
    using Concierge.Utility.Dtos;
    using Concierge.Utility.Extensions;

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

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Hide();
        }

        private void Read()
        {
            this.AutosaveCheckBox.UpdatingValue();
            this.CoinWeightCheckBox.UpdatingValue();
            this.EncumbranceCheckBox.UpdatingValue();
            this.MuteCheckBox.UpdatingValue();
            this.CheckVersionCheckBox.UpdatingValue();

            this.AutosaveCheckBox.IsChecked = ConciergeSettings.AutosaveEnabled;
            this.AutosaveInterval.Value = ConciergeSettings.AutosaveInterval;
            this.CoinWeightCheckBox.IsChecked = ConciergeSettings.UseCoinWeight;
            this.EncumbranceCheckBox.IsChecked = ConciergeSettings.UseEncumbrance;
            this.IntervalTextBox.Text = this.FormattedInterval;
            this.MuteCheckBox.IsChecked = ConciergeSettings.MuteSounds;
            this.CheckVersionCheckBox.IsChecked = ConciergeSettings.CheckVersion;

            if (ConciergeSettings.AutosaveEnabled)
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
            this.CheckVersionCheckBox.UpdatedValue();
        }

        private bool Write()
        {
            if (Program.CcsFile.AbsolutePath.IsNullOrWhiteSpace() && (this.AutosaveCheckBox.IsChecked ?? false))
            {
                ConciergeMessageBox.Show(
                    "You must save this sheet before enabling autosave.",
                    "Warning",
                    ConciergeWindowButtons.Ok,
                    ConciergeWindowIcons.Alert);

                return false;
            }

            var conciergeSettings = new ConciergeSettingsDto()
            {
                AutosaveEnabled = this.AutosaveCheckBox.IsChecked ?? false,
                AutosaveInterval = (int)this.AutosaveInterval.Value,
                CheckVersion = this.CheckVersionCheckBox.IsChecked ?? false,
                MuteSounds = this.MuteCheckBox.IsChecked ?? false,
                UseCoinWeight = this.CoinWeightCheckBox.IsChecked ?? false,
                UseEncumbrance = this.EncumbranceCheckBox.IsChecked ?? false,
            };

            ConciergeSettings.UpdateSettings(conciergeSettings);

            return true;
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
            if (this.Write())
            {
                this.Hide();
            }
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
