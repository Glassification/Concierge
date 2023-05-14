// <copyright file="GlossaryWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows.Utility
{
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Common.Extensions;
    using Concierge.Data;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Persistence;

    /// <summary>
    /// Interaction logic for GlossaryWindow.xaml.
    /// </summary>
    public partial class GlossaryWindow : ConciergeWindow
    {
        public GlossaryWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();
        }

        public override string HeaderText => "Glossary";

        public override string WindowName => nameof(GlossaryWindow);

        public override ConciergeWindow? ShowNonBlockingWindow()
        {
            this.LoadTreeView();
            this.ShowNonBlockingConciergeWindow();

            return null;
        }

        private void LoadTreeView()
        {
            this.GlossaryTreeView.Items.Clear();
            foreach (var item in Defaults.Glossary)
            {
                if (!item.Search(this.SearchFilter.FilterText))
                {
                    continue;
                }

                var treeViewItem = new GlossaryTreeViewItem(item);

                if (!item.GlossaryEntries.IsEmpty())
                {
                    this.CreateItems(item, treeViewItem.Items);
                }

                this.GlossaryTreeView.Items.Add(treeViewItem);
            }
        }

        private void CreateItems(GlossaryEntry entry, ItemCollection itemCollection)
        {
            foreach (var item in entry.GlossaryEntries)
            {
                if (!item.Search(this.SearchFilter.FilterText))
                {
                    continue;
                }

                var treeViewItem = new GlossaryTreeViewItem(item);

                if (!item.GlossaryEntries.IsEmpty())
                {
                    this.CreateItems(item, treeViewItem.Items);
                }

                itemCollection.Add(treeViewItem);
            }
        }

        private void ClearSelection()
        {
            if (this.GlossaryTreeView.SelectedItem is TreeViewItem item)
            {
                item.IsSelected = false;
                this.MarkdownViewer.Markdown = string.Empty;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Result = ConciergeWindowResult.Exit;
            this.CloseConciergeWindow();
        }

        private void GlossaryTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.GlossaryTreeView.SelectedItem is not GlossaryTreeViewItem glossaryTreeViewItem)
            {
                return;
            }

            var markdown = glossaryTreeViewItem.GlossaryEntry.Markdown;
            if (markdown.IsNullOrWhiteSpace())
            {
                if (glossaryTreeViewItem.Items.GetItemAt(0) is not GlossaryTreeViewItem innerItem)
                {
                    return;
                }

                glossaryTreeViewItem.IsExpanded = true;
                innerItem.IsSelected = true;
            }
            else
            {
                ConciergeSound.TapNavigation();
                this.MarkdownViewer.Markdown = markdown;
            }
        }

        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            this.GlossaryTreeView.ExpandAll();
        }

        private void CollapseButton_Click(object sender, RoutedEventArgs e)
        {
            this.GlossaryTreeView.CollapseAll();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.ClearSelection();
        }

        private void GlossaryTreeView_Filtered(object sender, RoutedEventArgs e)
        {
            this.ClearSelection();
            this.LoadTreeView();
        }
    }
}
