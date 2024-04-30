// <copyright file="IntegerUpDownTransparentControl.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Common.Utilities;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for IntegerUpDownTransparentControl.xaml.
    /// </summary>
    public partial class IntegerUpDownTransparentControl : UserControl
    {
        public static readonly DependencyProperty ValueFontSizeProperty =
           DependencyProperty.Register(
               "ValueFontSize",
               typeof(int),
               typeof(IntegerUpDownTransparentControl),
               new UIPropertyMetadata(25));

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                "Minimum",
                typeof(int),
                typeof(IntegerUpDownTransparentControl),
                new UIPropertyMetadata(0));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                "Maximum",
                typeof(int),
                typeof(IntegerUpDownTransparentControl),
                new UIPropertyMetadata(100));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(int),
                typeof(IntegerUpDownTransparentControl),
                new PropertyMetadata(0, new PropertyChangedCallback(OnValuePropertyChanged)));

        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register(
                "Increment",
                typeof(int),
                typeof(IntegerUpDownTransparentControl),
                new UIPropertyMetadata(1));

        private static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent(
                "ValueChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(IntegerUpDownTransparentControl));

        private static readonly RoutedEvent IncreaseClickedEvent =
            EventManager.RegisterRoutedEvent(
                "IncreaseClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(IntegerUpDownTransparentControl));

        private static readonly RoutedEvent DecreaseClickedEvent =
            EventManager.RegisterRoutedEvent(
                "DecreaseClicked",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(IntegerUpDownTransparentControl));

        public IntegerUpDownTransparentControl()
        {
            this.InitializeComponent();
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

        public int Delta => this.Value > this.LastValue ? 1 : this.Value < this.LastValue ? -1 : 0;

        public int Minimum
        {
            get
            {
                return (int)this.GetValue(MinimumProperty);
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

        public int Maximum
        {
            get
            {
                return (int)this.GetValue(MaximumProperty);
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

        public int Value
        {
            get
            {
                return (int)this.GetValue(ValueProperty);
            }

            set
            {
                value = Math.Clamp(value, this.Minimum, this.Maximum);
                this.TextBlockValue.Text = value.ToString();

                this.LastValue = (int)this.GetValue(ValueProperty);
                this.SetValue(ValueProperty, value);
                this.UpdateSpinnerStatus();

                this.RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
            }
        }

        public int Increment
        {
            get { return (int)this.GetValue(IncrementProperty); }
            set { this.SetValue(IncrementProperty, value); }
        }

        private int LastValue { get; set; }

        private static void OnValuePropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is IntegerUpDownControl conciergeIntegerUpDown)
            {
                conciergeIntegerUpDown.TextBoxValue.Text = e.NewValue.ToString();
            }
        }

        private void UpdateSpinnerStatus()
        {
            DisplayUtility.SetControlEnableState(this.Increase, this.Value < this.Maximum);
            DisplayUtility.SetControlEnableState(this.Decrease, this.Value > this.Minimum);
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            if (this.Value < this.Maximum)
            {
                this.Value += this.Increment;
                this.RaiseEvent(new RoutedEventArgs(IncreaseClickedEvent));
                SoundService.PlayUpdateValue();
            }
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            if (this.Value > this.Minimum)
            {
                this.Value -= this.Increment;
                this.RaiseEvent(new RoutedEventArgs(DecreaseClickedEvent));
                SoundService.PlayUpdateValue();
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Control_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
