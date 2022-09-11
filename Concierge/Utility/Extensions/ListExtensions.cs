// <copyright file="ListExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Extensions
{
    using System.Collections.Generic;

    public static class ListExtensions
    {
        public static void Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            (list[indexB], list[indexA]) = (list[indexA], list[indexB]);
        }

        public static bool IsEmpty<T>(this IList<T> list)
        {
            return list.Count == 0;
        }

        public static IList<T> DeepCopy<T>(this IList<T> list)
            where T : ICopyable<T>
        {
            var copy = new List<T>();

            foreach (var item in list)
            {
                copy.Add(item.DeepCopy());
            }

            return copy;
        }
    }
}
