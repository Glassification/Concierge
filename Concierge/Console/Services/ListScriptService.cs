// <copyright file="ListScriptService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Services
{
    using System.Linq;

    using Concierge.Character.Details;
    using Concierge.Character.Equipable;
    using Concierge.Character.Magic;
    using Concierge.Character.Vitals;
    using Concierge.Console.Enums;
    using Concierge.Console.Scripts;

    public sealed class ListScriptService : ScriptService
    {
        private static readonly string[] names =
        [
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
        ];

        private static readonly string[] actions =
        [
            "AddItem",
            "RemoveItem",
            "Count",
            "AddCategory",
            "GetId",
        ];

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
                "inventory" => new ListScript<Inventory>([.. Defaults.Inventory], character.Equipment.Inventory).Evaluate(command),
                "weapons" => new ListScript<Weapon>([.. Defaults.Weapons], character.Equipment.Weapons).Evaluate(command),
                "augmentation" => new ListScript<Augment>([.. Defaults.Augmentation], character.Equipment.Augmentation).Evaluate(command),
                "spells" => new ListScript<Spell>([.. Defaults.Spells], character.SpellCasting.Spells).Evaluate(command),
                "magicclasses" => new ListScript<MagicalClass>([], character.SpellCasting.MagicalClasses).Evaluate(command),
                "ability" => new ListScript<Ability>([.. Defaults.Abilities], character.Detail.Abilities).Evaluate(command),
                "language" => new ListScript<Language>([.. Defaults.Languages], character.Detail.Languages).Evaluate(command),
                "classresource" => new ListScript<ClassResource>([], character.Vitality.ClassResources).Evaluate(command),
                "statuseffect" => new ListScript<StatusEffect>([], character.Vitality.Status.StatusEffects).Evaluate(command),
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
