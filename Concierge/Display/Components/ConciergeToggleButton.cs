﻿// <copyright file="ConciergeToggleButton.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Utility;

    public sealed class ConciergeToggleButton : ToggleButton
    {
        public ConciergeToggleButton()
            : base()
        {
            this.OriginalForeground = null;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Stretch;

            this.Click += this.Button_Click;
            this.MouseEnter += this.Button_MouseEnter;
            this.MouseLeave += this.Button_MouseLeave;
        }

        private SolidColorBrush? OriginalForeground { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!this.IsChecked ?? false)
            {
                this.OriginalForeground = (SolidColorBrush)this.Foreground;
            }

            this.Foreground = Brushes.White;
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!this.IsChecked ?? false)
            {
                this.Foreground = this.OriginalForeground;
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}