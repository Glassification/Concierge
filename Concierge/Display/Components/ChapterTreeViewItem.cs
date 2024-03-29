﻿// <copyright file="ChapterTreeViewItem.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Character.Journals;
    using Concierge.Common;
    using MaterialDesignThemes.Wpf;

    public sealed class ChapterTreeViewItem : TreeViewItem
    {
        private const int ElementSize = 23;

        public ChapterTreeViewItem(Chapter chapter)
        {
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("Display/Dictionaries/TreeViewDictionary.xaml", UriKind.RelativeOrAbsolute),
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
                FontSize = ElementSize,
            };

            var packIcon = new PackIcon()
            {
                Kind = PackIconKind.BookOpenVariant,
                Foreground = Brushes.SteelBlue,
                Width = ElementSize,
                Height = ElementSize,
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
                if (item is DocumentTreeViewItem documentItem && documentItem.Foreground == ConciergeBrushes.TreeItemHover)
                {
                    return true;
                }
            }

            return false;
        }

        private void Item_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            this.Foreground = this.IsHighlightedOnChild() ? Brushes.White : ConciergeBrushes.TreeItemHover;
        }

        private void Item_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            this.Foreground = Brushes.White;
        }
    }
}
