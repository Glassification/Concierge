// <copyright file="ConciergeTreeView.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Character.Journals;
    using Concierge.Common;
    using Concierge.Common.Extensions;

    public sealed class ConciergeTreeView : TreeView
    {
        public ConciergeTreeView()
            : base()
        {
            var scaling = ResolutionScaling.DpiFactor;
            this.LayoutTransform = new ScaleTransform(scaling, scaling, 0.5, 0.5);
        }

        public TreeViewItem? GetTreeViewItem(object item)
        {
            foreach (var item1 in this.Items)
            {
                if (item1 is ChapterTreeViewItem chapter && item is ChapterTreeViewItem itemChapter)
                {
                    if (chapter.Chapter.Id.Equals(itemChapter.Chapter.Id))
                    {
                        return chapter;
                    }
                }

                if (item1 is TreeViewItem treeViewItem1)
                {
                    foreach (var item2 in treeViewItem1.Items)
                    {
                        if (item2 is DocumentTreeViewItem document && item is DocumentTreeViewItem itemDocument)
                        {
                            if (document.Document.Id.Equals(itemDocument.Document.Id))
                            {
                                return document;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public TreeViewItem? GetTreeViewItemByDocument(Document document)
        {
            foreach (var item1 in this.Items)
            {
                if (item1 is TreeViewItem treeViewItem1)
                {
                    foreach (var item2 in treeViewItem1.Items)
                    {
                        if (item2 is DocumentTreeViewItem documentItem)
                        {
                            if (documentItem.Document.Id.Equals(document.Id))
                            {
                                return documentItem;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public void ExpandAll()
        {
            ExpandHelper(this.Items, true);
        }

        public void CollapseAll()
        {
            ExpandHelper(this.Items, false);
        }

        public bool SetButtonControlsEnableState(params ConciergeDesignButton[] buttons)
        {
            var hasSelection = this.SelectedItem is not null;
            foreach (var button in buttons)
            {
                button.SetEnableState(hasSelection);
            }

            return hasSelection;
        }

        private static void ExpandHelper(ItemCollection items, bool expand)
        {
            foreach (var item in items)
            {
                if (item is not TreeViewItem treeViewItem)
                {
                    return;
                }

                if (!treeViewItem.Items.IsEmpty)
                {
                    ExpandHelper(treeViewItem.Items, expand);
                }

                treeViewItem.IsExpanded = expand;
            }
        }
    }
}
