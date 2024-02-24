// <copyright file="IEquipable.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Equipable
{
    using Concierge.Character.Enums;

    /// <summary>
    /// Represents an item that can be equipped.
    /// </summary>
    public interface IEquipable
    {
        /// <summary>
        /// Gets or sets the amount of the equipable item.
        /// </summary>
        int Amount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the equipable item is attuned.
        /// </summary>
        bool Attuned { get; set; }

        /// <summary>
        /// Gets or sets the equipment slot of the equipable item.
        /// </summary>
        EquipmentSlot EquipmentSlot { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the equipable item is currently equipped.
        /// </summary>
        bool IsEquipped { get; set; }

        /// <summary>
        /// Gets or sets the name of the equipable item.
        /// </summary>
        string Name { get; set; }
    }
}
