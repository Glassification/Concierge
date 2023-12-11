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
    }
}
