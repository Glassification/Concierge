namespace Concierge.Presentation.Components
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public class ConciergeDataGrid : DataGrid
    {
        public static readonly RoutedEvent SortedEvent = EventManager.RegisterRoutedEvent(
            "Sorted", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ConciergeDataGrid));

        public event RoutedEventHandler Sorted
        {
            add { AddHandler(SortedEvent, value); }
            remove { RemoveHandler(SortedEvent, value); }
        }

        void RaiseSortedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(ConciergeDataGrid.SortedEvent);
            RaiseEvent(newEventArgs);
        }

        protected override void OnSorting(DataGridSortingEventArgs eventArgs)
        {
            base.OnSorting(eventArgs);
            RaiseSortedEvent();
        }
    }
}
