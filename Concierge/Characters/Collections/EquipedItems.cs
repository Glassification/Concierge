// <copyright file="EquipedItems.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Collections
{
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Characters.Enums;
    using Newtonsoft.Json;

    public class EquipedItems
    {
        public EquipedItems()
        {
            this.Head = new List<Inventory>();
            this.Torso = new List<Inventory>();
            this.Hands = new List<Inventory>();
            this.Legs = new List<Inventory>();
            this.Feet = new List<Inventory>();
        }

        public List<Inventory> Head { get; }

        public List<Inventory> Torso { get; }

        public List<Inventory> Hands { get; }

        public List<Inventory> Legs { get; }

        public List<Inventory> Feet { get; }

        [JsonIgnore]
        public List<Inventory> Equipable
        {
            get
            {
                var list = new List<Inventory>();
                foreach (var item in Program.CcsFile.Character.Inventories)
                {
                    if (this.IsEquiped(item))
                    {
                        list.Add(item);
                    }
                }

                return list;
            }
        }

        public void Equip(Inventory item, EquipSlot equipSlot)
        {
            switch (equipSlot)
            {
                case EquipSlot.Head:
                    this.Head.Add(item);
                    break;
                case EquipSlot.Torso:
                    this.Torso.Add(item);
                    break;
                case EquipSlot.Hands:
                    this.Hands.Add(item);
                    break;
                case EquipSlot.Legs:
                    this.Legs.Add(item);
                    break;
                case EquipSlot.Feet:
                    this.Feet.Add(item);
                    break;
            }
        }

        public void Dequip(Inventory item, EquipSlot equipSlot)
        {
            switch (equipSlot)
            {
                case EquipSlot.Head:
                    this.Head.Remove(item);
                    break;
                case EquipSlot.Torso:
                    this.Torso.Remove(item);
                    break;
                case EquipSlot.Hands:
                    this.Hands.Remove(item);
                    break;
                case EquipSlot.Legs:
                    this.Legs.Remove(item);
                    break;
                case EquipSlot.Feet:
                    this.Feet.Remove(item);
                    break;
            }
        }

        public bool IsEquiped(Inventory item)
        {
            var count = IsInList(this.Head, item) +
                IsInList(this.Torso, item) +
                IsInList(this.Hands, item) +
                IsInList(this.Legs, item) +
                IsInList(this.Feet, item);

            return count >= item.Amount;
        }

        private static int IsInList(List<Inventory> list, Inventory item)
        {
            return list.Where(x => x.ID.Equals(item.ID)).ToList().Count;
        }
    }
}
