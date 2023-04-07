// <copyright file="PopupButton.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows;
    using System.Windows.Controls;
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

        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register(
               "Text",
               typeof(string),
               typeof(PopupButton),
               new UIPropertyMetadata("Invalid"));

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

        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        public void AddClickEvent(RoutedEventHandler click)
        {
            this.RealButton.Click += click;
        }
    }
}
