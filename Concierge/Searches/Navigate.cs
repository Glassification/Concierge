// <copyright file="Navigate.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Searches
{
    using System.Windows.Controls;

    using Concierge.Character.Journals;
    using Concierge.Common.Utilities;
    using Concierge.Display.Components;
    using Concierge.Display.Pages;

    /// <summary>
    /// Represents a utility class for navigating to search results within the Concierge application.
    /// </summary>
    public sealed class Navigate
    {
        private SearchResult searchResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="Navigate"/> class.
        /// </summary>
        public Navigate()
        {
            this.searchResult = new SearchResult();
        }

        /// <summary>
        /// Navigates to the specified search result.
        /// </summary>
        /// <param name="searchResult">The search result to navigate to.</param>
        public void Go(SearchResult searchResult)
        {
            this.searchResult = searchResult;

            if (this.GoToDataGrid())
            {
                return;
            }

            if (this.GoToTextBlock())
            {
                return;
            }

            if (this.GoToTreeView())
            {
                return;
            }

            if (this.GoToDocument())
            {
                return;
            }
        }

        private bool GoToDocument()
        {
            if (this.searchResult.ConciergePage is not Page conciergePage || this.searchResult.Item is not Document document)
            {
                return false;
            }

            var treeViews = DisplayUtility.FindVisualChildren<ConciergeTreeView>(conciergePage);
            foreach (var treeView in treeViews)
            {
                var item = treeView.GetTreeViewItemByDocument(document);

                if (item is null)
                {
                    return false;
                }

                item.IsSelected = true;
                item.Focus();

                if (item is DocumentTreeViewItem)
                {
                    if (item.Parent is ChapterTreeViewItem parentItem)
                    {
                        parentItem.IsExpanded = true;
                    }

                    (this.searchResult.ConciergePage as JournalPage)?.HighlightSearchResults(this.searchResult);
                }

                return true;
            }

            return true;
        }

        private bool GoToDataGrid()
        {
            if (this.searchResult.ConciergePage is not Page conciergePage)
            {
                return false;
            }

            var dataGrids = DisplayUtility.FindVisualChildren<ConciergeDataGrid>(conciergePage);
            foreach (var dataGrid in dataGrids)
            {
                var index = dataGrid.Items.IndexOf(this.searchResult.Item);
                if (index >= 0)
                {
                    dataGrid.SetSelectedIndex(index);

                    return true;
                }
            }

            return false;
        }

        private bool GoToTextBlock()
        {
            if (this.searchResult.Item is not ConciergeTextBlock textBlock)
            {
                return false;
            }

            textBlock.Highlight();

            return true;
        }

        private bool GoToTreeView()
        {
            if (this.searchResult.ConciergePage is not Page conciergePage)
            {
                return false;
            }

            var treeViews = DisplayUtility.FindVisualChildren<ConciergeTreeView>(conciergePage);
            foreach (var treeView in treeViews)
            {
                var item = treeView.GetTreeViewItem(this.searchResult.Item);

                if (item is null)
                {
                    return false;
                }

                item.IsSelected = true;
                item.Focus();

                if (item is DocumentTreeViewItem && item.Parent is ChapterTreeViewItem parentItem)
                {
                    parentItem.IsExpanded = true;
                }

                return true;
            }

            return false;
        }
    }
}
