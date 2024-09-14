// <copyright file="HelpRunner.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Runners
{
    using System;

    using Concierge.Common.Extensions;
    using Concierge.Console.Enums;

    public sealed class HelpRunner : Runner
    {
        private const string HelpMessage =
            @"The console is used to get direct access to certain commands. Commands have the form Name.Action(Argument) ex. Inventory.AddItem(Crowbar) to add a crowbar to your inventory.

Use List.Commands() to see all valid commands.";

        public HelpRunner()
        {
        }

        public override string[] Names => throw new NotImplementedException();

        public override string[] Actions => throw new NotImplementedException();

        public override ConsoleResult Run(ConsoleCommand command)
        {
            return new ConsoleResult(HelpMessage, ResultType.Information);
        }

        public override string List()
        {
            return string.Empty;
        }

        public override bool Contains(ConsoleCommand command)
        {
            return command.Name.ContainsIgnoreCase("Help");
        }
    }
}
