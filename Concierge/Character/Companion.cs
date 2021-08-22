// <copyright file="Companion.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using System.Collections.Generic;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Character.Statuses;

    public class Companion
    {
        public Companion()
        {
            this.Vitality = new Vitality();
            this.Attributes = new Attributes();
            this.Attacks = new List<Weapon>();
            this.Name = string.Empty;
        }

        public string Name { get; set; }

        public int ArmorClass { get; set; }

        public Vitality Vitality { get; set; }

        public int Movement { get; set; }

        public Attributes Attributes { get; set; }

        public int Perception { get; set; }

        public VisionTypes Vision { get; set; }

        public List<Weapon> Attacks { get; set; }
    }
}
