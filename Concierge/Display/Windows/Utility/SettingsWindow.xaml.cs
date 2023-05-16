// <copyright file="SettingsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Utility
{
    using System;
    using System.Linq;
    using System.Windows;

    using Concierge.Commands;
    using Concierge.Common.Enums;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Configuration;
    using Concierge.Configuration.Dtos;
    using Concierge.Configuration.Objects;
    using Concierge.Display;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Persistence;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for SettingsWindow.xaml.
    /// </summary>
    public partial class SettingsWindow : ConciergeWindow
    {
        private readonly FileAccessService fileAccessService = new ();

        public SettingsWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.UnitOfMeasurementComboBox.ItemsSource = Enum.GetValues(typeof(UnitTypes)).Cast<UnitTypes>();
        }

        public override string HeaderText => "Settings";

        public override string WindowName => nameof(SettingsWindow);

        private string FormattedInterval => $"Autosave Interval:  {Defaults.AutosaveIntervals[(int)this.AutosaveInterval.Value]} minute{((int)this.AutosaveInterval.Value > 0 ? "s" : string.Empty)}";

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
            this.DefaultOpenCheckBox.UpdatingValue();
            this.DefaultSaveCheckBox.UpdatingValue();

            this.AutosaveCheckBox.IsChecked = AppSettingsManager.UserSettings.Autosaving.Enabled;
            this.AutosaveInterval.Value = AppSettingsManager.UserSettings.Autosaving.Interval;
            this.CoinWeightCheckBox.IsChecked = AppSettingsManager.UserSettings.UseCoinWeight;
            this.EncumbranceCheckBox.IsChecked = AppSettingsManager.UserSettings.UseEncumbrance;
            this.IntervalTextBox.Text = this.FormattedInterval;
            this.MuteCheckBox.IsChecked = AppSettingsManager.UserSettings.MuteSounds;
            this.CheckVersionCheckBox.IsChecked = AppSettingsManager.UserSettings.CheckVersion;
            this.UnitOfMeasurementComboBox.Text = AppSettingsManager.UserSettings.UnitOfMeasurement.ToString();
            this.DefaultSaveCheckBox.IsChecked = AppSettingsManager.UserSettings.DefaultFolder.UseSaveFolder;
            this.DefaultOpenCheckBox.IsChecked = AppSettingsManager.UserSettings.DefaultFolder.UseOpenFolder;
            this.OpenTextBox.Text = AppSettingsManager.UserSettings.DefaultFolder.OpenFolder;
            this.SaveTextBox.Text = AppSettingsManager.UserSettings.DefaultFolder.SaveFolder;

            DisplayUtility.SetControlEnableState(this.AutosaveInterval, AppSettingsManager.UserSettings.Autosaving.Enabled);
            DisplayUtility.SetControlEnableState(this.IntervalTextBox, AppSettingsManager.UserSettings.Autosaving.Enabled);
            DisplayUtility.SetControlEnableState(this.SaveFolderButton, AppSettingsManager.UserSettings.DefaultFolder.UseSaveFolder);
            DisplayUtility.SetControlEnableState(this.SaveTextBoxBackground, AppSettingsManager.UserSettings.DefaultFolder.UseSaveFolder);
            DisplayUtility.SetControlEnableState(this.OpenFolderButton, AppSettingsManager.UserSettings.DefaultFolder.UseOpenFolder);
            DisplayUtility.SetControlEnableState(this.OpenTextBoxBackground, AppSettingsManager.UserSettings.DefaultFolder.UseOpenFolder);

            this.AutosaveCheckBox.UpdatedValue();
            this.CoinWeightCheckBox.UpdatedValue();
            this.EncumbranceCheckBox.UpdatedValue();
            this.MuteCheckBox.UpdatedValue();
            this.CheckVersionCheckBox.UpdatedValue();
            this.DefaultOpenCheckBox.UpdatedValue();
            this.DefaultSaveCheckBox.UpdatedValue();
        }

        private bool UpdateSettings()
        {
            if (Program.CcsFile.IsFileSaved(this.AutosaveCheckBox.IsChecked))
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
                Autosaving = new Autosave()
                {
                    Enabled = this.AutosaveCheckBox.IsChecked ?? false,
                    Interval = (int)this.AutosaveInterval.Value,
                },
                CheckVersion = this.CheckVersionCheckBox.IsChecked ?? false,
                DefaultFolder = new DefaultFolders()
                {
                    OpenFolder = this.OpenTextBox.Text,
                    SaveFolder = this.SaveTextBox.Text,
                    UseSaveFolder = this.DefaultSaveCheckBox.IsChecked ?? false,
                    UseOpenFolder = this.DefaultOpenCheckBox.IsChecked ?? false,
                },
                MuteSounds = this.MuteCheckBox.IsChecked ?? false,
                UseCoinWeight = this.CoinWeightCheckBox.IsChecked ?? false,
                UseEncumbrance = this.EncumbranceCheckBox.IsChecked ?? false,
                UnitOfMeasurement = !Enum.TryParse(this.UnitOfMeasurementComboBox.Text, out UnitTypes value) ? default : value,
            };

            Program.UndoRedoService.AddCommand(new UpdateSettingsCommand(oldSettings, conciergeSettings));
            AppSettingsManager.UpdateSettings(conciergeSettings, Program.IsDebug);

            return true;
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
            DisplayUtility.SetControlEnableState(this.AutosaveInterval, true);
            DisplayUtility.SetControlEnableState(this.IntervalTextBox, true);
        }

        private void AutosaveCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SetControlEnableState(this.AutosaveInterval, false);
            DisplayUtility.SetControlEnableState(this.IntervalTextBox, false);
        }

        private void SaveFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var folder = this.fileAccessService.OpenFolder();
            this.Activate();
            if (!folder.IsNullOrWhiteSpace())
            {
                this.SaveTextBox.Text = folder;
            }
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var folder = this.fileAccessService.OpenFolder();
            this.Activate();
            if (!folder.IsNullOrWhiteSpace())
            {
                this.OpenTextBox.Text = folder;
            }
        }

        private void DefaultSaveCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SetControlEnableState(this.SaveFolderButton, true);
            DisplayUtility.SetControlEnableState(this.SaveTextBoxBackground, true);
        }

        private void DefaultSaveCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SetControlEnableState(this.SaveFolderButton, false);
            DisplayUtility.SetControlEnableState(this.SaveTextBoxBackground, false);
        }

        private void DefaultOpenCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SetControlEnableState(this.OpenFolderButton, true);
            DisplayUtility.SetControlEnableState(this.OpenTextBoxBackground, true);
        }

        private void DefaultOpenCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SetControlEnableState(this.OpenFolderButton, false);
            DisplayUtility.SetControlEnableState(this.OpenTextBoxBackground, false);
        }
    }
}
