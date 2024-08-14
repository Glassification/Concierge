// <copyright file="Runner.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Runners
{
    using System;
    using System.Linq;
    using System.Text;

    using Concierge.Common.Extensions;

    /// <summary>
    /// Represents a service for running scripts associated with console commands.
    /// </summary>
    public abstract class Runner
    {
        /// <summary>
        /// The indentation string used for formatting output.
        /// </summary>
        protected const string Indent = "   ";

        /// <summary>
        /// Gets the names of the commands supported by this script service.
        /// </summary>
        public abstract string[] Names { get; }

        /// <summary>
        /// Gets the actions supported by this script service.
        /// </summary>
        public abstract string[] Actions { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this is the first item in a list.
        /// </summary>
        protected bool IsFirstList { get; set; }

        /// <summary>
        /// Runs the script associated with the specified console command.
        /// </summary>
        /// <param name="command">The console command to execute.</param>
        /// <returns>The result of executing the command.</returns>
        public abstract ConsoleResult Run(ConsoleCommand command);

        /// <summary>
        /// Generates a string representation of the available actions and commands for this script service.
        /// </summary>
        /// <returns>A string representation of the available actions and commands.</returns>
        public virtual string List()
        {
            var builder = new StringBuilder();

            builder.Append($"{(this.IsFirstList ? string.Empty : Indent)}|--");
            foreach (var action in this.Actions)
            {
                builder.Append($"{action}, ");
            }

            builder.Remove(builder.Length - 2, 2);
            builder.AppendLine();
            foreach (var name in this.Names)
            {
                builder.Append($"{Indent}|   |--{name}\n");
            }

            var builderString = builder.ToString();
            builderString = builderString.ReplaceLast("|", "\\", StringComparison.InvariantCultureIgnoreCase);

            return builderString;
        }

        /// <summary>
        /// Determines whether this script service contains the specified command name.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <returns><c>true</c> if this script service contains the specified command name; otherwise, <c>false</c>.</returns>
        public virtual bool Contains(string name)
        {
            return this.Names.Contains(name, StringComparer.InvariantCultureIgnoreCase);
        }
    }
}
