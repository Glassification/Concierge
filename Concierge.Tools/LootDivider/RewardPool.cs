// <copyright file="RewardPool.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.LootDivider
{
    using System;

    using Concierge.Common;
    using Concierge.Tools.Enums;

    /// <summary>
    /// Represents an abstract class for managing a reward pool with various currencies.
    /// </summary>
    public abstract class RewardPool
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RewardPool"/> class.
        /// </summary>
        public RewardPool()
        {
            this.CurrencyList = new int[Constants.Currencies];
        }

        /// <summary>
        /// Gets an array representing the currency amounts for each denomination.
        /// </summary>
        public int[] CurrencyList { get; }

        /// <summary>
        /// Gets the total value of the reward pool in a combined currency format.
        /// </summary>
        public double Total => Math.Round((this.Copper / 100.0) + (this.Silver / 10.0) + (this.Electrum / 5.0) + this.Gold + (this.Platinum * 10.0), 2);

        /// <summary>
        /// Gets or sets the amount of copper coins in the reward pool.
        /// </summary>
        public int Copper
        {
            get => this.CurrencyList[(int)Currency.Copper];
            set => this.CurrencyList[(int)Currency.Copper] = value;
        }

        /// <summary>
        /// Gets or sets the amount of silver coins in the reward pool.
        /// </summary>
        public int Silver
        {
            get => this.CurrencyList[(int)Currency.Silver];
            set => this.CurrencyList[(int)Currency.Silver] = value;
        }

        /// <summary>
        /// Gets or sets the amount of electrum coins in the reward pool.
        /// </summary>
        public int Electrum
        {
            get => this.CurrencyList[(int)Currency.Electrum];
            set => this.CurrencyList[(int)Currency.Electrum] = value;
        }

        /// <summary>
        /// Gets or sets the amount of gold coins in the reward pool.
        /// </summary>
        public int Gold
        {
            get => this.CurrencyList[(int)Currency.Gold];
            set => this.CurrencyList[(int)Currency.Gold] = value;
        }

        /// <summary>
        /// Gets or sets the amount of platinum coins in the reward pool.
        /// </summary>
        public int Platinum
        {
            get => this.CurrencyList[(int)Currency.Platinum];
            set => this.CurrencyList[(int)Currency.Platinum] = value;
        }
    }
}
