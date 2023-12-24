// <copyright file="Defaults.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge
{
    using System.Collections.Generic;
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
            var defaultListReadWriter = new DefaultListReadWriter(Program.ErrorService, Program.Logger);

            Weapons = new ReadOnlyCollection<Weapon>(defaultListReadWriter.ReadJson<List<Weapon>>(Properties.Resources.Weapon));
            Ammunition = new ReadOnlyCollection<Ammunition>(defaultListReadWriter.ReadJson<List<Ammunition>>(Properties.Resources.Ammunition));
            Spells = new ReadOnlyCollection<Spell>(defaultListReadWriter.ReadJson<List<Spell>>(Properties.Resources.Spell));
            Inventory = new ReadOnlyCollection<Inventory>(defaultListReadWriter.ReadJson<List<Inventory>>(Properties.Resources.Inventory));
            Languages = new ReadOnlyCollection<Language>(defaultListReadWriter.ReadJson<List<Language>>(Properties.Resources.Language));
            Abilities = new ReadOnlyCollection<Ability>(defaultListReadWriter.ReadJson<List<Ability>>(Properties.Resources.Ability));
            Subrace = new ReadOnlyCollection<CategoryItem>(defaultListReadWriter.ReadJson<List<CategoryItem>>(Properties.Resources.Subrace));
            Subclass = new ReadOnlyCollection<CategoryItem>(defaultListReadWriter.ReadJson<List<CategoryItem>>(Properties.Resources.Subclass));
            Resources = new ReadOnlyCollection<ClassResource>(defaultListReadWriter.ReadJson<List<ClassResource>>(Properties.Resources.Resource));
            Names = new ReadOnlyCollection<Name>(defaultListReadWriter.ReadJson<List<Name>>(Properties.Resources.Names));
            Glossary = new ReadOnlyCollection<GlossaryEntry>(defaultListReadWriter.ReadJson<List<GlossaryEntry>>(Properties.Resources.Glossary));
            MagicClasses = new ReadOnlyCollection<MagicClass>(defaultListReadWriter.ReadJson<List<MagicClass>>(Properties.Resources.MagicClass));
            StatusEffects = new ReadOnlyCollection<StatusEffect>(defaultListReadWriter.ReadJson<List<StatusEffect>>(Properties.Resources.StatusEffect));
            Armor = new ReadOnlyCollection<Armor>(defaultListReadWriter.ReadJson<List<Armor>>(Properties.Resources.Armor));
            Classes = new ReadOnlyCollection<DetailedItem>(defaultListReadWriter.ReadJson<List<DetailedItem>>(Properties.Resources.Class));
            Races = new ReadOnlyCollection<DetailedItem>(defaultListReadWriter.ReadJson<List<DetailedItem>>(Properties.Resources.Race));

            AutosaveIntervals = new ReadOnlyCollection<int>(defaultListReadWriter.ReadList<int>(Properties.Resources.AutosaveInterval));
            Alignment = new ReadOnlyCollection<string>(defaultListReadWriter.ReadList<string>(Properties.Resources.Alignment));
            Backgrounds = new ReadOnlyCollection<string>(defaultListReadWriter.ReadList<string>(Properties.Resources.Background));
            Levels = new ReadOnlyCollection<int>(defaultListReadWriter.ReadList<int>(Properties.Resources.LevelExp));
            ProficiencyLevels = new ReadOnlyCollection<int>(defaultListReadWriter.ReadList<int>(Properties.Resources.ProficiencyLevel));
            Tools = new ReadOnlyCollection<string>(defaultListReadWriter.ReadList<string>(Properties.Resources.Tool));
            Games = new ReadOnlyCollection<string>(defaultListReadWriter.ReadList<string>(Properties.Resources.Game));
            Instruments = new ReadOnlyCollection<string>(defaultListReadWriter.ReadList<string>(Properties.Resources.Instrument));
            ItemCategories = new ReadOnlyCollection<string>(defaultListReadWriter.ReadList<string>(Properties.Resources.Category));
        }

        public static ReadOnlyCollection<int> AutosaveIntervals { get; }

        public static ReadOnlyCollection<Weapon> Weapons { get; }

        public static ReadOnlyCollection<Ammunition> Ammunition { get; }

        public static ReadOnlyCollection<Spell> Spells { get; }

        public static ReadOnlyCollection<Inventory> Inventory { get; }

        public static ReadOnlyCollection<Language> Languages { get; }

        public static ReadOnlyCollection<string> Alignment { get; }

        public static ReadOnlyCollection<string> Backgrounds { get; }

        public static ReadOnlyCollection<DetailedItem> Races { get; }

        public static ReadOnlyCollection<int> Levels { get; }

        public static ReadOnlyCollection<int> ProficiencyLevels { get; }

        public static ReadOnlyCollection<DetailedItem> Classes { get; }

        public static ReadOnlyCollection<MagicClass> MagicClasses { get; }

        public static ReadOnlyCollection<Ability> Abilities { get; }

        public static ReadOnlyCollection<StatusEffect> StatusEffects { get; }

        public static ReadOnlyCollection<ClassResource> Resources { get; }

        public static ReadOnlyCollection<string> Tools { get; }

        public static ReadOnlyCollection<string> Games { get; }

        public static ReadOnlyCollection<string> Instruments { get; }

        public static ReadOnlyCollection<string> ItemCategories { get; }

        public static ReadOnlyCollection<CategoryItem> Subrace { get; }

        public static ReadOnlyCollection<CategoryItem> Subclass { get; }

        public static ReadOnlyCollection<Name> Names { get; }

        public static ReadOnlyCollection<GlossaryEntry> Glossary { get; }

        public static ReadOnlyCollection<Armor> Armor { get; }

        public static int CurrentAutosaveInterval => AutosaveIntervals[AppSettingsManager.UserSettings.Autosaving.Interval];
    }
}
