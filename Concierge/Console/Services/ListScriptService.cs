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

    public sealed class ListScriptService : ScriptService
    {
        private static readonly string[] names = new string[]
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

        private static readonly string[] actions = new string[]
        {
            "AddItem",
            "RemoveItem",
            "Count",
        };

        public ListScriptService()
            : this(true)
        {
        }

        public ListScriptService(bool isFirst)
        {
            this.IsFirstList = isFirst;
        }

        public override string[] Names => names;

        public override string[] Actions => actions;

        public override ConsoleResult Run(ConsoleCommand command)
        {
            return this.RunListScript(command);
        }

        private ConsoleResult RunListScript(ConsoleCommand command)
        {
            var character = Program.CcsFile.Character;

            return command.Name.ToLower() switch
            {
                "inventory" => new ListScript<Inventory>(Constants.Inventories.ToList(), character.Inventories).Evaluate(command),
                "weapons" => new ListScript<Weapon>(Constants.Weapons.ToList(), character.Weapons).Evaluate(command),
                "ammunition" => new ListScript<Ammunition>(Constants.Ammunitions.ToList(), character.Ammunitions).Evaluate(command),
                "spells" => new ListScript<Spell>(Constants.Spells.ToList(), character.Spells).Evaluate(command),
                "magicclasses" => new ListScript<MagicClass>(new List<MagicClass>(), character.MagicClasses).Evaluate(command),
                "ability" => new ListScript<Ability>(Constants.Abilities.ToList(), character.Abilities).Evaluate(command),
                "language" => new ListScript<Language>(Constants.Languages.ToList(), character.Languages).Evaluate(command),
                "classresource" => new ListScript<ClassResource>(new List<ClassResource>(), character.ClassResources).Evaluate(command),
                "statuseffect" => new ListScript<StatusEffect>(new List<StatusEffect>(), character.StatusEffects).Evaluate(command),
                "all" => this.RunAllListScripts(command),
                _ => new ConsoleResult($"Error: '{command}' does not contain a valid command.", ResultType.Error),
            };
        }

        private ConsoleResult RunAllListScripts(ConsoleCommand command)
        {
            var scriptList = this.Names.ToList();
            var result = true;

            scriptList.Remove("All");
            foreach (var script in scriptList)
            {
                var consoleResult = this.RunListScript(new ConsoleCommand($"{script}.{command.Action}({command.Argument})"));
                result &= consoleResult.Type == ResultType.Success || consoleResult.Type == ResultType.Warning;
            }

            return result ?
                new ConsoleResult("All lists updated.", ResultType.Success) :
                new ConsoleResult("Error: Could not update all lists.", ResultType.Error);
        }
    }
}
