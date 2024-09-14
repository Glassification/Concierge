// <copyright file="DivideLootInputControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for DivideLootInputControl.xaml.
    /// </summary>
    public partial class DivideLootInputControl : UserControl
    {
        public const int MaxValue = 99999;

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

        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register(
                "ButtonStyle",
                typeof(Style),
                typeof(DivideLootInputControl),
                new UIPropertyMetadata());

        private static readonly RoutedEvent SelectionEvent =
            EventManager.RegisterRoutedEvent(
                "Selection",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(DivideLootInputControl));

        public DivideLootInputControl()
        {
            this.InitializeComponent();
        }

        public event RoutedEventHandler Selection
        {
            add { this.AddHandler(SelectionEvent, value); }
            remove { this.RemoveHandler(SelectionEvent, value); }
        }

        public Brush LabelTextColor
        {
            get => (Brush)this.GetValue(LabelTextColorProperty);
            set => this.SetValue(LabelTextColorProperty, value);
        }

        public string LabelText
        {
            get { return (string)this.GetValue(LabelTextProperty); }
            set { this.SetValue(LabelTextProperty, value); }
        }

        public Style ButtonStyle
        {
            get { return (Style)this.GetValue(ButtonStyleProperty); }
            set { this.SetValue(ButtonStyleProperty, value); }
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

        public int InputValue => int.Parse(this.CoinAmount.Text);

        public void SetInputValue(int amount)
        {
            this.CoinAmount.Text = $"{Math.Min(this.InputValue + amount, MaxValue)}";
        }

        public void ClearInputValue()
        {
            this.CoinAmount.Text = "0";
        }

        public void Initialize(DivideLootSelection style)
        {
            var foreground = style == DivideLootSelection.Players ? Brushes.White : Brushes.Black;

            this.CoinAmount.Foreground = foreground;
            this.ClearButton.Foreground = foreground;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.ClearInputValue();
        }

        private void ConciergeTextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(SelectionEvent));
        }
    }
}
