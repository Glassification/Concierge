// <copyright file="Wealth.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Statuses
{
    using Concierge.Character.Items;
    using Concierge.Utility;
    using Concierge.Utility.Utilities;
    using Newtonsoft.Json;

    public class Wealth : ICopyable<Wealth>
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
        public double ItemValue
        {
            get
            {
                var itemValue = Program.CcsFile.Character.EquippedItems.Value;
                foreach (Inventory item in Program.CcsFile.Character.Inventories)
                {
                    itemValue += CharacterUtility.GetGoldValue(item.Value, item.CoinType);
                }

                return itemValue;
            }
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
