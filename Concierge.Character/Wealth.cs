// <copyright file="Wealth.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a collection of different types of coins to measure wealth.
    /// </summary>
    public sealed class Wealth : ICopyable<Wealth>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Wealth"/> class with all coin values set to zero.
        /// </summary>
        public Wealth()
        {
            this.Copper = 0;
            this.Silver = 0;
            this.Electrum = 0;
            this.Gold = 0;
            this.Platinum = 0;
        }

        /// <summary>
        /// Gets or sets the amount of copper coins.
        /// </summary>
        public int Copper { get; set; }

        /// <summary>
        /// Gets or sets the amount of silver coins.
        /// </summary>
        public int Silver { get; set; }

        /// <summary>
        /// Gets or sets the amount of electrum coins.
        /// </summary>
        public int Electrum { get; set; }

        /// <summary>
        /// Gets or sets the amount of gold coins.
        /// </summary>
        public int Gold { get; set; }

        /// <summary>
        /// Gets or sets the amount of platinum coins.
        /// </summary>
        public int Platinum { get; set; }

        /// <summary>
        /// Gets the total value of all coins, converted to gold.
        /// </summary>
        [JsonIgnore]
        public double TotalValue => (this.Copper / 100.0) + (this.Silver / 10.0) + (this.Electrum / 2.0) + this.Gold + (this.Platinum * 10.0);

        /// <summary>
        /// Gets the total number of coins.
        /// </summary>
        [JsonIgnore]
        public int TotalCoins => this.Copper + this.Silver + this.Electrum + this.Gold + this.Platinum;

        /// <summary>
        /// Gets the total weight of coins.
        /// </summary>
        [JsonIgnore]
        public double TotalWeight => this.TotalCoins / Constants.CoinGroup;

        /// <summary>
        /// Converts a value from a specific coin type to gold.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="coinType">The type of the coin.</param>
        /// <returns>The value converted to gold.</returns>
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

        /// <summary>
        /// Formats a gold value for display.
        /// </summary>
        /// <param name="value">The gold value to format.</param>
        /// <returns>A formatted string representing the gold value.</returns>
        public static string FormatGoldValue(double value)
        {
            return $"¤ {string.Format("{0:0.00}", value)}";
        }

        /// <summary>
        /// Gets the coin type based on its name.
        /// </summary>
        /// <param name="name">The name of the coin type.</param>
        /// <returns>The corresponding coin type.</returns>
        public static CoinType GetCoinType(string name)
        {
            if (name.ContainsIgnoreCase(CoinType.Copper.ToString()))
            {
                return CoinType.Copper;
            }

            if (name.ContainsIgnoreCase(CoinType.Silver.ToString()))
            {
                return CoinType.Silver;
            }

            if (name.ContainsIgnoreCase(CoinType.Electrum.ToString()))
            {
                return CoinType.Electrum;
            }

            if (name.ContainsIgnoreCase(CoinType.Platinum.ToString()))
            {
                return CoinType.Platinum;
            }

            return CoinType.Gold;
        }

        /// <summary>
        /// Creates a deep copy of this Wealth object.
        /// </summary>
        /// <returns>A new <see cref="Wealth"/> object with the same property values.</returns>
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
