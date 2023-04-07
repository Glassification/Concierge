// <copyright file="ReadWriterScriptService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Services
{
    using System.Collections.ObjectModel;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Items;
    using Concierge.Character.Spellcasting;
    using Concierge.Character.Statuses;
    using Concierge.Console.Enums;
    using Concierge.Console.Scripts;
    using Concierge.Persistence;
    using Concierge.Primitives;
    using Concierge.Tools.Generators.Names;
    using Concierge.Utility;

    public sealed class ReadWriterScriptService : ScriptService
    {
        private static readonly string[] names = new string[]
        {
            "Ability",
            "Ammunition",
            "Inventory",
            "Language",
            "Names",
            "Resources",
            "Spell",
            "Subclass",
            "Subrace",
            "Weapon",
            "Alignment",
            "AutosaveInterval",
            "Background",
            "Category",
            "ClassName",
            "Game",
            "Instrument",
            "LevelExp",
            "MagicClassName",
            "Proficiency",
            "Race",
            "StatusEffect",
            "Tool",
            "Character",
        };

        private static readonly string[] actions = new string[]
        {
            "Read",
            "Write",
            "New",
        };

        public ReadWriterScriptService()
            : this(true)
        {
        }

        public ReadWriterScriptService(bool isFirst)
        {
            this.IsFirstList = isFirst;
        }

        public override string[] Names => names;

        public override string[] Actions => actions;

        public override ConsoleResult Run(ConsoleCommand command)
        {
            return command.Name.ToLower() switch
            {
                "ability" => new ReadWriterScript<ReadOnlyCollection<Ability>>(Constants.Abilities).Evaluate(command),
                "ammunition" => new ReadWriterScript<ReadOnlyCollection<Ammunition>>(Constants.Ammunitions).Evaluate(command),
                "inventory" => new ReadWriterScript<ReadOnlyCollection<Inventory>>(Constants.Inventories).Evaluate(command),
                "language" => new ReadWriterScript<ReadOnlyCollection<Language>>(Constants.Languages).Evaluate(command),
                "names" => new ReadWriterScript<ReadOnlyCollection<Name>>(Constants.Names).Evaluate(command),
                "resources" => new ReadWriterScript<ReadOnlyCollection<ClassResource>>(Constants.Resources).Evaluate(command),
                "spell" => new ReadWriterScript<ReadOnlyCollection<Spell>>(Constants.Spells).Evaluate(command),
                "subclass" => new ReadWriterScript<ReadOnlyCollection<CategoryItem>>(Constants.Subclass).Evaluate(command),
                "subrace" => new ReadWriterScript<ReadOnlyCollection<CategoryItem>>(Constants.Subrace).Evaluate(command),
                "weapon" => new ReadWriterScript<ReadOnlyCollection<Weapon>>(Constants.Weapons).Evaluate(command),
                "alignment" => new ReadWriterScript<ReadOnlyCollection<string>>(Constants.Alignment).Evaluate(command),
                "autosaveinterval" => new ReadWriterScript<ReadOnlyCollection<int>>(Constants.AutosaveIntervals).Evaluate(command),
                "background" => new ReadWriterScript<ReadOnlyCollection<string>>(Constants.Backgrounds).Evaluate(command),
                "category" => new ReadWriterScript<ReadOnlyCollection<string>>(Constants.ItemCategories).Evaluate(command),
                "classname" => new ReadWriterScript<ReadOnlyCollection<string>>(Constants.Classes).Evaluate(command),
                "game" => new ReadWriterScript<ReadOnlyCollection<string>>(Constants.Games).Evaluate(command),
                "instrument" => new ReadWriterScript<ReadOnlyCollection<string>>(Constants.Instruments).Evaluate(command),
                "levelexp" => new ReadWriterScript<ReadOnlyCollection<int>>(Constants.Levels).Evaluate(command),
                "magicclassname" => new ReadWriterScript<ReadOnlyCollection<string>>(Constants.MagicClasses).Evaluate(command),
                "proficiency" => new ReadWriterScript<ReadOnlyCollection<int>>(Constants.ProficiencyLevels).Evaluate(command),
                "race" => new ReadWriterScript<ReadOnlyCollection<string>>(Constants.Races).Evaluate(command),
                "statuseffect" => new ReadWriterScript<ReadOnlyCollection<string>>(Constants.StatusEffects).Evaluate(command),
                "tool" => new ReadWriterScript<ReadOnlyCollection<string>>(Constants.Tools).Evaluate(command),
                "character" => RunForCharacterSheet(command),
                _ => new ConsoleResult($"Error: '{command}' does not contain a valid command.", ResultType.Error),
            };
        }

        private static ConsoleResult RunForCharacterSheet(ConsoleCommand command)
        {
            var result = new ReadWriterScript<CcsFile>(Program.CcsFile).Evaluate(command);

            return result;
        }
    }
}
