// <copyright file="SettingsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.UtilityInterface
{
    using System;
    using System.Linq;
    using System.Windows;

    using Concierge.Commands;
    using Concierge.Configuration;
    using Concierge.Configuration.Dtos;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Tools.Interface;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Units.Enums;

    /// <summary>
    /// Interaction logic for SettingsWindow.xaml.
    /// </summary>
    public partial class SettingsWindow : ConciergeWindow
    {
        public SettingsWindow()
        {
            this.InitializeComponent();
            this.UnitOfMeasurementComboBox.ItemsSource = Enum.GetValues(typeof(UnitTypes)).Cast<UnitTypes>();
        }

        private string FormattedInterval => $"Autosave Interval: {Constants.AutosaveIntervals[(int)this.AutosaveInterval.Value]} minute{((int)this.AutosaveInterval.Value > 0 ? "s" : string.Empty)}";

        public override void ShowEdit<T>(T item)
        {
            this.FillFields();
            this.ShowConciergeWindow();
        }

        protected override void ReturnAndClose()
        {
            if (this.UpdateSettings())
            {
                this.CloseConciergeWindow();
            }
        }

        private void FillFields()
        {
            this.AutosaveCheckBox.UpdatingValue();
            this.CoinWeightCheckBox.UpdatingValue();
            this.EncumbranceCheckBox.UpdatingValue();
            this.MuteCheckBox.UpdatingValue();
            this.CheckVersionCheckBox.UpdatingValue();
            this.CenterWindowsCheckBox.UpdatingValue();
            this.DisplayTaskBarCheckBox.UpdatingValue();

            this.AutosaveCheckBox.IsChecked = AppSettingsManager.UserSettings.AutosaveEnabled;
            this.AutosaveInterval.Value = AppSettingsManager.UserSettings.AutosaveInterval;
            this.CoinWeightCheckBox.IsChecked = AppSettingsManager.UserSettings.UseCoinWeight;
            this.EncumbranceCheckBox.IsChecked = AppSettingsManager.UserSettings.UseEncumbrance;
            this.IntervalTextBox.Text = this.FormattedInterval;
            this.MuteCheckBox.IsChecked = AppSettingsManager.UserSettings.MuteSounds;
            this.CheckVersionCheckBox.IsChecked = AppSettingsManager.UserSettings.CheckVersion;
            this.UnitOfMeasurementComboBox.Text = AppSettingsManager.UserSettings.UnitOfMeasurement.ToString();
            this.CenterWindowsCheckBox.IsChecked = AppSettingsManager.UserSettings.AttemptToCenterWindows;
            this.DisplayTaskBarCheckBox.IsChecked = AppSettingsManager.UserSettings.DisplayTaskBar;

            if (AppSettingsManager.UserSettings.AutosaveEnabled)
            {
                this.EnableAutosaveControls();
            }
            else
            {
                this.DisableAutosaveControls();
            }

            this.AutosaveCheckBox.UpdatedValue();
            this.CoinWeightCheckBox.UpdatedValue();
            this.EncumbranceCheckBox.UpdatedValue();
            this.MuteCheckBox.UpdatedValue();
            this.CheckVersionCheckBox.UpdatedValue();
            this.CenterWindowsCheckBox.UpdatedValue();
            this.DisplayTaskBarCheckBox.UpdatedValue();
        }

        private bool UpdateSettings()
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

            var oldSettings = AppSettingsManager.ToUserSettingsDto();
            var conciergeSettings = new UserSettingsDto()
            {
                AttemptToCenterWindows = this.CenterWindowsCheckBox.IsChecked ?? false,
                AutosaveEnabled = this.AutosaveCheckBox.IsChecked ?? false,
                AutosaveInterval = (int)this.AutosaveInterval.Value,
                CheckVersion = this.CheckVersionCheckBox.IsChecked ?? false,
                DisplayTaskBar = this.DisplayTaskBarCheckBox.IsChecked ?? false,
                MuteSounds = this.MuteCheckBox.IsChecked ?? false,
                UseCoinWeight = this.CoinWeightCheckBox.IsChecked ?? false,
                UseEncumbrance = this.EncumbranceCheckBox.IsChecked ?? false,
                UnitOfMeasurement = !Enum.TryParse(this.UnitOfMeasurementComboBox.Text, out UnitTypes value) ? default : value,
            };

            Program.UndoRedoService.AddCommand(new UpdateSettingsCommand(oldSettings, conciergeSettings));
            AppSettingsManager.UpdateSettings(conciergeSettings);

            return true;
        }

        private void EnableAutosaveControls()
        {
            this.IntervalTextBox.IsEnabled = true;
            this.AutosaveInterval.IsEnabled = true;
            this.IntervalTextBox.Opacity = 1;
            this.AutosaveInterval.Opacity = 1;
        }

        private void DisableAutosaveControls()
        {
            this.IntervalTextBox.IsEnabled = false;
            this.AutosaveInterval.IsEnabled = false;
            this.IntervalTextBox.Opacity = 0.5;
            this.AutosaveInterval.Opacity = 0.5;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateSettings();
            this.InvokeApplyChanges();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void AutosaveInterval_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.IntervalTextBox.Text = this.FormattedInterval;
        }

        private void AutosaveCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.EnableAutosaveControls();
        }

        private void AutosaveCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.DisableAutosaveControls();
        }
    }
}
