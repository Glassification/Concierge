// <copyright file="Health.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Statuses
{
    using System;

    using Concierge.Utility;

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

        public int TemporaryHealth { get; set; }

        public int BaseHealth
        {
            get => this.baseHealthField;
            set
            {
                var boundHp = Math.Min(value, this.MaxHealth);
                this.baseHealthField = Math.Max(boundHp, -this.MaxHealth);
            }
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
