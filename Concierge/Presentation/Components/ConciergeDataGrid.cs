// <copyright file="ConciergeDataGrid.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.Components
{
    using System.Windows;
    using System.Windows.Controls;

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

        protected override void OnSorting(DataGridSortingEventArgs eventArgs)
        {
            base.OnSorting(eventArgs);
            this.RaiseSortedEvent();
        }
    }
}
