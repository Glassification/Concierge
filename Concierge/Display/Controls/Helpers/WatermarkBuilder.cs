// <copyright file="WatermarkBuilder.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;

    /// <summary>
    /// Class that provides the Watermark attached property.
    /// </summary>
    public static class WatermarkBuilder
    {
        /// <summary>
        /// Watermark Attached Dependency Property.
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached(
           "Watermark",
           typeof(object),
           typeof(WatermarkBuilder),
           new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnWatermarkChanged)));

        /// <summary>
        /// Dictionary of ItemsControls.
        /// </summary>
        private static readonly Dictionary<object, ItemsControl> itemsControls = [];

        /// <summary>
        /// Gets the Watermark property.  This dependency property indicates the watermark for the control.
        /// </summary>
        /// <param name="d"><see cref="DependencyObject"/> to get the property from.</param>
        /// <returns>The value of the Watermark property.</returns>
        public static object GetWatermark(DependencyObject d)
        {
            return d.GetValue(WatermarkProperty);
        }

        /// <summary>
        /// Sets the Watermark property.  This dependency property indicates the watermark for the control.
        /// </summary>
        /// <param name="d"><see cref="DependencyObject"/> to set the property on.</param>
        /// <param name="value">value of the property.</param>
        public static void SetWatermark(DependencyObject d, object value)
        {
            d.SetValue(WatermarkProperty, value);
        }

        /// <summary>
        /// Handles changes to the Watermark property.
        /// </summary>
        /// <param name="d"><see cref="DependencyObject"/> that fired the event.</param>
        /// <param name="e">A <see cref="DependencyPropertyChangedEventArgs"/> that contains the event data.</param>
        private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Control)d;
            control.Loaded += Control_Loaded;

            if (d is ComboBox)
            {
                control.GotKeyboardFocus += Control_GotKeyboardFocus;
                control.LostKeyboardFocus += Control_Loaded;
            }
            else if (d is TextBox)
            {
                control.GotKeyboardFocus += Control_GotKeyboardFocus;
                control.LostKeyboardFocus += Control_Loaded;
                ((TextBox)control).TextChanged += Control_GotKeyboardFocus;
            }

            if (d is ItemsControl and not ComboBox)
            {
                ItemsControl i = (ItemsControl)d;

                i.ItemContainerGenerator.ItemsChanged += ItemsChanged;
                itemsControls.Add(i.ItemContainerGenerator, i);

                DependencyPropertyDescriptor prop = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, i.GetType());
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
                prop.AddValueChanged(i, ItemsSourceChanged);
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            }
        }

        /// <summary>
        /// Handle the GotFocus event on the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="RoutedEventArgs"/> that contains the event data.</param>
        private static void Control_GotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            var control = (Control)sender;
            if (ShouldShowWatermark(control))
            {
                ShowWatermark(control);
            }
            else
            {
                RemoveWatermark(control);
            }
        }

        /// <summary>
        /// Handle the Loaded and LostFocus event on the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="RoutedEventArgs"/> that contains the event data.</param>
        private static void Control_Loaded(object sender, RoutedEventArgs e)
        {
            var control = (Control)sender;
            if (ShouldShowWatermark(control))
            {
                ShowWatermark(control);
            }
        }

        /// <summary>
        /// Event handler for the items source changed event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="EventArgs"/> that contains the event data.</param>
        private static void ItemsSourceChanged(object sender, EventArgs e)
        {
            var itemsControl = (ItemsControl)sender;
            if (itemsControl.ItemsSource is not null)
            {
                if (ShouldShowWatermark(itemsControl))
                {
                    ShowWatermark(itemsControl);
                }
                else
                {
                    RemoveWatermark(itemsControl);
                }
            }
            else
            {
                ShowWatermark(itemsControl);
            }
        }

        /// <summary>
        /// Event handler for the items changed event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="ItemsChangedEventArgs"/> that contains the event data.</param>
        private static void ItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            if (itemsControls.TryGetValue(sender, out ItemsControl? itemsControl))
            {
                if (ShouldShowWatermark(itemsControl))
                {
                    ShowWatermark(itemsControl);
                }
                else
                {
                    RemoveWatermark(itemsControl);
                }
            }
        }

        /// <summary>
        /// Remove the watermark from the specified element.
        /// </summary>
        /// <param name="control">Element to remove the watermark from.</param>
        private static void RemoveWatermark(UIElement control)
        {
            var layer = AdornerLayer.GetAdornerLayer(control);
            if (layer is not null)
            {
                var adorners = layer.GetAdorners(control);
                if (adorners is null)
                {
                    return;
                }

                foreach (var adorner in adorners)
                {
                    if (adorner is WatermarkAdorner)
                    {
                        adorner.Visibility = Visibility.Hidden;
                        layer.Remove(adorner);
                    }
                }
            }
        }

        /// <summary>
        /// Show the watermark on the specified control.
        /// </summary>
        /// <param name="control">Control to show the watermark on.</param>
        private static void ShowWatermark(Control control)
        {
            var layer = AdornerLayer.GetAdornerLayer(control);
            if (layer is not null)
            {
                layer.Add(new WatermarkAdorner(control, GetWatermark(control)));
            }
        }

        /// <summary>
        /// Indicates whether or not the watermark should be shown on the specified control.
        /// </summary>
        /// <param name="c"><see cref="Control"/> to test.</param>
        /// <returns>true if the watermark should be shown; false otherwise.</returns>
        private static bool ShouldShowWatermark(Control c)
        {
            if (c is ComboBox comboBox)
            {
                return comboBox.Text.Equals(string.Empty);
            }
            else if (c is TextBoxBase textBoxBase && textBoxBase is TextBox textBox)
            {
                return textBox.Text.Equals(string.Empty);
            }
            else if (c is ItemsControl itemsControl)
            {
                return itemsControl.Items.Count == 0;
            }
            else
            {
                return false;
            }
        }
    }
}
