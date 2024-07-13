// <copyright file="Inclusivity.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Enums
{
    /// <summary>
    /// Specifies the type of inclusivity or exclusivity for a range.
    /// </summary>
    public enum Inclusivity
    {
        /// <summary>
        /// Indicates that the range is exclusive of both the start and end values (i.e., start &lt; value &lt; end).
        /// </summary>
        Exclusive,

        /// <summary>
        /// Indicates that the range is inclusive of both the start and end values (i.e., start &lt;= value &lt;= end).
        /// </summary>
        Inclusive,

        /// <summary>
        /// Indicates that the range is inclusive of the start value and exclusive of the end value (i.e., start &lt;= value &lt; end).
        /// </summary>
        LeftInclusive,

        /// <summary>
        /// Indicates that the range is exclusive of the start value and inclusive of the end value (i.e., start &lt; value &lt;= end).
        /// </summary>
        RightInclusive,
    }
}
