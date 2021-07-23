// <copyright file="Companion.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters
{
    using System.Collections.Generic;

    using Concierge.Characters.Characteristics;
    using Concierge.Characters.Enums;
    using Concierge.Characters.Items;
    using Concierge.Characters.Status;

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
