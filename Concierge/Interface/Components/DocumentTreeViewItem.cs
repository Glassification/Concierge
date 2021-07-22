// <copyright file="DocumentTreeViewItem.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.Components
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Characters.Notes;
    using MaterialDesignThemes.Wpf;

    public class DocumentTreeViewItem : TreeViewItem
    {
        public DocumentTreeViewItem(Document document)
        {
            this.Document = document;
            this.Header = this.CreateHeader();
            this.Foreground = Brushes.White;
            this.IsExpanded = document.IsExpanded;
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
    }
}
