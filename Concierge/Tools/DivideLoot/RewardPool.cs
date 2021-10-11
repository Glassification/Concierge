// <copyright file="RewardPool.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.DivideLoot
{
    using System;

    using Concierge.Tools.Enums;
    using Concierge.Utility;

    public abstract class RewardPool
    {
        public RewardPool()
        {
            this.CurrencyList = new int[Constants.Currencies];
        }

        public int[] CurrencyList { get; }

        public double Total => Math.Round((this.Copper / 100.0) + (this.Silver / 10.0) + (this.Electrum / 5.0) + this.Gold + (this.Platinum * 10.0), 2);

        public int Copper
        {
            get => this.CurrencyList[(int)Currency.Copper];
            set => this.CurrencyList[(int)Currency.Copper] = value;
        }

        public int Silver
        {
            get => this.CurrencyList[(int)Currency.Silver];
            set => this.CurrencyList[(int)Currency.Silver] = value;
        }

        public int Electrum
        {
            get => this.CurrencyList[(int)Currency.Electrum];
            set => this.CurrencyList[(int)Currency.Electrum] = value;
        }

        public int Gold
        {
            get => this.CurrencyList[(int)Currency.Gold];
            set => this.CurrencyList[(int)Currency.Gold] = value;
        }

        public int Platinum
        {
            get => this.CurrencyList[(int)Currency.Platinum];
            set => this.CurrencyList[(int)Currency.Platinum] = value;
        }
    }
}
