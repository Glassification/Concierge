// <copyright file="ConsoleCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console
{
    using System.Text.RegularExpressions;

    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    public class ConsoleCommand
    {
        private readonly Regex textInParentheses = new (@"\(.*?\)", RegexOptions.Compiled);

        public ConsoleCommand(string command)
        {
            this.Name = string.Empty;
            this.Action = string.Empty;
            this.Argument = string.Empty;
            this.Command = command;

            this.Parse(command);
        }

        public string Action { get; set; }

        public string Argument { get; set; }

        public string Command { get; init; }

        public bool IsValid { get; private set; }

        public string Name { get; set; }

        private void Parse(string command)
        {
            command = command.Strip(Constants.ConsolePrompt);

            var tokens = command.Split('.');
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
                this.Argument = this.textInParentheses.Match(tokens[0]).Value.Strip("(").Strip(")");
            }
            else
            {
                this.Action = tokens[1];
            }
        }
    }
}
