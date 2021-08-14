// <copyright file="Ammunition.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Items
{
    using System;

    using Concierge.Characters.Enums;

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

        public Guid Id { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
