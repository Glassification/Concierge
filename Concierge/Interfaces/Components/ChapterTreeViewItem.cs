// <copyright file="ChapterTreeViewItem.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Components
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Character.Notes;
    using Concierge.Utility;
    using MaterialDesignThemes.Wpf;

    public sealed class ChapterTreeViewItem : TreeViewItem
    {
        public ChapterTreeViewItem(Chapter chapter)
        {
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("Interfaces/Dictionary/TreeViewDictionary.xaml", UriKind.RelativeOrAbsolute),
            };

            this.Chapter = chapter;
            this.Header = this.CreateHeader();
            this.Foreground = Brushes.White;
            this.IsExpanded = chapter.IsExpanded;
            this.Style = resourceDictionary["TreeViewItemFullWidthStyling"] as Style;

            this.MouseEnter += this.Item_MouseEnter;
            this.MouseLeave += this.Item_MouseLeave;
        }

        public Chapter Chapter { get; set; }

        private Grid CreateHeader()
        {
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(30),
            });
            grid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(1, GridUnitType.Star),
            });

            var textBlock = new TextBlock()
            {
                Text = $" {this.Chapter.Name}",
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 25,
            };

            var packIcon = new PackIcon()
            {
                Kind = PackIconKind.BookOpenVariant,
                Width = 25,
                Height = 25,
            };

            grid.Children.Add(packIcon);
            grid.Children.Add(textBlock);

            Grid.SetColumn(packIcon, 0);
            Grid.SetColumn(textBlock, 1);

            return grid;
        }

        private bool IsHighlightedOnChild()
        {
            foreach (var item in this.Items)
            {
                if (item is DocumentTreeViewItem documentItem && documentItem.Foreground == ConciergeColors.NoteTreeItemHover)
                {
                    return true;
                }
            }

            return false;
        }

        private void Item_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            this.Foreground = this.IsHighlightedOnChild() ? Brushes.White : ConciergeColors.NoteTreeItemHover;
        }

        private void Item_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            this.Foreground = Brushes.White;
        }
    }
}
