﻿// <copyright file="DiceRoll.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.DiceRoller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Common.Enums;

    /// <summary>
    /// Represents the result of rolling dice in D&amp;D 5E.
    /// </summary>
    public sealed class DiceRoll : IDiceRoll
    {
        private const int RollLimit = 20;

        private static readonly Random random = new ();
        private readonly int number;

        private List<int> diceList;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiceRoll"/> class with the specified parameters.
        /// </summary>
        /// <param name="sides">The number of sides on each die.</param>
        /// <param name="number">The number of dice to roll.</param>
        /// <param name="modifier">An additional modifier to apply to the roll.</param>
        public DiceRoll(Dice sides, int number = 1, int modifier = 0)
        {
            this.Sides = (int)sides;
            this.Modifier = modifier;
            this.number = number;
            this.diceList = [.. RollDice(number, (int)sides)];
            this.Max = ((int)sides * number) + modifier;
            this.Min = number + modifier;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiceRoll"/> class with the provided dice list.
        /// </summary>
        /// <param name="sides">The number of sides on each die.</param>
        /// <param name="list">The list of individual die roll results.</param>
        /// <param name="modifier">An additional modifier to apply to the roll.</param>
        public DiceRoll(int sides, int[] list, int modifier = 0)
        {
            this.Sides = sides;
            this.Modifier = modifier;
            this.number = list.Length;
            this.diceList = new List<int>(list);
            this.Max = (sides * list.Length) + modifier;
            this.Min = list.Length + modifier;
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
                var str = $"{this.diceList.Count}d{this.Sides}(";
                foreach (int die in this.diceList)
                {
                    str += die + ", ";
                }

                str = $"{str.Trim([',', ' '])})";
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
        public int Total => Math.Max(this.diceList.Sum() + this.Modifier, 0);

        /// <summary>
        /// Gets the maximum possible value of the rolled dice.
        /// </summary>
        public int Max { get; private set; }

        /// <summary>
        /// Gets the minimum possible value of the rolled dice.
        /// </summary>
        public int Min { get; private set; }

        /// <summary>
        /// Rolls a set of dice with the specified number of sides and dice count.
        /// </summary>
        /// <param name="diceNumber">The number of dice to roll.</param>
        /// <param name="diceSides">The number of sides on each die.</param>
        /// <returns>An array of rolled die values.</returns>
        public static int[] RollDice(int diceNumber, Dice diceSides)
        {
            return RollDice(diceNumber, (int)diceSides);
        }

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
        /// This method cannot roll a 1.
        /// </summary>
        /// <param name="hitDie">The type of hit die.</param>
        /// <returns>The rolled hit die value.</returns>
        public static int RollHitDie(Dice hitDie)
        {
            var val = 1;
            for (int i = 0; i < RollLimit && val == 1; i++)
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
            this.diceList = [.. RollDice(this.number, this.Sides)];
        }
    }
}
