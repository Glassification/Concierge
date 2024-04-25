// <copyright file="Defaults.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Concierge.Character.Details;
    using Concierge.Character.Equipable;
    using Concierge.Character.Magic;
    using Concierge.Character.Vitals;
    using Concierge.Configuration;
    using Concierge.Data;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Tools.Generators.Names;

    /// <summary>
    /// Provides default data for various aspects of the application, such as weapons, spells, classes, and races.
    /// </summary>
    public static class Defaults
    {
        private static readonly DefaultListReadWriter defaultListReadWriter = new (Program.ErrorService, Program.Logger);

        static Defaults()
        {
            Weapons = ReadJson<Weapon>(Properties.Resources.Weapon, nameof(Properties.Resources.Weapon));
            Augmentation = ReadJson<Augment>(Properties.Resources.Augmentation, nameof(Properties.Resources.Augmentation));
            Spells = ReadJson<Spell>(Properties.Resources.Spell, nameof(Properties.Resources.Spell));
            Inventory = ReadJson<Inventory>(Properties.Resources.Inventory, nameof(Properties.Resources.Inventory));
            Languages = ReadJson<Language>(Properties.Resources.Language, nameof(Properties.Resources.Language));
            Abilities = ReadJson<Ability>(Properties.Resources.Ability, nameof(Properties.Resources.Ability));
            Subrace = ReadJson<CategoryItem>(Properties.Resources.Subrace, nameof(Properties.Resources.Subrace));
            Subclass = ReadJson<CategoryItem>(Properties.Resources.Subclass, nameof(Properties.Resources.Subclass));
            Resources = ReadJson<ClassResource>(Properties.Resources.Resource, nameof(Properties.Resources.Resource));
            Names = ReadJson<Name>(Properties.Resources.Names, nameof(Properties.Resources.Names));
            Glossary = ReadJson<GlossaryEntry>(Properties.Resources.Glossary, nameof(Properties.Resources.Glossary));
            MagicClasses = ReadJson<MagicalClass>(Properties.Resources.MagicClass, nameof(Properties.Resources.MagicClass));
            StatusEffects = ReadJson<StatusEffect>(Properties.Resources.StatusEffect, nameof(Properties.Resources.StatusEffect));
            Armor = ReadJson<Armor>(Properties.Resources.Armor, nameof(Properties.Resources.Armor));
            Classes = ReadJson<DetailedItem>(Properties.Resources.Class, nameof(Properties.Resources.Class));
            Races = ReadJson<DetailedItem>(Properties.Resources.Race, nameof(Properties.Resources.Race));

            AutosaveIntervals = ReadList<int>(Properties.Resources.AutosaveInterval, nameof(Properties.Resources.AutosaveInterval));
            Alignment = ReadList<string>(Properties.Resources.Alignment, nameof(Properties.Resources.Alignment));
            Backgrounds = ReadList<string>(Properties.Resources.Background, nameof(Properties.Resources.Background));
            Levels = ReadList<int>(Properties.Resources.LevelExp, nameof(Properties.Resources.LevelExp));
            Tools = ReadList<string>(Properties.Resources.Tool, nameof(Properties.Resources.Tool));
            Games = ReadList<string>(Properties.Resources.Game, nameof(Properties.Resources.Game));
            Instruments = ReadList<string>(Properties.Resources.Instrument, nameof(Properties.Resources.Instrument));
            ItemCategories = ReadList<string>(Properties.Resources.Category, nameof(Properties.Resources.Category));
        }

        /// <summary>
        /// Gets the collection of default autosave intervals.
        /// </summary>
        public static ReadOnlyCollection<int> AutosaveIntervals { get; }

        /// <summary>
        /// Gets the collection of default weapons.
        /// </summary>
        public static ReadOnlyCollection<Weapon> Weapons { get; }

        /// <summary>
        /// Gets the collection of default augmentations.
        /// </summary>
        public static ReadOnlyCollection<Augment> Augmentation { get; }

        /// <summary>
        /// Gets the collection of default spells.
        /// </summary>
        public static ReadOnlyCollection<Spell> Spells { get; }

        /// <summary>
        /// Gets the collection of default inventory items.
        /// </summary>
        public static ReadOnlyCollection<Inventory> Inventory { get; }

        /// <summary>
        /// Gets the collection of default languages.
        /// </summary>
        public static ReadOnlyCollection<Language> Languages { get; }

        /// <summary>
        /// Gets the collection of default alignment values.
        /// </summary>
        public static ReadOnlyCollection<string> Alignment { get; }

        /// <summary>
        /// Gets the collection of default backgrounds.
        /// </summary>
        public static ReadOnlyCollection<string> Backgrounds { get; }

        /// <summary>
        /// Gets the collection of default races.
        /// </summary>
        public static ReadOnlyCollection<DetailedItem> Races { get; }

        /// <summary>
        /// Gets the collection of default levels or experience values.
        /// </summary>
        public static ReadOnlyCollection<int> Levels { get; }

        /// <summary>
        /// Gets the collection of default classes.
        /// </summary>
        public static ReadOnlyCollection<DetailedItem> Classes { get; }

        /// <summary>
        /// Gets the collection of default magical classes.
        /// </summary>
        public static ReadOnlyCollection<MagicalClass> MagicClasses { get; }

        /// <summary>
        /// Gets the collection of default abilities.
        /// </summary>
        public static ReadOnlyCollection<Ability> Abilities { get; }

        /// <summary>
        /// Gets the collection of default status effects.
        /// </summary>
        public static ReadOnlyCollection<StatusEffect> StatusEffects { get; }

        /// <summary>
        /// Gets the collection of default class resources.
        /// </summary>
        public static ReadOnlyCollection<ClassResource> Resources { get; }

        /// <summary>
        /// Gets the collection of default tools.
        /// </summary>
        public static ReadOnlyCollection<string> Tools { get; }

        /// <summary>
        /// Gets the collection of default games.
        /// </summary>
        public static ReadOnlyCollection<string> Games { get; }

        /// <summary>
        /// Gets the collection of default musical instruments.
        /// </summary>
        public static ReadOnlyCollection<string> Instruments { get; }

        /// <summary>
        /// Gets the collection of default item categories.
        /// </summary>
        public static ReadOnlyCollection<string> ItemCategories { get; }

        /// <summary>
        /// Gets the collection of default subraces.
        /// </summary>
        public static ReadOnlyCollection<CategoryItem> Subrace { get; }

        /// <summary>
        /// Gets the collection of default subclasses.
        /// </summary>
        public static ReadOnlyCollection<CategoryItem> Subclass { get; }

        /// <summary>
        /// Gets the collection of default names.
        /// </summary>
        public static ReadOnlyCollection<Name> Names { get; }

        /// <summary>
        /// Gets the collection of default glossary entries.
        /// </summary>
        public static ReadOnlyCollection<GlossaryEntry> Glossary { get; }

        /// <summary>
        /// Gets the collection of default armor items.
        /// </summary>
        public static ReadOnlyCollection<Armor> Armor { get; }

        /// <summary>
        /// Gets the current autosave interval based on user settings.
        /// </summary>
        public static int CurrentAutosaveInterval => AutosaveIntervals[AppSettingsManager.UserSettings.Autosaving.Interval];

        private static ReadOnlyCollection<T> ReadJson<T>(byte[] json, string name)
        {
            Program.Logger.Info($"Loading {name}...");
            return new ReadOnlyCollection<T>(defaultListReadWriter.ReadJson<List<T>>(json));
        }

        private static ReadOnlyCollection<T> ReadList<T>(string path, string name)
        {
            Program.Logger.Info($"Loading {name}...");
            return new ReadOnlyCollection<T>(defaultListReadWriter.ReadList<T>(path));
        }
    }
}
