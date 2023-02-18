// <copyright file="DisplayUtility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Commands;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    public static class DisplayUtility
    {
        public static string FormatConciergePageForDisplay(ConciergePage conciergePage)
        {
            return $"{conciergePage.ToString().FormatFromEnum()} Page";
        }

        public static void SortListFromDataGrid<T>(ConciergeDataGrid dataGrid, List<T> list, ConciergePage conciergePage)
        {
            if (dataGrid.Items.IsEmpty)
            {
                return;
            }

            var oldList = new List<T>(list);

            list.Clear();
            foreach (var item in dataGrid.Items)
            {
                list.Add((T)item);
            }

            Program.UndoRedoService.AddCommand(
                new ListOrderCommand<T>(
                    list,
                    oldList,
                    new List<T>(list),
                    conciergePage));
            Program.Modify();
        }

        public static T? GetElementUnderMouse<T>()
            where T : UIElement
        {
            return FindVisualParent<T>(Mouse.DirectlyOver as UIElement);
        }

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

        public static int IncrementUsedSlots(int used, int total)
        {
            if (used < total)
            {
                ConciergeSound.UpdateValue();

                return used + 1;
            }

            return used;
        }

        public static void SetCursor(int spent, int total, Func<int, int, bool> func, Cursor cursor)
        {
            if (func(spent, total))
            {
                Mouse.OverrideCursor = cursor;
            }
        }

        public static void SetBorderColour(int spent, int total, Grid grid, Border border, string currentName)
        {
            border.BorderBrush =
                spent != total && currentName.Equals(grid.Name)
                ? ConciergeBrushes.BorderHighlight
                : grid.Background;
        }

        public static T? GetPropertyValue<T>(object source, string propertyName)
        {
            return (T?)source.GetType()?.GetProperty(propertyName)?.GetValue(source, null);
        }

        public static Brush SetUsedTextStyle(int total, int used)
        {
            return total <= used ? Brushes.DarkRed : Brushes.White;
        }

        public static Brush SetUsedBoxStyle(int total, int used)
        {
            return total <= used ? Brushes.IndianRed : ConciergeBrushes.UsedBox;
        }

        public static Brush SetTotalTextStyle(int total, int used)
        {
            return total <= used ? Brushes.DarkGray : Brushes.White;
        }

        public static Brush SetTotalBoxStyle(int total, int used)
        {
            return total <= used ? ConciergeBrushes.TotalDarkBox : ConciergeBrushes.TotalLightBox;
        }
    }
}
