// <copyright file="DiceRoll.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.DiceRoller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Common.Enums;

    /// <summary>
    /// Represents the result of rolling dice in a role-playing game.
    /// </summary>
    public sealed class DiceRoll : IDiceRoll
    {
        private static readonly Random random = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="DiceRoll"/> class with the specified parameters.
        /// </summary>
        /// <param name="sides">The number of sides on each die.</param>
        /// <param name="number">The number of dice to roll.</param>
        /// <param name="modifier">An additional modifier to apply to the roll.</param>
        public DiceRoll(Dice sides, int number, int modifier)
        {
            this.Sides = (int)sides;
            this.Modifier = modifier;
            this.Number = number;
            this.DiceList = RollDice(number, (int)sides).ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiceRoll"/> class with the provided dice list.
        /// </summary>
        /// <param name="sides">The number of sides on each die.</param>
        /// <param name="list">The list of individual die roll results.</param>
        /// <param name="modifier">An additional modifier to apply to the roll.</param>
        public DiceRoll(int sides, int[] list, int modifier)
        {
            this.Sides = sides;
            this.Modifier = modifier;
            this.Number = list.Length;
            this.DiceList = new List<int>(list);
        }

        public static DiceRoll Empty => new (Common.Enums.Dice.None, 0, 0);

        /// <summary>
        /// Gets the number of sides on each die.
        /// </summary>
        public int Sides { get; init; }

        /// <summary>
        /// Gets the additional modifier applied to the roll.
        /// </summary>
        public int Modifier { get; init; }

        /// <summary>
        /// Gets the textual representation of the rolled dice.
        /// </summary>
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
                    str = $"{str}{(this.Modifier > 0 ? " + " : " - ")}{Math.Abs(this.Modifier)}";
                }

                return str;
            }
        }

        /// <summary>
        /// Gets the total sum of the rolled dice results along with the modifier.
        /// </summary>
        public int Total => Math.Max(this.DiceList.Sum() + this.Modifier, 0);

        private List<int> DiceList { get; set; }

        private int Number { get; init; }

        /// <summary>
        /// Rolls a set of dice with the specified number of sides and dice count.
        /// </summary>
        /// <param name="diceNumber">The number of dice to roll.</param>
        /// <param name="diceSides">The number of sides on each die.</param>
        /// <returns>An array of rolled die values.</returns>
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

        /// <summary>
        /// Rolls a hit die corresponding to a character's hit points calculation.
        /// </summary>
        /// <param name="hitDie">The type of hit die.</param>
        /// <returns>The rolled hit die value.</returns>
        public static int RollHitDie(Dice hitDie)
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

        /// <summary>
        /// Re-rolls the dice and computes the total for the dice roll.
        /// </summary>
        public void ReRoll()
        {
            this.DiceList = RollDice(this.Number, this.Sides).ToList();
        }
    }
}
