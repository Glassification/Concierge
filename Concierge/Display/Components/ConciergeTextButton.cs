// <copyright file="ConciergeTextButton.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Utility;

    public sealed class ConciergeTextButton : Button
    {
        public ConciergeTextButton()
            : base()
        {
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.Foreground = Brushes.White;
            this.FontSize = 15;
            this.Width = 200;
            this.Height = 50;
            this.Margin = new Thickness(5);
            this.SnapsToDevicePixels = true;

            this.Click += this.Button_Click;
            this.MouseEnter += this.Button_MouseEnter;
            this.MouseLeave += this.Button_MouseLeave;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
            Program.Logger.Info($"{this.Name} clicked.");
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
