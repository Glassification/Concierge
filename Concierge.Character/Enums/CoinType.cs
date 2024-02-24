// <copyright file="CoinType.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Enum representing different types of coins.
    /// </summary>
    public enum CoinType
    {
        [Description("cp")]
        Copper,
        [Description("sp")]
        Silver,
        [Description("ep")]
        Electrum,
        [Description("gp")]
        Gold,
        [Description("pp")]
        Platinum,
    }
}
