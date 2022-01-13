// <copyright file="StringUtility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Utility.Extensions;

    public static class StringUtility
    {
        public static string CreateCharacters(string character, int count)
        {
            var characters = string.Empty;

            for (int i = 0; i < count; i++)
            {
                characters += character;
            }

            return characters;
        }

        public static List<string> FormatEnumForDisplay(Type enumType)
        {
            var stringArray = Enum.GetNames(enumType);

            for (int i = 0; i < stringArray.Length; i++)
            {
                stringArray[i] = stringArray[i].FormatFromEnum();
            }

            return stringArray.ToList();
        }

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
