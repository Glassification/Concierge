// <copyright file="ConciergeNavigate.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Searching
{
    using System.Collections.Generic;
    using System.Windows.Controls;

    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.NotesPageInterface;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    public class ConciergeNavigate
    {
        public ConciergeNavigate()
        {
        }

        private SearchResult SearchResult { get; set; }

        public void Navigate(SearchResult searchResult)
        {
            this.SearchResult = searchResult;

            if (this.NavigateToDataGrid())
            {
                return;
            }

            if (this.NavigateToTreeView())
            {
                return;
            }

            if (this.NavigateToTextBlock())
            {
                return;
            }
        }

        private bool NavigateToDataGrid()
        {
            var dataGrids = Utilities.FindVisualChildren<ConciergeDataGrid>(this.SearchResult.ConciergePage as Page);

            foreach (var dataGrid in dataGrids)
            {
                var index = dataGrid.Items.IndexOf(this.SearchResult.Item);
                if (index >= 0)
                {
                    dataGrid.SetSelectedIndex(index);

                    return true;
                }
            }

            return false;
        }

        private bool NavigateToTextBlock()
        {
            if (this.SearchResult.Item is not ConciergeTextBlock)
            {
                return false;
            }

            (this.SearchResult.Item as ConciergeTextBlock).Highlight();

            return true;
        }

        private bool NavigateToTreeView()
        {
            var treeViews = Utilities.FindVisualChildren<TreeView>(this.SearchResult.ConciergePage as Page);

            foreach (var treeView in treeViews)
            {
                var item = treeView.GetTreeViewItem(this.SearchResult.Item);

                if (item is not null)
                {
                    item.IsSelected = true;
                    item.Focus();

                    if (item is DocumentTreeViewItem)
                    {
                        (this.SearchResult.ConciergePage as NotesPage).HighlightSearchResults(this.SearchResult);
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
