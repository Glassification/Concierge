// <copyright file="WealthScript.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Scripts
{
    using System;

    using Concierge.Character;
    using Concierge.Common.Extensions;
    using Concierge.Console.Enums;

    public sealed class WealthScript : IScript
    {
        private readonly Wealth wealth;

        public WealthScript(Wealth wealth)
        {
            this.wealth = wealth;
        }

        public ConsoleResult Evaluate(ConsoleCommand command)
        {
            if (command.Action.Equals("Add", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.AddWealth(command);
            }

            if (command.Action.Equals("Subtract", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.SubtractWealth(command);
            }

            if (command.Action.Equals("Count", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.Count(command);
            }

            return new ConsoleResult($"Implementation for '{command.Action}' not found.", ResultType.NotImplemented);
        }

        private ConsoleResult AddWealth(ConsoleCommand command)
        {
            if (!int.TryParse(command.Argument, out int value))
            {
                return new ConsoleResult($"'{command.Argument}' is not a number.", ResultType.Error);
            }

            this.Modify(command.Name, value);

            return new ConsoleResult($"{value} {command.Name} added.", ResultType.Success);
        }

        private ConsoleResult SubtractWealth(ConsoleCommand command)
        {
            if (!int.TryParse(command.Argument, out int value))
            {
                return new ConsoleResult($"'{command.Argument}' is not a number.", ResultType.Error);
            }

            this.Modify(command.Name, -value);

            return new ConsoleResult($"{value} {command.Name} subtracted.", ResultType.Success);
        }

        private ConsoleResult Count(ConsoleCommand command)
        {
            if (command.Argument.IsNullOrWhiteSpace())
            {
                return new ConsoleResult($"{this.Modify(command.Name, 0)} {command.Name}.", ResultType.Success);
            }

            return new ConsoleResult($"Counting specific items is not implemented.", ResultType.Error);
        }

        private int Modify(string wealthType, int amount)
        {
            wealthType = wealthType.ToLower();
            switch (wealthType)
            {
                case "copper":
                    this.wealth.Copper += amount;
                    return this.wealth.Copper;
                case "silver":
                    this.wealth.Silver += amount;
                    return this.wealth.Silver;
                case "electrum":
                    this.wealth.Electrum += amount;
                    return this.wealth.Electrum;
                case "gold":
                    this.wealth.Gold += amount;
                    return this.wealth.Gold;
                case "platinum":
                    this.wealth.Platinum += amount;
                    return this.wealth.Platinum;
                default:
                    return 0;
            }
        }
    }
}
