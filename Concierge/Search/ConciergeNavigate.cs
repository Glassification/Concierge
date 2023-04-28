// <copyright file="ConciergeNavigate.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Search
{
    using System.Windows.Controls;

    using Concierge.Character.Journal;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Display.Components;
    using Concierge.Display.Pages;
    using Concierge.Display.Windows;

    public sealed class ConciergeNavigate
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

            if (this.NavigateToTextBlock())
            {
                return;
            }

            if (this.NavigateToTreeView())
            {
                return;
            }

            if (this.NavigateToDocument())
            {
                return;
            }
        }

        private bool NavigateToDocument()
        {
            if (this.SearchResult.ConciergePage is not Page conciergePage || this.SearchResult.Item is not Document document)
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

                    (this.SearchResult.ConciergePage as JournalPage)?.HighlightSearchResults(this.SearchResult);
                }

                return true;
            }

            return true;
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

            var treeViews = DisplayUtility.FindVisualChildren<ConciergeTreeView>(conciergePage);
            foreach (var treeView in treeViews)
            {
                var item = treeView.GetTreeViewItem(this.SearchResult.Item);

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
