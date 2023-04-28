// <copyright file="GlossaryTreeViewItem.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Tools.Glossary;
    using MaterialDesignThemes.Wpf;

    public sealed class GlossaryTreeViewItem : TreeViewItem
    {
        public GlossaryTreeViewItem(GlossaryEntry glossaryEntry)
        {
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("Display/Dictionaries/TreeViewDictionary.xaml", UriKind.RelativeOrAbsolute),
            };

            this.GlossaryEntry = glossaryEntry;
            this.Header = this.CreateHeader();
            this.Foreground = Brushes.White;
            this.IsExpanded = glossaryEntry.IsExpanded;
            this.Style = resourceDictionary["TreeViewItemFullWidthStyling"] as Style;

            this.MouseEnter += this.Item_MouseEnter;
            this.MouseLeave += this.Item_MouseLeave;
        }

        public GlossaryEntry GlossaryEntry { get; set; }

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
                Text = $" {this.GlossaryEntry.Name}",
                TextWrapping = TextWrapping.Wrap,
                FontSize = 15,
            };

            var packIcon = new PackIcon()
            {
                Kind = PackIconKind.BookOpen,
                Foreground = Brushes.SteelBlue,
                Width = 20,
                Height = 20,
            };

            grid.Children.Add(packIcon);
            grid.Children.Add(textBlock);

            Grid.SetColumn(packIcon, 0);
            Grid.SetColumn(textBlock, 1);

            return grid;
        }

        private void DeselectParent(Brush color)
        {
            var item = this.GetSelectedTreeViewItemParent();
            if (item is not null)
            {
                item.Foreground = color;
            }
        }

        private void Item_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Foreground = ConciergeBrushes.TreeItemHover;
            this.DeselectParent(Brushes.White);
        }

        private void Item_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Foreground = Brushes.White;
            this.DeselectParent(ConciergeBrushes.TreeItemHover);
        }
    }
}
