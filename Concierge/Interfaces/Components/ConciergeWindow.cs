// <copyright file="ConciergeWindow.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Components
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    using Concierge.Character.Enums;
    using Concierge.Configuration;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;

    public abstract class ConciergeWindow : Window
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
                this.Close();
            };

            this.openAnimation.Freeze();
            this.hideAnimation.Freeze();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler? ApplyChanges;

        public abstract string HeaderText { get; }

        public ConciergePage ConciergePage { get; set; }

        protected PopupButtons ButtonPress { get; set; }

        protected ConciergeWindowResult Result { get; set; }

        public virtual bool ShowAdd<T>(T item)
        {
            Program.Logger.Error($"No implemented ShowAdd method for {item}.");
            return false;
        }

        public virtual void ShowEdit<T>(T item)
        {
            Program.Logger.Error($"No implemented ShowEdit method for {item}.");
        }

        public virtual void ShowEdit<T>(T item, bool equippedItem)
        {
            Program.Logger.Error($"No implemented ShowEdit method for {item}.");
        }

        public virtual ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            Program.Logger.Error("No implemented ShowWizardSetup method.");
            return ConciergeWindowResult.NoResult;
        }

        public virtual ConciergeWindowResult ShowHeal<T>(T item)
        {
            Program.Logger.Error($"No implemented ShowHeal method for {item}.");
            return ConciergeWindowResult.NoResult;
        }

        public virtual ConciergeWindowResult ShowDamage<T>(T item)
        {
            Program.Logger.Error($"No implemented ShowDamage method for {item}.");
            return ConciergeWindowResult.NoResult;
        }

        public virtual Color ShowColorWindow(Color color)
        {
            Program.Logger.Error($"No implemented ShowColorWindow method for {color}.");
            return Colors.Transparent;
        }

        public virtual PopupButtons ShowPopup()
        {
            Program.Logger.Error("No implemented ShowPopup method.");
            return PopupButtons.None;
        }

        public virtual void ShowWindow()
        {
            Program.Logger.Error("No implemented ShowWindow method.");
        }

        protected virtual void ReturnAndClose()
        {
            this.Result = ConciergeWindowResult.OK;
            this.CloseConciergeWindow();
        }

        protected void ShowConciergeWindow()
        {
            Program.Logger.Info($"{this.Title} opened.");

            this.Title = this.HeaderText;
            this.SetOpenLocation();
            this.BeginAnimation(OpacityProperty, this.openAnimation);
            this.ShowDialog();
        }

        protected void CloseConciergeWindow()
        {
            Program.Logger.Info($"{this.Title} closed.");

            this.BeginAnimation(OpacityProperty, this.hideAnimation);
        }

        protected void InvokeApplyChanges()
        {
            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void SetOpenLocation()
        {
            if (!AppSettingsManager.UserSettings.AttemptToCenterWindows)
            {
                return;
            }

            var properties = Program.GetMainWindowProperties();
            var offset =
                properties.Location.X == 0 || properties.WindowState != WindowState.Maximized ?
                0 :
                Math.Abs(properties.Location.X) - properties.ActualWidth;

            offset = properties.Location.X > 0 ? -offset : offset;

            this.Left = properties.Location.X + offset + (properties.Center.X - (this.Width / 2));
            this.Top = properties.Location.Y + (properties.Center.Y - (this.Height / 2));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Program.Logger.Info($"{e.Key} key pressed.");
                    this.ButtonPress = PopupButtons.Cancel;
                    this.Result = ConciergeWindowResult.Exit;
                    this.CloseConciergeWindow();
                    break;
                case Key.Enter:
                    Program.Logger.Info($"{e.Key} key pressed.");
                    this.ReturnAndClose();
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
