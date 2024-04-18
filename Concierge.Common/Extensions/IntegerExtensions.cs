// <copyright file="IntegerExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Extensions
{
    /// <summary>
    /// Provides extension methods for integer values.
    /// </summary>
    public static class IntegerExtensions
    {
        /// <summary>
        /// Gets the ordinal postfix for an integer value.
        /// </summary>
        /// <param name="value">The integer value.</param>
        /// <param name="includeNumber">Specifies whether to include the number in the result.</param>
        /// <returns>The ordinal postfix (e.g., "st", "nd", "rd", or "th") for the integer value.</returns>
        public static string GetPostfix(this int value, bool includeNumber)
        {
            var postfix = value switch
            {
                0 => string.Empty,
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th",
            };

            return $"{(includeNumber ? value : string.Empty)}{postfix}";
        }

        /// <summary>
        /// Converts an integer value representing a spell slot into its corresponding textual representation.
        /// </summary>
        /// <param name="value">The integer value representing the spell slot.</param>
        /// <returns>The textual representation of the spell slot, such as "first", "second", etc.</returns>
        public static string ToSpellSlot(this int value)
        {
            return value switch
            {
                1 => "first",
                2 => "second",
                3 => "third",
                4 => "fourth",
                5 => "fifth",
                6 => "sixth",
                7 => "seventh",
                8 => "eighth",
                9 => "ninth",
                _ => string.Empty,
            };
        }

        /// <summary>
        /// Rounds the given integer value to the nearest multiple of ten.
        /// </summary>
        /// <param name="value">The integer value to be rounded.</param>
        /// <returns>The nearest multiple of ten to the given value.</returns>
        public static int NearestMultipleOfTen(this int value)
        {
            var rem = value % 10;
            return rem >= 5 ? (value - rem + 10) : (value - rem);
        }
    }
}
