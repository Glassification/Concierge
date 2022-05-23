// <copyright file="DiceRoll.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.RollDice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DiceRoll : IDiceRoll
    {
        private static readonly Random random = new ();

        public DiceRoll(int sides, int[] list, string modifier = "")
        {
            this.Sides = sides;
            this.Modifier = modifier;
            this.DiceList = new List<int>(list);
        }

        public int Sides { get; init; }

        public string Modifier { get; init; }

        public string Dice
        {
            get
            {
                var str = $"{this.DiceList.Count}d{this.Sides}(";

                foreach (int die in this.DiceList)
                {
                    str += die + ", ";
                }

                str = str.Trim(new char[] { ',', ' ' });
                str = $"{str}){this.Modifier}";

                return str;
            }
        }

        public int Total => Math.Max(this.DiceList.Sum(), 0);

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
    }
}
