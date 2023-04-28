// <copyright file="DiceRoll.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.DiceRoller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Common.Enums;

    public sealed class DiceRoll : IDiceRoll
    {
        private static readonly Random random = new ();

        public DiceRoll(int sides, int[] list, int modifier)
        {
            this.Sides = sides;
            this.Modifier = modifier;
            this.DiceList = new List<int>(list);
        }

        public int Sides { get; init; }

        public int Modifier { get; init; }

        public string Dice
        {
            get
            {
                var str = $"{this.DiceList.Count}d{this.Sides}(";

                foreach (int die in this.DiceList)
                {
                    str += die + ", ";
                }

                str = $"{str.Trim(new char[] { ',', ' ' })})";
                if (this.Modifier != 0)
                {
                    str = $"{str}{(this.Modifier > 0 ? " + " : " - ")}{this.Modifier}";
                }

                return str;
            }
        }

        public int Total => Math.Max(this.DiceList.Sum() + this.Modifier, 0);

        private List<int> DiceList { get; init; }

        public static int[] RollDice(int diceNumber, int diceSides)
        {
            var rolledDice = new int[diceNumber];
            for (int i = 0; i < diceNumber; i++)
            {
                var val = random.Next(1, diceSides + 1);
                rolledDice[i] = val;
            }

            return rolledDice;
        }

        public static int RollHitDie(HitDie hitDie)
        {
            var val = 1;
            while (val == 1)
            {
                val = random.Next(1, (int)hitDie + 1);
            }

            return val;
        }

        public override string ToString()
        {
            return $"[{this.Dice}] Total {this.Total}";
        }
    }
}
