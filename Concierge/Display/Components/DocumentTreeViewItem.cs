// <copyright file="DocumentTreeViewItem.cs" company="Thomas Beckett">
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
    using Concierge.Common.Extensions;
    using MaterialDesignThemes.Wpf;

    public sealed class DocumentTreeViewItem : TreeViewItem
    {
        private const int ElementSize = 19;

        public DocumentTreeViewItem(Document document)
        {
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("Display/Dictionaries/TreeViewDictionary.xaml", UriKind.RelativeOrAbsolute),
            };

            this.Document = document;
            this.Header = this.CreateHeader();
            this.Foreground = Brushes.White;
            this.IsExpanded = document.IsExpanded;
            this.Style = resourceDictionary["TreeViewItemFullWidthStyling"] as Style;

            this.MouseEnter += this.Item_MouseEnter;
            this.MouseLeave += this.Item_MouseLeave;
        }

        public Document Document { get; set; }

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
                Text = $" {this.Document.Name}",
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.Wrap,
                FontSize = ElementSize,
            };

            var packIcon = new PackIcon()
            {
                Kind = PackIconKind.PaperOutline,
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
