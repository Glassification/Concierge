// <copyright file="OSVersion.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Enums
{
    using System;

    [Flags]
    public enum OSVersion
    {
        Unknown = 0,
        Windows7 = 1,
        Windows8 = 2,
        Windows10 = 4,
        Windows11 = 8,
    }
}
