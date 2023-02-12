// <copyright file="DoubleUpDownControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for DoubleUpDown.xaml.
    /// </summary>
    public partial class DoubleUpDownControl : UserControl
    {
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                "Minimum",
                typeof(double),
                typeof(DoubleUpDownControl),
                new UIPropertyMetadata(0.0));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                "Maximum",
                typeof(double),
                typeof(DoubleUpDownControl),
                new UIPropertyMetadata(100.0));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(double),
                typeof(DoubleUpDownControl),
                new PropertyMetadata(0.0, new PropertyChangedCallback(OnValuePropertyChanged)));

        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register(
                "Increment",
                typeof(double),
                typeof(DoubleUpDownControl),
                new UIPropertyMetadata(1.0));

        private static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent(
                "ValueChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(DoubleUpDownControl));

        private static readonly RoutedEvent IncreaseClickedEvent =
            EventManager.RegisterRoutedEvent(
                "IncreaseClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(DoubleUpDownControl));

        private static readonly RoutedEvent DecreaseClickedEvent =
            EventManager.RegisterRoutedEvent(
                "DecreaseClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(DoubleUpDownControl));

        public DoubleUpDownControl()
        {
            this.InitializeComponent();
            this.Minimum = 0;
            this.Increment = 1;
            this.Maximum = double.MaxValue;
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

        public double Minimum
        {
            get
            {
                return (double)this.GetValue(MinimumProperty);
            }

            set
            {
                this.SetValue(MinimumProperty, value);
                if (this.Value < value)
                {
                    this.Value = value;
                }
                else
                {
                    this.UpdateSpinnerStatus();
                }
            }
        }

        public double Maximum
        {
            get
            {
                return (double)this.GetValue(MaximumProperty);
            }

            set
            {
                this.SetValue(MaximumProperty, value);
                if (this.Value > value)
                {
                    this.Value = value;
                }
                else
                {
                    this.UpdateSpinnerStatus();
                }
            }
        }

        public double Value
        {
            get
            {
                return (double)this.GetValue(ValueProperty);
            }

            set
            {
                value = Math.Min(value, this.Maximum);
                value = Math.Max(value, this.Minimum);
                this.TextBoxValue.Text = value.ToString();

                this.SetValue(ValueProperty, value);
                this.UpdateSpinnerStatus();

                this.RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
            }
        }

        public double Increment
        {
            get { return (double)this.GetValue(IncrementProperty); }
            set { this.SetValue(IncrementProperty, value); }
        }

        private static void OnValuePropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is DoubleUpDownControl conciergeDoubleUpDown)
            {
                conciergeDoubleUpDown.TextBoxValue.Text = e.NewValue.ToString();
            }
        }

        private void UpdateSpinnerStatus()
        {
            this.SetIncreaseButtonStatus(this.Value, this.Maximum);
            this.SetDecreaseButtonStatus(this.Value, this.Minimum);
        }

        private void SetIncreaseButtonStatus(double value, double max)
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

        private void SetDecreaseButtonStatus(double value, double min)
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
            var isValidNumber = double.TryParse(currentText, out var value);

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
            Program.NotTyping();
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

        private void TextBoxValue_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            this.Value += (e.Delta > 0 ? 1 : -1) * this.Increment;
            this.TextBoxValue.Select(this.TextBoxValue.Text.Length, 0);
            ConciergeSound.UpdateValue();
        }

        private void TextBoxValue_GotFocus(object sender, RoutedEventArgs e)
        {
            Program.Typing();
        }

        private void TextBoxValue_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(this.TextBoxValue.Text);
        }
    }
}
