// <copyright file="AdventurersWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    using Concierge.Character.Companions;
    using Concierge.Character.Journals;
    using Concierge.Commands;
    using Concierge.Common.Extensions;
    using Concierge.Common.Utilities;
    using Concierge.Display.Components;
    using Concierge.Display.Enums;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for AdventurersWindow.xaml.
    /// </summary>
    public partial class AdventurersWindow : ConciergeWindow
    {
        private const string PartyMembers = "Party Members";

        public AdventurersWindow()
        {
            this.InitializeComponent();
            this.UseRoundedCorners();
        }

        public override string HeaderText => "Adventuring Party";

        public override string WindowName => nameof(AdventurersWindow);

        public override object? ShowWindow()
        {
            this.Draw();
            this.SetAdventurersDataGridControlState();
            this.ShowConciergeWindow();

            return null;
        }

        public override void ShowEdit<T>(T item)
        {
            if (item is not Adventurer adventurer)
            {
                return;
            }

            WindowService.ShowEdit(adventurer, typeof(AdventurerWindow), this.Window_ApplyChanges, ConciergePages.None);
            this.Draw();
        }

        public void Draw()
        {
            var adventurers = Program.CcsFile.Character.Adventurers;

            this.AdventurersDataGrid.Items.Clear();
            foreach (var adventurer in adventurers)
            {
                this.AdventurersDataGrid.Items.Add(adventurer);
            }

            DisplayUtility.SetControlEnableState(this.CreateEntriesButton, this.AdventurersDataGrid.Items.Count > 0);
        }

        public void SetAdventurersDataGridControlState()
        {
            this.AdventurersDataGrid.SetButtonControlsEnableState(this.UpButton, this.DownButton, this.EditButton, this.DeleteButton);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseConciergeWindow();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.AdventurersDataGrid.UnselectAll();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var added = WindowService.ShowAdd(
                Program.CcsFile.Character.Adventurers,
                typeof(AdventurerWindow),
                this.Window_ApplyChanges,
                ConciergePages.None);

            this.Draw();
            if (added)
            {
                this.AdventurersDataGrid.SetSelectedIndex(this.AdventurersDataGrid.LastIndex);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.AdventurersDataGrid.SelectedItem is Adventurer adventurer)
            {
                this.ShowEdit(adventurer);
                this.Draw();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.AdventurersDataGrid.SelectedItem is not Adventurer adventurer)
            {
                return;
            }

            var index = this.AdventurersDataGrid.SelectedIndex;

            Program.UndoRedoService.AddCommand(new DeleteCommand<Adventurer>(Program.CcsFile.Character.Adventurers, adventurer, index, ConciergePages.None));
            Program.CcsFile.Character.Adventurers.Remove(adventurer);

            this.Draw();
            this.AdventurersDataGrid.SetSelectedIndex(index);
        }

        private void Window_ApplyChanges(object sender, EventArgs e)
        {
            this.Draw();
        }

        private void AdventurersDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.SetAdventurersDataGridControlState();
        }

        private void AdventurersDataGrid_Sorted(object sender, RoutedEventArgs e)
        {
            this.AdventurersDataGrid.SortListFromDataGrid(Program.CcsFile.Character.Adventurers, ConciergePages.None);
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            var index = this.AdventurersDataGrid.NextItem(Program.CcsFile.Character.Adventurers, Program.CcsFile.Character.Adventurers.Count - 1, 1, ConciergePages.None);
            if (index != -1)
            {
                this.Draw();
                this.AdventurersDataGrid.SetSelectedIndex(index);
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            var index = this.AdventurersDataGrid.NextItem(Program.CcsFile.Character.Adventurers, 0, -1, ConciergePages.None);
            if (index != -1)
            {
                this.Draw();
                this.AdventurersDataGrid.SetSelectedIndex(index);
            }
        }

        private void CreateEntriesButton_Click(object sender, RoutedEventArgs e)
        {
            var adventurers = Program.CcsFile.Character.Adventurers;
            if (adventurers.IsEmpty())
            {
                return;
            }

            var chapters = Program.CcsFile.Character.Journal.Chapters;
            var chapter = chapters.Where(x => x.Name.Equals(PartyMembers)).FirstOrDefault();
            var commands = new List<Command>();
            if (chapter is null)
            {
                chapter = new Chapter(PartyMembers);

                commands.Add(new AddCommand<Chapter>(chapters, chapter, ConciergePages.None));
                chapters.Add(chapter);
            }

            foreach (var adventurer in Program.CcsFile.Character.Adventurers)
            {
                var page = chapter.Documents.Where(x => x.Name.Equals(adventurer.CharacterName)).FirstOrDefault();
                if (page is null)
                {
                    var document = new Document(adventurer.CharacterName);

                    commands.Add(new AddCommand<Document>(chapter.Documents, document, ConciergePages.None));
                    chapter.Documents.Add(document);
                }
            }

            if (!commands.IsEmpty())
            {
                Program.MainWindow?.JournalPage.Draw();
                Program.MainWindow?.DisplayStatusText("Generated missing character entries in journal.");
                Program.UndoRedoService.AddCommand(new CompositeCommand(ConciergePages.None, [.. commands]));
            }
            else
            {
                Program.MainWindow?.DisplayStatusText("No missing character entires detected in journal.");
            }
        }
    }
}
