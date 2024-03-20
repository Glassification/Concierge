// <copyright file="ConciergeDesignToggleButton.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Common;
    using Concierge.Services;

    public class ConciergeDesignToggleButton : ToggleButton
    {
        private SolidColorBrush? originalForeground;

        public ConciergeDesignToggleButton()
            : base()
        {
            this.originalForeground = ConciergeBrushes.DarkPink;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Stretch;

            var scaling = ResolutionScaling.DpiFactor;
            this.LayoutTransform = new ScaleTransform(scaling, scaling, 0.5, 0.5);

            this.Click += this.Button_Click;
            this.MouseEnter += this.Button_MouseEnter;
            this.MouseLeave += this.Button_MouseLeave;
        }

        public void ResetScaling()
        {
            this.LayoutTransform = new ScaleTransform(1, 1, 0.5, 0.5);
        }

        public void UnCheck()
        {
            this.IsChecked = false;
            this.Foreground = this.originalForeground;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSoundService.TapNavigation();
            Program.Logger.Info($"{this.Name} clicked.");
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!this.IsChecked ?? false)
            {
                this.originalForeground = (SolidColorBrush)this.Foreground;
            }

            this.Foreground = Brushes.White;
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!this.IsChecked ?? false)
            {
                this.Foreground = this.originalForeground;
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
