// <copyright file="ReadWriterRunner.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Runners
{
    using System.Collections.ObjectModel;

    using Concierge.Character.Details;
    using Concierge.Character.Equipable;
    using Concierge.Character.Magic;
    using Concierge.Character.Vitals;
    using Concierge.Common;
    using Concierge.Console.Enums;
    using Concierge.Console.Scripts;
    using Concierge.Data;
    using Concierge.Tools.Generators.Names;

    public sealed class ReadWriterRunner : Runner
    {
        private static readonly string[] names =
        [
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
            "GameState",
        ];

        private static readonly string[] actions =
        [
            "Read",
            "Write",
            "New",
        ];

        public ReadWriterRunner()
            : this(true)
        {
        }

        public ReadWriterRunner(bool isFirst)
        {
            this.IsFirstList = isFirst;
        }

        public override string[] Names => names;

        public override string[] Actions => actions;

        public override ConsoleResult Run(ConsoleCommand command)
        {
            return command.Name.ToLower() switch
            {
                "ability" => new ReadWriterScript<ReadOnlyCollection<Ability>>(Defaults.Abilities).Evaluate(command),
                "augmentation" => new ReadWriterScript<ReadOnlyCollection<Augment>>(Defaults.Augmentation).Evaluate(command),
                "inventory" => new ReadWriterScript<ReadOnlyCollection<Inventory>>(Defaults.Inventory).Evaluate(command),
                "language" => new ReadWriterScript<ReadOnlyCollection<Language>>(Defaults.Languages).Evaluate(command),
                "names" => new ReadWriterScript<ReadOnlyCollection<Name>>(Defaults.Names).Evaluate(command),
                "resources" => new ReadWriterScript<ReadOnlyCollection<ClassResource>>(Defaults.Resources).Evaluate(command),
                "spell" => new ReadWriterScript<ReadOnlyCollection<Spell>>(Defaults.Spells).Evaluate(command),
                "subclass" => new ReadWriterScript<ReadOnlyCollection<CategoryItem>>(Defaults.Subclass).Evaluate(command),
                "subrace" => new ReadWriterScript<ReadOnlyCollection<CategoryItem>>(Defaults.Subrace).Evaluate(command),
                "weapon" => new ReadWriterScript<ReadOnlyCollection<Weapon>>(Defaults.Weapons).Evaluate(command),
                "alignment" => new ReadWriterScript<ReadOnlyCollection<string>>(Defaults.Alignment).Evaluate(command),
                "autosaveinterval" => new ReadWriterScript<ReadOnlyCollection<int>>(Defaults.AutosaveIntervals).Evaluate(command),
                "background" => new ReadWriterScript<ReadOnlyCollection<string>>(Defaults.Backgrounds).Evaluate(command),
                "category" => new ReadWriterScript<ReadOnlyCollection<string>>(Defaults.ItemCategories).Evaluate(command),
                "classname" => new ReadWriterScript<ReadOnlyCollection<DetailedItem>>(Defaults.Classes).Evaluate(command),
                "game" => new ReadWriterScript<ReadOnlyCollection<string>>(Defaults.Games).Evaluate(command),
                "instrument" => new ReadWriterScript<ReadOnlyCollection<string>>(Defaults.Instruments).Evaluate(command),
                "levelexp" => new ReadWriterScript<ReadOnlyCollection<int>>(Defaults.Levels).Evaluate(command),
                "magicclassname" => new ReadWriterScript<ReadOnlyCollection<MagicalClass>>(Defaults.MagicClasses).Evaluate(command),
                "race" => new ReadWriterScript<ReadOnlyCollection<DetailedItem>>(Defaults.Races).Evaluate(command),
                "statuseffect" => new ReadWriterScript<ReadOnlyCollection<StatusEffect>>(Defaults.StatusEffects).Evaluate(command),
                "tool" => new ReadWriterScript<ReadOnlyCollection<string>>(Defaults.Tools).Evaluate(command),
                "character" => new ReadWriterScript<CcsFile>(Program.CcsFile).Evaluate(command),
                "gamestate" => new ReadWriterScript<int>(Constants.Void).Evaluate(command),
                _ => new ConsoleResult($"Error: '{command}' does not contain a valid command.", ResultType.Error),
            };
        }
    }
}
