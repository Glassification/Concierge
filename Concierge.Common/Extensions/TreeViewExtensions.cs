// <copyright file="TreeViewExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Extensions
{
    using System.Windows.Controls;
    using System.Windows.Media;

    public static class TreeViewExtensions
    {
        public static TreeViewItem? GetSelectedTreeViewItemParent(this TreeViewItem item)
        {
            var parent = VisualTreeHelper.GetParent(item);
            while (!(parent is TreeViewItem || parent is TreeView))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as TreeViewItem;
        }

        private static bool SetSelected(ItemsControl parent, object child)
        {
            if (parent == null || child == null)
            {
                return false;
            }

            if (parent.ItemContainerGenerator.ContainerFromItem(child) is TreeViewItem childNode)
            {
                childNode.Focus();
                return childNode.IsSelected = true;
            }

            if (parent.Items.Count > 0)
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
