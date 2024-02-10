// <copyright file="UndoRedoService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common
{
    using System;
    using System.Collections.Generic;

    using Concierge.Commands;
    using Concierge.Display;
    using Concierge.Display.Enums;

    public sealed class UndoRedoService
    {
        public UndoRedoService()
        {
            this.Current = null;
            this.RedoStack = new Stack<Command>();
            this.UndoStack = new Stack<Command>();
        }

        public delegate void StackChangedEventHandler(object sender, EventArgs e);

        public event StackChangedEventHandler? StackChanged;

        public bool CanRedo => this.RedoStack.Count > 0;

        public bool CanUndo => this.UndoStack.Count > 0;

        public bool UpdateAutosaveTimer { get; private set; }

        private Command? Current { get; set; }

        private Stack<Command> RedoStack { get; set; }

        private Stack<Command> UndoStack { get; set; }

        public void Clear()
        {
            Program.Logger.Info($"Clear all Undo/Redo commands.");

            this.RedoStack.Clear();
            this.UndoStack.Clear();
            this.Current = null;
            this.StackChanged?.Invoke(this, new EventArgs());
        }

        public void AddCommand(Command command)
        {
            if (command.ShouldAdd())
            {
                Program.Modify();
                Program.Logger.Info($"Add new {command.GetType()}.");

                this.UndoStack.Push(command);
                this.RedoStack.Clear();
                this.StackChanged?.Invoke(this, new EventArgs());
            }
        }

        public void Redo(MainWindow mainWindow)
        {
            if (!this.CanRedo)
            {
                return;
            }

            this.Current = this.RedoStack.Pop();
            this.UndoStack.Push(this.Current);

            if (this.Current.ConciergePage != ConciergePage.None)
            {
                mainWindow.MoveSelection(this.Current.ConciergePage);
            }

            this.UpdateAutosaveTimer = this.Current is UpdateSettingsCommand;
            this.Current.Redo();
            this.StackChanged?.Invoke(this, new EventArgs());

            Program.Logger.Info($"Redo {this.Current.GetType()}.");
            Program.Modify();
        }

        public void Undo(MainWindow mainWindow)
        {
            if (!this.CanUndo)
            {
                return;
            }

            this.UpdateAutosaveTimer = false;
            this.Current = this.UndoStack.Pop();
            this.RedoStack.Push(this.Current);

            if (this.Current.ConciergePage != ConciergePage.None)
            {
                mainWindow.MoveSelection(this.Current.ConciergePage);
            }

            this.UpdateAutosaveTimer = this.Current is UpdateSettingsCommand;
            this.Current.Undo();
            this.StackChanged?.Invoke(this, new EventArgs());

            Program.Logger.Info($"Undo {this.Current.GetType()}.");
            Program.Modify();
        }

        public List<Command> GetRedoCommands()
        {
            return [.. this.RedoStack];
        }

        public List<Command> GetUndoCommands()
        {
            return [.. this.UndoStack];
        }
    }
}
