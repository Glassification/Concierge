// <copyright file="NotesPage.xaml.cs" company="Thomas Beckett">
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
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Character.Notes;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Primitives;
    using Concierge.Services;
    using Concierge.Tools.Interface;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    using SearchResult = Concierge.Search.SearchResult;

    /// <summary>
    /// Interaction logic for NotesPage.xaml.
    /// </summary>
    public partial class NotesPage : Page, IConciergePage
    {
        private const int MaxUndoQueue = 25;

        private Document? selectedDocument;

        public NotesPage()
        {
            this.InitializeComponent();
            this.SelectedDocument = null;
            this.Lock = false;
            this.FontFamilyList.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            this.FontSizeList.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            this.NotesTextBox.UndoLimit = MaxUndoQueue;
            this.NotesTreeView.Background = ConciergeColors.TreeViewBackground;

            this.SetDefaultFontStyle();
            this.ClearTextBox();
        }

        public Document? SelectedDocument
        {
            get
            {
                return this.selectedDocument;
            }

            set
            {
                this.selectedDocument = value;
                this.NotesTextBox.IsEnabled = value != null;
                this.ToolbarStackPanel.IsEnabled = value != null;
                this.ToolbarStackPanel.Opacity = value is null ? 0.5 : 1;
            }
        }

        public ConciergePage ConciergePage => ConciergePage.Notes;

        public bool HasEditableDataGrid => false;

        private bool Lock { get; set; }

        private bool SelectionLock { get; set; }

        public void Draw()
        {
            this.DrawTreeView();
        }

        public void Edit(object itemToEdit)
        {
            throw new NotImplementedException();
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
                this.SelectedDocument.Rtf = this.SaveCurrentDocument();
                Program.Modify();
            }
        }

        public void HighlightSearchResults(SearchResult searchResult)
        {
            this.ClearHighlightSelection();

            var flowDocument = this.NotesTextBox.Document;
            for (
                var startPointer = flowDocument.ContentStart;
                startPointer is not null && startPointer.CompareTo(flowDocument.ContentEnd) <= 0;
                startPointer = startPointer?.GetNextContextPosition(LogicalDirection.Forward))
            {
                if (startPointer.CompareTo(flowDocument.ContentEnd) == 0)
                {
                    break;
                }

                var parsedString = startPointer.GetTextInRun(LogicalDirection.Forward);
                if (parsedString.IsNullOrWhiteSpace())
                {
                    continue;
                }

                var match = searchResult.SearchRegex.Match(parsedString);
                if (!match.Success)
                {
                    continue;
                }

                var indexOfParseString = match.Index;
                startPointer = startPointer.GetPositionAtOffset(indexOfParseString);
                if (startPointer is not null)
                {
                    var nextPointer = startPointer.GetPositionAtOffset(searchResult.SearchText.Length);
                    var searchedTextRange = new TextRange(startPointer, nextPointer);
                    searchedTextRange.ApplyPropertyValue(TextElement.BackgroundProperty, ConciergeColors.HighlightRichTextBox);
                }
            }
        }

        public void ClearHighlightSelection()
        {
            this.NotesTextBox.SelectAll();
            this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, ConciergeColors.TotalLightBoxBrush);
        }

        private void DrawTreeView()
        {
            var selectedItem = this.NotesTreeView.SelectedItem;
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

                treeViewChapter.Items.Add(new ButtonTreeViewItem(this.AddDocumentButton_Click, chapter));
                this.NotesTreeView.Items.Add(treeViewChapter);
            }

            this.NotesTreeView.Items.Add(new ButtonTreeViewItem(this.AddChapterButton_Click, null));

            if (selectedItem is not null)
            {
                var item = this.NotesTreeView.GetTreeViewItem(selectedItem);

                if (item is not null)
                {
                    item.IsSelected = true;
                    item.Focus();
                }
            }
        }

        private void SetDefaultFontStyle()
        {
            this.NotesTextBox.FontSize = 20;
            this.NotesTextBox.FontFamily = new FontFamily("Calibri");
            this.NotesTextBox.Foreground = Brushes.White;
        }

        private void LoadCurrentDocument(string text)
        {
            var dataFormat = DataFormats.Rtf;

            if (!text.IsRtf())
            {
                text = string.Empty;
                dataFormat = DataFormats.Text;
                this.SetDefaultFontStyle();
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
                Program.ErrorService.LogError(ex);
                return string.Empty;
            }
        }

        private void NotesTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.Lock)
            {
                return;
            }

            this.Lock = true;

            if (this.NotesTreeView?.SelectedItem is DocumentTreeViewItem treeViewItem)
            {
                if (this.SelectedDocument != null)
                {
                    this.SaveTextBox();
                }

                this.NotesTextBox.IsUndoEnabled = false;
                this.NotesTextBox.IsUndoEnabled = true;
                this.SelectedDocument = treeViewItem.Document;
                this.LoadCurrentDocument(this.SelectedDocument.Rtf);
                this.ResetUndoQueue();
                ConciergeSound.TapNavigation();
            }
            else if (this.NotesTreeView?.SelectedItem is ChapterTreeViewItem)
            {
                if (this.SelectedDocument != null)
                {
                    this.SaveTextBox();
                }

                this.NotesTextBox.IsUndoEnabled = false;
                this.NotesTextBox.IsUndoEnabled = true;
                this.SelectedDocument = null;
                this.ClearTextBox();
                this.ResetUndoQueue();
                ConciergeSound.TapNavigation();
            }

            this.Lock = false;
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
            if (this.ButtonBold.IsChecked ?? false)
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
            if (this.ButtonItalic.IsChecked ?? false)
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
            if (this.ButtonUnderline.IsChecked ?? false)
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
            this.SelectionLock = true;
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
            this.ColorPicker.SelectedColor = obj == DependencyProperty.UnsetValue ? CustomColor.White : new CustomColor(obj.ToString() ?? "White", true);

            this.SelectionLock = false;
        }

        private void FontFamilyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SelectionLock)
            {
                return;
            }

            if (this.FontFamilyList.SelectedItem != null)
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, this.FontFamilyList.SelectedItem);
            }
        }

        private void FontSizeList_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.SelectionLock)
            {
                return;
            }

            if (double.TryParse(this.FontSizeList.Text, out _))
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, this.FontSizeList.Text);
            }
        }

        private void ResetUndoQueue()
        {
            this.NotesTextBox.UndoLimit = 0;
            this.NotesTextBox.UndoLimit = MaxUndoQueue;
        }

        private void ClearWorkspace()
        {
            this.SaveTextBox();
            this.ClearTextBox();
            this.ResetUndoQueue();

            if (this.NotesTreeView?.SelectedItem is TreeViewItem item)
            {
                item.IsSelected = false;
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this.ClearWorkspace();
        }

        private void MoveTreeViewItem(int increment, bool useZero, Func<int, int, bool> func)
        {
            if (this.NotesTreeView.SelectedItem is DocumentTreeViewItem document && this.SelectedDocument is not null)
            {
                if (document.Parent is not ChapterTreeViewItem chapterItem)
                {
                    return;
                }

                var chapterIndex = this.NotesTreeView.Items.IndexOf(chapterItem);
                var index = chapterItem.Items.IndexOf(document);

                if (func(index, useZero ? 0 : chapterItem.Items.Count - 2))
                {
                    this.SelectedDocument.Rtf = this.SaveCurrentDocument();
                    this.SwapTreeViewItem(chapterItem.Chapter.Documents, index, index + increment);

                    var newIndex = (this.NotesTreeView.Items[chapterIndex] as ChapterTreeViewItem)?.Items[index + increment];
                    if (newIndex is DocumentTreeViewItem documentIndex)
                    {
                        documentIndex.IsSelected = true;
                    }

                    (this.NotesTreeView.SelectedItem as TreeViewItem)?.Focus();
                    Program.Modify();
                }
            }
            else if (this.NotesTreeView.SelectedItem is ChapterTreeViewItem chapter)
            {
                var index = this.NotesTreeView.Items.IndexOf(chapter);

                if (func(index, useZero ? 0 : this.NotesTreeView.Items.Count - 2))
                {
                    this.SwapTreeViewItem(Program.CcsFile.Character.Chapters, index, index + increment);
                    var newIndex = this.NotesTreeView.Items[index + increment];
                    if (newIndex is ChapterTreeViewItem chapterIndex)
                    {
                        chapterIndex.IsSelected = true;
                    }

                    (this.NotesTreeView.SelectedItem as TreeViewItem)?.Focus();
                    Program.Modify();
                }
            }
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
            if (sender is ChapterTreeViewItem chapterItem)
            {
                chapterItem.Chapter.IsExpanded = true;
            }
            else if (sender is DocumentTreeViewItem documentItem)
            {
                documentItem.Document.IsExpanded = true;
            }
        }

        private void TreeViewItem_Collapsed(object sender, RoutedEventArgs e)
        {
            if (sender is ChapterTreeViewItem chapterItem)
            {
                chapterItem.Chapter.IsExpanded = false;
            }
            else if (sender is DocumentTreeViewItem documentItem)
            {
                documentItem.Document.IsExpanded = false;
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            this.MoveTreeViewItem(1, false, (x, y) => x < y);
        }

        private void AddChapterButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not ConciergeDesignButton button)
            {
                return;
            }

            ConciergeWindowService.ShowAdd<Chapter?>(
                button.Tag as Chapter,
                typeof(ModifyNotesWindow),
                this.Window_ApplyChanges,
                ConciergePage.Notes);
            this.Draw();
        }

        private void AddDocumentButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not ConciergeDesignButton button)
            {
                return;
            }

            ConciergeWindowService.ShowAdd<Chapter?>(
                button.Tag as Chapter,
                typeof(ModifyNotesWindow),
                this.Window_ApplyChanges,
                ConciergePage.Notes);
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
                ConciergeWindowService.ShowEdit<Chapter>(
                    chapterTreeViewItem.Chapter,
                    typeof(ModifyNotesWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.Notes);
            }
            else if (this.NotesTreeView.SelectedItem is DocumentTreeViewItem documentTreeViewItem)
            {
                ConciergeWindowService.ShowEdit<Document>(
                    documentTreeViewItem.Document,
                    typeof(ModifyNotesWindow),
                    this.Window_ApplyChanges,
                    ConciergePage.Notes);
            }

            this.Draw();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.NotesTreeView.SelectedItem == null)
            {
                return;
            }

            if (this.NotesTreeView.SelectedItem is ChapterTreeViewItem chapterTreeViewItem)
            {
                var result = ConciergeMessageBox.Show(
                    "Are you sure yo want to delete this chapter and all pages within?",
                    "Warning",
                    ConciergeWindowButtons.YesNo,
                    ConciergeWindowIcons.Question);

                if (result != ConciergeWindowResult.Yes)
                {
                    return;
                }

                var index = Program.CcsFile.Character.Chapters.IndexOf(chapterTreeViewItem.Chapter);
                Program.UndoRedoService.AddCommand(new DeleteCommand<Chapter>(Program.CcsFile.Character.Chapters, chapterTreeViewItem.Chapter, index, this.ConciergePage));
                Program.CcsFile.Character.Chapters.Remove(chapterTreeViewItem.Chapter);
            }
            else if (this.NotesTreeView.SelectedItem is DocumentTreeViewItem documentTreeViewItem)
            {
                var result = ConciergeMessageBox.Show(
                    "Are you sure yo want to delete this page?",
                    "Warning",
                    ConciergeWindowButtons.YesNo,
                    ConciergeWindowIcons.Question);

                if (result != ConciergeWindowResult.Yes)
                {
                    return;
                }

                var chapter = Program.CcsFile.Character.GetChapter(documentTreeViewItem.Document.Id);
                var index = chapter.Documents.IndexOf(documentTreeViewItem.Document);
                Program.UndoRedoService.AddCommand(new DeleteCommand<Document>(chapter.Documents, documentTreeViewItem.Document, index, this.ConciergePage));
                chapter.Documents.Remove(documentTreeViewItem.Document);
            }

            this.ClearWorkspace();
            this.Draw();

            Program.Modify();
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(ModifyNotesWindow):
                    this.DrawTreeView();
                    break;
            }
        }

        private void NotesTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Program.IsTyping = true;
        }

        private void NotesTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Program.IsTyping = false;
        }

        private void NotesTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.IBeam;
        }

        private void NotesTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void ColorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            if (this.SelectionLock)
            {
                return;
            }

            this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, this.ColorPicker.SelectedColor.Color.ToString());
        }
    }
}
