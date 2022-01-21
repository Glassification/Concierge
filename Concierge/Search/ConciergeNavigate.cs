// <copyright file="ConciergeNavigate.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Search
{
    using System.Windows.Controls;

    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.NotesPageInterface;
    using Concierge.Utility.Extensions;
    using Concierge.Utility.Utilities;

    public class ConciergeNavigate
    {
        public ConciergeNavigate()
        {
            this.SearchResult = new SearchResult();
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
            if (this.SearchResult.ConciergePage is not Page conciergePage)
            {
                return false;
            }

            var dataGrids = DisplayUtility.FindVisualChildren<ConciergeDataGrid>(conciergePage);
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
            if (this.SearchResult.Item is not ConciergeTextBlock textBlock)
            {
                return false;
            }

            textBlock.Highlight();

            return true;
        }

        private bool NavigateToTreeView()
        {
            if (this.SearchResult.ConciergePage is not Page conciergePage)
            {
                return false;
            }

            var treeViews = DisplayUtility.FindVisualChildren<TreeView>(conciergePage);
            foreach (var treeView in treeViews)
            {
                var item = treeView.GetTreeViewItem(this.SearchResult.Item);

                if (item is not null)
                {
                    item.IsSelected = true;
                    item.Focus();

                    if (item is DocumentTreeViewItem)
                    {
                        (this.SearchResult.ConciergePage as NotesPage)?.HighlightSearchResults(this.SearchResult);
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
