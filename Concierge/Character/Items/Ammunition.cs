// <copyright file="Ammunition.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Items
{
    using System;

    using Concierge.Character.Enums;
    using Concierge.Utility;

    public class Ammunition : ICopyable<Ammunition>
    {
        private int used;

        public Ammunition()
        {
            this.Name = string.Empty;
            this.Bonus = string.Empty;
            this.Id = Guid.NewGuid();
        }

        public string Bonus { get; set; }

        public CoinType CoinType { get; set; }

        public DamageTypes DamageType { get; set; }

        public Guid Id { get; init; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public int Used
        {
            get
            {
                return this.used;
            }

            set
            {
                if (value <= this.Quantity)
                {
                    this.used = value;
                }
            }
        }

        public int Value { get; set; }

        public Ammunition DeepCopy()
        {
            return new Ammunition()
            {
                Name = this.Name,
                Quantity = this.Quantity,
                Bonus = this.Bonus,
                DamageType = this.DamageType,
                Used = this.Used,
                Id = this.Id,
                CoinType = this.CoinType,
                Value = this.Value,
            };
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
