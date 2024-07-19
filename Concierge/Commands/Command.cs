// <copyright file="Command.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Commands
{
    using Concierge.Display.Enums;

    /// <summary>
    /// Represents a base abstract class for implementing commands in a command pattern.
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        public Command()
        {
        }

        /// <summary>
        /// Gets or sets the ConciergePage associated with this command.
        /// </summary>
        public ConciergePages ConciergePage { get; set; }

        /// <summary>
        /// Performs the redo operation for this command.
        /// </summary>
        public abstract void Redo();

        /// <summary>
        /// Performs the undo operation for this command.
        /// </summary>
        public abstract void Undo();

        /// <summary>
        /// Determines whether the command should be added to the command history.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the command should be added to the command history; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool ShouldAdd()
        {
            return true;
        }
    }
}
