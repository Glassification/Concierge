// <copyright file="HeaderControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Character.Dispositions;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
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

        public void Draw(Disposition disposition)
        {
            this.HeaderGrid.Children.Clear();

            var text = $"{disposition.Name}  ~  {GetLevelRace(disposition)}{StringUtility.CreateCharacters(" ", 10)}{GetAlignmentAndBackground(disposition)}";
            var nameBlock = CreateTextBlock(20, CleanNameBlock(text), this.HeaderGrid.ActualWidth * 0.60);
            this.HeaderGrid.Children.Add(nameBlock);
            Grid.SetColumn(nameBlock, 0);

            var classBlock = CreateTextBlock(20, GetClasses(disposition), this.HeaderGrid.ActualWidth * 0.40);
            this.HeaderGrid.Children.Add(classBlock);
            Grid.SetColumn(classBlock, 1);
        }

        private static string CleanNameBlock(string text)
        {
            if (text.Strip("~").IsNullOrWhiteSpace())
            {
                return string.Empty;
            }

            if (text.TrimStart().First() == '~' || text.TrimEnd().Last() == '~')
            {
                text = text.Strip("~");
            }

            return text;
        }

        private static TextBlock CreateTextBlock(double fontSize, string text, double columnWidth)
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
                return CreateTextBlock(fontSize - 1, text, columnWidth);
            }
            else
            {
                return textBlock;
            }
        }

        private static string GetLevelRace(Disposition disposition)
        {
            return disposition.Level > 0 || !disposition.Race.IsNullOrWhiteSpace() ? $"Lvl {disposition.Level}  {disposition.Race}" : string.Empty;
        }

        private static string GetAlignmentAndBackground(Disposition disposition)
        {
            return $"{disposition.Alignment}   {disposition.Background}".Trim();
        }

        private static string GetClasses(Disposition disposition)
        {
            var builder = new StringBuilder();
            if (!disposition.Class1.ToString().IsNullOrWhiteSpace())
            {
                builder.Append($"{disposition.Class1}");
            }

            if (!disposition.Class2.ToString().IsNullOrWhiteSpace())
            {
                builder.Append($", {disposition.Class2}");
            }

            if (!disposition.Class3.ToString().IsNullOrWhiteSpace())
            {
                builder.Append($", {disposition.Class3}");
            }

            return builder.ToString();
        }
    }
}
