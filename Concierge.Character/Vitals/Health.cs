// <copyright file="Health.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Vitals
{
    using System;

    using Concierge.Common;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the health of a character, including maximum health, base health, and temporary health.
    /// </summary>
    public sealed class Health : ICopyable<Health>
    {
        private int baseHealthField;

        /// <summary>
        /// Initializes a new instance of the <see cref="Health"/> class with default values.
        /// </summary>
        public Health()
        {
            this.MaxHealth = 0;
            this.BaseHealth = 0;
            this.TemporaryHealth = 0;
        }

        /// <summary>
        /// Gets or sets the maximum health of the character.
        /// </summary>
        public int MaxHealth { get; set; }

        /// <summary>
        /// Gets or sets the base health of the character.
        /// </summary>
        public int BaseHealth
        {
            get => this.baseHealthField;
            set => this.baseHealthField = Math.Clamp(value, -this.MaxHealth, this.MaxHealth);
        }

        /// <summary>
        /// Gets or sets the temporary health of the character.
        /// </summary>
        public int TemporaryHealth { get; set; }

        /// <summary>
        /// Gets a value indicating whether the character's base health is empty.
        /// </summary>
        [JsonIgnore]
        public bool IsEmpty => this.BaseHealth <= -this.MaxHealth;

        /// <summary>
        /// Gets a value indicating whether the character's base health is full.
        /// </summary>
        [JsonIgnore]
        public bool IsFull => this.BaseHealth >= this.MaxHealth;

        /// <summary>
        /// Gets a value indicating whether the character's health is zero.
        /// </summary>
        [JsonIgnore]
        public bool IsZero => this.BaseHealth + this.TemporaryHealth == 0;

        /// <summary>
        /// Resets the character's health to its maximum value.
        /// </summary>
        public void ResetHealth()
        {
            this.BaseHealth = this.MaxHealth;
            this.TemporaryHealth = 0;
        }

        /// <summary>
        /// Creates a deep copy of the health object.
        /// </summary>
        /// <returns>A new instance of the <see cref="Health"/> class with the same property values as the original.</returns>
        public Health DeepCopy()
        {
            return new Health()
            {
                MaxHealth = this.MaxHealth,
                TemporaryHealth = this.TemporaryHealth,
                BaseHealth = this.BaseHealth,
            };
        }
    }
}
