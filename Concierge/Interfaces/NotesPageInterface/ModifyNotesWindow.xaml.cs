// <copyright file="ModifyNotesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.NotesPageInterface
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Notes;
    using Concierge.Commands;
    using Concierge.Interfaces.Enums;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for ModifyEquippedItemsWindow.xaml.
    /// </summary>
    public partial class ModifyNotesWindow : Window
    {
        private const string NewChapter = "--New Chapter--";

        private readonly ConciergePage conciergePage;

        public ModifyNotesWindow(ConciergePage conciergePage)
        {
            this.InitializeComponent();
            this.conciergePage = conciergePage;
        }

        public delegate void ApplyChangesEventHandler(object sender, EventArgs e);

        public event ApplyChangesEventHandler ApplyChanges;

        private bool IsEdit { get; set; }

        private Chapter CurrentChapter { get; set; }

        private Document CurrentDocument { get; set; }

        public void ShowAdd()
        {
            this.HeaderTextBlock.Text = "Add Note";

            this.SetupWindow(false);
            this.ShowDialog();
        }

        public void ShowEdit(Chapter chapter)
        {
            this.HeaderTextBlock.Text = "Edit Chapter";
            this.ChapterComboBox.Text = chapter.Name;
            this.DocumentTextBox.Text = chapter.Name;
            this.CurrentChapter = chapter;
            this.CurrentDocument = null;

            this.SetupWindow(true);
            this.ShowDialog();
        }

        public void ShowEdit(Document document)
        {
            this.HeaderTextBlock.Text = "Edit Page";
            this.ChapterComboBox.Text = Program.CcsFile.Character.GetChapterByDocumentId(document.Id).Name;
            this.DocumentTextBox.Text = document.Name;
            this.CurrentChapter = null;
            this.CurrentDocument = document;

            this.SetupWindow(true);
            this.ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Hide();
        }

        private void SetupWindow(bool isEdit)
        {
            this.IsEdit = isEdit;
            this.ClearFields();
            this.GenerateChapterComboBox();
            this.ChapterComboBox.IsEnabled = !isEdit;
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
            if (this.CurrentChapter == null)
            {
                var oldItem = this.CurrentDocument.DeepCopy() as Document;
                this.CurrentDocument.Name = this.DocumentTextBox.Text;
                Program.UndoRedoService.AddCommand(new EditCommand<Document>(this.CurrentDocument, oldItem, this.conciergePage));
            }
            else
            {
                var oldItem = this.CurrentChapter.DeepCopy() as Chapter;
                this.CurrentChapter.Name = this.DocumentTextBox.Text;
                Program.UndoRedoService.AddCommand(new EditCommand<Chapter>(this.CurrentChapter, oldItem, this.conciergePage));
            }
        }

        private void ToNote()
        {
            var chapter = this.ChapterComboBox.SelectedItem as Chapter;

            if (chapter.IsNewChapterPlaceholder)
            {
                var newChapter = new Chapter(this.DocumentTextBox.Text);
                Program.CcsFile.Character.Chapters.Add(newChapter);
                Program.UndoRedoService.AddCommand(new AddCommand<Chapter>(Program.CcsFile.Character.Chapters, newChapter, this.conciergePage));
            }
            else
            {
                var newDocument = new Document(this.DocumentTextBox.Text);
                chapter.Documents.Add(newDocument);
                Program.UndoRedoService.AddCommand(new AddCommand<Document>(chapter.Documents, newDocument, this.conciergePage));
            }
        }

        private void OkApplyChanges()
        {
            Program.Modify();

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
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.OkApplyChanges();
            this.Hide();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.OkApplyChanges();

            if (!this.IsEdit)
            {
                if (this.ChapterComboBox.Text.Equals(NewChapter))
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

            this.ApplyChanges?.Invoke(this, new EventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Hide();
                    break;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
