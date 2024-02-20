// <copyright file="Equipment.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Equipable
{
    using System.Collections.Generic;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Newtonsoft.Json;

    public sealed class Equipment : ICopyable<Equipment>
    {
        public Equipment()
        {
            this.Ammunition = [];
            this.Defense = new Defense();
            this.Inventory = [];
            this.Weapons = [];
            this.EquippedItems = new EquippedItems(this.Inventory, this.Weapons);
        }

        public List<Ammunition> Ammunition { get; set; }

        public Defense Defense { get; set; }

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

        [JsonIgnore]
        public EquippedItems EquippedItems { get; set; }

        public List<Inventory> Inventory { get; set; }

        public List<Weapon> Weapons { get; set; }

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
