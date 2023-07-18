// <copyright file="DisplayUtility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Common;

    /// <summary>
    /// Provides utility methods for display-related operations.
    /// </summary>
    public static class DisplayUtility
    {
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
            while (element != null)
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
            if (depObj != null)
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
        /// Sets the mouse cursor based on the specified condition.
        /// </summary>
        /// <param name="spent">The value representing the amount spent.</param>
        /// <param name="total">The value representing the total amount.</param>
        /// <param name="func">The condition function that determines the cursor change.</param>
        /// <param name="cursor">The cursor to set.</param>
        public static void SetCursor(int spent, int total, Func<int, int, bool> func, Cursor cursor)
        {
            if (func(spent, total))
            {
                Mouse.OverrideCursor = cursor;
            }
        }

        /// <summary>
        /// Sets the border color of a Grid element based on the spent and total values.
        /// </summary>
        /// <param name="spent">The value representing the amount spent.</param>
        /// <param name="total">The value representing the total amount.</param>
        /// <param name="grid">The Grid element.</param>
        /// <param name="border">The Border element.</param>
        /// <param name="currentName">The name of the current element.</param>
        public static void SetBorderColour(int spent, int total, Grid grid, Border border, string currentName)
        {
            border.BorderBrush =
                spent != total && currentName.Equals(grid.Name)
                ? ConciergeBrushes.BorderHighlight
                : grid.Background;
        }

        /// <summary>
        /// Gets the value of a property from the specified source object using reflection.
        /// </summary>
        /// <typeparam name="T">The type of the property value to retrieve.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value of the property, or null if not found.</returns>
        public static T? GetPropertyValue<T>(object source, string propertyName)
        {
            return (T?)source.GetType()?.GetProperty(propertyName)?.GetValue(source, null);
        }

        /// <summary>
        /// Sets the text style for the used amount based on the total and used values.
        /// </summary>
        /// <param name="total">The value representing the total amount.</param>
        /// <param name="used">The value representing the used amount.</param>
        /// <returns>The Brush representing the text style.</returns>
        public static Brush SetUsedTextStyle(int total, int used)
        {
            return total <= used ? Brushes.DarkRed : Brushes.White;
        }

        /// <summary>
        /// Sets the box style for the used amount based on the total and used values.
        /// </summary>
        /// <param name="total">The value representing the total amount.</param>
        /// <param name="used">The value representing the used amount.</param>
        /// <returns>The Brush representing the box style.</returns>
        public static Brush SetUsedBoxStyle(int total, int used)
        {
            return total <= used ? Brushes.IndianRed : ConciergeBrushes.UsedBox;
        }

        /// <summary>
        /// Sets the text style for the total amount based on the total and used values.
        /// </summary>
        /// <param name="total">The value representing the total amount.</param>
        /// <param name="used">The value representing the used amount.</param>
        /// <returns>The Brush representing the text style.</returns>
        public static Brush SetTotalTextStyle(int total, int used)
        {
            return total <= used ? Brushes.DarkGray : Brushes.White;
        }

        /// <summary>
        /// Sets the box style for the total amount based on the total and used values.
        /// </summary>
        /// <param name="total">The value representing the total amount.</param>
        /// <param name="used">The value representing the used amount.</param>
        /// <returns>The Brush representing the box style.</returns>
        public static Brush SetTotalBoxStyle(int total, int used)
        {
            return total <= used ? ConciergeBrushes.TotalDarkBox : ConciergeBrushes.TotalLightBox;
        }

        /// <summary>
        /// Sets the enable state of a control element and adjusts its opacity.
        /// </summary>
        /// <param name="element">The control element.</param>
        /// <param name="isEnabled">The state indicating whether the control is enabled.</param>
        public static void SetControlEnableState(FrameworkElement element, bool isEnabled)
        {
            element.IsEnabled = isEnabled;
            element.Opacity = isEnabled ? 1 : 0.5;
        }

        public static List<ComboBoxItem> GenerateSelectorComboBox<T>(ReadOnlyCollection<T> defaultItems, List<T> customItems)
            where T : IUnique
        {
            var combinedItems = new List<T>();
            combinedItems.AddRange(defaultItems);
            combinedItems.AddRange(customItems);
            combinedItems.Sort(new UniqueComparer<T>());

            var comboBoxItems = new List<ComboBoxItem>();
            foreach (var item in combinedItems)
            {
                comboBoxItems.Add(new ComboBoxItem()
                {
                    Content = item.Name,
                    Foreground = item.IsCustom ? Brushes.PowderBlue : Brushes.White,
                    Tag = item,
                });
            }

            return comboBoxItems;
        }
    }
}
