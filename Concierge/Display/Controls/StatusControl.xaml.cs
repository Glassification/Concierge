// <copyright file="StatusControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Common;
    using Concierge.Common.Utilities;
    using Concierge.Configuration;
    using MaterialDesignThemes.Wpf;

    using Constants = Concierge.Common.Constants;

    /// <summary>
    /// Interaction logic for StatusControl.xaml.
    /// </summary>
    public partial class StatusControl : UserControl
    {
        public StatusControl()
        {
            this.InitializeComponent();
        }

        public void ClearActiveFile()
        {
            this.ActiveFileNameTextBlock.Text = string.Empty;
            this.ActiveFileNameTextBlock.ToolTip = string.Empty;
        }

        public void DrawActiveFile(CcsFile ccsFile)
        {
            this.ActiveFileNameTextBlock.Text = ccsFile.FileName;
            this.ActiveFileNameTextBlock.ToolTip = ccsFile.AbsolutePath;
            SetTextSize(this.ActiveFileNameTextBlock, this.ActiveFileNameTextBlock.FontSize, this.ActualWidth * 0.20);
        }

        public void DrawInformation(string text)
        {
            this.AlertMessageTextBlock.Text = text;
            SetTextSize(this.AlertMessageTextBlock, this.AlertMessageTextBlock.FontSize, this.ActualWidth * 0.65);
        }

        public void DrawTime()
        {
            this.DateTimeTextBlock.Text = ConciergeDateTime.StatusMenuNow;
            this.DateTimeTextBlock.ToolTip = ConciergeDateTime.ToolTipNow;
            SetTextSize(this.DateTimeTextBlock, this.DateTimeTextBlock.FontSize, this.ActualWidth * 0.10);
        }

        public void DrawWifi()
        {
            if (!AppSettingsManager.StartUp.EnableNetworkAccess)
            {
                this.WifiIcon.Visibility = Visibility.Collapsed;
            }

            var internet = SystemUtility.GetInternetStatus();

            this.WifiIcon.Visibility = Visibility.Visible;
            this.WifiIcon.Kind = internet.Icon;
            this.WifiIcon.ToolTip = internet.ToString();
        }

        public void DrawBattery()
        {
            var batteryStatus = SystemUtility.GetBatteryStatus();

            this.BatteryIcon.Kind = batteryStatus.Icon;
            this.BatteryIcon.ToolTip = batteryStatus.ToString();
        }

        public void DrawVolume()
        {
            var isMuted = AppSettingsManager.UserSettings.MuteSounds;

            this.VolumeIcon.Kind = isMuted ? PackIconKind.VolumeMute : PackIconKind.VolumeHigh;
            this.VolumeIcon.ToolTip = isMuted ? "Sound Off" : "Sound On";
        }

        private static TextBlock SetTextSize(TextBlock textBlock, double fontSize, double columnWidth)
        {
            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            if (fontSize <= Constants.FontSizeLimit)
            {
                textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
                return textBlock;
            }
            else if (columnWidth < textBlock.DesiredSize.Width)
            {
                return SetTextSize(textBlock, fontSize - 1, columnWidth);
            }
            else
            {
                return textBlock;
            }
        }
    }
}
