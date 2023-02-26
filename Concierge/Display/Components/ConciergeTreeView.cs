// <copyright file="ConciergeTreeView.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Components
{
    using System.Windows.Controls;
    using System.Windows.Media;

    using Concierge.Utility;

    public sealed class ConciergeTreeView : TreeView
    {
        public ConciergeTreeView()
            : base()
        {
            var scaling = ResolutionScaling.DpiFactor;
            this.LayoutTransform = new ScaleTransform(scaling, scaling, 0.5, 0.5);
        }

        public void ExpandAll()
        {
            this.ExpandHelper(this.Items, true);
        }

        public void CollapseAll()
        {
            this.ExpandHelper(this.Items, false);
        }

        private void ExpandHelper(ItemCollection items, bool expand)
        {
            foreach (var item in items)
            {
                if (item is not TreeViewItem treeViewItem)
                {
                    return;
                }

                if (!treeViewItem.Items.IsEmpty)
                {
                    this.ExpandHelper(treeViewItem.Items, expand);
                }

                treeViewItem.IsExpanded = expand;
            }
        }
    }
}
