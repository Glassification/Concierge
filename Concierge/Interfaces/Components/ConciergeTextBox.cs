// <copyright file="ConciergeTextBox.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Components
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Utility;

    public class ConciergeTextBox : TextBox
    {
        public ConciergeTextBox()
            : base()
        {
            this.Background = ConciergeColors.ControlBackgroundBrush;
            this.Foreground = Brushes.White;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.Margin = new Thickness(5);
            this.BorderThickness = new Thickness(0);
            this.FontSize = 15;

            this.GotFocus += this.SoundEffect_GotFocus;
            this.MouseEnter += this.Button_MouseEnter;
            this.MouseLeave += this.Button_MouseLeave;
        }

        public bool IsUpdating { get; private set; }

        public void UpdatingValue()
        {
            this.IsUpdating = true;
        }

        public void UpdatedValue()
        {
            this.IsUpdating = false;
        }

        private void SoundEffect_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!this.IsUpdating)
            {
                ConciergeSound.UpdateValue();
            }

            this.IsUpdating = false;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.IBeam;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
