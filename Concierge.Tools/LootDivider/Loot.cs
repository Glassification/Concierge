// <copyright file="Loot.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.LootDivider
{
    using Concierge.Tools.Enums;

    /// <summary>
    /// Represents a sealed class for managing loot with various currencies.
    /// </summary>
    public sealed class Loot : RewardPool
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Loot"/> class with specific currency amounts.
        /// </summary>
        /// <param name="cp">The amount of copper coins.</param>
        /// <param name="sp">The amount of silver coins.</param>
        /// <param name="ep">The amount of electrum coins.</param>
        /// <param name="gp">The amount of gold coins.</param>
        /// <param name="pp">The amount of platinum coins.</param>
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
