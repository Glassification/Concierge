// <copyright file="Command.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Interfaces.Enums;

    public abstract class Command
    {
        public Command()
        {
            this.ModifiedState = Program.Modified;
        }

        public ConciergePage ConciergePage { get; set; }

        public bool ModifiedState { get; set; }

        public abstract void Redo();

        public abstract void Undo();
    }
}
