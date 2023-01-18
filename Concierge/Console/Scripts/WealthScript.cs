// <copyright file="WealthScript.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Scripts
{
    using System;

    using Concierge.Character.Statuses;
    using Concierge.Console.Enums;
    using Concierge.Utility.Extensions;

    public class WealthScript : IScript
    {
        public WealthScript(Wealth wealth)
        {
            this.Wealth = wealth;
        }

        private Wealth Wealth { get; set; }

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

            return new ConsoleResult($"Implementation for '{command.Action}' not found.", ResultType.Error);
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
                    this.Wealth.Copper += amount;
                    return this.Wealth.Copper;
                case "silver":
                    this.Wealth.Silver += amount;
                    return this.Wealth.Silver;
                case "electrum":
                    this.Wealth.Electrum += amount;
                    return this.Wealth.Electrum;
                case "gold":
                    this.Wealth.Gold += amount;
                    return this.Wealth.Gold;
                case "platinum":
                    this.Wealth.Platinum += amount;
                    return this.Wealth.Platinum;
                default:
                    return 0;
            }
        }
    }
}
