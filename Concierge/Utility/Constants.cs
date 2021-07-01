// <copyright file="Constants.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Concierge.Characters.Collections;
    using Concierge.Persistence;

    public static class Constants
    {
        public const int MAX_LEVEL = 20;
        public const string NEW_FILE = "<NEW_FILE>";
        public const int BASE_DC = 8;
        public const int COIN_GROUP = 50;
        public const int MAX_SCORE = 30;
        public const int MIN_SCORE = 0;
        public const int BASE_PERCEPTION = 10;
        public const int MAX_CLASSES = 3;

        static Constants()
        {
            AutosaveIntervals = new ReadOnlyCollection<int>(new List<int> { 1, 5, 10, 15, 20, 30, 45, 60, 90, 120 });

            Weapons = new ReadOnlyCollection<Weapon>(DefaultListLoader.LoadWeaponList());
            Ammunitions = new ReadOnlyCollection<Ammunition>(DefaultListLoader.LoadAmmunitionList());
            Spells = new ReadOnlyCollection<Spell>(DefaultListLoader.LoadSpellList());
            Inventories = new ReadOnlyCollection<Inventory>(DefaultListLoader.LoadInventoryList());
            Languages = new ReadOnlyCollection<Language>(DefaultListLoader.LoadLanguageList());
        }

        public static ReadOnlyCollection<int> AutosaveIntervals { get; private set; }

        public static ReadOnlyCollection<Weapon> Weapons { get; private set; }

        public static ReadOnlyCollection<Ammunition> Ammunitions { get; private set; }

        public static ReadOnlyCollection<Spell> Spells { get; private set; }

        public static ReadOnlyCollection<Inventory> Inventories { get; private set; }

        public static ReadOnlyCollection<Language> Languages { get; private set; }
    }
}
