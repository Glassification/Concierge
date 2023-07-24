// <copyright file="DivideLootInputControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for DivideLootInputControl.xaml.
    /// </summary>
    public partial class DivideLootInputControl : UserControl
    {
        public static readonly DependencyProperty LabelTextColorProperty =
            DependencyProperty.Register(
                "LabelTextColor",
                typeof(Brush),
                typeof(SpellSlotsControl),
                new UIPropertyMetadata(Brushes.Black));

        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register(
                "LabelText",
                typeof(string),
                typeof(DivideLootInputControl),
                new UIPropertyMetadata("Input"));

        public static readonly DependencyProperty FillBrushProperty =
            DependencyProperty.Register(
                "FillBrush",
                typeof(Brush),
                typeof(SpellSlotsControl), // No idea why it only works this way
                new UIPropertyMetadata(Brushes.Transparent));

        public DivideLootInputControl()
        {
            this.InitializeComponent();
        }

        public Brush LabelTextColor
        {
            get
            {
                return (Brush)this.GetValue(LabelTextColorProperty);
            }

            set
            {
                this.SetValue(LabelTextColorProperty, value);
                this.InputUpDown.ForegroundBrush = value;
                this.InputUpDown.ForegroundColor = value is SolidColorBrush solidBrush ? solidBrush.Color : Colors.White;
            }
        }

        public string LabelText
        {
            get { return (string)this.GetValue(LabelTextProperty); }
            set { this.SetValue(LabelTextProperty, value); }
        }

        public Brush FillBrush
        {
            get
            {
                return (Brush)this.GetValue(FillBrushProperty);
            }

            set
            {
                this.SetValue(FillBrushProperty, value);
                this.DivideLootGrid.Background = value;
                this.DivideLootBorder.BorderBrush = value;
            }
        }

        public int InputValue => this.InputUpDown.Value;

        public void ResetInputValue()
        {
            this.InputUpDown.Value = 0;
        }
    }
}
