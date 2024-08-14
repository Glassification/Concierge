// <copyright file="ConsoleCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console
{
    using System;
    using System.Text.RegularExpressions;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a console command with a name, action, and optional argument.
    /// </summary>
    public sealed partial class ConsoleCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleCommand"/> class with the specified command string.
        /// </summary>
        /// <param name="command">The command string to parse.</param>
        public ConsoleCommand(string command)
        {
            this.Name = string.Empty;
            this.Action = string.Empty;
            this.Argument = string.Empty;
            this.Command = string.Empty;

            this.Parse(command);
        }

        /// <summary>
        /// Gets or sets the action associated with the command.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the argument associated with the command.
        /// </summary>
        public string Argument { get; set; }

        /// <summary>
        /// Gets the original command string.
        /// </summary>
        public string Command { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the command is valid.
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Gets or sets the name of the command.
        /// </summary>
        public string Name { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }

        [GeneratedRegex(@"\(.*?\)", RegexOptions.Compiled)]
        private static partial Regex TextInParenthesesRegex();

        private void Parse(string command)
        {
            command = command.Strip(Constants.ConsolePrompt);
            this.Command = command;

            if (command.StartsWith("Echo", StringComparison.InvariantCultureIgnoreCase))
            {
                this.IsValid = true;
                this.Name = "Echo";
                this.Argument = command.ReplaceFirst("Echo", string.Empty, StringComparison.InvariantCultureIgnoreCase);
                return;
            }

            var tokens = command.Split('.', 2);
            if (tokens.Length < 1)
            {
                this.IsValid = false;
                return;
            }

            this.Name = tokens[0];
            this.IsValid = true;
            if (tokens.Length < 2)
            {
                return;
            }

            if (tokens[1].Contains('('))
            {
                this.Action = tokens[1][..tokens[1].IndexOf('(')];
                this.Argument = TextInParenthesesRegex().Match(tokens[1]).Value.Strip("(", ")");
            }
            else
            {
                this.Action = tokens[1];
            }
        }
    }
}
