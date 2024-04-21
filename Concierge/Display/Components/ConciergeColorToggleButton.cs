// <copyright file="ConciergeColorToggleButton.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    using Concierge.Services;

    public sealed class ConciergeColorToggleButton : ToggleButton
    {
        public ConciergeColorToggleButton()
            : base()
        {
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Stretch;

            this.Click += this.Button_Click;
            this.MouseEnter += this.Button_MouseEnter;
            this.MouseLeave += this.Button_MouseLeave;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SoundService.PlayNavigation();
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
