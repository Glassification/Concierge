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
        public JournalWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();

            this.ConciergePage = ConciergePage.None;
            this.CurrentEntry = Entry.Empty;
            this.DescriptionTextBlock.DataContext = this.Description;

            this.SetMouseOverEvents(this.DocumentTextBox, this.DocumentTextBackground);
        }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} {this.TreeViewButtonType}";

        public override string WindowName => nameof(JournalWindow);

        private bool Editing { get; set; }

        private TreeViewButtonType TreeViewButtonType { get; set; }

        private Entry CurrentEntry { get; set; }

        public override bool ShowAdd<T>(T item)
        {
            if (item is Chapter chapter)
            {
                this.TreeViewButtonType = TreeViewButtonType.Document;
                this.CurrentEntry = chapter;
            }
            else
            {
                this.TreeViewButtonType = TreeViewButtonType.Chapter;
            }

            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.CreationDateLabel.Visibility = Visibility.Collapsed;

            this.DocumentTextBox.Focus();
            this.ShowConciergeWindow();

            return false;
        }

        public override void ShowEdit<T>(T note)
        {
            this.Editing = true;
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
            this.TreeViewButtonType = TreeViewButtonType.Chapter;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.DocumentTextBox.Text = chapter.Name;
            this.CreationDateLabel.Text = $"Creation Date: {chapter.Created}";
            this.CurrentEntry = chapter;

            this.ShowConciergeWindow();
        }

        private void EditDocument(Document document)
        {
            this.TreeViewButtonType = TreeViewButtonType.Document;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.DocumentTextBox.Text = document.Name;
            this.CreationDateLabel.Text = $"Creation Date: {document.Created}";
            this.CurrentEntry = document;

            this.ShowConciergeWindow();
        }

        private void UpdateEntry()
        {
            if (this.CurrentEntry is Document document)
            {
                var oldItem = document.DeepCopy();
                document.Name = this.DocumentTextBox.Text;
                Program.UndoRedoService.AddCommand(new EditCommand<Document>(document, oldItem, this.ConciergePage));
            }
            else if (this.CurrentEntry is Chapter chapter)
            {
                var oldItem = chapter.DeepCopy();
                chapter.Name = this.DocumentTextBox.Text;
                Program.UndoRedoService.AddCommand(new EditCommand<Chapter>(chapter, oldItem, this.ConciergePage));
            }
        }

        private void ToEntry()
        {
            if (this.TreeViewButtonType == TreeViewButtonType.Chapter)
            {
                var newChapter = new Chapter(this.DocumentTextBox.Text);
                Program.CcsFile.Character.Journal.Chapters.Add(newChapter);
                Program.UndoRedoService.AddCommand(new AddCommand<Chapter>(Program.CcsFile.Character.Journal.Chapters, newChapter, this.ConciergePage));
            }
            else if (this.CurrentEntry is Chapter chapter)
            {
                var newDocument = new Document(this.DocumentTextBox.Text);
                chapter.Documents.Add(newDocument);
                Program.UndoRedoService.AddCommand(new AddCommand<Document>(chapter.Documents, newDocument, this.ConciergePage));
            }
        }

        private void OkApplyChanges()
        {
            if (this.DocumentTextBox.Text.IsNullOrWhiteSpace())
            {
                return;
            }

            if (this.Editing)
            {
                this.UpdateEntry();
            }
            else
            {
                this.ToEntry();
            }
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
            this.DocumentTextBox.Text = string.Empty;

            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }
    }
}
