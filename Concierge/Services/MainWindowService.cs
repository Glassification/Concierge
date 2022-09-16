// <copyright file="MainWindowService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    using Concierge.Character;
    using Concierge.Character.Characteristics;
    using Concierge.Interfaces;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using MaterialDesignThemes.Wpf;

    public sealed class MainWindowService
    {
        public MainWindowService(RoutedEventHandler selectedEvent)
        {
            this.SelectedEvent = selectedEvent;
        }

        private RoutedEventHandler SelectedEvent { get; set; }

        public ListViewItem GenerateListViewItem(
            IConciergePage conciergePage,
            string pageName,
            PackIconKind packIconKind)
        {
            var packIcon = new PackIcon()
            {
                Kind = packIconKind,
                Foreground = Brushes.White,
                Height = 28,
                Width = 28,
                Margin = new Thickness(10),
            };
            var textBlock = new TextBlock()
            {
                Text = pageName,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(20, 10, 20, 10),
                FontSize = 12,
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
            listViewItem.Selected += this.SelectedEvent;
            listViewItem.MouseEnter += this.ListViewItem_MouseEnter;
            listViewItem.MouseLeave += this.ListViewItem_MouseLeave;

            return listViewItem;
        }

        public void GenerateCharacterStatusBar(StackPanel stackPanel, ConciergeCharacter character)
        {
            stackPanel.Children.Clear();

            if (character.Properties.CharacterIcon?.UseCustomImage ?? false)
            {
                stackPanel.Children.Add(GenerateCharacterIcon(character.Properties.CharacterIcon));
            }

            if (!character.Properties.Name.IsNullOrWhiteSpace())
            {
                stackPanel.Children.Add(GenerateTextBlock(character.Properties.Name));
            }

            if (!character.Properties.Race.IsNullOrWhiteSpace())
            {
                stackPanel.Children.Add(GenerateTextBlock(character.Properties.Race));
            }

            if (!character.Properties.Background.IsNullOrWhiteSpace())
            {
                stackPanel.Children.Add(GenerateTextBlock(character.Properties.Background));
            }

            if (!character.Properties.Alignment.IsNullOrWhiteSpace())
            {
                stackPanel.Children.Add(GenerateTextBlock(character.Properties.Alignment));
            }

            if (character.Properties.Level > 0)
            {
                stackPanel.Children.Add(GenerateTextBlock($"Level {character.Properties.Level}"));
            }

            if (!character.Properties.GetClasses.IsNullOrWhiteSpace())
            {
                stackPanel.Children.Add(GenerateTextBlock(character.Properties.GetClasses));
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

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void ListViewItem_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
