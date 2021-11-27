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

    /// <summary>
    /// Interaction logic for KonamiCodeWindow.xaml.
    /// </summary>
    public partial class KonamiCodeWindow : Window
    {
        public KonamiCodeWindow()
        {
            this.InitializeComponent();
            this.VideoControl.Source = new Uri(@$"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Properties\Resources\Videos\hey-whats-going-on.wmv");
        }

        public void ShowWindow()
        {
            this.ShowDialog();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.VideoControl.Stop();
                    this.Hide();
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
            this.Hide();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
