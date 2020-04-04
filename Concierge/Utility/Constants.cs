namespace Concierge.Utility
{
    using Concierge.Characters.Collections;
    using Concierge.Persistence;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

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

        public enum Abilities { NONE, STR, DEX, CON, INT, WIS, CHA };
        public enum ArmorType { None, Light, Medium, Heavy, Massive };
        public enum ArmorStealth { Normal, Disadvantage };
        public enum Checks { Normal, Advantage, Disadvantage, Fail };
        public enum DamageTypes { None, Bludgeoning, Piercing, Slashing, Acid, Cold, Fire, Force, Lightning, Necrotic, Poison, Psychic, Radiant, Thunder };
        public enum WeaponTypes { Battleaxe, Blowgun, Club, Dagger, Dart, Flail, Glaive, Greataxe, Greatclub, Greatsword, Halberd, HandCrossbow, Handaxe, HeavyCrossbow, Javelin, Lance, LightCrossbow, LightHammer, Longbow, Longsword, Mace, Maul, Morningstar, Net, Pike, Quarterstaff, Rapier, Scimitar, Shortbow, Shortsword, Sickle, Sling, Spear, Trident, WarPick, Warhammer, Whip };
        public enum PopupButtons { OK, Apply, Cancel, AddWeapon, AddAmmo, WeaponProficiency, ArmorProficiency, ShieldProficiency, ToolProficiency, AddMagicClass, AddSpell, AddChapter, AddPage };
        public enum ConditionTypes { Cured, Afflicted };
        public enum ExhaustionType { Normal, One, Two, Three, Four, Five, Six };
        public enum ClassTypes { Barbarian, Bard, Cleric, Druid, Fighter, Monk, Paladin, Ranger, Rouge, Sorcerer, Warlock, Wizard};
        public enum ArcaneSchools { Abjuration, Conjuration, Divination, Enchantment, Evocation, Illusion, Necromancy, Transmutation, Universal };
        public enum VisionTypes { Normal, Blindsight, Darkvision, Truesight };

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
