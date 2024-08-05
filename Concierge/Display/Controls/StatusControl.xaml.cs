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
            var volume = AppSettingsManager.UserSettings.Volume;

            this.VolumeIcon.Kind = GetSoundIcon(isMuted, volume);
            this.VolumeIcon.ToolTip = isMuted ? "Sound Off" : $"Sound On ({volume}%)";
        }

        private static PackIconKind GetSoundIcon(bool isMuted, int volume)
        {
            if (isMuted)
            {
                return PackIconKind.VolumeMute;
            }

            return volume switch
            {
                int n when n <= 33 => PackIconKind.VolumeLow,
                int n when n > 33 && n <= 66 => PackIconKind.VolumeMedium,
                int n when n > 66 => PackIconKind.VolumeHigh,
                _ => PackIconKind.VolumeMute,
            };
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
