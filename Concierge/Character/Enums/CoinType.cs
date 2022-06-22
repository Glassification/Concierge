// <copyright file="CoinType.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Enums
{
    using System.ComponentModel;

    public enum CoinType
    {
        [Description("")]
        None,
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
