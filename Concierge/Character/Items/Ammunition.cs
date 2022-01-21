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
        public Ammunition()
        {
            this.Name = string.Empty;
            this.Bonus = string.Empty;
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public string Bonus { get; set; }

        public DamageTypes DamageType { get; set; }

        public int Used { get; set; }

        public Guid Id { get; init; }

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
            };
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
