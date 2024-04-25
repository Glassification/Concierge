// <copyright file="IScript.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Scripts
{
    /// <summary>
    /// Defines the contract for a script to be executed by a script service.
    /// </summary>
    public interface IScript
    {
        /// <summary>
        /// Evaluates the specified console command.
        /// </summary>
        /// <param name="command">The console command to evaluate.</param>
        /// <returns>The result of evaluating the command.</returns>
        ConsoleResult Evaluate(ConsoleCommand command);
    }
}
