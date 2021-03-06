﻿// <copyright file="Constants.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;

    using Concierge.Characters.Collections;
    using Concierge.Persistence;

    public static class Constants
    {
        public const int MaxLevel = 20;
        public const int BaseDC = 8;
        public const int CoinGroup = 50;
        public const int MaxScore = 30;
        public const int MinScore = 0;
        public const int BasePerception = 10;
        public const int MaxClasses = 3;

        private static readonly int[] levels =
        {
            300, 900, 2700, 6500, 14000, 23000, 34000, 48000, 64000, 85000, 100000,
            120000, 140000, 165000, 195000, 225000, 265000, 305000, 355000, 0,
        };

        private static readonly int[] proficiencies = { 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6 };

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
            "Charlatan",
            "Criminal",
            "Entertainer",
            "Folk Hero",
            "Guild Artisan",
            "Haunted One",
            "Hermit",
            "Noble",
            "Outlander",
            "Sage",
            "Sailor",
            "Soldier",
            "Urchin",
        };

        static Constants()
        {
            AutosaveIntervals = new ReadOnlyCollection<int>(new List<int> { 1, 5, 10, 15, 20, 30, 45, 60, 90, 120 });

            Weapons = new ReadOnlyCollection<Weapon>(DefaultListLoader.LoadWeaponList());
            Ammunitions = new ReadOnlyCollection<Ammunition>(DefaultListLoader.LoadAmmunitionList());
            Spells = new ReadOnlyCollection<Spell>(DefaultListLoader.LoadSpellList());
            Inventories = new ReadOnlyCollection<Inventory>(DefaultListLoader.LoadInventoryList());
            Languages = new ReadOnlyCollection<Language>(DefaultListLoader.LoadLanguageList());

            Alignment = new ReadOnlyCollection<string>(alignment);
            Backgrounds = new ReadOnlyCollection<string>(backgrounds);
            Races = new ReadOnlyCollection<string>(races);
            Levels = new ReadOnlyCollection<int>(levels);
            Proficiencies = new ReadOnlyCollection<int>(proficiencies);
        }

        public static ReadOnlyCollection<int> AutosaveIntervals { get; private set; }

        public static ReadOnlyCollection<Weapon> Weapons { get; private set; }

        public static ReadOnlyCollection<Ammunition> Ammunitions { get; private set; }

        public static ReadOnlyCollection<Spell> Spells { get; private set; }

        public static ReadOnlyCollection<Inventory> Inventories { get; private set; }

        public static ReadOnlyCollection<Language> Languages { get; private set; }

        public static ReadOnlyCollection<string> Alignment { get; }

        public static ReadOnlyCollection<string> Backgrounds { get; }

        public static ReadOnlyCollection<string> Races { get; }

        public static ReadOnlyCollection<int> Levels { get; }

        public static ReadOnlyCollection<int> Proficiencies { get; }

        public static string AssemblyVersion
        {
            get
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return $"{version.Major}.{version.Minor}.{version.Build}";
            }
        }
    }
}
