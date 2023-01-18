// <copyright file="ListScriptService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Items;
    using Concierge.Character.Spellcasting;
    using Concierge.Character.Statuses;
    using Concierge.Console.Enums;
    using Concierge.Console.Scripts;
    using Concierge.Utility;

    public class ListScriptService : IScriptService
    {
        public static readonly string[] ListScripts = new string[]
        {
            "Inventory",
            "Weapons",
            "Ammunition",
            "Spells",
            "MagicClasses",
            "Ability",
            "Language",
            "ClassResource",
            "StatusEffect",
            "All",
        };

        public ListScriptService()
        {
        }

        public ConsoleResult Run(ConsoleCommand command)
        {
            return RunListScript(command);
        }

        private static ConsoleResult RunListScript(ConsoleCommand command)
        {
            return command.Name.ToLower() switch
            {
                "inventory" => new ListScript<Inventory>(Constants.Inventories.ToList(), Program.CcsFile.Character.Inventories).Evaluate(command),
                "weapons" => new ListScript<Weapon>(Constants.Weapons.ToList(), Program.CcsFile.Character.Weapons).Evaluate(command),
                "ammunition" => new ListScript<Ammunition>(Constants.Ammunitions.ToList(), Program.CcsFile.Character.Ammunitions).Evaluate(command),
                "spells" => new ListScript<Spell>(Constants.Spells.ToList(), Program.CcsFile.Character.Spells).Evaluate(command),
                "magicclasses" => new ListScript<MagicClass>(new List<MagicClass>(), Program.CcsFile.Character.MagicClasses).Evaluate(command),
                "ability" => new ListScript<Ability>(Constants.Abilities.ToList(), Program.CcsFile.Character.Abilities).Evaluate(command),
                "language" => new ListScript<Language>(Constants.Languages.ToList(), Program.CcsFile.Character.Languages).Evaluate(command),
                "classresource" => new ListScript<ClassResource>(new List<ClassResource>(), Program.CcsFile.Character.ClassResources).Evaluate(command),
                "statuseffect" => new ListScript<StatusEffect>(new List<StatusEffect>(), Program.CcsFile.Character.StatusEffects).Evaluate(command),
                "all" => RunAllListScripts(command),
                _ => new ConsoleResult($"Error: '{command}' does not contain a valid command.", ResultType.Error),
            };
        }

        private static ConsoleResult RunAllListScripts(ConsoleCommand command)
        {
            var scriptList = ListScripts.ToList();
            var result = true;

            scriptList.Remove("All");
            foreach (var script in scriptList)
            {
                var consoleResult = RunListScript(new ConsoleCommand($"{script}.{command.Action}({command.Argument})"));
                result &= consoleResult.Type == ResultType.Success || consoleResult.Type == ResultType.Warning;
            }

            return result ?
                new ConsoleResult("All lists updated.", ResultType.Success) :
                new ConsoleResult("Error: Could not update all lists.", ResultType.Error);
        }
    }
}
