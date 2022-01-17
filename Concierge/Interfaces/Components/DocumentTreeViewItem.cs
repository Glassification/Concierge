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
    using Concierge.Utility.Colors;
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
            this.Style = resourceDictionary["TreeViewItemStyling"] as Style;

            this.MouseEnter += this.Item_MouseEnter;
            this.MouseLeave += this.Item_MouseLeave;
        }

        public Document Document { get; set; }

        private StackPanel CreateHeader()
        {
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
            };

            var textBlock = new TextBlock()
            {
                Text = $" {this.Document.Name}",
                FontWeight = FontWeights.Normal,
                FontSize = 20,
            };

            var packIcon = new PackIcon()
            {
                Kind = PackIconKind.PaperOutline,
                Width = 20,
                Height = 20,
            };

            stackPanel.Children.Add(packIcon);
            stackPanel.Children.Add(textBlock);

            return stackPanel;
        }

        private void Item_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            this.Foreground = ConciergeColors.NoteTreeItemHover;
        }

        private void Item_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            this.Foreground = Brushes.White;
        }
    }
}
