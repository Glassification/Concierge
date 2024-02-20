// <copyright file="Recovery.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Enums
{
    using System.ComponentModel;

    public enum Recovery
    {
        [Description("N/A")]
        None,
        [Description("long rest")]
        LongRest,
        [Description("short rest")]
        ShortRest,
        [Description("short or long rest")]
        ShortOrLongRest,
    }
}
