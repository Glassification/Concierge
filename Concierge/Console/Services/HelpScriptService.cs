// <copyright file="HelpScriptService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Services
{
    using System;

    public sealed class HelpScriptService : ScriptService
    {
        private const string HelpMessage =
            @"The console is used to get direct access to certain commands. Commands have the form Name.Action(Argument) ex. Inventory.AddItem(Crowbar) to add a crowbar to your inventory.

Use List.Commands() to see all valid commands.";

        public HelpScriptService()
        {
        }

        public override string[] Names => throw new NotImplementedException();

        public override string[] Actions => throw new NotImplementedException();

        public override ConsoleResult Run(ConsoleCommand command)
        {
            return new ConsoleResult(HelpMessage, Enums.ResultType.Success);
        }

        public override string List()
        {
            return string.Empty;
        }
    }
}
