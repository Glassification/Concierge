// <copyright file="ConciergeWindow.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Components
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Animations;
    using Concierge.Character.Enums;
    using Concierge.Configuration;
    using Concierge.Interfaces.Enums;
    using Concierge.Primitives;
    using Concierge.Utility;

    public abstract class ConciergeWindow : Window
    {
        private readonly WindowAnimation windowAnimation;

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

            this.windowAnimation = new WindowAnimation(WindowAnimation.DefaultAnimationSpeed, this.Window_OnClose);
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

        public virtual CustomColor ShowColorWindow(CustomColor color)
        {
            Program.Logger.Error($"No implemented ShowColorWindow method for {color}.");
            return CustomColor.Invalid;
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
            this.BeginAnimation(OpacityProperty, this.windowAnimation.Open);
            this.ShowDialog();
        }

        protected void CloseConciergeWindow()
        {
            Program.Logger.Info($"{this.Title} closed.");

            this.BeginAnimation(OpacityProperty, this.windowAnimation.Close);
        }

        protected void InvokeApplyChanges()
        {
            this.ApplyChanges?.Invoke(this, new EventArgs());
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

        private void Window_OnClose(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}
