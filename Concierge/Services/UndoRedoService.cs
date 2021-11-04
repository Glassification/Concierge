// <copyright file="UndoRedoService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Commands;

    public class UndoRedoService
    {
        public UndoRedoService()
        {
            this.Current = null;
            this.RedoStack = new Stack<Command>();
            this.UndoStack = new Stack<Command>();
        }

        public delegate void StackChangedEventHandler(object sender, EventArgs e);

        public event StackChangedEventHandler StackChanged;

        public bool CanRedo => this.RedoStack.Count > 0;

        public bool CanUndo => this.UndoStack.Count > 0;

        private Command Current { get; set; }

        private Stack<Command> RedoStack { get; set; }

        private Stack<Command> UndoStack { get; set; }

        public void Clear()
        {
            this.RedoStack.Clear();
            this.UndoStack.Clear();
            this.Current = null;
            this.StackChanged?.Invoke(this, new EventArgs());
        }

        public void AddCommand(Command command)
        {
            if (command is null)
            {
                return;
            }

            this.UndoStack.Push(command);
            this.RedoStack.Clear();
            this.StackChanged?.Invoke(this, new EventArgs());
        }

        public void Redo()
        {
            if (!this.CanRedo)
            {
                return;
            }

            this.Current = this.RedoStack.Pop();
            this.UndoStack.Push(this.Current);
            this.Current.Redo();
            this.StackChanged?.Invoke(this, new EventArgs());
        }

        public void Undo()
        {
            if (!this.CanUndo)
            {
                return;
            }

            this.Current = this.UndoStack.Pop();
            this.RedoStack.Push(this.Current);
            this.Current.Undo();
            this.StackChanged?.Invoke(this, new EventArgs());
        }

        public List<Command> GetRedoCommands()
        {
            return this.RedoStack.ToList();
        }

        public List<Command> GetUndoCommands()
        {
            return this.UndoStack.ToList();
        }
    }
}
