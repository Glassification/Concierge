﻿// <copyright file="NotesPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.NotesPageInterface
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    using Concierge.Character.Notes;
    using Concierge.Exceptions.Enums;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Interfaces.HelperInterface;
    using Concierge.Utility.Extensions;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Interaction logic for NotesPage.xaml.
    /// </summary>
    public partial class NotesPage : Page, IConciergePage
    {
        private const int MaxUndoQueue = 10;

        private readonly ModifyNotesWindow modifyNotesWindow = new ();
        private readonly ConciergeMessageWindow conciergeMessageWindow = new ();

        public NotesPage()
        {
            this.InitializeComponent();
            this.SelectedDocument = null;
            this.Lock = false;
            this.FontFamilyList.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            this.FontSizeList.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            this.NotesTextBox.UndoLimit = MaxUndoQueue;

            this.NotesTextBox.FontSize = 20;
            this.NotesTextBox.Foreground = Brushes.White;

            this.modifyNotesWindow.ApplyChanges += this.Window_ApplyChanges;
        }

        public Document SelectedDocument { get; set; }

        private bool Lock { get; set; }

        public void Draw()
        {
            this.DrawTreeView();
        }

        public void ClearTextBox()
        {
            this.NotesTextBox.Document.Blocks.Clear();
            this.SelectedDocument = null;
        }

        public void SaveTextBox()
        {
            if (this.SelectedDocument != null)
            {
                var savedDocument = this.SaveCurrentDocument();

                if (!savedDocument.Equals(this.SelectedDocument.RTF))
                {
                    Program.Modify();
                }

                this.SelectedDocument.RTF = savedDocument;
            }
        }

        private void DrawTreeView()
        {
            this.NotesTreeView.Items.Clear();

            foreach (var chapter in Program.CcsFile.Character.Chapters)
            {
                var treeViewChapter = new ChapterTreeViewItem(chapter);
                treeViewChapter.Expanded += this.TreeViewItem_Expanded;
                treeViewChapter.Collapsed += this.TreeViewItem_Collapsed;

                foreach (var document in chapter.Documents)
                {
                    var treeViewDocument = new DocumentTreeViewItem(document);
                    treeViewDocument.Expanded += this.TreeViewItem_Expanded;
                    treeViewDocument.Collapsed += this.TreeViewItem_Collapsed;

                    treeViewChapter.Items.Add(treeViewDocument);
                }

                this.NotesTreeView.Items.Add(treeViewChapter);
            }
        }

        private void LoadCurrentDocument(string text)
        {
            var dataFormat = DataFormats.Rtf;

            if (text == null)
            {
                text = string.Empty;
                dataFormat = DataFormats.Text;
                this.NotesTextBox.FontSize = 20;
                this.NotesTextBox.Foreground = Brushes.White;
            }

            var stream = new MemoryStream(Encoding.Default.GetBytes(text));
            this.NotesTextBox.Document.Blocks.Clear();
            this.NotesTextBox.Selection.Load(stream, dataFormat);
        }

        private string SaveCurrentDocument()
        {
            var range = new TextRange(this.NotesTextBox.Document.ContentStart, this.NotesTextBox.Document.ContentEnd);

            try
            {
                using var rtfMemoryStream = new MemoryStream();
                using var rtfStreamWriter = new StreamWriter(rtfMemoryStream);
                range.Save(rtfMemoryStream, DataFormats.Rtf);

                rtfMemoryStream.Flush();
                rtfMemoryStream.Position = 0;
                var sr = new StreamReader(rtfMemoryStream);

                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex, Severity.Release);
                return string.Empty;
            }
        }

        private void NotesTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!this.Lock)
            {
                this.Lock = true;

                if (this.NotesTreeView?.SelectedItem is DocumentTreeViewItem)
                {
                    var treeViewItem = this.NotesTreeView?.SelectedItem as DocumentTreeViewItem;
                    if (this.SelectedDocument != null)
                    {
                        this.SaveTextBox();
                    }

                    this.NotesTextBox.IsUndoEnabled = false;
                    this.NotesTextBox.IsUndoEnabled = true;
                    this.SelectedDocument = treeViewItem.Document;
                    this.LoadCurrentDocument(this.SelectedDocument.RTF);
                    this.ResetUndoQueue();
                }

                this.Lock = false;
            }
        }

        private void ButtonCut_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.NotesTextBox.Selection.Text);
            this.NotesTextBox.Selection.Text = string.Empty;
        }

        private void ButtonCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.NotesTextBox.Selection.Text);
        }

        private void ButtonPaste_Click(object sender, RoutedEventArgs e)
        {
            this.NotesTextBox.Selection.Text = Clipboard.GetText();
        }

        private void ButtonUndo_Click(object sender, RoutedEventArgs e)
        {
            this.NotesTextBox.Undo();
        }

        private void ButtonRedo_Click(object sender, RoutedEventArgs e)
        {
            this.NotesTextBox.Redo();
        }

        private void ButtonBold_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)this.ButtonBold.IsChecked)
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
            else
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            }
        }

        private void ButtonItalic_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)this.ButtonItalic.IsChecked)
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
            }
            else
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
            }
        }

        private void ButtonUnderline_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)this.ButtonUnderline.IsChecked)
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
            else
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
            }
        }

        private void NotesTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object obj;

            obj = this.NotesTextBox.Selection.GetPropertyValue(TextElement.FontWeightProperty);
            this.ButtonBold.IsChecked = (obj != DependencyProperty.UnsetValue) && obj.Equals(FontWeights.Bold);

            obj = this.NotesTextBox.Selection.GetPropertyValue(TextElement.FontStyleProperty);
            this.ButtonItalic.IsChecked = (obj != DependencyProperty.UnsetValue) && obj.Equals(FontStyles.Italic);

            obj = this.NotesTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            this.ButtonUnderline.IsChecked = (obj != DependencyProperty.UnsetValue) && obj.Equals(TextDecorations.Underline);

            obj = this.NotesTextBox.Selection.GetPropertyValue(TextElement.FontFamilyProperty);
            this.FontFamilyList.SelectedItem = obj;

            obj = this.NotesTextBox.Selection.GetPropertyValue(TextElement.FontSizeProperty);
            this.FontSizeList.Text = obj == DependencyProperty.UnsetValue ? string.Empty : obj.ToString();

            obj = this.NotesTextBox.Selection.GetPropertyValue(TextElement.ForegroundProperty);
            this.ColorPicker.SelectedColor = obj == DependencyProperty.UnsetValue ? Colors.White : (Color)ColorConverter.ConvertFromString(obj.ToString());
        }

        private void FontFamilyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.FontFamilyList.SelectedItem != null)
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, this.FontFamilyList.SelectedItem);
            }
        }

        private void FontSizeList_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(this.FontSizeList.Text, out _))
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, this.FontSizeList.Text);
            }
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, this.ColorPicker.SelectedColor.ToString());
        }

        private void ResetUndoQueue()
        {
            this.NotesTextBox.UndoLimit = 0;
            this.NotesTextBox.UndoLimit = MaxUndoQueue;
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this.SaveTextBox();
            this.ClearTextBox();
            this.ResetUndoQueue();

            if (this.NotesTreeView?.SelectedItem is TreeViewItem item)
            {
                item.IsSelected = false;
            }
        }

        private void MoveTreeViewItem(int increment, bool useZero, Func<int, int, bool> func)
        {
            if (this.NotesTreeView?.SelectedItem == null)
            {
                return;
            }

            Program.Modify();

            if (this.NotesTreeView.SelectedItem is DocumentTreeViewItem)
            {
                var item = this.NotesTreeView.SelectedItem as DocumentTreeViewItem;
                var chapterItem = item.Parent as ChapterTreeViewItem;
                var chapterIndex = this.NotesTreeView.Items.IndexOf(chapterItem);
                var index = chapterItem.Items.IndexOf(item);

                if (func(index, useZero ? 0 : chapterItem.Items.Count - 1))
                {
                    this.SelectedDocument.RTF = this.SaveCurrentDocument();
                    this.SwapTreeViewItem(chapterItem.Chapter.Documents, index, index + increment);

                    var newIndex = (this.NotesTreeView.Items[chapterIndex] as ChapterTreeViewItem).Items[index + increment];
                    (newIndex as DocumentTreeViewItem).IsSelected = true;
                }
            }
            else
            {
                var item = this.NotesTreeView.SelectedItem as ChapterTreeViewItem;
                var index = this.NotesTreeView.Items.IndexOf(item);

                if (func(index, useZero ? 0 : this.NotesTreeView.Items.Count - 1))
                {
                    this.SwapTreeViewItem(Program.CcsFile.Character.Chapters, index, index + increment);
                    var newIndex = this.NotesTreeView.Items[index + increment];
                    (newIndex as ChapterTreeViewItem).IsSelected = true;
                }
            }

            (this.NotesTreeView.SelectedItem as TreeViewItem).Focus();
        }

        private void SwapTreeViewItem<T>(IList<T> list, int oldIndex, int newIndex)
        {
            list.Swap(oldIndex, newIndex);
            this.Lock = true;
            this.DrawTreeView();
            this.Lock = false;
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            this.MoveTreeViewItem(-1, true, (x, y) => x > y);
        }

        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender is ChapterTreeViewItem)
            {
                var chapterItem = sender as ChapterTreeViewItem;
                chapterItem.Chapter.IsExpanded = true;
            }
            else
            {
                var documentItem = sender as DocumentTreeViewItem;
                documentItem.Document.IsExpanded = true;
            }
        }

        private void TreeViewItem_Collapsed(object sender, RoutedEventArgs e)
        {
            if (sender is ChapterTreeViewItem)
            {
                var chapterItem = sender as ChapterTreeViewItem;
                chapterItem.Chapter.IsExpanded = false;
            }
            else
            {
                var documentItem = sender as DocumentTreeViewItem;
                documentItem.Document.IsExpanded = false;
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            this.MoveTreeViewItem(1, false, (x, y) => x < y);
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            this.modifyNotesWindow.ShowAdd();
            this.Draw();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.NotesTreeView.SelectedItem == null)
            {
                return;
            }

            if (this.NotesTreeView.SelectedItem is ChapterTreeViewItem chapterTreeViewItem)
            {
                this.modifyNotesWindow.ShowEdit(chapterTreeViewItem.Chapter);
            }
            else if (this.NotesTreeView.SelectedItem is DocumentTreeViewItem documentTreeViewItem)
            {
                this.modifyNotesWindow.ShowEdit(documentTreeViewItem.Document);
            }

            this.Draw();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.NotesTreeView.SelectedItem == null)
            {
                return;
            }

            Program.Modify();

            if (this.NotesTreeView.SelectedItem is ChapterTreeViewItem chapterTreeViewItem)
            {
                var result = this.conciergeMessageWindow.ShowWindow(
                    "Are you sure yo want to delete this chapter and all pages within?",
                    "Warning",
                    ConciergeWindowButtons.YesNo,
                    ConciergeWindowIcons.Question);

                if (result != ConciergeWindowResult.Yes)
                {
                    return;
                }

                Program.CcsFile.Character.Chapters.Remove(chapterTreeViewItem.Chapter);
            }
            else if (this.NotesTreeView.SelectedItem is DocumentTreeViewItem documentTreeViewItem)
            {
                var result = this.conciergeMessageWindow.ShowWindow(
                    "Are you sure yo want to delete this page?",
                    "Warning",
                    ConciergeWindowButtons.YesNo,
                    ConciergeWindowIcons.Question);

                if (result != ConciergeWindowResult.Yes)
                {
                    return;
                }

                var chapter = Program.CcsFile.Character.GetChapterByDocumentId(documentTreeViewItem.Document.Id);
                chapter.Documents.Remove(documentTreeViewItem.Document);
            }

            this.Draw();
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case "ModifyNotesWindow":
                    this.DrawTreeView();
                    break;
            }
        }
    }
}