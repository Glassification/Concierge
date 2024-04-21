// <copyright file="ConciergeDesignButton.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Common;
    using Concierge.Services;

    public sealed class ConciergeDesignButton : Button
    {
        public ConciergeDesignButton()
            : base()
        {
            this.OriginalForeground = null;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Stretch;

            var scaling = ResolutionScaling.DpiFactor;
            this.LayoutTransform = new ScaleTransform(scaling, scaling, 0.5, 0.5);

            this.Click += this.Button_Click;
            this.MouseEnter += this.Button_MouseEnter;
            this.MouseLeave += this.Button_MouseLeave;
        }

        private SolidColorBrush? OriginalForeground { get; set; }

        public void ResetScaling()
        {
            this.LayoutTransform = new ScaleTransform(1, 1, 0.5, 0.5);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
            Program.Logger.Info($"{this.Name} clicked.");
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            this.OriginalForeground = (SolidColorBrush)this.Foreground;
            this.Foreground = Brushes.White;

            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Foreground = this.OriginalForeground;

            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
