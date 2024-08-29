// <copyright file="ConciergeTextBlock.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Common;

    public sealed class ConciergeTextBlock : TextBlock
    {
        public ConciergeTextBlock()
            : base()
        {
            this.Foreground = Brushes.White;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.TextWrapping = TextWrapping.Wrap;
            this.Margin = new Thickness(-1);

            this.OriginalBackgroundColor = null;
            this.OriginalForegroundColor = null;

            var scaling = ResolutionScaling.DpiFactor;
            this.LayoutTransform = new ScaleTransform(scaling, scaling, 0.5, 0.5);
        }

        public bool IsHighlighted => this.OriginalBackgroundColor is not null || this.OriginalForegroundColor is not null;

        private Brush? OriginalBackgroundColor { get; set; }

        private Brush? OriginalForegroundColor { get; set; }

        public void Highlight()
        {
            if (this.IsHighlighted)
            {
                return;
            }

            this.OriginalBackgroundColor = this.Background;
            this.OriginalForegroundColor = this.Foreground;

            this.Foreground = ConciergeBrushes.BorderHighlight;
            this.Background = ConciergeBrushes.Highlight;
        }

        public void ResetHighlight()
        {
            if (!this.IsHighlighted)
            {
                return;
            }

            this.Background = this.OriginalBackgroundColor;
            this.Foreground = this.OriginalForegroundColor;

            this.OriginalBackgroundColor = null;
            this.OriginalForegroundColor = null;
        }
    }
}
