// <copyright file="HitDie.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Enums
{
    /// <summary>
    /// Represents the different types of hit dice used in D&D.
    /// </summary>
    public enum HitDie
    {
        /// <summary>
        /// No hit die.
        /// </summary>
        None = 0,

        /// <summary>
        /// Six-sided hit die (d6).
        /// </summary>
        D6 = 6,

        /// <summary>
        /// Eight-sided hit die (d8).
        /// </summary>
        D8 = 8,

        /// <summary>
        /// Ten-sided hit die (d10).
        /// </summary>
        D10 = 10,

        /// <summary>
        /// Twelve-sided hit die (d12).
        /// </summary>
        D12 = 12,
    }
}
