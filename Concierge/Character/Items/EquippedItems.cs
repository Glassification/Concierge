﻿// <copyright file="EquippedItems.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Items
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Character.Enums;
    using Newtonsoft.Json;

    public class EquippedItems
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

        public List<Inventory> Head { get; }

        public List<Inventory> Torso { get; }

        public List<Inventory> Hands { get; }

        public List<Inventory> Legs { get; }

        public List<Inventory> Feet { get; }

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

        public void Equip(Inventory item, EquipmentSlot equipSlot)
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
        }

        public void Dequip(Inventory item, EquipmentSlot equipSlot)
        {
            AddToInventory(item);

            switch (equipSlot)
            {
                case EquipmentSlot.Head:
                    this.Head.Remove(item);
                    break;
                case EquipmentSlot.Torso:
                    this.Torso.Remove(item);
                    break;
                case EquipmentSlot.Hands:
                    this.Hands.Remove(item);
                    break;
                case EquipmentSlot.Legs:
                    this.Legs.Remove(item);
                    break;
                case EquipmentSlot.Feet:
                    this.Feet.Remove(item);
                    break;
            }
        }

        public EquipmentSlot GetEquippedItemSlot(Inventory item)
        {
            return this.Head.Any(x => x.EquppedId.Equals(item.EquppedId))
                ? EquipmentSlot.Head
                : this.Torso.Any(x => x.EquppedId.Equals(item.EquppedId))
                    ? EquipmentSlot.Torso
                    : this.Hands.Any(x => x.EquppedId.Equals(item.EquppedId))
                                    ? EquipmentSlot.Hands
                                    : this.Legs.Any(x => x.EquppedId.Equals(item.EquppedId)) ? EquipmentSlot.Legs : EquipmentSlot.Feet;
        }

        private static double GetWeight(List<Inventory> list)
        {
            var weight = 0.0;

            foreach (var item in list)
            {
                weight += item.Weight;
            }

            return weight;
        }

        private static int GetAttuned(List<Inventory> list)
        {
            var attuned = 0;

            foreach (var item in list)
            {
                if (item.Attuned)
                {
                    attuned++;
                }
            }

            return attuned;
        }

        private static void AddToInventory(Inventory item)
        {
            var inventory = Program.CcsFile.Character.Inventories;
            var existingItem = inventory.SingleOrDefault(x => x.Id.Equals(item.Id));

            if (existingItem == null)
            {
                item.EquppedId = Guid.Empty;
                item.Attuned = false;
                inventory.Add(item);
            }
            else
            {
                existingItem.Amount++;
            }
        }

        private static Inventory RemoveFromInventory(Inventory item)
        {
            if (item.Amount > 1)
            {
                item.Amount--;
            }
            else
            {
                Program.CcsFile.Character.Inventories.Remove(item);
            }

            return item.Copy();
        }
    }
}