// <copyright file="CompositeCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using System.Collections.Generic;

    using Concierge.Display.Enums;

    /// <summary>
    /// Represents a command that combines multiple commands into a single composite command.
    /// </summary>
    public sealed class CompositeCommand : Command
    {
        private readonly List<Command> commands = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeCommand"/> class.
        /// </summary>
        /// <param name="conciergePage">The ConciergePage associated with this composite command.</param>
        /// <param name="commands">The commands to be combined into the composite command.</param>
        public CompositeCommand(ConciergePages conciergePage, params Command[] commands)
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
