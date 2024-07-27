// <copyright file="TreeViewExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Extensions
{
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Provides extension methods for working with TreeView controls.
    /// </summary>
    public static class TreeViewExtensions
    {
        /// <summary>
        /// Retrieves the parent TreeViewItem of the specified TreeViewItem.
        /// </summary>
        /// <param name="item">The TreeViewItem for which to find the parent.</param>
        /// <returns>The parent TreeViewItem of the specified item, or null if the parent is not found.</returns>
        public static TreeViewItem? GetSelectedTreeViewItemParent(this TreeViewItem item)
        {
            var parent = VisualTreeHelper.GetParent(item);
            while (parent is not null && parent is not TreeViewItem & parent is not TreeView)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as TreeViewItem;
        }

        /// <summary>
        /// Sets the specified item as selected within the parent ItemsControl.
        /// </summary>
        /// <param name="parent">The parent ItemsControl in which to set the selected item.</param>
        /// <param name="child">The item to set as selected.</param>
        /// <returns>True if the item is successfully set as selected, false otherwise.</returns>
        private static bool SetSelected(ItemsControl parent, object child)
        {
            if (parent is null || child is null)
            {
                return false;
            }

            if (parent.ItemContainerGenerator.ContainerFromItem(child) is TreeViewItem childNode)
            {
                childNode.Focus();
                return childNode.IsSelected = true;
            }

            if (parent.Items.Count == 0)
            {
                foreach (var childItem in parent.Items)
                {
                    if (parent.ItemContainerGenerator.ContainerFromItem(childItem) is not ItemsControl childControl)
                    {
                        continue;
                    }

                    if (SetSelected(childControl, child))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
