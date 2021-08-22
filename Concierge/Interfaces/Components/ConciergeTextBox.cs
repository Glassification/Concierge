// <copyright file="ConciergeTextBox.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Components
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Utility;

    public class ConciergeTextBox : TextBox
    {
        public ConciergeTextBox()
            : base()
        {
            this.Background = Colours.ControlBackgroundBrush;
            this.Foreground = Brushes.White;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.Margin = new Thickness(5);
            this.BorderThickness = new Thickness(0);

            this.GotFocus += this.SoundEffect_GotFocus;
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
    }
}
