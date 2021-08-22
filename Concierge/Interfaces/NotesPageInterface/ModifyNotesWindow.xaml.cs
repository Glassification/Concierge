// <copyright file="ModifyNotesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.NotesPageInterface
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character.Notes;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    /// <summary>
    /// Interaction logic for ModifyEquippedItemsWindow.xaml.
    /// </summary>
    public partial class ModifyNotesWindow : Window
    {
        public ModifyNotesWindow()
        {
            this.InitializeComponent();
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
            this.SetupWindow(true);
            this.ChapterComboBox.Text = chapter.Name;
            this.DocumentTextBox.Text = chapter.Name;
            this.CurrentChapter = chapter;
            this.CurrentDocument = null;
            this.ShowDialog();
        }

        public void ShowEdit(Document document)
        {
            this.HeaderTextBlock.Text = "Edit Page";
            this.SetupWindow(true);
            this.ChapterComboBox.Text = Program.CcsFile.Character.GetChapterByDocumentId(document.Id).Name;
            this.DocumentTextBox.Text = document.Name;
            this.CurrentChapter = null;
            this.CurrentDocument = document;
            this.ShowDialog();
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
            this.ChapterComboBox.Items.Add(new Chapter("--New Chapter--")
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

        private void WriteEdit()
        {
            if (this.CurrentChapter == null)
            {
                this.CurrentDocument.Name = this.DocumentTextBox.Text;
            }
            else
            {
                this.CurrentChapter.Name = this.DocumentTextBox.Text;
            }
        }

        private void WriteNew()
        {
            var chapter = this.ChapterComboBox.SelectedItem as Chapter;

            if (chapter.IsNewChapterPlaceholder)
            {
                Program.CcsFile.Character.Chapters.Add(new Chapter(this.DocumentTextBox.Text));
            }
            else
            {
                chapter.Documents.Add(new Document(this.DocumentTextBox.Text));
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
                this.WriteEdit();
            }
            else
            {
                this.WriteNew();
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
                this.ClearFields();
                this.GenerateChapterComboBox();
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

        private void ChapterComboBox_DropDownOpened(object sender, EventArgs e)
        {
            ConciergeSound.UpdateValue();
        }
    }
}
