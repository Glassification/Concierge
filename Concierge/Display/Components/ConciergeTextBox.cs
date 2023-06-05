// <copyright file="ConciergeTextBox.cs" company="Thomas Beckett">
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

    public sealed class ConciergeTextBox : TextBox
    {
        public ConciergeTextBox()
            : base()
        {
            this.Background = ConciergeBrushes.ControlBackground;
            this.Foreground = Brushes.White;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.Margin = new Thickness(5);
            this.BorderThickness = new Thickness(0);
            this.FontSize = 15;
            this.GotFocus += this.ConciergeTextBox_GotFocus;
            this.LostFocus += this.ConciergeTextBox_LostFocus;
            this.PreviewMouseDown += this.ConciergeTextBox_MouseDown;
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

        private void ConciergeTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!this.IsUpdating)
            {
                ConciergeSoundService.UpdateValue();
            }

            this.IsUpdating = false;
            Program.Typing();
        }

        private void ConciergeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Program.NotTyping();
        }

        private void ConciergeTextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 3)
            {
                this.SelectAll();
                e.Handled = true;
            }
        }
    }
}
