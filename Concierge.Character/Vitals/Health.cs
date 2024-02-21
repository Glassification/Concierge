// <copyright file="Health.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Vitals
{
    using System;

    using Concierge.Common;
    using Newtonsoft.Json;

    public sealed class Health : ICopyable<Health>
    {
        private int baseHealthField;

        public Health()
        {
            this.MaxHealth = 0;
            this.BaseHealth = 0;
            this.TemporaryHealth = 0;
        }

        public int MaxHealth { get; set; }

        public int BaseHealth
        {
            get => this.baseHealthField;
            set
            {
                var boundHp = Math.Min(value, this.MaxHealth);
                this.baseHealthField = Math.Max(boundHp, -this.MaxHealth);
            }
        }

        public int TemporaryHealth { get; set; }

        [JsonIgnore]
        public bool IsEmpty => this.BaseHealth <= -this.MaxHealth;

        [JsonIgnore]
        public bool IsFull => this.BaseHealth >= this.MaxHealth;

        [JsonIgnore]
        public bool IsZero => this.BaseHealth + this.TemporaryHealth == 0;

        public void ResetHealth()
        {
            this.BaseHealth = this.MaxHealth;
        }

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
