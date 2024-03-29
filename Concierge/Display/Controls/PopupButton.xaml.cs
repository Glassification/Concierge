﻿// <copyright file="PopupButton.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for PopupButton.xaml.
    /// </summary>
    public partial class PopupButton : UserControl
    {
        public static readonly DependencyProperty IconKindProperty =
            DependencyProperty.Register(
                "IconKind",
                typeof(PackIconKind),
                typeof(PopupButton),
                new UIPropertyMetadata(PackIconKind.Error));

        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register(
                "IconColor",
                typeof(Brush),
                typeof(PopupButton),
                new UIPropertyMetadata(Brushes.Red));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(
                "Label",
                typeof(string),
                typeof(PopupButton),
                new UIPropertyMetadata("Your text here"));

        public static readonly DependencyProperty ShortcutProperty =
            DependencyProperty.Register(
                "Shortcut",
                typeof(string),
                typeof(PopupButton),
                new UIPropertyMetadata("Ctrl-_"));

        public PopupButton()
        {
            this.InitializeComponent();
        }

        public PackIconKind IconKind
        {
            get { return (PackIconKind)this.GetValue(IconKindProperty); }
            set { this.SetValue(IconKindProperty, value); }
        }

        public Brush IconColor
        {
            get { return (Brush)this.GetValue(IconColorProperty); }
            set { this.SetValue(IconColorProperty, value); }
        }

        public string Label
        {
            get { return (string)this.GetValue(LabelProperty); }
            set { this.SetValue(LabelProperty, value); }
        }

        public string Shortcut
        {
            get { return (string)this.GetValue(ShortcutProperty); }
            set { this.SetValue(ShortcutProperty, value); }
        }

        public void AddClickEvent(RoutedEventHandler click)
        {
            this.RealButton.Click += click;
        }

        private void RealButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void RealButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
