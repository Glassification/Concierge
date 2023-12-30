// <copyright file="StringUtility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Common.Extensions;

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

        /// <summary>
        /// Formats the names of enum values for display by converting underscores to spaces and adjusting letter casing.
        /// </summary>
        /// <param name="enumType">The type of the enum.</param>
        /// <returns>A list of formatted enum names.</returns>
        public static List<string> FormatEnumForDisplay(Type enumType)
        {
            var stringArray = Enum.GetNames(enumType);

            for (int i = 0; i < stringArray.Length; i++)
            {
                stringArray[i] = stringArray[i].FormatFromPascalCase();
            }

            return [.. stringArray];
        }

        /// <summary>
        /// Formats a name by inserting spaces before each uppercase letter.
        /// </summary>
        /// <param name="name">The name to be formatted.</param>
        /// <returns>The formatted name with spaces inserted.</returns>
        public static string FormatName(string name)
        {
            var ch = name.ToArray();
            int offset = 0;

            for (int i = 1; i < ch.Length; i++)
            {
                if (char.IsUpper(ch[i]))
                {
                    name = name.Insert(i + offset, " ");
                    offset++;
                }
            }

            return name;
        }
    }
}
