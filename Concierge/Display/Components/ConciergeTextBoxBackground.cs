// <copyright file="ConciergeTextBoxBackground.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Common;
    using Concierge.Common.Utilities;

    public sealed class ConciergeTextBoxBackground : Grid
    {
        public ConciergeTextBoxBackground()
        {
            this.Background = ConciergeBrushes.ControlBackground;
            this.Margin = new Thickness(0, 10, 20, 10);

            this.MouseDown += this.Control_MouseDown;
            this.MouseEnter += this.Control_MouseEnter;
            this.MouseLeave += this.Control_MouseLeave;
        }

        public bool IsReadOnly => this.ConciergeTextBlock?.IsReadOnly ?? false;

        public ConciergeTextBox? ConciergeTextBlock => DisplayUtility.FindVisualChildren<ConciergeTextBox>(this).FirstOrDefault();

        private void Control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.ConciergeTextBlock?.Focus();
        }

        private void Control_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!this.IsReadOnly)
            {
                Mouse.OverrideCursor = Cursors.IBeam;
            }
        }

        private void Control_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!this.IsReadOnly)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
    }
}
