// <copyright file="Equipment.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Equipable
{
    using System.Collections.Generic;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a collection of equipment including ammunition, defense, inventory, and weapons.
    /// </summary>
    public sealed class Equipment : ICopyable<Equipment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Equipment"/> class.
        /// </summary>
        public Equipment()
        {
            this.Ammunition = [];
            this.Defense = new Defense();
            this.Inventory = [];
            this.Weapons = [];
            this.EquippedItems = new EquippedItems(this.Inventory, this.Weapons);
        }

        /// <summary>
        /// Gets or sets the list of ammunition.
        /// </summary>
        public List<Ammunition> Ammunition { get; set; }

        /// <summary>
        /// Gets or sets the defense.
        /// </summary>
        public Defense Defense { get; set; }

        /// <summary>
        /// Gets the total value of the equipment.
        /// </summary>
        [JsonIgnore]
        public double EquipmentValue
        {
            get
            {
                var itemValue = 0.0;
                foreach (var item in this.Inventory)
                {
                    itemValue += Wealth.GetGoldValue(item.Value, item.CoinType) * item.Amount;
                }

                foreach (var weapon in this.Weapons)
                {
                    itemValue += Wealth.GetGoldValue(weapon.Value, weapon.CoinType);
                }

                foreach (var ammunition in this.Ammunition)
                {
                    itemValue += Wealth.GetGoldValue(ammunition.Value, ammunition.CoinType) * (ammunition.Quantity - ammunition.Used);
                }

                return itemValue;
            }
        }

        /// <summary>
        /// Gets or sets the equipped items.
        /// </summary>
        [JsonIgnore]
        public EquippedItems EquippedItems { get; set; }

        /// <summary>
        /// Gets or sets the list of inventory items.
        /// </summary>
        public List<Inventory> Inventory { get; set; }

        /// <summary>
        /// Gets or sets the list of weapons.
        /// </summary>
        public List<Weapon> Weapons { get; set; }

        /// <summary>
        /// Creates a deep copy of the equipment.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Equipment"/>.</returns>
        public Equipment DeepCopy()
        {
            return new Equipment()
            {
                Ammunition = [.. this.Ammunition.DeepCopy()],
                Defense = this.Defense.DeepCopy(),
                Inventory = [.. this.Inventory.DeepCopy()],
                Weapons = [.. this.Weapons.DeepCopy()],
            };
        }
    }
}
