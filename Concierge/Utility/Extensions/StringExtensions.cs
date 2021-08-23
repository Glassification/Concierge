// <copyright file="StringExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Extensions
{
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNumeric(this string str)
        {
            foreach (char c in str)
            {
                if (!char.IsDigit(c) && c != ',' && c != ';' && c != 'e' && c != 'E' && c != '+' && c != '"' && c != '.')
                {
                    return false;
                }
            }

            return true;
        }

        public static int CountCharacter(this string str, char character)
        {
            var count = 0;
            var regex = new Regex("\".*?\"");

            str = regex.Replace(str, m => m.Value.Replace(',', '@'));
            var array = str.ToCharArray();

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == character)
                {
                    count++;
                }
            }

            return count;
        }

        public static string ReplaceMultiple(this string str, string newCharacter, params string[] oldCharacters)
        {
            var cleanedString = str;

            foreach (var character in oldCharacters)
            {
                cleanedString = cleanedString.Replace(character, newCharacter);
            }

            return cleanedString;
        }
    }
}
