﻿// <copyright file="ConciergeButton.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.Components
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Utility;

    public class ConciergeButton : Button
    {
        public ConciergeButton()
            : base()
        {
            this.BorderBrush = null;
            this.OriginalForeground = null;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;

            this.Click += this.Button_Click;
            this.MouseEnter += this.Button_MouseEnter;
            this.MouseLeave += this.Button_MouseLeave;
        }

        private SolidColorBrush OriginalForeground { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.TapNavigation();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            this.OriginalForeground = (SolidColorBrush)this.Foreground;
            this.Foreground = Brushes.Black;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
           this.Foreground = this.OriginalForeground;
        }
    }
}