// <copyright file="ConciergeDataGrid.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Commands;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Display.Enums;

    public sealed class ConciergeDataGrid : DataGrid
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

            var scaling = ResolutionScaling.DpiFactor;
            this.LayoutTransform = new ScaleTransform(scaling, scaling, 0.5, 0.5);
        }

        public event RoutedEventHandler Sorted
        {
            add { this.AddHandler(SortedEvent, value); }
            remove { this.RemoveHandler(SortedEvent, value); }
        }

        public int LastIndex => this.Items.Count - 1;

        public void SortListFromDataGrid<T>(List<T> list, ConciergePages conciergePage)
        {
            if (this.Items.IsEmpty)
            {
                return;
            }

            var oldList = new List<T>(list);

            list.Clear();
            foreach (var item in this.Items)
            {
                list.Add((T)item);
            }

            Program.UndoRedoService.AddCommand(
                new ListOrderCommand<T>(
                    list,
                    oldList,
                    new List<T>(list),
                    conciergePage));
        }

        public void RaiseSortedEvent()
        {
            RoutedEventArgs newEventArgs = new (SortedEvent);
            this.RaiseEvent(newEventArgs);
        }

        public int NextItem<T>(List<T> list, int limit, int increment, ConciergePages conciergePage)
        {
            if (this.SelectedItem is null)
            {
                return -1;
            }

            var item = this.SelectedItem;
            var index = list.IndexOf((T)item);

            if (index != limit)
            {
                var oldList = new List<T>(list);
                list.Swap(index, index + increment);
                var newList = new List<T>(list);

                Program.UndoRedoService.AddCommand(new ListOrderCommand<T>(list, oldList, newList, conciergePage));

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

        public bool SetButtonControlsEnableState(params Button[] buttons)
        {
            var hasSelection = this.SelectedItem is not null;
            foreach (var button in buttons)
            {
                DisplayUtility.SetControlEnableState(button, hasSelection);
            }

            return hasSelection;
        }

        protected override void OnSorting(DataGridSortingEventArgs eventArgs)
        {
            base.OnSorting(eventArgs);
            this.RaiseSortedEvent();
        }
    }
}
