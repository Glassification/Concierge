// <copyright file="CompositeCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using System.Collections.Generic;

    using Concierge.Display.Enums;

    public sealed class CompositeCommand : Command
    {
        private readonly List<Command> commands = [];

        public CompositeCommand(ConciergePage conciergePage, params Command[] commands)
        {
            this.commands.AddRange(commands);
            this.ConciergePage = conciergePage;
        }

        public override void Redo()
        {
            this.commands.ForEach(x => x.Redo());
        }

        public override void Undo()
        {
            this.commands.ForEach(x => x.Undo());
        }

        public override bool ShouldAdd()
        {
            return this.commands.Count > 0;
        }
    }
}
