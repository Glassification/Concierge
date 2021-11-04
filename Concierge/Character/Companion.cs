// <copyright file="Companion.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using System.Collections.Generic;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Items;
    using Concierge.Character.Statuses;

    public class Companion
    {
        public Companion()
        {
            this.Vitality = new Vitality();
            this.Attributes = new Attributes();
            this.Attacks = new List<Weapon>();
            this.Properties = new CompanionProperties();
        }

        public CompanionProperties Properties { get; set; }

        public Vitality Vitality { get; set; }

        public Attributes Attributes { get; set; }

        public List<Weapon> Attacks { get; set; }
    }
}
