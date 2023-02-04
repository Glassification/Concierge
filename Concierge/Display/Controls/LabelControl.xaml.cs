// <copyright file="LabelControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

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
                new UIPropertyMetadata("Value"));

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

        public LabelControl()
        {
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "We need for bindings.")]
        private void OnPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
