﻿// <copyright file="Defaults.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System.Collections.ObjectModel;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Equipable;
    using Concierge.Character.Spellcasting;
    using Concierge.Character.Vitals;
    using Concierge.Configuration;
    using Concierge.Data;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Tools.Generators.Names;

    public static class Defaults
    {
        static Defaults()
        {
            Weapons = new ReadOnlyCollection<Weapon>(DefaultListReadWriter.ReadJson<Weapon>(Properties.Resources.Weapon));
            Ammunition = new ReadOnlyCollection<Ammunition>(DefaultListReadWriter.ReadJson<Ammunition>(Properties.Resources.Ammunition));
            Spells = new ReadOnlyCollection<Spell>(DefaultListReadWriter.ReadJson<Spell>(Properties.Resources.Spell));
            Inventory = new ReadOnlyCollection<Inventory>(DefaultListReadWriter.ReadJson<Inventory>(Properties.Resources.Inventory));
            Languages = new ReadOnlyCollection<Language>(DefaultListReadWriter.ReadJson<Language>(Properties.Resources.Language));
            Abilities = new ReadOnlyCollection<Ability>(DefaultListReadWriter.ReadJson<Ability>(Properties.Resources.Ability));
            Subrace = new ReadOnlyCollection<CategoryItem>(DefaultListReadWriter.ReadJson<CategoryItem>(Properties.Resources.Subrace));
            Subclass = new ReadOnlyCollection<CategoryItem>(DefaultListReadWriter.ReadJson<CategoryItem>(Properties.Resources.Subclass));
            Resources = new ReadOnlyCollection<ClassResource>(DefaultListReadWriter.ReadJson<ClassResource>(Properties.Resources.Resource));
            Names = new ReadOnlyCollection<Name>(DefaultListReadWriter.ReadJson<Name>(Properties.Resources.Names));
            Glossary = new ReadOnlyCollection<GlossaryEntry>(DefaultListReadWriter.ReadJson<GlossaryEntry>(Properties.Resources.Glossary));

            AutosaveIntervals = new ReadOnlyCollection<int>(DefaultListReadWriter.ReadGenericList<int>(Properties.Resources.AutosaveInterval));
            Alignment = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadGenericList<string>(Properties.Resources.Alignment));
            Backgrounds = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadGenericList<string>(Properties.Resources.Background));
            Races = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadGenericList<string>(Properties.Resources.Race));
            Levels = new ReadOnlyCollection<int>(DefaultListReadWriter.ReadGenericList<int>(Properties.Resources.LevelExp));
            ProficiencyLevels = new ReadOnlyCollection<int>(DefaultListReadWriter.ReadGenericList<int>(Properties.Resources.ProficiencyLevel));
            Classes = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadGenericList<string>(Properties.Resources.ClassName));
            MagicClasses = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadGenericList<string>(Properties.Resources.MagicClassName));
            StatusEffects = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadGenericList<string>(Properties.Resources.StatusEffect));
            Tools = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadGenericList<string>(Properties.Resources.Tool));
            Games = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadGenericList<string>(Properties.Resources.Game));
            Instruments = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadGenericList<string>(Properties.Resources.Instrument));
            ItemCategories = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadGenericList<string>(Properties.Resources.Category));
        }

        public static ReadOnlyCollection<int> AutosaveIntervals { get; }

        public static ReadOnlyCollection<Weapon> Weapons { get; }

        public static ReadOnlyCollection<Ammunition> Ammunition { get; }

        public static ReadOnlyCollection<Spell> Spells { get; }

        public static ReadOnlyCollection<Inventory> Inventory { get; }

        public static ReadOnlyCollection<Language> Languages { get; }

        public static ReadOnlyCollection<string> Alignment { get; }

        public static ReadOnlyCollection<string> Backgrounds { get; }

        public static ReadOnlyCollection<string> Races { get; }

        public static ReadOnlyCollection<int> Levels { get; }

        public static ReadOnlyCollection<int> ProficiencyLevels { get; }

        public static ReadOnlyCollection<string> Classes { get; }

        public static ReadOnlyCollection<string> MagicClasses { get; }

        public static ReadOnlyCollection<Ability> Abilities { get; }

        public static ReadOnlyCollection<string> StatusEffects { get; }

        public static ReadOnlyCollection<ClassResource> Resources { get; }

        public static ReadOnlyCollection<string> Tools { get; }

        public static ReadOnlyCollection<string> Games { get; }

        public static ReadOnlyCollection<string> Instruments { get; }

        public static ReadOnlyCollection<string> ItemCategories { get; }

        public static ReadOnlyCollection<CategoryItem> Subrace { get; }

        public static ReadOnlyCollection<CategoryItem> Subclass { get; }

        public static ReadOnlyCollection<Name> Names { get; }

        public static ReadOnlyCollection<GlossaryEntry> Glossary { get; }

        public static int CurrentAutosaveInterval => AutosaveIntervals[AppSettingsManager.UserSettings.Autosaving.Interval];
    }
}