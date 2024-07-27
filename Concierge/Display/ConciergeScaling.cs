// <copyright file="ConciergeScaling.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display
{
    using System.Windows;

    /// <summary>
    /// Provides methods for registering and handling scaling properties for the <see cref="MainWindow"/> class.
    /// </summary>
    public static class ConciergeScaling
    {
        /// <summary>
        /// Registers a new dependency property with the specified name.
        /// Determines the appropriate coerce value callback based on the presence of 'Y' in the property name.
        /// </summary>
        /// <param name="name">The name of the dependency property.</param>
        /// <returns>The registered dependency property.</returns>
        public static DependencyProperty Register(string name)
        {
            var coerceValueCallback = name.Contains('Y') ? new CoerceValueCallback(OnCoerceScaleValueY) : new CoerceValueCallback(OnCoerceScaleValueX);
            return DependencyProperty.Register(
                name,
                typeof(double),
                typeof(MainWindow),
                new UIPropertyMetadata(1.0, new PropertyChangedCallback(OnScaleValueChanged), coerceValueCallback));
        }

        /// <summary>
        /// Coerces the value of the X scaling property.
        /// </summary>
        /// <param name="o">The dependency object.</param>
        /// <param name="value">The value to coerce.</param>
        /// <returns>The coerced value.</returns>
        public static object OnCoerceScaleValueX(DependencyObject o, object value)
        {
            return o is MainWindow mainWindow ? mainWindow.OnCoerceScaleValue((double)value) : value;
        }

        /// <summary>
        /// Coerces the value of the Y scaling property.
        /// </summary>
        /// <param name="o">The dependency object.</param>
        /// <param name="value">The value to coerce.</param>
        /// <returns>The coerced value.</returns>
        public static object OnCoerceScaleValueY(DependencyObject o, object value)
        {
            return o is MainWindow mainWindow ? mainWindow.OnCoerceScaleValue((double)value) : value;
        }

        /// <summary>
        /// Handles changes to the scaling property value.
        /// </summary>
        /// <param name="o">The dependency object.</param>
        /// <param name="e">The event data.</param>
        public static void OnScaleValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is MainWindow mainWindow)
            {
                mainWindow.OnScaleValueChanged((double)e.OldValue, (double)e.NewValue);
            }
        }
    }
}
