using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private static int[] AutosaveIntervals = { 1, 5, 10, 15, 20, 30, 45, 60, 90, 120 };

        public enum Abilities { NONE, STR, DEX, CON, INT, WIS, CHA };
        public enum ArmorType { None, Light, Medium, Heavy, Massive };
        public enum ArmorStealth { Normal, Disadvantage };
        public enum Checks { Normal, Disadvantage, Fail };
        public enum DamageTypes { None, Bludgeoning, Piercing, Slashing, Acid, Cold, Fire, Force, Lightning, Necrotic, Poison, Psychic, Radiant, Thunder };

        public static int CalculateBonus(int score)
        {
            return (int)Math.Floor((score - 10) / 2.0);
        }
    }
}
