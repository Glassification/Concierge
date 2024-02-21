// <copyright file="Companion.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Character.Companions;
    using Concierge.Character.Vitals;
    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Common.Extensions;
    using Concierge.Tools.DiceRoller;

    public sealed class Companion : ICopyable<Companion>
    {
        private const double HealingRatio = 0.1;

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

        public DiceRoll RollHitDice(Dice hitDie, int constitution)
        {
            var roll = DiceRoll.RollDice(1, (int)hitDie);
            this.Health.BaseHealth += roll.FirstOrDefault(0) + constitution;

            return new DiceRoll((int)hitDie, roll, constitution);
        }

        public void RollShortRestHitDice(Dice hitDie, int constitution)
        {
            var threshold = this.Health.MaxHealth - (int)Math.Ceiling(this.Health.MaxHealth * HealingRatio);
            while (this.Health.BaseHealth < threshold && this.HitDice.Increment(hitDie.ToString()).dice != Dice.None)
            {
                this.RollHitDice(hitDie, constitution);
            }
        }

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
