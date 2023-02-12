// <copyright file="ConciergeSlider.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public sealed class ConciergeSlider : Slider
    {
        public ConciergeSlider()
            : base()
        {
            this.Minimum = 0;
            this.Margin = new Thickness(5, 0, 5, 0);
            this.IsMoveToPointEnabled = true;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.HorizontalAlignment = HorizontalAlignment.Stretch;

            this.MouseEnter += this.Slider_MouseEnter;
            this.MouseLeave += this.Slider_MouseLeave;
        }

        private void Slider_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Slider_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
