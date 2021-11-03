// <copyright file="Ammunition.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Items
{
    using System;

    using Concierge.Character.Enums;

    public class Ammunition
    {
        public Ammunition()
        {
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public string Bonus { get; set; }

        public DamageTypes DamageType { get; set; }

        public int Used { get; set; }

        public Guid Id { get; init; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
