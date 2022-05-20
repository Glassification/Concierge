// <copyright file="DiceRoll.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools
{
    using System;
    using System.Collections.Generic;

    public class DiceRoll
    {
        private static readonly Random random = new ();

        public DiceRoll(string roll, int[] list, int total)
        {
            this.Roll = roll;
            this.DiceList = new List<int>(list);
            this.Total = total;
        }

        public string Roll { get; init; }

        public string Dice
        {
            get
            {
                var str = string.Empty;

                foreach (int die in this.DiceList)
                {
                    str += die + ", ";
                }

                return str.Trim(new char[] { ',', ' ' });
            }
        }

        public int Total { get; init; }

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
