// <copyright file="TreeViewExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Extensions
{
    using System.Windows.Controls;

    using Concierge.Interfaces.Components;

    public static class TreeViewExtensions
    {
        public static TreeViewItem GetTreeViewItem(this TreeView treeView, object item)
        {
            foreach (var item1 in treeView.Items)
            {
                if (item1 is ChapterTreeViewItem)
                {
                    var chapter = item1 as ChapterTreeViewItem;

                    if (chapter.Chapter.Equals(item))
                    {
                        return chapter;
                    }
                }

                foreach (var item2 in (item1 as TreeViewItem).Items)
                {
                    if (item2 is DocumentTreeViewItem)
                    {
                        var document = item2 as DocumentTreeViewItem;

                        if (document.Document.Equals(item))
                        {
                            return document;
                        }
                    }
                }
            }

            return null;
        }

        public static bool SetSelectedItem(this TreeView treeView, object item)
        {
            return SetSelected(treeView, item);
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
                    var childControl = parent.ItemContainerGenerator.ContainerFromItem(childItem) as ItemsControl;
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
