// <copyright file="Wealth.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using System;

    using Concierge;

    using Concierge.Character.Enums;
    using Concierge.Character.Equipable;
    using Concierge.Common;
    using Newtonsoft.Json;

    public sealed class Wealth : ICopyable<Wealth>
    {
        public Wealth()
        {
            this.Copper = 0;
            this.Silver = 0;
            this.Electrum = 0;
            this.Gold = 0;
            this.Platinum = 0;
        }

        public int Copper { get; set; }

        public int Silver { get; set; }

        public int Electrum { get; set; }

        public int Gold { get; set; }

        public int Platinum { get; set; }

        [JsonIgnore]
        public double TotalValue => (this.Copper / 100.0) + (this.Silver / 10.0) + (this.Electrum / 2.0) + this.Gold + (this.Platinum * 10.0);

        [JsonIgnore]
        public int TotalCoins => this.Copper + this.Silver + this.Electrum + this.Gold + this.Platinum;

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Don't want this static.")]
        public double ItemValue
        {
            get
            {
                var itemValue = 0.0;
                foreach (var item in Program.CcsFile.Character.Equipment.Inventory)
                {
                    itemValue += GetGoldValue(item.Value, item.CoinType) * item.Amount;
                }

                foreach (var weapon in Program.CcsFile.Character.Equipment.Weapons)
                {
                    itemValue += GetGoldValue(weapon.Value, weapon.CoinType);
                }

                foreach (var ammunition in Program.CcsFile.Character.Equipment.Ammunition)
                {
                    itemValue += GetGoldValue(ammunition.Value, ammunition.CoinType) * (ammunition.Quantity - ammunition.Used);
                }

                return itemValue;
            }
        }

        public static double GetGoldValue(double value, CoinType coinType)
        {
            return coinType switch
            {
                CoinType.Copper => value / 100.0,
                CoinType.Silver => value / 10.0,
                CoinType.Electrum => value / 2.0,
                CoinType.Gold => value,
                CoinType.Platinum => value * 10.0,
                _ => value,
            };
        }

        public static string FormatGoldValue(double value)
        {
            return $"¤ {string.Format("{0:0.00}", value)}";
        }

        public static CoinType GetCoinType(string name)
        {
            if (name.Contains(CoinType.Copper.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return CoinType.Copper;
            }

            if (name.Contains(CoinType.Silver.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return CoinType.Silver;
            }

            if (name.Contains(CoinType.Electrum.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return CoinType.Electrum;
            }

            if (name.Contains(CoinType.Platinum.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return CoinType.Platinum;
            }

            return CoinType.Gold;
        }

        public Wealth DeepCopy()
        {
            return new Wealth()
            {
                Copper = this.Copper,
                Silver = this.Silver,
                Electrum = this.Electrum,
                Gold = this.Gold,
                Platinum = this.Platinum,
            };
        }
    }
}
