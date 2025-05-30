﻿// <copyright file="KonamiCodeWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Utility
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Common;
    using Concierge.Display.Components;

    /// <summary>
    /// Interaction logic for KonamiCodeWindow.xaml.
    /// </summary>
    public partial class KonamiCodeWindow : ConciergeWindow
    {
        public KonamiCodeWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.VideoControl.Source = new Uri(@$"{ConciergeFiles.ExecutingDirectory}\Properties\Resources\Videos\hey-whats-going-on.wmv");
        }

        public override string HeaderText => "Hey What's Goin' On";

        public override string WindowName => nameof(KonamiCodeWindow);

        public override object? ShowWindow()
        {
            this.ShowConciergeWindow();
            return null;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.VideoControl.Stop();
                    break;
            }
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                this.VideoControl.Play();
            }
        }

        private void VideoControl_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }
    }
}
