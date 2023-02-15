// <copyright file="Wealth.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Statuses
{
    using System;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Utility;
    using Concierge.Utility.Utilities;
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
                var itemValue = Program.CcsFile.Character.EquippedItems.Value;
                foreach (Inventory item in Program.CcsFile.Character.Inventories)
                {
                    itemValue += CharacterUtility.GetGoldValue(item.Value, item.CoinType) * item.Amount;
                }

                foreach (Weapon weapon in Program.CcsFile.Character.Weapons)
                {
                    itemValue += CharacterUtility.GetGoldValue(weapon.Value, weapon.CoinType);
                }

                foreach (Ammunition ammunition in Program.CcsFile.Character.Ammunitions)
                {
                    itemValue += CharacterUtility.GetGoldValue(ammunition.Value, ammunition.CoinType) * (ammunition.Quantity - ammunition.Used);
                }

                return itemValue;
            }
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
