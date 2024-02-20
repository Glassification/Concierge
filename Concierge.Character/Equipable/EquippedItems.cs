// <copyright file="EquippedItems.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Equipable
{
    using System.Collections.Generic;

    using Concierge.Character.Enums;

    public sealed class EquippedItems
    {
        private readonly List<Inventory> inventory;
        private readonly List<Weapon> weapons;

        public EquippedItems(List<Inventory> inventory, List<Weapon> weapons)
        {
            this.inventory = inventory;
            this.weapons = weapons;
        }

        public List<IEquipable> Head => this.GetEquippedItems(EquipmentSlot.Head);

        public List<IEquipable> Torso => this.GetEquippedItems(EquipmentSlot.Torso);

        public List<IEquipable> Hands => this.GetEquippedItems(EquipmentSlot.Hands);

        public List<IEquipable> Legs => this.GetEquippedItems(EquipmentSlot.Legs);

        public List<IEquipable> Feet => this.GetEquippedItems(EquipmentSlot.Feet);

        public int Attuned
        {
            get
            {
                var attuned = 0;
                foreach (var inventory in this.inventory)
                {
                    if (inventory.Attuned)
                    {
                        attuned++;
                    }
                }

                foreach (var weapon in this.weapons)
                {
                    if (weapon.Attuned)
                    {
                        attuned++;
                    }
                }

                return attuned;
            }
        }

        public static void Equip(IEquipable equipable, EquipmentSlot slot)
        {
            if (!equipable.IsEquipped)
            {
                equipable.IsEquipped = true;
                equipable.EquipmentSlot = slot;
            }
        }

        public static void Dequip(IEquipable equipable)
        {
            if (equipable.IsEquipped)
            {
                equipable.IsEquipped = false;
                equipable.EquipmentSlot = EquipmentSlot.None;
                equipable.Attuned = false;
            }
        }

        private List<IEquipable> GetEquippedItems(EquipmentSlot slot)
        {
            var items = new List<IEquipable>();
            foreach (var inventory in this.inventory)
            {
                if (inventory.EquipmentSlot == slot)
                {
                    items.Add(inventory);
                }
            }

            foreach (var weapon in this.weapons)
            {
                if (weapon.EquipmentSlot == slot)
                {
                    items.Add(weapon);
                }
            }

            return items;
        }
    }
}
