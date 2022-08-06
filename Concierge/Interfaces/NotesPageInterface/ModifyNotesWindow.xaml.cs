// <copyright file="ModifyNotesWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.NotesPageInterface
{
    using System.Collections.Generic;
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
        public ModifyNotesWindow()
        {
            this.InitializeComponent();
            this.ConciergePage = ConciergePage.None;
        }

        public override string HeaderText => $"{(this.Editing ? "Edit" : "Add")} {this.TreeViewButtonType}";

        private bool Editing { get; set; }

        private TreeViewButtonType TreeViewButtonType { get; set; }

        private Chapter? CurrentChapter { get; set; }

        private Document? CurrentDocument { get; set; }

        public override bool ShowAdd<T>(T item)
        {
            if (item is Chapter chapter)
            {
                this.TreeViewButtonType = TreeViewButtonType.Document;
                this.CurrentChapter = chapter;
            }
            else
            {
                this.TreeViewButtonType = TreeViewButtonType.Chapter;
                this.CurrentChapter = null;
            }

            this.Editing = false;
            this.HeaderTextBlock.Text = this.HeaderText;

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
            this.CurrentChapter = chapter;
            this.CurrentDocument = null;

            this.ShowConciergeWindow();
        }

        private void EditDocument(Document document)
        {
            this.TreeViewButtonType = TreeViewButtonType.Document;
            this.HeaderTextBlock.Text = this.HeaderText;
            this.DocumentTextBox.Text = document.Name;
            this.CurrentChapter = null;
            this.CurrentDocument = document;

            this.ShowConciergeWindow();
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
            if (this.TreeViewButtonType == TreeViewButtonType.Chapter)
            {
                var newChapter = new Chapter(this.DocumentTextBox.Text);
                Program.CcsFile.Character.Chapters.Add(newChapter);
                Program.UndoRedoService.AddCommand(new AddCommand<Chapter>(Program.CcsFile.Character.Chapters, newChapter, this.ConciergePage));
            }
            else
            {
                var newDocument = new Document(this.DocumentTextBox.Text);
                this.CurrentChapter?.Documents.Add(newDocument);
                Program.UndoRedoService.AddCommand(new AddCommand<Document>(this.CurrentChapter?.Documents ?? new List<Document>(), newDocument, this.ConciergePage));
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
            this.DocumentTextBox.Text = string.Empty;

            this.InvokeApplyChanges();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }
    }
}
