// <copyright file="ConciergeMath.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common
{
    using System;

    using Concierge.Common.Enums;

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
        public static int Bonus(int score) => (int)Math.Floor((score - 10) / 2.0);

        /// <summary>
        /// Calculates the concentration threshold based on the damage received.
        /// Divides the damage by 2 (rounded down), and takes the greater of the result or 10.
        /// </summary>
        /// <param name="damage">The amount of damage received.</param>
        /// <returns>The concentration threshold.</returns>
        public static int Concentration(int damage) => Math.Max((int)Math.Floor(damage / 2.0), 10);

        /// <summary>
        /// Calculates the amount of an item that is recoverable.
        /// Divides the amount by 2 (rounded down).
        /// </summary>
        /// <param name="amount">The amount to calculate the recovery for.</param>
        /// <returns>The recovered amount.</returns>
        public static int Recover(int amount) => (int)Math.Floor(amount / 2.0);

        /// <summary>
        /// Calculates the amount of spent resources to regain.
        /// Divides the amount by 2 (rounded down) and takes the greater of the result or 1.
        /// </summary>
        /// <param name="spent">The amount to calculate the regain for.</param>
        /// <returns>The recovered amount.</returns>
        public static int Regain(int spent) => Math.Max(spent -= Math.Max(spent / 2, 1), 0);

        /// <summary>
        /// Determines if a value falls within a specified range according to the specified inclusivity or exclusivity.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="start">The start of the range.</param>
        /// <param name="end">The end of the range.</param>
        /// <param name="inclusivity">The type of inclusivity/exclusivity to be applied.</param>
        /// <returns>
        /// A boolean indicating whether the value falls within the specified range according to the specified inclusivity/exclusivity.
        /// </returns>
        public static bool Between(int value, int start, int end, Inclusivity inclusivity)
        {
            return Between((double)value, (double)start, (double)end, inclusivity);
        }

        /// <summary>
        /// Determines if a value falls within a specified range according to the specified inclusivity or exclusivity.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <param name="start">The start of the range.</param>
        /// <param name="end">The end of the range.</param>
        /// <param name="inclusivity">The type of inclusivity/exclusivity to be applied.</param>
        /// <returns>
        /// A boolean indicating whether the value falls within the specified range according to the specified inclusivity/exclusivity.
        /// </returns>
        public static bool Between(double value, double start, double end, Inclusivity inclusivity)
        {
            return inclusivity switch
            {
                Inclusivity.Inclusive => value >= start && value <= end,
                Inclusivity.Exclusive => value > start && value < end,
                Inclusivity.LeftInclusive => value >= start && value < end,
                Inclusivity.RightInclusive => value > start && value <= end,
                _ => false,
            };
        }

        /// <summary>
        /// Rounds a double value to ensure that the last digit of the rounded value is a multiple of 5,
        /// with a precision of 2 decimal places.
        /// </summary>
        /// <param name="value">The value to be rounded.</param>
        /// <returns>The rounded value with the last digit being a multiple of 5.</returns>
        public static double RoundLastDigitTo5(double value)
        {
            var shifted = (int)Math.Round(Math.Round(value, SignificantDigits) * 100);
            var remainder = shifted % 10;

            shifted = remainder < 5 ? (shifted / 10 * 10) + 5 : ((shifted / 10) + 1) * 10;
            return Math.Round(shifted / 100.0, SignificantDigits);
        }
    }
}
