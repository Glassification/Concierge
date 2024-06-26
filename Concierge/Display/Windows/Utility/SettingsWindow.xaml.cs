﻿// <copyright file="SettingsWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Utility
{
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;

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

            this.UnitOfMeasurementComboBox.ItemsSource = ComboBoxGenerator.UnitTypesComboBox();
            this.HeaderAlignmentComboBox.ItemsSource = ComboBoxGenerator.HorizontalAlignmentComboBox();
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.AutosaveCheckBox);
            this.SetMouseOverEvents(this.AutosaveInterval);
            this.SetMouseOverEvents(this.CoinWeightCheckBox);
            this.SetMouseOverEvents(this.EncumbranceCheckBox);
            this.SetMouseOverEvents(this.IntervalTextBox);
            this.SetMouseOverEvents(this.MuteCheckBox);
            this.SetMouseOverEvents(this.CheckVersionCheckBox);
            this.SetMouseOverEvents(this.UnitOfMeasurementComboBox);
            this.SetMouseOverEvents(this.HeaderAlignmentComboBox);
            this.SetMouseOverEvents(this.HealingThreshold);
            this.SetMouseOverEvents(this.VolumeSlider);
            this.SetMouseOverEvents(this.DefaultSaveCheckBox);
            this.SetMouseOverEvents(this.DefaultOpenCheckBox);
            this.SetMouseOverEvents(this.OpenTextBox, this.OpenTextBackground);
            this.SetMouseOverEvents(this.SaveTextBox, this.SaveTextBackground);
        }

        public override string HeaderText => "Settings";

        public override string WindowName => nameof(SettingsWindow);

        private string FormattedInterval
        {
            get
            {
                var interval = Defaults.AutosaveIntervals[(int)this.AutosaveInterval.Value];
                return $"Autosave Interval:  {interval} {"minute".Pluralize("s", interval)}";
            }
        }

        private string FormattedThreshold => $"Short Rest Healing: {this.HealingThreshold.Value}%";

        private string FormattedVolume => $"Volume: {this.VolumeSlider.Value}%";

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
            this.HealingThreshold.Value = AppSettingsManager.UserSettings.HealingThreshold;
            this.HealingThresholdLabel.Text = this.FormattedThreshold;
            this.MuteCheckBox.IsChecked = AppSettingsManager.UserSettings.MuteSounds;
            this.CheckVersionCheckBox.IsChecked = AppSettingsManager.UserSettings.CheckVersion;
            this.UnitOfMeasurementComboBox.Text = AppSettingsManager.UserSettings.UnitOfMeasurement.ToString();
            this.HeaderAlignmentComboBox.Text = AppSettingsManager.UserSettings.HeaderAlignment.ToString();
            this.VolumeSlider.Value = AppSettingsManager.UserSettings.Volume;
            this.VolumeLabel.Text = this.FormattedVolume;
            this.DefaultSaveCheckBox.IsChecked = AppSettingsManager.UserSettings.DefaultFolder.UseSaveFolder;
            this.DefaultOpenCheckBox.IsChecked = AppSettingsManager.UserSettings.DefaultFolder.UseOpenFolder;
            this.OpenTextBox.Text = AppSettingsManager.UserSettings.DefaultFolder.OpenFolder;
            this.SaveTextBox.Text = AppSettingsManager.UserSettings.DefaultFolder.SaveFolder;

            DisplayUtility.SetControlEnableState(this.AutosaveInterval, AppSettingsManager.UserSettings.Autosaving.Enabled);
            DisplayUtility.SetControlEnableState(this.IntervalTextBox, AppSettingsManager.UserSettings.Autosaving.Enabled);
            DisplayUtility.SetControlEnableState(this.SaveFolderButton, AppSettingsManager.UserSettings.DefaultFolder.UseSaveFolder);
            DisplayUtility.SetControlEnableState(this.SaveTextBackground, AppSettingsManager.UserSettings.DefaultFolder.UseSaveFolder);
            DisplayUtility.SetControlEnableState(this.OpenFolderButton, AppSettingsManager.UserSettings.DefaultFolder.UseOpenFolder);
            DisplayUtility.SetControlEnableState(this.OpenTextBackground, AppSettingsManager.UserSettings.DefaultFolder.UseOpenFolder);
            DisplayUtility.SetControlEnableState(this.VolumeLabel, !AppSettingsManager.UserSettings.MuteSounds);
            DisplayUtility.SetControlEnableState(this.VolumeSlider, !AppSettingsManager.UserSettings.MuteSounds);

            this.OpenWarningVisibility();
            this.SaveWarningVisibility();

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
                    ConciergeButtons.Ok,
                    ConciergeIcons.Alert);

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
                    OpenFolder = Path.GetDirectoryName(this.OpenTextBox.Text) ?? string.Empty,
                    SaveFolder = Path.GetDirectoryName(this.SaveTextBox.Text) ?? string.Empty,
                    UseSaveFolder = this.DefaultSaveCheckBox.IsChecked ?? false,
                    UseOpenFolder = this.DefaultOpenCheckBox.IsChecked ?? false,
                },
                HeaderAlignment = this.HeaderAlignmentComboBox.Text.TryToEnum<HorizontalAlignment>(),
                HealingThreshold = (int)this.HealingThreshold.Value,
                MuteSounds = this.MuteCheckBox.IsChecked ?? false,
                UseCoinWeight = this.CoinWeightCheckBox.IsChecked ?? false,
                UseEncumbrance = this.EncumbranceCheckBox.IsChecked ?? false,
                UnitOfMeasurement = this.UnitOfMeasurementComboBox.Text.TryToEnum<UnitTypes>(),
                Volume = (int)this.VolumeSlider.Value,
            };

            Program.UndoRedoService.AddCommand(new UpdateSettingsCommand(oldSettings, conciergeSettings));
            AppSettingsManager.UpdateSettings(conciergeSettings, Program.IsDebug);

            return true;
        }

        private void SaveWarningVisibility()
        {
            this.SaveFolderWarning.Visibility =
                this.SaveTextBox.Text.IsNullOrWhiteSpace() ?
                Visibility.Collapsed :
                Directory.Exists(this.SaveTextBox.Text) ?
                    Visibility.Collapsed :
                    Visibility.Visible;
        }

        private void OpenWarningVisibility()
        {
            this.OpenFolderWarning.Visibility =
                this.OpenTextBox.Text.IsNullOrWhiteSpace() ?
                Visibility.Collapsed :
                Directory.Exists(this.OpenTextBox.Text) ?
                    Visibility.Collapsed :
                    Visibility.Visible;
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

            SoundService.SetVolume();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();

            SoundService.SetVolume();
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
            if (!folder.IsNullOrWhiteSpace())
            {
                this.SaveTextBox.Text = folder;
            }
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var folder = this.fileAccessService.OpenFolder();
            if (!folder.IsNullOrWhiteSpace())
            {
                this.OpenTextBox.Text = folder;
            }
        }

        private void DefaultSaveCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SetControlEnableState(this.SaveFolderButton, true);
            DisplayUtility.SetControlEnableState(this.SaveTextBackground, true);
            this.SaveWarningVisibility();
        }

        private void DefaultSaveCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SetControlEnableState(this.SaveFolderButton, false);
            DisplayUtility.SetControlEnableState(this.SaveTextBackground, false);
            this.SaveFolderWarning.Visibility = Visibility.Collapsed;
        }

        private void DefaultOpenCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SetControlEnableState(this.OpenFolderButton, true);
            DisplayUtility.SetControlEnableState(this.OpenTextBackground, true);
            this.OpenWarningVisibility();
        }

        private void DefaultOpenCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SetControlEnableState(this.OpenFolderButton, false);
            DisplayUtility.SetControlEnableState(this.OpenTextBackground, false);
            this.OpenFolderWarning.Visibility = Visibility.Collapsed;
        }

        private void SaveTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.SaveWarningVisibility();
        }

        private void OpenTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.OpenWarningVisibility();
        }

        private void HealingThreshold_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.HealingThresholdLabel.Text = this.FormattedThreshold;
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.VolumeLabel.Text = this.FormattedVolume;
        }

        private void MuteCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SetControlEnableState(this.VolumeLabel, false);
            DisplayUtility.SetControlEnableState(this.VolumeSlider, false);
        }

        private void MuteCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DisplayUtility.SetControlEnableState(this.VolumeLabel, true);
            DisplayUtility.SetControlEnableState(this.VolumeSlider, true);
        }
    }
}
