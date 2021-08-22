// <copyright file="ConciergeDataGrid.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Components
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Utility.Extensions;

    public class ConciergeDataGrid : DataGrid
    {
        public static readonly RoutedEvent SortedEvent = EventManager.RegisterRoutedEvent(
            "Sorted", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ConciergeDataGrid));

        public event RoutedEventHandler Sorted
        {
            add { this.AddHandler(SortedEvent, value); }
            remove { this.RemoveHandler(SortedEvent, value); }
        }

        public void RaiseSortedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(ConciergeDataGrid.SortedEvent);
            this.RaiseEvent(newEventArgs);
        }

        public int NextItem<T>(List<T> list, int limit, int increment)
        {
            if (this.SelectedItem == null)
            {
                return -1;
            }

            Program.Modify();

            var item = (T)this.SelectedItem;
            var index = list.IndexOf(item);

            if (index != limit)
            {
                list.Swap(index, index + increment);
                return index + increment;
            }

            return -1;
        }

        protected override void OnSorting(DataGridSortingEventArgs eventArgs)
        {
            base.OnSorting(eventArgs);
            this.RaiseSortedEvent();
        }
    }
}
