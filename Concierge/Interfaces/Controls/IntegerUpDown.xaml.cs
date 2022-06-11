// <copyright file="IntegerUpDown.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for IntegerUpDown.xaml.
    /// </summary>
    public partial class IntegerUpDown : UserControl
    {
        public static readonly DependencyProperty ValueFontSizeProperty =
            DependencyProperty.Register(
                "ValueFontSize",
                typeof(int),
                typeof(IntegerUpDown),
                new UIPropertyMetadata(25));

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                "Minimum",
                typeof(int),
                typeof(IntegerUpDown),
                new UIPropertyMetadata(0));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                "Maximum",
                typeof(int),
                typeof(IntegerUpDown),
                new UIPropertyMetadata(100));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(int),
                typeof(IntegerUpDown),
                new PropertyMetadata(0, new PropertyChangedCallback(OnValuePropertyChanged)));

        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register(
                "Increment",
                typeof(int),
                typeof(IntegerUpDown),
                new UIPropertyMetadata(1));

        private static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent(
            "ValueChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(IntegerUpDown));

        private static readonly RoutedEvent IncreaseClickedEvent =
            EventManager.RegisterRoutedEvent(
                "IncreaseClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(IntegerUpDown));

        private static readonly RoutedEvent DecreaseClickedEvent =
            EventManager.RegisterRoutedEvent(
                "DecreaseClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(IntegerUpDown));

        public IntegerUpDown()
        {
            this.InitializeComponent();
            this.Minimum = 0;
            this.Increment = 1;
            this.Maximum = int.MaxValue;
            this.Margin = new Thickness(0, 0, 20, 0);
            this.Height = 40;
        }

        public event RoutedEventHandler ValueChanged
        {
            add { this.AddHandler(ValueChangedEvent, value); }
            remove { this.RemoveHandler(ValueChangedEvent, value); }
        }

        public event RoutedEventHandler IncreaseClicked
        {
            add { this.AddHandler(IncreaseClickedEvent, value); }
            remove { this.RemoveHandler(IncreaseClickedEvent, value); }
        }

        public event RoutedEventHandler DecreaseClicked
        {
            add { this.AddHandler(DecreaseClickedEvent, value); }
            remove { this.RemoveHandler(DecreaseClickedEvent, value); }
        }

        public int ValueFontSize
        {
            get { return (int)this.GetValue(ValueFontSizeProperty); }
            set { this.SetValue(ValueFontSizeProperty, value); }
        }

        public int Minimum
        {
            get { return (int)this.GetValue(MinimumProperty); }
            set { this.SetValue(MinimumProperty, value); }
        }

        public int Maximum
        {
            get { return (int)this.GetValue(MaximumProperty); }
            set { this.SetValue(MaximumProperty, value); }
        }

        public int Value
        {
            get
            {
                return (int)this.GetValue(ValueProperty);
            }

            set
            {
                value = Math.Min(value, this.Maximum);
                value = Math.Max(value, this.Minimum);
                this.TextBoxValue.Text = value.ToString();

                this.SetIncreaseButtonStatus(value, this.Maximum);
                this.SetDecreaseButtonStatus(value, this.Minimum);
                this.SetValue(ValueProperty, value);

                this.RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
            }
        }

        public int Increment
        {
            get { return (int)this.GetValue(IncrementProperty); }
            set { this.SetValue(IncrementProperty, value); }
        }

        public void UpdateSpinnerStatus()
        {
            this.SetIncreaseButtonStatus(this.Value, this.Maximum);
            this.SetDecreaseButtonStatus(this.Value, this.Minimum);
        }

        private static void OnValuePropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is IntegerUpDown conciergeIntegerUpDown)
            {
                conciergeIntegerUpDown.TextBoxValue.Text = e.NewValue.ToString();
            }
        }

        private void SetIncreaseButtonStatus(int value, int max)
        {
            if (value < max)
            {
                this.Increase.IsEnabled = true;
                this.Increase.Opacity = 1;
            }
            else
            {
                this.Increase.IsEnabled = false;
                this.Increase.Opacity = 0.5;
            }
        }

        private void SetDecreaseButtonStatus(int value, int min)
        {
            if (value > min)
            {
                this.Decrease.IsEnabled = true;
                this.Decrease.Opacity = 1;
            }
            else
            {
                this.Decrease.IsEnabled = false;
                this.Decrease.Opacity = 0.5;
            }
        }

        private void AttemptToSetValue()
        {
            var currentText = this.TextBoxValue.Text;
            var isValidNumber = int.TryParse(currentText, out var value);

            if (!isValidNumber)
            {
                this.TextBoxValue.Text = this.Value.ToString();
                return;
            }

            this.Value = value;
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            if (this.Value < this.Maximum)
            {
                this.Value += this.Increment;
                this.RaiseEvent(new RoutedEventArgs(IncreaseClickedEvent));
                ConciergeSound.UpdateValue();
            }
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            if (this.Value > this.Minimum)
            {
                this.Value -= this.Increment;
                this.RaiseEvent(new RoutedEventArgs(DecreaseClickedEvent));
                ConciergeSound.UpdateValue();
            }
        }

        private void TextBoxValue_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsDown && e.Key == Key.Up && this.Value < this.Maximum)
            {
                this.Value += this.Increment;
                this.RaiseEvent(new RoutedEventArgs(IncreaseClickedEvent));
            }
            else if (e.IsDown && e.Key == Key.Down && this.Value > this.Minimum)
            {
                this.Value -= this.Increment;
                this.RaiseEvent(new RoutedEventArgs(DecreaseClickedEvent));
            }
            else if (e.IsDown && e.Key == Key.Enter)
            {
                this.AttemptToSetValue();
            }
        }

        private void TextBoxValue_LostFocus(object sender, RoutedEventArgs e)
        {
            this.AttemptToSetValue();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void TextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.IBeam;
        }

        private void Control_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
