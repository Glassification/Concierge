// <copyright file="JournalPage.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Pages
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Character.Journals;
    using Concierge.Commands;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Data;
    using Concierge.Display;
    using Concierge.Display.Components;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;
    using Concierge.Display.Windows;
    using Concierge.Search;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for JournalPage.xaml.
    /// </summary>
    public partial class JournalPage : ConciergePage
    {
        private const int MaxUndoQueue = 25;

        private Document? selectedDocument;
        private bool isLoading;
        private bool isLocked;
        private bool selectionLock;

        public JournalPage()
        {
            this.InitializeComponent();

            this.SelectedDocument = null;
            this.FontFamilyList.ItemsSource = ComboBoxGenerator.FontFamilyComboBox();
            this.FontSizeList.ItemsSource = ComboBoxGenerator.FontSizeComboBox();
            this.NotesTextBox.UndoLimit = MaxUndoQueue;
            this.HasEditableDataGrid = false;
            this.ConciergePages = ConciergePages.Journal;

            this.BoldButton.Initialize(ConciergeBrushes.Deer);
            this.ItalicButton.Initialize(ConciergeBrushes.Deer);
            this.UnderlineButton.Initialize(ConciergeBrushes.Deer);

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
                this.NotesTextBox.IsEnabled = value is not null;

                DisplayUtility.SetControlEnableState(this.ToolbarStackPanel, value is not null);
            }
        }

        public override void Draw(bool isNewCharacterSheet = false)
        {
            this.DrawTreeView();
            if (isNewCharacterSheet)
            {
                this.ClearTextBox();
            }
        }

        public override void Edit(object itemToEdit)
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
            if (this.SelectedDocument is not null)
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
            this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, ConciergeColors.ControlForeBlue);
        }

        private void DrawTreeView()
        {
            var selectedItem = this.NotesTreeView.SelectedItem;
            this.NotesTreeView.Items.Clear();

            foreach (var chapter in Program.CcsFile.Character.Journal.Chapters)
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

            this.SetNotesTreeViewControlState();
        }

        private void SetDefaultFontStyle()
        {
            this.NotesTextBox.FontSize = FontService.DefaultSize;
            this.NotesTextBox.FontFamily = FontService.DefaultFont;
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
                    this.SwapTreeViewItem(Program.CcsFile.Character.Journal.Chapters, index, index + increment);
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
            this.isLocked = true;
            this.DrawTreeView();
            this.isLocked = false;
        }

        private void SetNotesTreeViewControlState()
        {
            this.NotesTreeView.SetButtonControlsEnableState(this.UpButton, this.DownButton, this.EditButton, this.DeleteButton);
        }

        private bool IsCaretAtEnd()
        {
            var contentEnd = this.NotesTextBox.Document.ContentEnd;
            var caretPosition = this.NotesTextBox.CaretPosition;
            var compare = contentEnd.CompareTo(caretPosition);
            if (compare > 0)
            {
                var backOne = contentEnd.GetPositionAtOffset(-1, LogicalDirection.Backward);
                return backOne.CompareTo(caretPosition) == 0;
            }

            return contentEnd.CompareTo(caretPosition) == 0;
        }

        private void CutButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.NotesTextBox.Selection.Text);
            this.NotesTextBox.Selection.Text = string.Empty;
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.NotesTextBox.Selection.Text);
        }

        private void PasteButton_Click(object sender, RoutedEventArgs e)
        {
            this.NotesTextBox.Selection.Text = Clipboard.GetText();
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            this.NotesTextBox.Undo();
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            this.NotesTextBox.Redo();
        }

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.BoldButton.IsChecked ?? false)
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }
            else
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            }
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ItalicButton.IsChecked ?? false)
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
            }
            else
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
            }
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.UnderlineButton.IsChecked ?? false)
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
            else
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
            }
        }

        private void FontFamilyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.selectionLock)
            {
                return;
            }

            if (this.FontFamilyList.SelectedItem is ComboBoxItemControl control && control.Tag is FontFamily fontFamily)
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, fontFamily);
            }
        }

        private void FontSizeList_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.selectionLock)
            {
                return;
            }

            if (double.TryParse(this.FontSizeList.Text, out _))
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, this.FontSizeList.Text);
            }
        }

        private void NotesTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.isLocked)
            {
                return;
            }

            this.isLocked = true;
            if (this.NotesTreeView?.SelectedItem is DocumentTreeViewItem treeViewItem)
            {
                if (this.SelectedDocument is not null)
                {
                    this.SaveTextBox();
                }

                this.isLoading = true;
                this.NotesTextBox.IsUndoEnabled = false;
                this.NotesTextBox.IsUndoEnabled = true;
                this.SelectedDocument = treeViewItem.Document;
                this.LoadCurrentDocument(this.SelectedDocument.Rtf);
                this.ResetUndoQueue();
                SoundService.PlayNavigation();
            }
            else if (this.NotesTreeView?.SelectedItem is ChapterTreeViewItem)
            {
                if (this.SelectedDocument is not null)
                {
                    this.SaveTextBox();
                }

                this.NotesTextBox.IsUndoEnabled = false;
                this.NotesTextBox.IsUndoEnabled = true;
                this.SelectedDocument = null;
                this.ClearTextBox();
                this.ResetUndoQueue();
                SoundService.PlayNavigation();
            }

            this.SetNotesTreeViewControlState();
            this.isLocked = false;
        }

        private void NotesTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (this.IsCaretAtEnd() && !this.isLoading)
            {
                return;
            }

            this.selectionLock = true;
            object obj;

            obj = this.NotesTextBox.Selection.GetPropertyValue(TextElement.FontWeightProperty);
            this.BoldButton.IsChecked = (obj != DependencyProperty.UnsetValue) && obj.Equals(FontWeights.Bold);

            obj = this.NotesTextBox.Selection.GetPropertyValue(TextElement.FontStyleProperty);
            this.ItalicButton.IsChecked = (obj != DependencyProperty.UnsetValue) && obj.Equals(FontStyles.Italic);

            obj = this.NotesTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            this.UnderlineButton.IsChecked = (obj != DependencyProperty.UnsetValue) && obj.Equals(TextDecorations.Underline);

            obj = this.NotesTextBox.Selection.GetPropertyValue(TextElement.FontFamilyProperty);
            this.FontFamilyList.Text = obj is FontFamily fontFamily ? fontFamily.Source : FontService.DefaultFont.Source;

            obj = this.NotesTextBox.Selection.GetPropertyValue(TextElement.FontSizeProperty);
            this.FontSizeList.Text = obj == DependencyProperty.UnsetValue ? string.Empty : obj.ToString();

            obj = this.NotesTextBox.Selection.GetPropertyValue(TextElement.ForegroundProperty);
            this.ColorPicker.SelectedColor = obj == DependencyProperty.UnsetValue ? CustomColor.White : new CustomColor(obj.ToString() ?? "White", true);

            this.isLoading = false;
            this.selectionLock = false;
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

        private void ColorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            if (!this.selectionLock)
            {
                this.NotesTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, this.ColorPicker.SelectedColor.Color.ToString());
            }
        }

        private void AddChapterButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not ConciergeDesignButton button)
            {
                return;
            }

            WindowService.ShowAdd(
                button.Tag as Chapter,
                typeof(JournalWindow),
                this.Window_ApplyChanges,
                ConciergePages.Journal);
            this.Draw();
        }

        private void AddDocumentButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not ConciergeDesignButton button)
            {
                return;
            }

            WindowService.ShowAdd(
                button.Tag as Chapter,
                typeof(JournalWindow),
                this.Window_ApplyChanges,
                ConciergePages.Journal);
            this.Draw();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NotesTreeView.SelectedItem is null)
            {
                return;
            }

            if (this.NotesTreeView.SelectedItem is ChapterTreeViewItem chapterTreeViewItem)
            {
                WindowService.ShowEdit(
                    chapterTreeViewItem.Chapter,
                    typeof(JournalWindow),
                    this.Window_ApplyChanges,
                    ConciergePages.Journal);
            }
            else if (this.NotesTreeView.SelectedItem is DocumentTreeViewItem documentTreeViewItem)
            {
                WindowService.ShowEdit(
                    documentTreeViewItem.Document,
                    typeof(JournalWindow),
                    this.Window_ApplyChanges,
                    ConciergePages.Journal);
            }

            this.Draw();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NotesTreeView.SelectedItem is null)
            {
                return;
            }

            if (this.NotesTreeView.SelectedItem is ChapterTreeViewItem chapterTreeViewItem)
            {
                var result = ConciergeMessageBox.Show(
                    "Are you sure yo want to delete this chapter and all pages within?",
                    "Warning",
                    ConciergeButtons.Yes | ConciergeButtons.No,
                    ConciergeIcons.Question);

                if (result != ConciergeResult.Yes)
                {
                    return;
                }

                var index = Program.CcsFile.Character.Journal.Chapters.IndexOf(chapterTreeViewItem.Chapter);
                Program.UndoRedoService.AddCommand(new DeleteCommand<Chapter>(Program.CcsFile.Character.Journal.Chapters, chapterTreeViewItem.Chapter, index, this.ConciergePages));
                Program.CcsFile.Character.Journal.Chapters.Remove(chapterTreeViewItem.Chapter);
            }
            else if (this.NotesTreeView.SelectedItem is DocumentTreeViewItem documentTreeViewItem)
            {
                var result = ConciergeMessageBox.Show(
                    "Are you sure yo want to delete this page?",
                    "Warning",
                    ConciergeButtons.Yes | ConciergeButtons.No,
                    ConciergeIcons.Question);

                if (result != ConciergeResult.Yes)
                {
                    return;
                }

                var chapter = Program.CcsFile.Character.Journal.GetChapter(documentTreeViewItem.Document.Id);
                var index = chapter.Documents.IndexOf(documentTreeViewItem.Document);
                Program.UndoRedoService.AddCommand(new DeleteCommand<Document>(chapter.Documents, documentTreeViewItem.Document, index, this.ConciergePages));
                chapter.Documents.Remove(documentTreeViewItem.Document);
            }

            this.ClearWorkspace();
            this.Draw();
        }

        private void NotesTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Program.Typing();
        }

        private void NotesTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Program.NotTyping();
        }

        private void NotesTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.IBeam;
        }

        private void NotesTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.ClearWorkspace();
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            this.MoveTreeViewItem(-1, true, (x, y) => x > y);
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            this.MoveTreeViewItem(1, false, (x, y) => x < y);
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            switch (sender?.GetType()?.Name)
            {
                case nameof(JournalWindow):
                    this.DrawTreeView();
                    break;
            }
        }
    }
}
