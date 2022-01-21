// <copyright file="ConciergeDesignButton.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Components
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Utility;

    public class ConciergeDesignButton : Button
    {
        public ConciergeDesignButton()
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
