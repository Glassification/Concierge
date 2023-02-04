// <copyright file="ConciergeLabel.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public sealed class ConciergeLabel : TextBlock
    {
        public ConciergeLabel()
            : base()
        {
            this.Foreground = Brushes.White;
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.TextWrapping = TextWrapping.Wrap;
            this.FontSize = 20;
            this.Margin = new Thickness(10, 0, 0, 5);
        }
    }
}
