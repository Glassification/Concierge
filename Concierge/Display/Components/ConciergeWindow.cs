// <copyright file="ConciergeWindow.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interop;

    using Concierge.Animations;
    using Concierge.Character;
    using Concierge.Character.Enums;
    using Concierge.Display.Enums;
    using Concierge.Exceptions;
    using Concierge.Primitives;
    using Concierge.Utility;

    public abstract partial class ConciergeWindow : Window
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
            this.Background = ConciergeBrushes.WindowBackground;
            this.HandleEnter = false;

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

        protected ConciergeWindow? NonBlockingWindow { get; set; }

        protected bool HandleEnter { get; set; }

        public virtual bool ShowAdd<T>(T item)
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowAdd), item));
            return false;
        }

        public virtual bool ShowAdd<T>(T item, ICreature creature)
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowAdd), item));
            return false;
        }

        public virtual void ShowEdit<T>(T item)
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowEdit), item));
        }

        public virtual void ShowEdit<T>(T item, object sender)
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowEdit), item));
        }

        public virtual void ShowEdit<T>(T item, bool equippedItem)
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowEdit), item));
        }

        public virtual ConciergeWindowResult ShowWizardSetup(string buttonText)
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowWizardSetup)));
            return ConciergeWindowResult.NoResult;
        }

        public virtual ConciergeWindowResult ShowHeal<T>(T item)
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowHeal), item));
            return ConciergeWindowResult.NoResult;
        }

        public virtual ConciergeWindowResult ShowDamage<T>(T item)
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowDamage), item));
            return ConciergeWindowResult.NoResult;
        }

        public virtual CustomColor ShowColorWindow(CustomColor color)
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowColorWindow), color));
            return CustomColor.Invalid;
        }

        public virtual PopupButtons ShowPopup()
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowPopup)));
            return PopupButtons.None;
        }

        public virtual object? ShowWindow()
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowWindow)));
            return null;
        }

        public virtual ConciergeWindow? ShowNonBlockingWindow()
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowWindow)));
            return null;
        }

        [LibraryImport("dwmapi.dll", EntryPoint = "DwmSetWindowAttribute")]
        internal static partial int DwmSetWindowAttribute(
            IntPtr hwnd,
            DWMWINDOWATTRIBUTE attribute,
            ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute,
            uint cbAttribute);

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

        protected void ShowNonBlockingConciergeWindow()
        {
            Program.Logger.Info($"{this.Title} opened.");

            this.Title = this.HeaderText;
            this.BeginAnimation(OpacityProperty, this.windowAnimation.Open);
            this.Show();
        }

        protected void CloseConciergeWindow()
        {
            Program.Logger.Info($"{this.Title} closed.");

            this.NonBlockingWindow?.CloseConciergeWindow();
            this.BeginAnimation(OpacityProperty, this.windowAnimation.Close);
        }

        protected void InvokeApplyChanges()
        {
            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        protected void UseRoundedCorners()
        {
            IntPtr hWnd = new WindowInteropHelper(GetWindow(this)).EnsureHandle();
            var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
            var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
            Marshal.ThrowExceptionForHR(DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(uint)));
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
                    if (!this.HandleEnter)
                    {
                        this.ReturnAndClose();
                    }

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
