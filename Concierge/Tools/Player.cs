// <copyright file="Player.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools
{
    using Concierge.Tools.Enums;

    public class Player
    {
        public const int Currencies = 5;

        public Player(string name)
        {
            this.Name = name;
            this.CurrencyList = new int[Currencies];
        }

        public Player(int cp, int sp, int ep, int gp, int pp)
        {
            this.CurrencyList = new int[Currencies];
            this.CurrencyList[(int)Currency.Copper] = cp;
            this.CurrencyList[(int)Currency.Silver] = sp;
            this.CurrencyList[(int)Currency.Electrum] = ep;
            this.CurrencyList[(int)Currency.Gold] = gp;
            this.CurrencyList[(int)Currency.Platinum] = pp;
        }

        public int[] CurrencyList { get; }

        public string Name { get; set; }

        public double Total => (this.Copper / 100.0) + (this.Silver / 10.0) + (this.Electrum / 5.0) + this.Gold + (this.Platinum * 10.0);

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
