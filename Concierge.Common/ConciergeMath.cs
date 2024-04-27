// <copyright file="ConciergeMath.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common
{
    using System;

    /// <summary>
    /// Provides constants and static methods for 5E D&amp;D calculations.
    /// </summary>
    public static class ConciergeMath
    {
        /// <summary>
        /// The number of significant digits used for rounding.
        /// </summary>
        public const int SignificantDigits = 2;

        /// <summary>
        /// Calculates the bonus value based on a given score.
        /// Subtract 10 from the ability score and then divides the total by 2 (rounded down).
        /// </summary>
        /// <param name="score">The score to calculate the bonus for.</param>
        /// <returns>The calculated bonus value.</returns>
        public static int Bonus(int score)
        {
            return (int)Math.Floor((score - 10) / 2.0);
        }

        /// <summary>
        /// Calculates the concentration threshold based on the damage received.
        /// Divides the damage by 2 (rounded down), and takes the greater of the result or 10.
        /// </summary>
        /// <param name="damage">The amount of damage received.</param>
        /// <returns>The concentration threshold.</returns>
        public static int Concentration(int damage)
        {
            return Math.Max((int)Math.Floor(damage / 2.0), 10);
        }

        /// <summary>
        /// Calculates the amount of an item that is recoverable.
        /// Divides the amount by 2 (rounded down).
        /// </summary>
        /// <param name="amount">The amount to calculate the recovery for.</param>
        /// <returns>The recovered amount.</returns>
        public static int Recover(int amount)
        {
            return (int)Math.Floor(amount / 2.0);
        }

        /// <summary>
        /// Calculates the amount of spent resources to regain.
        /// Divides the amount by 2 (rounded down) and takes the greater of the result or 1.
        /// </summary>
        /// <param name="spent">The amount to calculate the regain for.</param>
        /// <returns>The recovered amount.</returns>
        public static int Regain(int spent)
        {
            return Math.Max(spent -= Math.Max(spent / 2, 1), 0);
        }

        /// <summary>
        /// Determines whether a specified value is between a start (inclusive) and an end (exclusive) value.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="start">The start of the range (inclusive).</param>
        /// <param name="end">The end of the range (exclusive).</param>
        /// <returns>True if the value is between the start and end values; otherwise, false.</returns>
        public static bool Between(int value, int start, int end)
        {
            return value >= start && value < end;
        }
    }
}
