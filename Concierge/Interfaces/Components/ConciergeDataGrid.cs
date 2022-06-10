// <copyright file="ConciergeDataGrid.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.Components
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Commands;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    public class ConciergeDataGrid : DataGrid
    {
        public static readonly RoutedEvent SortedEvent = EventManager.RegisterRoutedEvent(
            "Sorted",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(ConciergeDataGrid));

        public ConciergeDataGrid()
            : base()
        {
            this.AutoGenerateColumns = false;
            this.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            this.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            this.HeadersVisibility = DataGridHeadersVisibility.Column;
            this.IsReadOnly = true;
            this.CanUserResizeColumns = false;
            this.CanUserReorderColumns = false;
            this.HorizontalGridLinesBrush = Brushes.Transparent;
            this.VerticalGridLinesBrush = Brushes.Transparent;
            this.CanUserAddRows = false;
            this.BorderThickness = new Thickness(0);
            this.SelectionMode = DataGridSelectionMode.Single;
            this.SelectionUnit = DataGridSelectionUnit.FullRow;

            var scaling = Constants.DpiFactor;
            this.LayoutTransform = new ScaleTransform(scaling, scaling, 0.5, 0.5);
        }

        public event RoutedEventHandler Sorted
        {
            add { this.AddHandler(SortedEvent, value); }
            remove { this.RemoveHandler(SortedEvent, value); }
        }

        public int LastIndex => this.Items.Count - 1;

        public void RaiseSortedEvent()
        {
            RoutedEventArgs newEventArgs = new (SortedEvent);
            this.RaiseEvent(newEventArgs);
        }

        public int NextItem<T>(List<T> list, int limit, int increment, ConciergePage conciergePage)
        {
            if (this.SelectedItem == null)
            {
                return -1;
            }

            var item = (T)this.SelectedItem;
            var index = list.IndexOf(item);

            if (index != limit)
            {
                var oldList = new List<T>(list);
                list.Swap(index, index + increment);
                var newList = new List<T>(list);

                Program.UndoRedoService.AddCommand(new ListOrderCommand<T>(list, oldList, newList, conciergePage));
                Program.Modify();

                return index + increment;
            }

            return -1;
        }

        public void SetSelectedIndex(int index)
        {
            if (this.Items.IsEmpty || index < 0)
            {
                return;
            }

            if (index == this.Items.Count)
            {
                index--;
            }

            this.SelectedIndex = index;

            this.ScrollIntoView(this.SelectedItem);
        }

        protected override void OnSorting(DataGridSortingEventArgs eventArgs)
        {
            base.OnSorting(eventArgs);
            this.RaiseSortedEvent();
        }
    }
}
