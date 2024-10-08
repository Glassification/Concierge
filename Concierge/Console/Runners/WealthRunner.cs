﻿// <copyright file="WealthRunner.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Runners
{
    using System.Linq;

    using Concierge.Common.Extensions;
    using Concierge.Console.Enums;
    using Concierge.Console.Scripts;

    public sealed class WealthRunner : Runner
    {
        private static readonly string[] names =
        [
            "Copper",
            "Silver",
            "Electrum",
            "Gold",
            "Platinum",
            "All",
        ];

        private static readonly string[] actions =
        [
            "Add",
            "Subtract",
            "Count",
        ];

        public WealthRunner()
            : this(true)
        {
        }

        public WealthRunner(bool isFirst)
        {
            this.IsFirstList = isFirst;
        }

        public override string[] Names => names;

        public override string[] Actions => actions;

        public override ConsoleResult Run(ConsoleCommand command)
        {
            return command.Name.EqualsIgnoreCase("All") ? this.RunAllWealthScripts(command) : RunWealthScript(command);
        }

        private static ConsoleResult RunWealthScript(ConsoleCommand command)
        {
            return new WealthScript(Program.CcsFile.Character.Wealth).Evaluate(command);
        }

        private ConsoleResult RunAllWealthScripts(ConsoleCommand command)
        {
            var scriptList = this.Names.ToList();
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
