// <copyright file="KonamiCodeWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.UtilityInterface
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Interfaces.Components;

    /// <summary>
    /// Interaction logic for KonamiCodeWindow.xaml.
    /// </summary>
    public partial class KonamiCodeWindow : ConciergeWindow
    {
        public KonamiCodeWindow()
        {
            this.InitializeComponent();
            this.VideoControl.Source = new Uri(@$"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Properties\Resources\Videos\hey-whats-going-on.wmv");
        }

        public override string HeaderText => "Hey What's Goin' On";

        public override void ShowWindow()
        {
            this.ShowConciergeWindow();
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
