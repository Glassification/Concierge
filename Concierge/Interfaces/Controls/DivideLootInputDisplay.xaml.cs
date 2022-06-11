// <copyright file="DivideLootInputDisplay.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for DivideLootInputDisplay.xaml.
    /// </summary>
    public partial class DivideLootInputDisplay : UserControl
    {
        public static readonly DependencyProperty LabelColorProperty =
            DependencyProperty.Register(
                "LabelColor",
                typeof(Brush),
                typeof(DivideLootInputDisplay),
                new UIPropertyMetadata(Brushes.Red));

        public static readonly DependencyProperty LabelTextColorProperty =
            DependencyProperty.Register(
                "LabelTextColor",
                typeof(Brush),
                typeof(DivideLootInputDisplay),
                new UIPropertyMetadata(Brushes.Black));

        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register(
                "LabelText",
                typeof(string),
                typeof(DivideLootInputDisplay),
                new UIPropertyMetadata("Input"));

        public DivideLootInputDisplay()
        {
            this.InitializeComponent();
        }

        public Brush LabelColor
        {
            get { return (Brush)this.GetValue(LabelColorProperty); }
            set { this.SetValue(LabelColorProperty, value); }
        }

        public Brush LabelTextColor
        {
            get { return (Brush)this.GetValue(LabelTextColorProperty); }
            set { this.SetValue(LabelTextColorProperty, value); }
        }

        public string LabelText
        {
            get { return (string)this.GetValue(LabelTextProperty); }
            set { this.SetValue(LabelTextProperty, value); }
        }

        public int InputValue => this.InputUpDown.Value;

        public void ResetInputValue()
        {
            this.InputUpDown.Value = 0;
        }
    }
}
