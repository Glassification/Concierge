// <copyright file="ConciergeWindow.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Components
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media.Animation;

    using Concierge.Character.Enums;
    using Concierge.Configuration;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility.Colors;

    public class ConciergeWindow : Window
    {
        private const double FadeAnimationSpeed = 0.15;

        private readonly DoubleAnimation openAnimation;
        private readonly DoubleAnimation hideAnimation;

        public ConciergeWindow()
        {
            this.AllowsTransparency = true;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStyle = WindowStyle.None;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Left = 0;
            this.Top = 0;
            this.Background = ConciergeColors.WindowBackground;
            this.BorderBrush = ConciergeColors.RectangleBorderHighlight;
            this.BorderThickness = new Thickness(1);

            this.MouseDown += this.Window_MouseDown;
            this.KeyDown += this.Window_KeyDown;

            this.openAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(FadeAnimationSpeed)),
                FillBehavior = FillBehavior.Stop,
            };
            this.hideAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(FadeAnimationSpeed)),
                FillBehavior = FillBehavior.Stop,
            };
            this.hideAnimation.Completed += (s, e) =>
            {
                this.Hide();
            };

            this.openAnimation.Freeze();
            this.hideAnimation.Freeze();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        protected PopupButtons ButtonPress { get; set; }

        protected ConciergeWindowResult Result { get; set; }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.HideConciergeWindow();
        }

        protected void ShowConciergeWindow()
        {
            this.SetOpenLocation();
            this.BeginAnimation(OpacityProperty, this.openAnimation);
            this.ShowDialog();
        }

        protected void HideConciergeWindow()
        {
            this.BeginAnimation(OpacityProperty, this.hideAnimation);
        }

        protected void InvokeApplyChanges()
        {
            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void SetOpenLocation()
        {
            if (!AppSettingsManager.Settings.AttemptToCenterWindows)
            {
                return;
            }

            var properties = Program.GetMainWindowProperties();
            var offset = properties.Location.X == 0 || properties.WindowState != WindowState.Maximized ? 0 : Math.Abs(properties.Location.X) - properties.ActualWidth;

            offset = properties.Location.X > 0 ? -offset : offset;

            this.Left = properties.Location.X + offset + (properties.Center.X - (this.Width / 2));
            this.Top = properties.Location.Y + (properties.Center.Y - (this.Height / 2));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.ButtonPress = PopupButtons.Cancel;
                    this.Result = ConciergeWindowResult.Exit;
                    this.HideConciergeWindow();
                    break;
            }
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
