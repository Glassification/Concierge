// <copyright file="Constants.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System.Collections.ObjectModel;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Items;
    using Concierge.Character.Spellcasting;
    using Concierge.Persistence.ReadWriters;

    public static class Constants
    {
        public const int MaxLevel = 20;
        public const int BaseDC = 8;
        public const int CoinGroup = 50;
        public const int MaxScore = 30;
        public const int MinScore = 0;
        public const int BasePerception = 10;
        public const int MaxClasses = 3;
        public const int MaxAttunedItems = 3;
        public const int SignificantDigits = 2;
        public const int Currencies = 5;
        public const int MaxDepth = 10;

        public const string Designer = "Thomas Beckett";
        public const string License = "This program is provided as is. Thomas Beckett Inc. is not responsible for any injuries or TPKs that may result from the use of this product.";
        public const string Copyright = "2022 Most Rights Reserved.";

        private static readonly int[] levels =
        {
            300,
            900,
            2700,
            6500,
            14000,
            23000,
            34000,
            48000,
            64000,
            85000,
            100000,
            120000,
            140000,
            165000,
            195000,
            225000,
            265000,
            305000,
            355000,
            0,
        };

        private static readonly int[] proficiencies = { 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6 };

        private static readonly int[] autosaveIntervals = { 1, 5, 10, 15, 20, 30, 45, 60, 90, 120 };

        private static readonly string[] alignment =
        {
            "Lawful Good",
            "Neutral Good",
            "Chaotic Good",
            "Lawful Neutral",
            "True Neutral",
            "Chaotic Neutral",
            "Lawful Evil",
            "Neutral Evil",
            "Chaotic Evil",
        };

        private static readonly string[] races =
        {
            "Aarakocra",
            "Aasimar",
            "Bugbear",
            "Dragonborn",
            "Dwarf",
            "Elf",
            "Genasi",
            "Genasi",
            "Gnome",
            "Goliath",
            "Half-Elf",
            "Half-Orc",
            "Halfling",
            "Human",
            "Kenku",
            "Kenku",
            "Tabaxi",
            "Tiefling",
            "Tortle",
            "Triton",
            "Warforged",
            "Yuan-ti-Pureblood",
        };

        private static readonly string[] backgrounds =
        {
            "Acolyte",
            "Anthropologist",
            "Archaeologist",
            "Caravan Specialist",
            "Charlatan",
            "City Watch",
            "Cloistered Scholar",
            "Courtier",
            "Criminal",
            "Dissenter",
            "Entertainer",
            "Folk Hero",
            "Gladiator",
            "Guild Artisan",
            "Guild Merchant",
            "Harborfolk",
            "Haunted One",
            "Hermit",
            "Inheritor",
            "Initiate",
            "Inquisitor",
            "Investigator",
            "Knight",
            "Noble",
            "Outlander",
            "Pirate",
            "Sage",
            "Sailor",
            "Soldier",
            "Urchin",
        };

        private static readonly string[] classes =
        {
            "Artificer",
            "Barbarian",
            "Bard",
            "Blood Hunter",
            "Cleric",
            "Druid",
            "Fighter",
            "Gunslinger",
            "Monk",
            "Paladin",
            "Ranger",
            "Rogue",
            "Sorcerer",
            "Warlock",
            "Wizard",
        };

        private static readonly string[] statusEffects =
        {
            "Acid",
            "Cold",
            "Fire",
            "Force",
            "Lightning",
            "Necrotic",
            "Poison",
            "Psychic",
            "Radiant",
            "Thunder",
            "Nonmagical",
            "Magic Weapons",
            "Bludgeoning",
            "Slashing",
            "Piercing",
            "Spells",
        };

        static Constants()
        {
            AutosaveIntervals = new ReadOnlyCollection<int>(autosaveIntervals);

            Weapons = new ReadOnlyCollection<Weapon>(DefaultListReadWriter.ReadWeaponList());
            Ammunitions = new ReadOnlyCollection<Ammunition>(DefaultListReadWriter.ReadAmmunitionList());
            Spells = new ReadOnlyCollection<Spell>(DefaultListReadWriter.ReadSpellList());
            Inventories = new ReadOnlyCollection<Inventory>(DefaultListReadWriter.ReadInventoryList());
            Languages = new ReadOnlyCollection<Language>(DefaultListReadWriter.ReadLanguageList());
            Abilities = new ReadOnlyCollection<Ability>(DefaultListReadWriter.ReadAbilityList());

            Alignment = new ReadOnlyCollection<string>(alignment);
            Backgrounds = new ReadOnlyCollection<string>(backgrounds);
            Races = new ReadOnlyCollection<string>(races);
            Levels = new ReadOnlyCollection<int>(levels);
            Proficiencies = new ReadOnlyCollection<int>(proficiencies);
            Classes = new ReadOnlyCollection<string>(classes);
            StatusEffects = new ReadOnlyCollection<string>(statusEffects);
        }

        public static ReadOnlyCollection<int> AutosaveIntervals { get; }

        public static ReadOnlyCollection<Weapon> Weapons { get; }

        public static ReadOnlyCollection<Ammunition> Ammunitions { get; }

        public static ReadOnlyCollection<Spell> Spells { get; }

        public static ReadOnlyCollection<Inventory> Inventories { get; }

        public static ReadOnlyCollection<Language> Languages { get; }

        public static ReadOnlyCollection<string> Alignment { get; }

        public static ReadOnlyCollection<string> Backgrounds { get; }

        public static ReadOnlyCollection<string> Races { get; }

        public static ReadOnlyCollection<int> Levels { get; }

        public static ReadOnlyCollection<int> Proficiencies { get; }

        public static ReadOnlyCollection<string> Classes { get; }

        public static ReadOnlyCollection<Ability> Abilities { get; }

        public static ReadOnlyCollection<string> StatusEffects { get; }
    }
}
