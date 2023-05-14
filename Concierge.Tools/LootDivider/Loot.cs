// <copyright file="Loot.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.LootDivider
{
    using Concierge.Tools.Enums;

    public sealed class Loot : RewardPool
    {
        public Loot(int cp, int sp, int ep, int gp, int pp)
            : base()
        {
            this.CurrencyList[(int)Currency.Copper] = cp;
            this.CurrencyList[(int)Currency.Silver] = sp;
            this.CurrencyList[(int)Currency.Electrum] = ep;
            this.CurrencyList[(int)Currency.Gold] = gp;
            this.CurrencyList[(int)Currency.Platinum] = pp;
        }
    }
}
