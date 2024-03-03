// <copyright file="ProficiencyLevel.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides methods to retrieve the proficiency bonus based on character level.
    /// </summary>
    public static class ProficiencyLevel
    {
        /// <summary>
        /// The starting proficiency bonus.
        /// </summary>
        public const int BaseProficiency = 2;

        private static readonly Dictionary<int, int> levels = new ()
        {
            { 1, 2 },
            { 2, 2 },
            { 3, 2 },
            { 4, 2 },
            { 5, 3 },
            { 6, 3 },
            { 7, 3 },
            { 8, 3 },
            { 9, 4 },
            { 10, 4 },
            { 11, 4 },
            { 12, 4 },
            { 13, 5 },
            { 14, 5 },
            { 15, 5 },
            { 16, 5 },
            { 17, 6 },
            { 18, 6 },
            { 19, 6 },
            { 20, 6 },
        };

        /// <summary>
        /// Retrieves the proficiency bonus based on the character level.
        /// </summary>
        /// <param name="level">The character's level.</param>
        /// <returns>The proficiency bonus corresponding to the character's level.</returns>
        public static int Get(int level)
        {
            if (levels.TryGetValue(level, out int value))
            {
                return value;
            }

            return BaseProficiency;
        }
    }
}
