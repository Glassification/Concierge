// <copyright file="EquippedItems.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Equipable
{
    using System.Collections.Generic;

    using Concierge.Character.Enums;

    /// <summary>
    /// Represents a collection of equipped items, including inventory items and weapons.
    /// </summary>
    public sealed class EquippedItems
    {
        private readonly List<Inventory> inventory;
        private readonly List<Weapon> weapons;

        /// <summary>
        /// Initializes a new instance of the <see cref="EquippedItems"/> class.
        /// </summary>
        /// <param name="inventory">The list of inventory items.</param>
        /// <param name="weapons">The list of weapons.</param>
        public EquippedItems(List<Inventory> inventory, List<Weapon> weapons)
        {
            this.inventory = inventory;
            this.weapons = weapons;
        }

        /// <summary>
        /// Gets the list of items equipped on the head slot.
        /// </summary>
        public List<IEquipable> Head => this.GetEquippedItems(EquipmentSlot.Head);

        /// <summary>
        /// Gets the list of items equipped on the torso slot.
        /// </summary>
        public List<IEquipable> Torso => this.GetEquippedItems(EquipmentSlot.Torso);

        /// <summary>
        /// Gets the list of items equipped on the hands slot.
        /// </summary>
        public List<IEquipable> Hands => this.GetEquippedItems(EquipmentSlot.Hands);

        /// <summary>
        /// Gets the list of items equipped on the legs slot.
        /// </summary>
        public List<IEquipable> Legs => this.GetEquippedItems(EquipmentSlot.Legs);

        /// <summary>
        /// Gets the list of items equipped on the feet slot.
        /// </summary>
        public List<IEquipable> Feet => this.GetEquippedItems(EquipmentSlot.Feet);

        /// <summary>
        /// Gets the number of attuned items.
        /// </summary>
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

        /// <summary>
        /// Equips an item to the specified equipment slot.
        /// </summary>
        /// <param name="equipable">The item to equip.</param>
        /// <param name="slot">The equipment slot.</param>
        public static void Equip(IEquipable equipable, EquipmentSlot slot)
        {
            if (!equipable.IsEquipped)
            {
                equipable.IsEquipped = true;
                equipable.EquipmentSlot = slot;
            }
        }

        /// <summary>
        /// De-equips an item.
        /// </summary>
        /// <param name="equipable">The item to de-equip.</param>
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
