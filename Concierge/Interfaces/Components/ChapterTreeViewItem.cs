// <copyright file="ChapterTreeViewItem.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Components
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Character.Notes;
    using MaterialDesignThemes.Wpf;

    public class ChapterTreeViewItem : TreeViewItem
    {
        public ChapterTreeViewItem(Chapter chapter)
        {
            this.Chapter = chapter;
            this.Header = this.CreateHeader();
            this.Foreground = Brushes.White;
            this.IsExpanded = chapter.IsExpanded;
        }

        public Chapter Chapter { get; set; }

        private StackPanel CreateHeader()
        {
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
            };

            var textBlock = new TextBlock()
            {
                Text = $" {this.Chapter.Name}",
                FontWeight = FontWeights.Bold,
                FontSize = 25,
            };

            var packIcon = new PackIcon()
            {
                Kind = PackIconKind.BookOpenVariant,
                Width = 25,
                Height = 25,
            };

            stackPanel.Children.Add(packIcon);
            stackPanel.Children.Add(textBlock);

            return stackPanel;
        }
    }
}
