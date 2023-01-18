// <copyright file="WealthScriptService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Services
{
    using System;
    using System.Linq;
    using System.Text;

    using Concierge.Console.Enums;
    using Concierge.Console.Scripts;

    public class WealthScriptService : IScriptService
    {
        public static readonly string[] WealthScripts = new string[]
        {
            "Copper",
            "Silver",
            "Electrum",
            "Gold",
            "Platinum",
            "All",
        };

        public WealthScriptService()
        {
        }

        public ConsoleResult Run(ConsoleCommand command)
        {
            return command.Name.Equals("All", StringComparison.InvariantCultureIgnoreCase) ? RunAllWealthScripts(command) : RunWealthScript(command);
        }

        private static ConsoleResult RunWealthScript(ConsoleCommand command)
        {
            return new WealthScript(Program.CcsFile.Character.Wealth).Evaluate(command);
        }

        private static ConsoleResult RunAllWealthScripts(ConsoleCommand command)
        {
            var scriptList = WealthScripts.ToList();
            var result = true;

            scriptList.Remove("All");
            foreach (var script in scriptList)
            {
                var consoleResult = RunWealthScript(new ConsoleCommand($"{script}.{command.Action}({command.Argument})"));
                result &= consoleResult.Type == ResultType.Success || consoleResult.Type == ResultType.Warning;
            }

            return result ?
                new ConsoleResult("All wealth updated.", ResultType.Success) :
                new ConsoleResult("Error: Could not update all wealth.", ResultType.Error);
        }
    }
}
