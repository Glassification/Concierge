// <copyright file="SettingsWindow.xaml.cs" company="Thomas Beckett">
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
    using Concierge.Configuration;
    using Concierge.Configuration.Dtos;
    using Concierge.Configuration.Objects;
    using Concierge.Display;
    using Concierge.Display.Components;
    using Concierge.Services;
    using MaterialDesignThemes.Wpf;

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
                return $"Interval:  {interval} {"minute".Pluralize("s", interval)}";
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

        private static void SetWarning(PackIcon packIcon, ConciergeTextBox path)
        {
            packIcon.Visibility =
                Directory.Exists(path.Text) ?
                    Visibility.Collapsed :
                    Visibility.Visible;
        }

        private void FillFields()
        {
            var userSettings = AppSettingsManager.UserSettings;

            this.AutosaveCheckBox.UpdatingValue();
            this.CoinWeightCheckBox.UpdatingValue();
            this.EncumbranceCheckBox.UpdatingValue();
            this.MuteCheckBox.UpdatingValue();
            this.CheckVersionCheckBox.UpdatingValue();
            this.DefaultOpenCheckBox.UpdatingValue();
            this.DefaultSaveCheckBox.UpdatingValue();

            this.AutosaveCheckBox.IsChecked = userSettings.Autosaving.Enabled;
            this.AutosaveInterval.Value = userSettings.Autosaving.Interval;
            this.CoinWeightCheckBox.IsChecked = userSettings.UseCoinWeight;
            this.EncumbranceCheckBox.IsChecked = userSettings.UseEncumbrance;
            this.IntervalTextBox.Text = this.FormattedInterval;
            this.HealingThreshold.Value = userSettings.HealingThreshold;
            this.HealingThresholdLabel.Text = this.FormattedThreshold;
            this.MuteCheckBox.IsChecked = userSettings.MuteSounds;
            this.CheckVersionCheckBox.IsChecked = userSettings.CheckVersion;
            this.UnitOfMeasurementComboBox.Text = userSettings.UnitOfMeasurement.ToString();
            this.HeaderAlignmentComboBox.Text = userSettings.HeaderAlignment.ToString();
            this.VolumeSlider.Value = userSettings.Volume;
            this.VolumeLabel.Text = this.FormattedVolume;
            this.DefaultSaveCheckBox.IsChecked = userSettings.DefaultFolder.UseSaveFolder;
            this.DefaultOpenCheckBox.IsChecked = userSettings.DefaultFolder.UseOpenFolder;
            this.OpenTextBox.Text = userSettings.DefaultFolder.OpenFolder;
            this.SaveTextBox.Text = userSettings.DefaultFolder.SaveFolder;

            this.AutosaveInterval.SetEnableState(userSettings.Autosaving.Enabled);
            this.IntervalTextBox.SetEnableState(userSettings.Autosaving.Enabled);
            this.SaveFolderButton.SetEnableState(userSettings.DefaultFolder.UseSaveFolder);
            this.SaveTextBackground.SetEnableState(userSettings.DefaultFolder.UseSaveFolder);
            this.OpenFolderButton.SetEnableState(userSettings.DefaultFolder.UseOpenFolder);
            this.OpenTextBackground.SetEnableState(userSettings.DefaultFolder.UseOpenFolder);
            this.VolumeLabel.SetEnableState(!userSettings.MuteSounds);
            this.VolumeSlider.SetEnableState(!userSettings.MuteSounds);

            SetWarning(this.OpenFolderWarning, this.OpenTextBox);
            SetWarning(this.SaveFolderWarning, this.SaveTextBox);

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
                ConciergeMessageBox.ShowWarning("You must save this sheet before enabling auto-save.");

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
            this.AutosaveInterval.SetEnableState(true);
            this.IntervalTextBox.SetEnableState(true);
        }

        private void AutosaveCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.AutosaveInterval.SetEnableState(false);
            this.IntervalTextBox.SetEnableState(false);
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
            this.SaveFolderButton.SetEnableState(true);
            this.SaveTextBackground.SetEnableState(true);
            SetWarning(this.SaveFolderWarning, this.SaveTextBox);
        }

        private void DefaultSaveCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.SaveFolderButton.SetEnableState(false);
            this.SaveTextBackground.SetEnableState(false);
            this.SaveFolderWarning.Visibility = Visibility.Collapsed;
        }

        private void DefaultOpenCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.OpenFolderButton.SetEnableState(true);
            this.OpenTextBackground.SetEnableState(true);
            SetWarning(this.OpenFolderWarning, this.OpenTextBox);
        }

        private void DefaultOpenCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.OpenFolderButton.SetEnableState(false);
            this.OpenTextBackground.SetEnableState(false);
            this.OpenFolderWarning.Visibility = Visibility.Collapsed;
        }

        private void SaveTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetWarning(this.SaveFolderWarning, this.SaveTextBox);
        }

        private void OpenTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetWarning(this.OpenFolderWarning, this.OpenTextBox);
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
            this.VolumeLabel.SetEnableState(false);
            this.VolumeSlider.SetEnableState(false);
        }

        private void MuteCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.VolumeLabel.SetEnableState(true);
            this.VolumeSlider.SetEnableState(true);
        }
    }
}
