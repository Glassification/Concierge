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

    /// <summary>
    /// Provides functionality for managing undo and redo operations within the Concierge application.
    /// </summary>
    public sealed class UndoRedoService
    {
        private readonly Stack<Command> redoStack;
        private readonly Stack<Command> undoStack;

        private Command? current;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedoService"/> class.
        /// </summary>
        public UndoRedoService()
        {
            this.current = null;
            this.redoStack = new Stack<Command>();
            this.undoStack = new Stack<Command>();
        }

        /// <summary>
        /// Represents the event handler for stack change events.
        /// </summary>
        public delegate void StackChangedEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Occurs when the undo or redo stack changes.
        /// </summary>
        public event StackChangedEventHandler? StackChanged;

        /// <summary>
        /// Gets a value indicating whether there are actions available for redo.
        /// </summary>
        public bool CanRedo => this.redoStack.Count > 0;

        /// <summary>
        /// Gets a value indicating whether there are actions available for undo.
        /// </summary>
        public bool CanUndo => this.undoStack.Count > 0;

        /// <summary>
        /// Gets a value indicating whether to update the autosave timer.
        /// </summary>
        public bool UpdateAutosaveTimer { get; private set; }

        /// <summary>
        /// Clears all undo and redo commands.
        /// </summary>
        public void Clear()
        {
            Program.Logger.Info($"Clear all Undo/Redo commands.");

            this.redoStack.Clear();
            this.undoStack.Clear();
            this.current = null;
            this.StackChanged?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Adds a new command to the undo stack.
        /// </summary>
        /// <param name="command">The command to add.</param>
        public void AddCommand(Command command)
        {
            if (command.ShouldAdd())
            {
                Program.Modify();
                Program.Logger.Info($"Add new {command.GetType()}.");

                this.undoStack.Push(command);
                this.redoStack.Clear();
                this.StackChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Redoes the last undone action.
        /// </summary>
        /// <param name="mainWindow">The main window of the application.</param>
        public void Redo(MainWindow mainWindow)
        {
            if (!this.CanRedo)
            {
                return;
            }

            this.current = this.redoStack.Pop();
            this.undoStack.Push(this.current);

            if (this.current.ConciergePage != ConciergePages.None)
            {
                mainWindow.MoveSelection(this.current.ConciergePage);
            }

            this.UpdateAutosaveTimer = this.current is UpdateSettingsCommand;
            this.current.Redo();
            this.StackChanged?.Invoke(this, new EventArgs());

            Program.Logger.Info($"Redo {this.current.GetType()}.");
            Program.Modify();
        }

        /// <summary>
        /// Undoes the last action.
        /// </summary>
        /// <param name="mainWindow">The main window of the application.</param>
        public void Undo(MainWindow mainWindow)
        {
            if (!this.CanUndo)
            {
                return;
            }

            this.UpdateAutosaveTimer = false;
            this.current = this.undoStack.Pop();
            this.redoStack.Push(this.current);

            if (this.current.ConciergePage != ConciergePages.None)
            {
                mainWindow.MoveSelection(this.current.ConciergePage);
            }

            this.UpdateAutosaveTimer = this.current is UpdateSettingsCommand;
            this.current.Undo();
            this.StackChanged?.Invoke(this, new EventArgs());

            Program.Logger.Info($"Undo {this.current.GetType()}.");
            Program.Modify();
        }

        /// <summary>
        /// Gets the list of commands available for redoing.
        /// </summary>
        /// <returns>The list of commands available for redoing.</returns>
        public List<Command> GetRedoCommands()
        {
            return [.. this.redoStack];
        }

        /// <summary>
        /// Gets the list of commands available for undoing.
        /// </summary>
        /// <returns>The list of commands available for undoing.</returns>
        public List<Command> GetUndoCommands()
        {
            return [.. this.undoStack];
        }
    }
}
