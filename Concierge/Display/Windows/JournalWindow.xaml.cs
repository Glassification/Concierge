// <copyright file="JournalWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System.Windows;

    using Concierge.Character.Journals;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;

    /// <summary>
    /// Interaction logic for JournalWindow.xaml.
    /// </summary>
    public partial class JournalWindow : ConciergeWindow
    {
        private bool editing;
        private TreeButtonType treeViewButtonType;
        private Entry currentEntry = Entry.Empty;

        public JournalWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ConciergePage = ConciergePages.None;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.DocumentTextBox, this.DocumentTextBackground);
        }

        public override string HeaderText => $"{(this.editing ? "Edit" : "Add")} {this.treeViewButtonType}";

        public override string WindowName => nameof(JournalWindow);

        public override bool ShowAdd<T>(T item)
        {
            if (item is Chapter chapter)
            {
                this.treeViewButtonType = TreeButtonType.Page;
                this.currentEntry = chapter;
            }
            else
            {
                this.treeViewButtonType = TreeButtonType.Chapter;
            }

            this.editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.CreationDateLabel.Visibility = Visibility.Collapsed;

            this.DocumentTextBox.Focus();
            this.ShowConciergeWindow();

            return false;
        }

        public override void ShowEdit<T>(T note)
        {
            this.editing = true;
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
            if (this.OkApplyChanges())
            {
                this.CloseConciergeWindow();
            }
            else
            {
                this.InvalidNameMessage();
            }
        }

        private void EditChapter(Chapter chapter)
        {
            this.treeViewButtonType = TreeButtonType.Chapter;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.DocumentTextBox.Text = chapter.Name;
            this.CreationDateLabel.Text = $"Creation Date: {chapter.Created}";
            this.currentEntry = chapter;

            this.ShowConciergeWindow();
        }

        private void EditDocument(Document document)
        {
            this.treeViewButtonType = TreeButtonType.Page;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.DocumentTextBox.Text = document.Name;
            this.CreationDateLabel.Text = $"Creation Date: {document.Created}";
            this.currentEntry = document;

            this.ShowConciergeWindow();
        }

        private void UpdateEntry()
        {
            if (this.currentEntry is Document document)
            {
                var oldItem = document.DeepCopy();
                document.Name = this.DocumentTextBox.Text;
                Program.UndoRedoService.AddCommand(new EditCommand<Document>(document, oldItem, this.ConciergePage));
            }
            else if (this.currentEntry is Chapter chapter)
            {
                var oldItem = chapter.DeepCopy();
                chapter.Name = this.DocumentTextBox.Text;
                Program.UndoRedoService.AddCommand(new EditCommand<Chapter>(chapter, oldItem, this.ConciergePage));
            }
        }

        private void ToEntry()
        {
            if (this.treeViewButtonType == TreeButtonType.Chapter)
            {
                var newChapter = new Chapter(this.DocumentTextBox.Text);
                Program.CcsFile.Character.Journal.Chapters.Add(newChapter);
                Program.UndoRedoService.AddCommand(new AddCommand<Chapter>(Program.CcsFile.Character.Journal.Chapters, newChapter, this.ConciergePage));
            }
            else if (this.currentEntry is Chapter chapter)
            {
                var newDocument = new Document(this.DocumentTextBox.Text);
                chapter.Documents.Add(newDocument);
                Program.UndoRedoService.AddCommand(new AddCommand<Document>(chapter.Documents, newDocument, this.ConciergePage));
            }
        }

        private void InvalidNameMessage()
        {
            ConciergeMessageBox.ShowWarning($"Cannot create a {this.treeViewButtonType.ToString().ToLower()} with a missing name. Add a valid one to continue.");
        }

        private bool OkApplyChanges()
        {
            if (this.DocumentTextBox.Text.IsNullOrWhiteSpace())
            {
                return false;
            }

            if (this.editing)
            {
                this.UpdateEntry();
            }
            else
            {
                this.ToEntry();
            }

            return true;
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
            if (!this.OkApplyChanges())
            {
                this.InvalidNameMessage();
                return;
            }

            this.DocumentTextBox.Text = string.Empty;
            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }
    }
}
