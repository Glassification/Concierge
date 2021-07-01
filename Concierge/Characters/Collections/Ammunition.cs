// <copyright file="Ammunition.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Collections
{
    using System;

    using Concierge.Characters.Enums;

    public class Ammunition
    {
        public Ammunition()
        {
            this.ID = Guid.NewGuid();
        }

        public Ammunition(Guid id)
        {
            this.ID = id;
        }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public string Bonus { get; set; }

        public DamageTypes DamageType { get; set; }

        public int Used { get; set; }

        public Guid ID { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
