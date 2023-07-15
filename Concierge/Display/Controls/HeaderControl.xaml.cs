// <copyright file="HeaderControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Configuration;

    /// <summary>
    /// Interaction logic for HeaderControl.xaml.
    /// </summary>
    public partial class HeaderControl : UserControl
    {
        public HeaderControl()
        {
            this.InitializeComponent();
        }

        public void Draw(params string[] info)
        {
            var cleanedInfo = info.RemoveEmpty();
            var width = 100.0 / cleanedInfo.Length;
            this.HeaderGrid.Children.Clear();
            this.HeaderGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < cleanedInfo.Length; i++)
            {
                this.HeaderGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(width, GridUnitType.Star),
                });

                var textBlock = this.CreateTextBlock(20, cleanedInfo[i], this.HeaderGrid.ActualWidth / cleanedInfo.Length);

                this.HeaderGrid.Children.Add(textBlock);
                Grid.SetColumn(textBlock, i);
            }
        }

        private TextBlock CreateTextBlock(double fontSize, string text, double columnWidth)
        {
            var textBlock = new TextBlock()
            {
                Text = text,
                TextWrapping = TextWrapping.NoWrap,
                Margin = new Thickness(5, 0, 5, 0),
                Foreground = Brushes.White,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = AppSettingsManager.UserSettings.HeaderAlignment,
                FontSize = fontSize,
            };

            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            if (fontSize <= Constants.FontSizeLimit)
            {
                textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
                return textBlock;
            }
            else if (columnWidth < textBlock.DesiredSize.Width)
            {
                return this.CreateTextBlock(fontSize - 1, text, columnWidth);
            }
            else
            {
                return textBlock;
            }
        }
    }
}
