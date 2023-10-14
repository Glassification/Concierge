// <copyright file="PopupToggleButton.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Services;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for PopupToggleButton.xaml.
    /// </summary>
    public partial class PopupToggleButton : UserControl
    {
        public static readonly DependencyProperty IconKindProperty =
           DependencyProperty.Register(
               "IconKind",
               typeof(PackIconKind),
               typeof(PopupToggleButton),
               new UIPropertyMetadata(PackIconKind.Error));

        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register(
                "IconColor",
                typeof(Brush),
                typeof(PopupToggleButton),
                new UIPropertyMetadata(Brushes.Red));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(
                "Label",
                typeof(string),
                typeof(PopupToggleButton),
                new UIPropertyMetadata("Your text here"));

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(
                "IsChecked",
                typeof(bool),
                typeof(PopupToggleButton),
                new UIPropertyMetadata(false));

        public PopupToggleButton()
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

        public bool IsChecked
        {
            get { return (bool)this.GetValue(IsCheckedProperty); }
            set { this.SetValue(IsCheckedProperty, value); }
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

        private void RealButton_Checked(object sender, RoutedEventArgs e)
        {
            this.IsChecked = true;
            this.ExpandIcon.Kind = PackIconKind.MenuDown;
        }

        private void RealButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.IsChecked = false;
            this.ExpandIcon.Kind = PackIconKind.MenuRight;
        }

        private void RealButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSoundService.TapNavigation();
        }
    }
}
