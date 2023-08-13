// <copyright file="UsedItem.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools
{
    using System;

    using Concierge.Tools.DiceRoller;

    /// <summary>
    /// Represents an item that has been used in an action, such as in combat, with associated attack and damage rolls.
    /// </summary>
    public sealed class UsedItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsedItem"/> class with default values.
        /// </summary>
        public UsedItem()
            : this(DiceRoll.Empty, DiceRoll.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsedItem"/> class with the specified attack, damage, name, and description.
        /// </summary>
        /// <param name="attack">The attack roll associated with the used item.</param>
        /// <param name="damage">The damage roll associated with the used item.</param>
        /// <param name="name">The name of the used item.</param>
        /// /// <param name="damageType">The type of damage of the used item.</param>
        /// <param name="description">A description of the used item.</param>
        public UsedItem(IDiceRoll attack, IDiceRoll damage, string name, string damageType, string description)
        {
            this.Attack = attack;
            this.Damage = damage;
            this.Name = name;
            this.DamageType = damageType;
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets the attack roll associated with the used item.
        /// </summary>
        public IDiceRoll Attack { get; set; }

        /// <summary>
        /// Gets or sets the damage roll associated with the used item.
        /// </summary>
        public IDiceRoll Damage { get; set; }

        /// <summary>
        /// Gets or sets the name of the used item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of damage of the used item.
        /// </summary>
        public string DamageType { get; set; }

        /// <summary>
        /// Gets or sets a description of the used item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets a value indicating whether the used item heals.
        /// </summary>
        public bool IsHealing => this.DamageType.Contains("Heal", StringComparison.InvariantCultureIgnoreCase);
    }
}
