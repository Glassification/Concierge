// <copyright file="DocumentTreeViewItem.cs" company="Thomas Beckett">
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
    using Concierge.Utility.Extensions;
    using MaterialDesignThemes.Wpf;

    public class DocumentTreeViewItem : TreeViewItem
    {
        public DocumentTreeViewItem(Document document)
        {
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("Interfaces/Dictionary/TreeViewDictionary.xaml", UriKind.RelativeOrAbsolute),
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
                FontSize = 20,
            };

            var packIcon = new PackIcon()
            {
                Kind = PackIconKind.PaperOutline,
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
            this.Foreground = ConciergeColors.NoteTreeItemHover;
            this.DeselectParent(Brushes.White);
        }

        private void Item_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Foreground = Brushes.White;
            this.DeselectParent(ConciergeColors.NoteTreeItemHover);
        }
    }
}
