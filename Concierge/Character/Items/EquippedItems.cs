// <copyright file="EquippedItems.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Items
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Character.Enums;
    using Concierge.Utility;
    using Concierge.Utility.Extensions;
    using Newtonsoft.Json;

    public class EquippedItems : ICopyable<EquippedItems>
    {
        public EquippedItems()
        {
            this.Head = new List<Inventory>();
            this.Torso = new List<Inventory>();
            this.Hands = new List<Inventory>();
            this.Legs = new List<Inventory>();
            this.Feet = new List<Inventory>();
        }

        [JsonIgnore]
        public static List<Inventory> Equipable
        {
            get
            {
                var list = new List<Inventory>();
                foreach (var item in Program.CcsFile.Character.Inventories)
                {
                    if (item.Amount > 0)
                    {
                        list.Add(item);
                    }
                }

                return list;
            }
        }

        public List<Inventory> Head { get; init; }

        public List<Inventory> Torso { get; init; }

        public List<Inventory> Hands { get; init; }

        public List<Inventory> Legs { get; init; }

        public List<Inventory> Feet { get; init; }

        [JsonIgnore]
        public double Weight
        {
            get
            {
                var weight = 0.0;

                weight += GetWeight(this.Head);
                weight += GetWeight(this.Torso);
                weight += GetWeight(this.Hands);
                weight += GetWeight(this.Legs);
                weight += GetWeight(this.Feet);

                return weight;
            }
        }

        [JsonIgnore]
        public int Attuned
        {
            get
            {
                var attuned = 0;

                attuned += GetAttuned(this.Head);
                attuned += GetAttuned(this.Torso);
                attuned += GetAttuned(this.Hands);
                attuned += GetAttuned(this.Legs);
                attuned += GetAttuned(this.Feet);

                return attuned;
            }
        }

        public Inventory Equip(Inventory item, EquipmentSlot equipSlot)
        {
            var newItem = RemoveFromInventory(item);
            switch (equipSlot)
            {
                case EquipmentSlot.Head:
                    this.Head.Add(newItem);
                    break;
                case EquipmentSlot.Torso:
                    this.Torso.Add(newItem);
                    break;
                case EquipmentSlot.Hands:
                    this.Hands.Add(newItem);
                    break;
                case EquipmentSlot.Legs:
                    this.Legs.Add(newItem);
                    break;
                case EquipmentSlot.Feet:
                    this.Feet.Add(newItem);
                    break;
            }

            return newItem;
        }

        public void Dequip(Guid id, Guid equippedId, EquipmentSlot equipSlot)
        {
            Inventory? item = null;
            switch (equipSlot)
            {
                case EquipmentSlot.Head:
                    item = this.Head.Where(x => x.Id.Equals(id) && x.EquppedId.Equals(equippedId)).FirstOrDefault();
                    break;
                case EquipmentSlot.Torso:
                    item = this.Torso.Where(x => x.Id.Equals(id) && x.EquppedId.Equals(equippedId)).FirstOrDefault();
                    break;
                case EquipmentSlot.Hands:
                    item = this.Hands.Where(x => x.Id.Equals(id) && x.EquppedId.Equals(equippedId)).FirstOrDefault();
                    break;
                case EquipmentSlot.Legs:
                    item = this.Legs.Where(x => x.Id.Equals(id) && x.EquppedId.Equals(equippedId)).FirstOrDefault();
                    break;
                case EquipmentSlot.Feet:
                    item = this.Feet.Where(x => x.Id.Equals(id) && x.EquppedId.Equals(equippedId)).FirstOrDefault();
                    break;
            }

            if (item is not null)
            {
                this.Dequip(item, equipSlot);
            }
        }

        public void Dequip(Inventory item, EquipmentSlot equipSlot)
        {
            switch (equipSlot)
            {
                case EquipmentSlot.Head:
                    this.Head.RemoveAll(x => x.EquppedId.Equals(item.EquppedId));
                    break;
                case EquipmentSlot.Torso:
                    this.Torso.RemoveAll(x => x.EquppedId.Equals(item.EquppedId));
                    break;
                case EquipmentSlot.Hands:
                    this.Hands.RemoveAll(x => x.EquppedId.Equals(item.EquppedId));
                    break;
                case EquipmentSlot.Legs:
                    this.Legs.RemoveAll(x => x.EquppedId.Equals(item.EquppedId));
                    break;
                case EquipmentSlot.Feet:
                    this.Feet.RemoveAll(x => x.EquppedId.Equals(item.EquppedId));
                    break;
            }

            AddToInventory(item);
        }

        public EquippedItems DeepCopy()
        {
            return new EquippedItems()
            {
                Head = this.Head.DeepCopy().ToList(),
                Torso = this.Torso.DeepCopy().ToList(),
                Hands = this.Hands.DeepCopy().ToList(),
                Legs = this.Legs.DeepCopy().ToList(),
                Feet = this.Feet.DeepCopy().ToList(),
            };
        }

        private static double GetWeight(List<Inventory> list)
        {
            var weight = 0.0;
            foreach (var item in list)
            {
                if (!item.ItemCategory.Contains("Armor"))
                {
                    weight += item.Weight.Value;
                }
            }

            return weight;
        }

        private static int GetAttuned(List<Inventory> list)
        {
            var attuned = 0;
            foreach (var item in list)
            {
                attuned += item.Attuned ? 1 : 0;
            }

            return attuned;
        }

        private static void AddToInventory(Inventory item)
        {
            var inventory = Program.CcsFile.Character.Inventories;
            var existingItem = inventory.SingleOrDefault(x => x.Id.Equals(item.Id));

            if (existingItem == null)
            {
                item.Attuned = false;
                item.EquppedId = Guid.Empty;
                inventory.Insert(item.Index, item);
                item.Index = 0;
            }
            else if (!existingItem.Equals(item))
            {
                item.Attuned = false;
                item.EquppedId = Guid.Empty;
                item.Id = Guid.NewGuid();
                inventory.Insert(item.Index, item);
                item.Index = 0;
            }
            else
            {
                existingItem.Amount++;
            }
        }

        private static Inventory RemoveFromInventory(Inventory item)
        {
            Inventory newItem;
            var index = Program.CcsFile.Character.Inventories.FindIndex(x => x.Id.Equals(item.Id));

            if (item.Amount > 1)
            {
                item.Amount--;
                newItem = item.DeepCopy();
                newItem.EquppedId = Guid.NewGuid();
                newItem.Amount = 1;
                newItem.Index = index;
            }
            else
            {
                Program.CcsFile.Character.Inventories.RemoveAll(x => x.Id.Equals(item.Id));
                newItem = item;
                newItem.EquppedId = Guid.NewGuid();
                newItem.Index = index;
            }

            return newItem;
        }
    }
}
