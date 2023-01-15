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

        public ConsoleResult Run(string command, string name)
        {
            return RunListScript(command, name);
        }

        private static ConsoleResult RunListScript(string command, string name)
        {
            return name switch
            {
                "Inventory" => new ListScript<Inventory>(Constants.Inventories.ToList(), Program.CcsFile.Character.Inventories, name).Evaluate(command),
                "Weapons" => new ListScript<Weapon>(Constants.Weapons.ToList(), Program.CcsFile.Character.Weapons, name).Evaluate(command),
                "Ammunition" => new ListScript<Ammunition>(Constants.Ammunitions.ToList(), Program.CcsFile.Character.Ammunitions, name).Evaluate(command),
                "Spells" => new ListScript<Spell>(Constants.Spells.ToList(), Program.CcsFile.Character.Spells, name).Evaluate(command),
                "MagicClasses" => new ListScript<MagicClass>(new List<MagicClass>(), Program.CcsFile.Character.MagicClasses, name).Evaluate(command),
                "Ability" => new ListScript<Ability>(Constants.Abilities.ToList(), Program.CcsFile.Character.Abilities, name).Evaluate(command),
                "Language" => new ListScript<Language>(Constants.Languages.ToList(), Program.CcsFile.Character.Languages, name).Evaluate(command),
                "ClassResource" => new ListScript<ClassResource>(new List<ClassResource>(), Program.CcsFile.Character.ClassResources, name).Evaluate(command),
                "StatusEffect" => new ListScript<StatusEffect>(new List<StatusEffect>(), Program.CcsFile.Character.StatusEffects, name).Evaluate(command),
                "All" => RunAllListScripts(command),
                _ => new ConsoleResult($"Error: '{command}' does not contain a valid command.", ResultType.Error),
            };
        }

        private static ConsoleResult RunAllListScripts(string command)
        {
            var scriptList = ListScripts.ToList();
            var result = true;

            scriptList.Remove("All");
            foreach (var script in scriptList)
            {
                result &= RunListScript(command, script).Type == ResultType.Success;
            }

            return result ?
                new ConsoleResult("All lists updated.", ResultType.Success) :
                new ConsoleResult("Error: Could not update all lists.", ResultType.Error);
        }
    }
}
