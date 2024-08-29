// <copyright file="DisplayUtility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Utilities
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Common;

    /// <summary>
    /// Provides utility methods for display-related operations.
    /// </summary>
    public static class DisplayUtility
    {
        /// <summary>
        /// Sets the text style for the total amount based on the total and used values.
        /// </summary>
        /// <param name="total">The value representing the total amount.</param>
        /// <param name="used">The value representing the used amount.</param>
        /// <returns>The Brush representing the text style.</returns>
        public static Brush SetTotalTextStyle(int total, int used) => total <= used ? Brushes.DarkGray : Brushes.White;

        /// <summary>
        /// Sets the box style for the total amount based on the total and used values.
        /// </summary>
        /// <param name="total">The value representing the total amount.</param>
        /// <param name="used">The value representing the used amount.</param>
        /// <returns>The Brush representing the box style.</returns>
        public static Brush SetTotalBoxStyle(int total, int used) => total <= used ? ConciergeBrushes.DarkGreyBox : ConciergeBrushes.ControlForeBlue;

        /// <summary>
        /// Retrieves the element of type T that is currently under the mouse cursor.
        /// </summary>
        /// <typeparam name="T">The type of the element to retrieve.</typeparam>
        /// <returns>The element of type T under the mouse cursor, or null if not found.</returns>
        public static T? GetElementUnderMouse<T>()
            where T : UIElement
        {
            return FindVisualParent<T>(Mouse.DirectlyOver as UIElement);
        }

        /// <summary>
        /// Finds the visual parent of the specified element of type T.
        /// </summary>
        /// <typeparam name="T">The type of the visual parent to find.</typeparam>
        /// <param name="element">The starting element to search from.</param>
        /// <returns>The visual parent of type T, or null if not found.</returns>
        public static T? FindVisualParent<T>(UIElement? element)
            where T : UIElement
        {
            while (element is not null)
            {
                if (element is T correctlyTyped)
                {
                    return correctlyTyped;
                }

                element = VisualTreeHelper.GetParent(element) as UIElement;
            }

            return null;
        }

        /// <summary>
        /// Finds all the visual children of the specified DependencyObject of type T.
        /// </summary>
        /// <typeparam name="T">The type of the visual children to find.</typeparam>
        /// <param name="depObj">The DependencyObject to search for visual children.</param>
        /// <returns>An enumerable collection of visual children of type T.</returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj)
            where T : DependencyObject
        {
            if (depObj is not null)
            {
                foreach (object rawChild in LogicalTreeHelper.GetChildren(depObj))
                {
                    if (rawChild is DependencyObject child)
                    {
                        if (child is T t)
                        {
                            yield return t;
                        }

                        foreach (T childOfChild in FindVisualChildren<T>(child))
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the enable state of a control element and adjusts its opacity.
        /// </summary>
        /// <param name="element">The control element.</param>
        /// <param name="isEnabled">The state indicating whether the control is enabled.</param>
        public static void SetControlEnableState(FrameworkElement element, bool isEnabled)
        {
            if (element is IOpacity opacity)
            {
                opacity.SetEnableState(isEnabled);
            }
            else
            {
                element.IsEnabled = isEnabled;
                element.Opacity = isEnabled ? 1 : 0.5;
            }
        }
    }
}
