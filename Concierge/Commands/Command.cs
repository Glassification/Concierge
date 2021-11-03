// <copyright file="Command.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    public abstract class Command
    {
        public abstract void Redo();

        public abstract void Undo();
    }
}
