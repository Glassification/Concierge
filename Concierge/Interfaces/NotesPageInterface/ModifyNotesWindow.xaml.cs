// <copyright file="ModifyNotesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.NotesPageInterface
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Concierge.Character.Notes;
    using Concierge.Commands;
    using Concierge.Interfaces.Components;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for ModifyEquippedItemsWindow.xaml.
    /// </summary>
    public partial class ModifyNotesWindow : ConciergeWindow
    {
        private const string NewChapter = "--New Chapter--";

        public ModifyNotesWindow()
        {
            this.InitializeComponent();
            this.ConciergePage = ConciergePage.None;
        }

        private bool IsEdit { get; set; }

        private Chapter? CurrentChapter { get; set; }

        private Document? CurrentDocument { get; set; }

        public override bool ShowAdd<T>(T item)
        {
            this.HeaderTextBlock.Text = "Add Chapter/Page";
            this.ChapterComboBox.SelectedIndex = 0;

            this.SetupWindow(false);
            this.ShowConciergeWindow();

            return false;
        }

        public override void ShowEdit<T>(T note)
        {
            if (note is Chapter chapter)
            {
                this.EditChapter(chapter);
            }
            else if (note is Document document)
            {
                this.EditDocument(document);
            }
        }

        protected override void ReturnAndClose()
        {
            this.OkApplyChanges();
            this.CloseConciergeWindow();
        }

        private void EditChapter(Chapter chapter)
        {
            this.SetupWindow(true);

            this.HeaderTextBlock.Text = "Edit Chapter";
            this.ChapterComboBox.Text = chapter.Name;
            this.DocumentTextBox.Text = chapter.Name;
            this.CurrentChapter = chapter;
            this.CurrentDocument = null;
            this.NameTextBlock.Text = "Chapter Name:";

            this.ShowConciergeWindow();
        }

        private void EditDocument(Document document)
        {
            this.SetupWindow(true);

            this.HeaderTextBlock.Text = "Edit Page";
            this.ChapterComboBox.Text = Program.CcsFile.Character.GetChapterByDocumentId(document.Id).Name;
            this.DocumentTextBox.Text = document.Name;
            this.CurrentChapter = null;
            this.CurrentDocument = document;
            this.NameTextBlock.Text = "Page Name:";

            this.ShowConciergeWindow();
        }

        private void SetupWindow(bool isEdit)
        {
            this.IsEdit = isEdit;
            this.ClearFields();
            this.GenerateChapterComboBox();
            this.ChapterComboBox.IsEnabled = !isEdit;
            this.ChapterComboBox.Opacity = this.ChapterComboBox.IsEnabled ? 1 : 0.5;
            this.ChapterLabel.Opacity = this.ChapterComboBox.IsEnabled ? 1 : 0.5;
        }

        private void GenerateChapterComboBox()
        {
            this.ChapterComboBox.Items.Clear();
            this.ChapterComboBox.Items.Add(new Chapter(NewChapter)
            {
                IsNewChapterPlaceholder = true,
            });

            foreach (var chapter in Program.CcsFile.Character.Chapters)
            {
                this.ChapterComboBox.Items.Add(chapter);
            }
        }

        private void ClearFields()
        {
            this.ChapterComboBox.Text = string.Empty;
            this.DocumentTextBox.Text = string.Empty;
        }

        private void UpdateNote()
        {
            if (this.CurrentChapter is null && this.CurrentDocument is not null)
            {
                var oldItem = this.CurrentDocument.DeepCopy();
                this.CurrentDocument.Name = this.DocumentTextBox.Text;
                Program.UndoRedoService.AddCommand(new EditCommand<Document>(this.CurrentDocument, oldItem, this.ConciergePage));
            }
            else if (this.CurrentChapter is not null && this.CurrentDocument is null)
            {
                var oldItem = this.CurrentChapter.DeepCopy();
                this.CurrentChapter.Name = this.DocumentTextBox.Text;
                Program.UndoRedoService.AddCommand(new EditCommand<Chapter>(this.CurrentChapter, oldItem, this.ConciergePage));
            }
        }

        private void ToNote()
        {
            if (this.ChapterComboBox.SelectedItem is not Chapter chapter)
            {
                return;
            }

            if (chapter.IsNewChapterPlaceholder)
            {
                var newChapter = new Chapter(this.DocumentTextBox.Text);
                Program.CcsFile.Character.Chapters.Add(newChapter);
                Program.UndoRedoService.AddCommand(new AddCommand<Chapter>(Program.CcsFile.Character.Chapters, newChapter, this.ConciergePage));
            }
            else
            {
                var newDocument = new Document(this.DocumentTextBox.Text);
                chapter.Documents.Add(newDocument);
                Program.UndoRedoService.AddCommand(new AddCommand<Document>(chapter.Documents, newDocument, this.ConciergePage));
            }
        }

        private void OkApplyChanges()
        {
            if ((this.ChapterComboBox.SelectedItem as Chapter) == null ||
                this.DocumentTextBox.Text.IsNullOrWhiteSpace())
            {
                return;
            }

            if (this.IsEdit)
            {
                this.UpdateNote();
            }
            else
            {
                this.ToNote();
            }

            Program.Modify();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReturnAndClose();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.OkApplyChanges();

            if (!this.IsEdit)
            {
                if (this.ChapterComboBox.Text.Equals(NewChapter) && !this.DocumentTextBox.Text.IsNullOrWhiteSpace())
                {
                    var chapterName = this.DocumentTextBox.Text;
                    this.ClearFields();
                    this.GenerateChapterComboBox();
                    this.ChapterComboBox.Text = chapterName;
                }
                else
                {
                    this.DocumentTextBox.Text = string.Empty;
                }
            }

            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }

        private void ChapterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var chapter = this.ChapterComboBox.SelectedItem as Chapter;
            this.NameTextBlock.Text = (chapter?.IsNewChapterPlaceholder ?? true) ? "Chapter Name:" : "Page Name:";
        }
    }
}
