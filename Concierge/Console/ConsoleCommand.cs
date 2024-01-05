// <copyright file="ConsoleCommand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console
{
    using System.Text.RegularExpressions;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Newtonsoft.Json;

    public sealed partial class ConsoleCommand
    {
        public ConsoleCommand(string command)
        {
            this.Name = string.Empty;
            this.Action = string.Empty;
            this.Argument = string.Empty;
            this.Command = string.Empty;

            this.Parse(command);
        }

        public string Action { get; set; }

        public string Argument { get; set; }

        public string Command { get; private set; }

        public bool IsValid { get; private set; }

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
