using Concierge.Characters.Collections;
using Concierge.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Concierge.Utility
{
    public static class Constants
    {
        public const int MAX_LEVEL = 20;
        public const string NEW_FILE = "<NEW_FILE>";
        public const int BASE_DC = 8;
        public const int COIN_GROUP = 50;
        public const int MAX_SCORE = 30;
        public const int MIN_SCORE = 0;
        public const int BASE_PERCEPTION = 10;

        public enum Abilities { NONE, STR, DEX, CON, INT, WIS, CHA };
        public enum ArmorType { None, Light, Medium, Heavy, Massive };
        public enum ArmorStealth { Normal, Disadvantage };
        public enum Checks { Normal, Disadvantage, Fail };
        public enum DamageTypes { None, Bludgeoning, Piercing, Slashing, Acid, Cold, Fire, Force, Lightning, Necrotic, Poison, Psychic, Radiant, Thunder };
        public enum WeaponTypes { Battleaxe, Blowgun, Club, Dagger, Dart, Flail, Glaive, Greataxe, Greatclub, Greatsword, Halberd, HandCrossbow, Handaxe, HeavyCrossbow, Javelin, Lance, LightCrossbow, LightHammer, Longbow, Longsword, Mace, Maul, Morningstar, Net, Pike, Quarterstaff, Rapier, Scimitar, Shortbow, Shortsword, Sickle, Sling, Spear, Trident, WarPick, Warhammer, Whip };

        static Constants()
        {
            AutosaveIntervals = new ReadOnlyCollection<int>(new List<int> { 1, 5, 10, 15, 20, 30, 45, 60, 90, 120 });

            Weapons = new ReadOnlyCollection<Weapon>(DefaultListLoader.LoadWeaponList());
            Ammunitions = new ReadOnlyCollection<Ammunition>(DefaultListLoader.LoadAmmunitionList());
            Spells = new ReadOnlyCollection<Spell>(DefaultListLoader.LoadSpellList());
            Inventories = new ReadOnlyCollection<Inventory>(DefaultListLoader.LoadInventoryList());
        }

        public static int CalculateBonus(int score)
        {
            return (int)Math.Floor((score - 10) / 2.0);
        }

        public static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public static ReadOnlyCollection<int> AutosaveIntervals { get; private set; }
        public static ReadOnlyCollection<Weapon> Weapons { get; private set; }
        public static ReadOnlyCollection<Ammunition> Ammunitions { get; private set; }
        public static ReadOnlyCollection<Spell> Spells { get; private set; }
        public static ReadOnlyCollection<Inventory> Inventories { get; private set; }
    }
}
