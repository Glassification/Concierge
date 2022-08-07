// <copyright file="Constants.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Windows;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Items;
    using Concierge.Character.Spellcasting;
    using Concierge.Configuration;
    using Concierge.Persistence.ReadWriters;

    public static class Constants
    {
        public const byte ColorSpace = 255;

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
        public const int MaxDepth = 20;
        public const int BrightnessTransition = 130;

        public const string Designer = "Thomas Beckett";
        public const string License = "This program is provided as is, without warranty. The end user is solely responsible for any injuries or TPKs that may result from the use of this product.";
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

        private static readonly int[] proficiencyLevels = { 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6 };

        private static readonly int[] autosaveIntervals = { 1, 5, 10, 15, 20, 30, 45, 60, 90, 120 };

        static Constants()
        {
            AutosaveIntervals = new ReadOnlyCollection<int>(autosaveIntervals);

            Weapons = new ReadOnlyCollection<Weapon>(DefaultListReadWriter.ReadJson<Weapon>(Properties.Resources.Weapon));
            Ammunitions = new ReadOnlyCollection<Ammunition>(DefaultListReadWriter.ReadJson<Ammunition>(Properties.Resources.Ammunition));
            Spells = new ReadOnlyCollection<Spell>(DefaultListReadWriter.ReadJson<Spell>(Properties.Resources.Spell));
            Inventories = new ReadOnlyCollection<Inventory>(DefaultListReadWriter.ReadJson<Inventory>(Properties.Resources.Inventory));
            Languages = new ReadOnlyCollection<Language>(DefaultListReadWriter.ReadJson<Language>(Properties.Resources.Language));
            Abilities = new ReadOnlyCollection<Ability>(DefaultListReadWriter.ReadJson<Ability>(Properties.Resources.Ability));

            /*Alignment = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadStrings(Properties.Resources.Alignment));
            Backgrounds = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadStrings(Properties.Resources.Background));
            Races = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadStrings(Properties.Resources.Race));
            Levels = new ReadOnlyCollection<int>(levels);
            ProficiencyLevels = new ReadOnlyCollection<int>(proficiencyLevels);
            Classes = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadStrings(Properties.Resources.Class));
            StatusEffects = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadStrings(Properties.Resources.StatusEffect));
            Resources = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadStrings(Properties.Resources.ClassResource));
            Tools = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadStrings(Properties.Resources.Tool));
            Games = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadStrings(Properties.Resources.Game));
            Instruments = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadStrings(Properties.Resources.Intrument));
            ItemCategories = new ReadOnlyCollection<string>(DefaultListReadWriter.ReadStrings(Properties.Resources.Category));*/

            Alignment = new ReadOnlyCollection<string>(new List<string>());
            Backgrounds = new ReadOnlyCollection<string>(new List<string>());
            Races = new ReadOnlyCollection<string>(new List<string>());
            Levels = new ReadOnlyCollection<int>(levels);
            ProficiencyLevels = new ReadOnlyCollection<int>(proficiencyLevels);
            Classes = new ReadOnlyCollection<string>(new List<string>());
            StatusEffects = new ReadOnlyCollection<string>(new List<string>());
            Resources = new ReadOnlyCollection<string>(new List<string>());
            Tools = new ReadOnlyCollection<string>(new List<string>());
            Games = new ReadOnlyCollection<string>(new List<string>());
            Instruments = new ReadOnlyCollection<string>(new List<string>());
            ItemCategories = new ReadOnlyCollection<string>(new List<string>());
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

        public static ReadOnlyCollection<int> ProficiencyLevels { get; }

        public static ReadOnlyCollection<string> Classes { get; }

        public static ReadOnlyCollection<Ability> Abilities { get; }

        public static ReadOnlyCollection<string> StatusEffects { get; }

        public static ReadOnlyCollection<string> Resources { get; }

        public static ReadOnlyCollection<string> Tools { get; }

        public static ReadOnlyCollection<string> Games { get; }

        public static ReadOnlyCollection<string> Instruments { get; }

        public static ReadOnlyCollection<string> ItemCategories { get; }

        public static int CurrentAutosaveInterval => AutosaveIntervals[AppSettingsManager.UserSettings.AutosaveInterval];
    }
}
