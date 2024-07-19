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
    using Concierge.Character.Aspects;
    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Common.Exceptions;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Data;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows.Helpers;
    using Concierge.Services;
    using Concierge.Tools;

    using Attribute = Concierge.Character.Aspects.Attribute;

    public abstract partial class ConciergeWindow : Window
    {
        private readonly WindowAnimation windowAnimation;
        private readonly StringResourceService resourceService;

        private UIElement? focusedElement;

        public ConciergeWindow()
        {
            this.AllowsTransparency = true;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStyle = WindowStyle.None;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Left = 0;
            this.Top = 0;
            this.Background = ConciergeBrushes.WindowBackground;
            this.Description = new NotifiableText();
            this.FocusedText = string.Empty;

            this.MouseDown += this.Window_MouseDown;
            this.MouseUp += this.Window_MouseUp;
            this.KeyDown += this.Window_KeyDown;

            this.windowAnimation = new WindowAnimation(WindowAnimation.DefaultAnimationSpeed, this.Window_OnClose);
            this.resourceService = new StringResourceService();
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler? ApplyChanges;

        public abstract string HeaderText { get; }

        public abstract string WindowName { get; }

        public ConciergePages ConciergePage { get; set; }

        protected ConciergeResult Result { get; set; }

        protected ConciergeWindow? NonBlockingWindow { get; set; }

        protected NotifiableText Description { get; set; }

        protected string FocusedText { get; set; }

        protected bool CloseOnEnter { get; set; }

        public virtual bool ShowAdd<T>(T item)
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

        public virtual ConciergeResult ShowWizardSetup(string buttonText)
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowWizardSetup)));
            return ConciergeResult.NoResult;
        }

        public virtual ConciergeResult ShowHeal<T>(T item)
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowHeal), item));
            return ConciergeResult.NoResult;
        }

        public virtual ConciergeResult ShowDamage<T>(T item)
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowDamage), item));
            return ConciergeResult.NoResult;
        }

        public virtual CustomColor ShowColorWindow(CustomColor color)
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowColorWindow), color));
            return CustomColor.Invalid;
        }

        public virtual CustomIcon ShowIconWindow(CustomIcon iconKind)
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowIconWindow), iconKind));
            return iconKind;
        }

        public virtual ConciergeResult ShowUseItemWindow(UsedItem usedItem)
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowUseItemWindow)));
            return ConciergeResult.NoResult;
        }

        public virtual AbilitySave ShowAbilityCheckWindow(Attribute attribute, int value)
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowAbilityCheckWindow)));
            return AbilitySave.None;
        }

        public virtual AbilitySave ShowAbilityCheckWindow(Skill skill, int value)
        {
            Program.Logger.Error(new ImplementedMethodException(nameof(this.ShowAbilityCheckWindow)));
            return AbilitySave.None;
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
            this.Result = ConciergeResult.OK;
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

        /// <summary>
        /// Applies rounded corners to the window if the operating system is Windows 11.
        /// </summary>
        /// <remarks>
        /// Windows 11 introduced rounded window corners as a design element.
        /// This method sets the window corner preference to rounded if the operating system is Windows 11.
        /// If the operating system is not Windows 11, it applies a standard border style to the window.
        /// </remarks>
        protected void UseRoundedCorners()
        {
            if (SystemUtility.GetWindowsVersion() == OSVersion.Windows11)
            {
                IntPtr hWnd = new WindowInteropHelper(GetWindow(this)).EnsureHandle();
                var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
                var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
                Marshal.ThrowExceptionForHR(DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(uint)));
            }
            else
            {
                this.BorderBrush = ConciergeBrushes.Border;
                this.BorderThickness = new Thickness(1);
            }
        }

        protected void SetMouseOverEvents(UIElement element)
        {
            element.MouseEnter += this.Control_MouseEnter;
            element.MouseLeave += this.Control_MouseLeave;
            element.GotFocus += this.Control_GotFocus;
            element.LostFocus += this.Control_LostFocus;
        }

        protected void SetMouseOverEvents(UIElement element, UIElement backgroundElement)
        {
            element.GotFocus += this.Control_GotFocus;
            element.LostFocus += this.Control_LostFocus;

            backgroundElement.MouseEnter += this.Control_MouseEnter;
            backgroundElement.MouseLeave += this.Control_MouseLeave;
        }

        protected void Control_MouseEnter(object sender, RoutedEventArgs e)
        {
            var controlName = this.resourceService.CleanString(sender.GetProperty("Name"));
            this.Description.Text = this.resourceService.GetPropertyDescription(this.WindowName, controlName, defaultDescription: controlName);
        }

        protected void Control_MouseLeave(object sender, RoutedEventArgs e)
        {
            this.Description.Text = this.FocusedText;
        }

        protected void Control_GotFocus(object sender, RoutedEventArgs e)
        {
            var controlName = sender.GetProperty("Name");
            var text = this.resourceService.GetPropertyDescription(this.WindowName, controlName, defaultDescription: controlName);
            this.Description.Text = text;
            this.FocusedText = text;
            this.focusedElement = sender as UIElement;
        }

        protected void Control_LostFocus(object sender, RoutedEventArgs e)
        {
            this.Description.Text = string.Empty;
            this.FocusedText = string.Empty;
            this.focusedElement = null;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Program.Logger.Info($"{e.Key} key pressed.");
                    this.Result = ConciergeResult.Exit;
                    this.CloseConciergeWindow();
                    break;
                case Key.Enter:
                    if (this.CloseOnEnter)
                    {
                        Program.Logger.Info($"{e.Key} key pressed.");
                        this.Result = ConciergeResult.OK;
                        this.CloseConciergeWindow();
                    }

                    break;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && this.focusedElement is not null)
            {
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this.focusedElement), null);
                Keyboard.ClearFocus();
            }
        }

        private void Window_OnClose(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}
