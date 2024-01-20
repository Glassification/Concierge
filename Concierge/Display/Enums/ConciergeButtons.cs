// <copyright file="ConciergeButtons.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Enums
{
    using System;

    [Flags]
    public enum ConciergeButtons
    {
        None = 0,
        Ok = 1,
        Yes = 2,
        No = 4,
        Apply = 8,
        Cancel = 16,
    }
}