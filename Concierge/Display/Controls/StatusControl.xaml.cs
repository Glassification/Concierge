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

    /// <summary>
    /// Interaction logic for StatusControl.xaml.
    /// </summary>
    public partial class StatusControl : UserControl
    {
        private const int FontSizeLimit = 10;

        public StatusControl()
        {
            this.InitializeComponent();
        }

        public void DrawActiveFile(CcsFile ccsFile)
        {
            this.ActiveFileNameTextBlock.Text = ccsFile.FileName;
            this.ActiveFileNameTextBlock.ToolTip = ccsFile.AbsolutePath;
            this.SetTextSize(this.ActiveFileNameTextBlock, this.ActiveFileNameTextBlock.FontSize, this.ActualWidth * 0.15);
        }

        public void DrawInformation(string text)
        {
            this.AlertMessageTextBlock.Text = text;
            this.SetTextSize(this.AlertMessageTextBlock, this.AlertMessageTextBlock.FontSize, this.ActualWidth * 0.58);
        }

        public void DrawCurrentPage(string text)
        {
            this.CurrentPageNameTextBlock.Text = text;
            this.SetTextSize(this.CurrentPageNameTextBlock, this.CurrentPageNameTextBlock.FontSize, this.ActualWidth * 0.15);
        }

        public void DrawTime()
        {
            this.DateTimeTextBlock.Text = ConciergeDateTime.StatusMenuNow;
            this.DateTimeTextBlock.ToolTip = ConciergeDateTime.ToolTipNow;
            this.SetTextSize(this.DateTimeTextBlock, this.DateTimeTextBlock.FontSize, this.ActualWidth * 0.11);
        }

        public void DrawWifi()
        {
            var connected = AppSettingsManager.StartUp.EnableNetworkAccess && InternetUtility.IsConnected;

            this.WifiIcon.Kind = connected ? PackIconKind.Wifi : PackIconKind.WifiOff;
            this.WifiIcon.ToolTip = connected ? "Connected" : "Not Connected";
        }

        private TextBlock SetTextSize(TextBlock textBlock, double fontSize, double columnWidth)
        {
            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            if (fontSize <= FontSizeLimit)
            {
                textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
                return textBlock;
            }
            else if (columnWidth < textBlock.DesiredSize.Width)
            {
                return this.SetTextSize(textBlock, fontSize - 1, columnWidth);
            }
            else
            {
                return textBlock;
            }
        }
    }
}
