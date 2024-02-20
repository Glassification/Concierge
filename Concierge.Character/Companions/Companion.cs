// <copyright file="Companion.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using System.Collections.Generic;

    using Concierge.Character.Companions;
    using Concierge.Character.Vitals;
    using Concierge.Common;
    using Concierge.Common.Extensions;

    public sealed class Companion : ICopyable<Companion>
    {
        public Companion()
        {
            this.Attributes = new CompanionAttributes();
            this.Health = new Health();
            this.HitDice = new HitDice();
            this.Properties = new CompanionProperties();
            this.CompanionImage = new Portrait();
            this.Weapons = [];
        }

        public CompanionAttributes Attributes { get; set; }

        public Portrait CompanionImage { get; set; }

        public Health Health { get; set; }

        public HitDice HitDice { get; set; }

        public CompanionProperties Properties { get; set; }

        public List<CompanionWeapon> Weapons { get; set; }

        public Companion DeepCopy()
        {
            return new Companion()
            {
                Attributes = this.Attributes.DeepCopy(),
                CompanionImage = this.CompanionImage.DeepCopy(),
                Health = this.Health.DeepCopy(),
                HitDice = this.HitDice.DeepCopy(),
                Properties = this.Properties.DeepCopy(),
                Weapons = [.. this.Weapons.DeepCopy()],
            };
        }
    }
}
