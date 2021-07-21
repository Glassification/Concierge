// <copyright file="Utilities.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;

    using Concierge.Characters;

    public static class Utilities
    {
        public static T GetPropertyValue<T>(object source, string propertyName)
        {
            return (T)source.GetType().GetProperty(propertyName).GetValue(source, null);
        }

        public static int CalculateBonus(int score)
        {
            return (int)Math.Floor((score - 10) / 2.0);
        }

        public static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
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

        public static bool ValidateClassLevel(Character character, Guid id, int newValue)
        {
            var totalLevel =
                (character.Class1.Id.Equals(id) ? 0 : character.Class1.Level) +
                (character.Class2.Id.Equals(id) ? 0 : character.Class2.Level) +
                (character.Class3.Id.Equals(id) ? 0 : character.Class3.Level);

            totalLevel += newValue;

            return totalLevel <= Constants.MaxLevel && totalLevel >= 0;
        }

        public static Brush SetUsedTextStyle(int total, int used)
        {
            return total <= used ? Brushes.DarkRed : Brushes.White;
        }

        public static Brush SetUsedBoxStyle(int total, int used)
        {
            return total <= used ? Brushes.IndianRed : Colours.UsedBoxBrush;
        }

        public static Brush SetTotalTextStyle(int total, int used)
        {
            return total <= used ? Brushes.DarkGray : Brushes.White;
        }

        public static Brush SetTotalBoxStyle(int total, int used)
        {
            return total <= used ? Colours.TotalDarkBoxBrush : Colours.TotalLightBoxBrush;
        }
    }
}
