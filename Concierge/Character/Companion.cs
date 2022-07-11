// <copyright file="Companion.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using System.Collections.Generic;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Items;
    using Concierge.Character.Statuses;
    using Concierge.Utility;

    public class Companion : ICopyable<Companion>
    {
        public Companion()
        {
            this.Vitality = new Vitality();
            this.Attributes = new Attributes();
            this.Attacks = new List<Weapon>();
            this.Properties = new CompanionProperties();
            this.CompanionImage = new CharacterImage();
        }

        public CompanionProperties Properties { get; set; }

        public CharacterImage CompanionImage { get; set; }

        public Vitality Vitality { get; set; }

        public Attributes Attributes { get; set; }

        public List<Weapon> Attacks { get; set; }

        public Companion DeepCopy()
        {
            return new Companion()
            {
                Vitality = this.Vitality.DeepCopy(),
                Attributes = this.Attributes.DeepCopy(),
                Attacks = new List<Weapon>(this.Attacks),
                Properties = this.Properties.DeepCopy(),
                CompanionImage = this.CompanionImage.DeepCopy(),
            };
        }
    }
}
