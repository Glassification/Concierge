// <copyright file="MainWindowService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Character;
    using Concierge.Character.Characteristics;
    using Concierge.Interfaces;
    using Concierge.Utility.Extensions;
    using MaterialDesignThemes.Wpf;

    public class MainWindowService
    {
        public MainWindowService()
        {
        }

        public ListViewItem CreateListViewItem(
            IConciergePage conciergePage,
            string pageName,
            PackIconKind packIconKind,
            RoutedEventHandler selectedEvent)
        {
            var packIcon = new PackIcon()
            {
                Kind = packIconKind,
                Height = 25,
                Width = 25,
                Margin = new Thickness(10),
            };
            var textBlock = new TextBlock()
            {
                Text = pageName,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(20, 10, 20, 10),
            };
            var stackPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
            };
            var listViewItem = new ListViewItem()
            {
                Height = 60,
                Tag = conciergePage,
            };

            stackPanel.Children.Add(packIcon);
            stackPanel.Children.Add(textBlock);

            listViewItem.Content = stackPanel;
            listViewItem.Selected += selectedEvent;

            return listViewItem;
        }

        public void GenerateCharacterStatusBar(StackPanel stackPanel, ConciergeCharacter character)
        {
            stackPanel.Children.Clear();

            if (character.CharacterIcon?.UseCustomImage ?? false)
            {
                stackPanel.Children.Add(GenerateCharacterIcon(character.CharacterIcon));
            }

            if (!character.Details.Name.IsNullOrWhiteSpace())
            {
                stackPanel.Children.Add(GenerateTextBlock(character.Details.Name));
            }

            if (!character.Details.Race.IsNullOrWhiteSpace())
            {
                stackPanel.Children.Add(GenerateTextBlock(character.Details.Race));
            }

            if (!character.Details.Background.IsNullOrWhiteSpace())
            {
                stackPanel.Children.Add(GenerateTextBlock(character.Details.Background));
            }

            if (!character.Details.Alignment.IsNullOrWhiteSpace())
            {
                stackPanel.Children.Add(GenerateTextBlock(character.Details.Alignment));
            }

            if (character.Level > 0)
            {
                stackPanel.Children.Add(GenerateTextBlock($"Level {character.Level}"));
            }

            if (!character.GetClasses.IsNullOrWhiteSpace())
            {
                stackPanel.Children.Add(GenerateTextBlock(character.GetClasses));
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
