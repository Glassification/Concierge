// <copyright file="ConsoleRun.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console
{
    using System.Collections.Generic;

    using Concierge.Console.Enums;
    using Concierge.Console.Runners;

    /// <summary>
    /// Represents a console command runner that manages and executes a collection of <see cref="Runner"/> instances based on a given <see cref="ConsoleCommand"/>.
    /// </summary>
    public sealed class ConsoleRun
    {
        private readonly List<Runner> runners = [];
        private readonly ConsoleCommand command;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleRun"/> class with the specified console command.
        /// </summary>
        /// <param name="command">The console command to be executed by the runners.</param>
        public ConsoleRun(ConsoleCommand command)
        {
            this.command = command;
        }

        /// <summary>
        /// Adds the specified <see cref="Runner"/> to the list of runners if it is capable of handling the command's name.
        /// </summary>
        /// <param name="runner">The runner to be added if it can handle the command.</param>
        public void AddIfExists(Runner runner)
        {
            if (runner.Contains(this.command.Name))
            {
                this.runners.Add(runner);
            }
        }

        /// <summary>
        /// Adds a default <see cref="UnknownRunner"/> to the list of runners.
        /// This method is typically called to ensure there is at least one runner available to handle the command.
        /// </summary>
        public void Validate()
        {
            this.runners.Add(new UnknownRunner());
        }

        /// <summary>
        /// Executes the command using the list of runners. Each runner is attempted in sequence until one successfully handles the command.
        /// </summary>
        /// <returns>
        /// A <see cref="ConsoleResult"/> representing the result of the command execution.
        /// If no runner successfully handles the command, the result will indicate that the command is not implemented.
        /// </returns>
        public ConsoleResult RunAll()
        {
            var result = ConsoleResult.Empty;
            foreach (var runner in this.runners)
            {
                result = runner.Run(this.command);
                if (result.Type != ResultType.NotImplemented)
                {
                    break;
                }
            }

            return result;
        }
    }
}
