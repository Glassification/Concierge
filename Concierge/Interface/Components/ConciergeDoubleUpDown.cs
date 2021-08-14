// <copyright file="ConciergeIntegerUpDown.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.Components
{
    using System.Windows;
    using System.Windows.Media;

    using Concierge.Utility;
    using Xceed.Wpf.Toolkit;

    public class ConciergeDoubleUpDown : DoubleUpDown
    {
        private bool updatingValue;

        public ConciergeDoubleUpDown()
            : base()
        {
            this.Background = Colours.ControlBackgroundBrush;
            this.BorderThickness = new Thickness(0);
            this.Foreground = Brushes.White;
            this.Margin = new Thickness(0, 0, 20, 0);
            this.Height = 40;
            this.Minimum = 0;
            this.TextAlignment = TextAlignment.Center;
            this.FontSize = 25;

            this.ValueChanged += this.CreateSound_ValueChanged;
        }

        public void UpdatingValue()
        {
            this.updatingValue = true;
        }

        private void CreateSound_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!this.updatingValue)
            {
                ConciergeSound.UpdateValue();
            }

            this.updatingValue = false;
        }
    }
}
