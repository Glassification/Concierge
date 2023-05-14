// <copyright file="ButtonTreeViewItem.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Journals;
    using Concierge.Common;
    using MaterialDesignThemes.Wpf;

    public sealed class ButtonTreeViewItem : TreeViewItem
    {
        public ButtonTreeViewItem(RoutedEventHandler clickEvent, Chapter? chapter)
        {
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("Display/Dictionaries/ButtonDictionary.xaml", UriKind.RelativeOrAbsolute),
            };

            var button = new ConciergeDesignButton()
            {
                Style = resourceDictionary["ConciergeDesignButtonStyle"] as Style,
                Content = new PackIcon()
                {
                    Kind = PackIconKind.PlusBox,
                },
                Foreground = ConciergeBrushes.Mint,
                Width = 30,
                Height = 30,
                Tag = chapter,
                ToolTip = $"Add New {(chapter is null ? "Chapter" : "Document")}",
            };
            button.Click += clickEvent;

            this.Header = button;
        }
    }
}
