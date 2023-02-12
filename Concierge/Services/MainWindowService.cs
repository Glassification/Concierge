// <copyright file="MainWindowService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Character.Characteristics;
    using Concierge.Display;
    using Concierge.Utility.Extensions;
    using MaterialDesignThemes.Wpf;

    public sealed class MainWindowService
    {
        public MainWindowService()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Code styling.")]
        public void GenerateCharacterStatusBar(StackPanel stackPanel, CharacterProperties properties)
        {
            stackPanel.Children.Clear();

            if (properties.CharacterIcon?.UseCustomImage ?? false)
            {
                stackPanel.Children.Add(GenerateCharacterIcon(properties.CharacterIcon));
            }

            if (!properties.Name.IsNullOrWhiteSpace())
            {
                stackPanel.Children.Add(GenerateTextBlock(properties.Name));
            }

            if (!properties.Race.Name.IsNullOrWhiteSpace())
            {
                stackPanel.Children.Add(GenerateTextBlock(properties.Race.ToString()));
            }

            if (!properties.Background.IsNullOrWhiteSpace())
            {
                stackPanel.Children.Add(GenerateTextBlock(properties.Background));
            }

            if (!properties.Alignment.IsNullOrWhiteSpace())
            {
                stackPanel.Children.Add(GenerateTextBlock(properties.Alignment));
            }

            if (properties.Level > 0)
            {
                stackPanel.Children.Add(GenerateTextBlock($"Level {properties.Level}"));
            }

            if (!properties.GetClasses.IsNullOrWhiteSpace())
            {
                stackPanel.Children.Add(GenerateTextBlock(properties.GetClasses));
            }
        }

        private static TextBlock GenerateTextBlock(string contents)
        {
            return new TextBlock()
            {
                FontSize = 20,
                Margin = new Thickness(25, 0, 25, 0),
                Foreground = Brushes.White,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = contents,
            };
        }

        private static Image GenerateCharacterIcon(CharacterImage characterImage)
        {
            return new Image()
            {
                Source = characterImage.ToImage(),
                Stretch = characterImage.Stretch,
                Margin = new Thickness(5, 5, 0, 5),
            };
        }
    }
}
