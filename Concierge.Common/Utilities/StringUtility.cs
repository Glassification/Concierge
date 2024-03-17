// <copyright file="StringUtility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Utilities
{
    /// <summary>
    /// Provides utility methods for string-related operations.
    /// </summary>
    public static class StringUtility
    {
        /// <summary>
        /// Creates a string consisting of the specified character repeated a specified number of times.
        /// </summary>
        /// <param name="character">The character to be repeated.</param>
        /// <param name="count">The number of times the character should be repeated.</param>
        /// <returns>A string consisting of the repeated character.</returns>
        public static string CreateCharacters(string character, int count)
        {
            var characters = string.Empty;

            for (int i = 0; i < count; i++)
            {
                characters += character;
            }

            return characters;
        }
    }
}
