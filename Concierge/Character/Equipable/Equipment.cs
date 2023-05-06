// <copyright file="Equipment.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Equipable
{
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Configuration;
    using Concierge.Data.Units;
    using Newtonsoft.Json;

    public class Equipment : ICopyable<Equipment>
    {
        public Equipment()
        {
            this.Ammunition = new List<Ammunition>();
            this.Armor = new Armor();
            this.Inventory = new List<Inventory>();
            this.Weapons = new List<Weapon>();
            this.EquippedItems = new EquippedItems(this.Inventory, this.Weapons);
        }

        public List<Ammunition> Ammunition { get; set; }

        public Armor Armor { get; set; }

        [JsonIgnore]
        public EquippedItems EquippedItems { get; set; }

        public List<Inventory> Inventory { get; set; }

        public List<Weapon> Weapons { get; set; }

        [JsonIgnore]
        public double CarryWeight
        {
            get
            {
                var weight = 0.0;

                foreach (var item in this.Inventory)
                {
                    if (!item.IgnoreWeight)
                    {
                        weight += item.Weight.Value * item.Amount;
                    }
                }

                foreach (var weapon in this.Weapons)
                {
                    if (!weapon.IgnoreWeight)
                    {
                        weight += weapon.Weight.Value;
                    }
                }

                weight += this.Armor.Weight.Value;
                weight += this.Armor.ShieldWeight.Value;

                if (AppSettingsManager.UserSettings.UseCoinWeight)
                {
                    weight += UnitConversion.Weight(AppSettingsManager.UserSettings.UnitOfMeasurement, Program.CcsFile.Character.Wealth.TotalCoins / Constants.CoinGroup);
                }

                return weight;
            }
        }

        public Equipment DeepCopy()
        {
            return new Equipment()
            {
                Ammunition = this.Ammunition.DeepCopy().ToList(),
                Armor = this.Armor.DeepCopy(),
                Inventory = this.Inventory.DeepCopy().ToList(),
                Weapons = this.Weapons.DeepCopy().ToList(),
            };
        }
    }
}
