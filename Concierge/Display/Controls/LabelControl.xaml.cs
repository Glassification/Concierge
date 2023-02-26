// <copyright file="LabelControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for LabelControl.xaml.
    /// </summary>
    public partial class LabelControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title",
                typeof(string),
                typeof(LabelControl),
                new UIPropertyMetadata("Label"));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(string),
                typeof(LabelControl),
                new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty TitleSizeProperty =
            DependencyProperty.Register(
                "TitleSize",
                typeof(int),
                typeof(LabelControl),
                new UIPropertyMetadata(20));

        public static readonly DependencyProperty ValueSizeProperty =
            DependencyProperty.Register(
                "ValueSize",
                typeof(int),
                typeof(LabelControl),
                new UIPropertyMetadata(15));

        public static readonly DependencyProperty IsIconProperty =
           DependencyProperty.Register(
               "IsIcon",
               typeof(bool),
               typeof(LabelControl),
               new UIPropertyMetadata(false));

        public static readonly RoutedEvent EditClickedEvent =
            EventManager.RegisterRoutedEvent(
                "EditClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(LabelControl));

        private PackIconKind iconKind;

        public LabelControl()
        {
            this.InitializeComponent();

            this.LabelValue.Visibility = Visibility.Visible;
            this.LabelIcon.Visibility = Visibility.Collapsed;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public event RoutedEventHandler EditClicked
        {
            add { this.AddHandler(EditClickedEvent, value); }
            remove { this.RemoveHandler(EditClickedEvent, value); }
        }

        public PackIconKind IconKind
        {
            get
            {
                return this.iconKind;
            }

            set
            {
                this.iconKind = value;
                this.LabelIcon.Kind = value;
            }
        }

        public int TitleSize
        {
            get { return (int)this.GetValue(TitleSizeProperty); }
            set { this.SetValue(TitleSizeProperty, value); }
        }

        public string Title
        {
            get { return (string)this.GetValue(TitleProperty); }
            set { this.SetValue(TitleProperty, value); }
        }

        public int ValueSize
        {
            get { return (int)this.GetValue(ValueSizeProperty); }
            set { this.SetValue(ValueSizeProperty, value); }
        }

        public string Value
        {
            get
            {
                return (string)this.GetValue(ValueProperty);
            }

            set
            {
                this.SetValue(ValueProperty, value);
                this.LabelValue.Text = value;
            }
        }

        public bool IsIcon
        {
            get
            {
                return (bool)this.GetValue(IsIconProperty);
            }

            set
            {
                this.SetValue(IsIconProperty, value);
                this.LabelValue.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
                this.LabelIcon.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "We need for bindings.")]
        private void OnPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(EditClickedEvent));
        }
    }
}
