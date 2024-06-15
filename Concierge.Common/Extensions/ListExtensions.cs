// <copyright file="ListExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Extensions
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides extension methods for working with lists.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Checks if the list is empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns><c>true</c> if the list is empty; otherwise, <c>false</c>.</returns>
        public static bool IsEmpty<T>(this IList<T> list) => list.Count == 0;

        /// <summary>
        /// Returns the count of elements in the list as a string.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list whose elements are to be counted. It can be null.</param>
        /// <returns>
        /// A string representing the count of elements in the list.
        /// If the list is null, returns "0".
        /// </returns>
        public static string StrCount<T>(this IList<T>? list) => list?.Count.ToString() ?? "0";

        /// <summary>
        /// Creates a deep copy of the list by deep copying each element in the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>A deep copy of the list.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when an element in the list does not implement the ICopyable{T} interface.</exception>
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

        /// <summary>
        /// Filters the list based on the provided filter text.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="filterText">The filter text.</param>
        /// <returns>A new list containing elements that match the filter text.</returns>
        public static IList<T> Filter<T>(this IList<T> list, string filterText)
        {
            if (filterText.IsNullOrWhiteSpace())
            {
                return list;
            }

            var filteredList = new List<T>();
            foreach (var item in list)
            {
                if (item?.SearchObject(filterText) ?? false)
                {
                    filteredList.Add(item);
                }
            }

            return filteredList;
        }

        /// <summary>
        /// Swaps the positions of two elements in the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="indexA">The index of the first element.</param>
        /// <param name="indexB">The index of the second element.</param>
        public static void Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            (list[indexB], list[indexA]) = (list[indexA], list[indexB]);
        }
    }
}
