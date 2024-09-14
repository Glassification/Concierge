// <copyright file="Companion.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Companions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Character.Vitals;
    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Common.Extensions;
    using Concierge.Tools.DiceRoller;

    /// <summary>
    /// Represents a companion character.
    /// </summary>
    public sealed class Companion : ICopyable<Companion>
    {
        private const double HealingRatio = 0.1;

        /// <summary>
        /// Initializes a new instance of the <see cref="Companion"/> class with default properties.
        /// </summary>
        public Companion()
        {
            this.Attributes = new CompanionAttributes();
            this.Health = new Health();
            this.HitDice = new HitDice();
            this.Properties = new CompanionProperties();
            this.CompanionImage = new Portrait();
            this.Weapons = [];
        }

        /// <summary>
        /// Gets or sets the attributes of the companion.
        /// </summary>
        public CompanionAttributes Attributes { get; set; }

        /// <summary>
        /// Gets or sets the image of the companion.
        /// </summary>
        public Portrait CompanionImage { get; set; }

        /// <summary>
        /// Gets or sets the health of the companion.
        /// </summary>
        public Health Health { get; set; }

        /// <summary>
        /// Gets or sets the hit dice of the companion.
        /// </summary>
        public HitDice HitDice { get; set; }

        /// <summary>
        /// Gets or sets the properties of the companion.
        /// </summary>
        public CompanionProperties Properties { get; set; }

        /// <summary>
        /// Gets or sets the weapons wielded by the companion.
        /// </summary>
        public List<CompanionWeapon> Weapons { get; set; }

        /// <summary>
        /// Rolls hit dice for the companion and increases its base health.
        /// </summary>
        /// <param name="hitDie">The type of hit dice to roll.</param>
        /// <returns>The result of the hit dice roll.</returns>
        public DiceRoll RollHitDice(Dice hitDie)
        {
            var roll = DiceRoll.RollDice(1, (int)hitDie);
            var constitution = ConciergeMath.Bonus(this.Attributes.Constitution);
            this.Health.BaseHealth += roll.FirstOrDefault(0) + constitution;

            return new DiceRoll((int)hitDie, roll, constitution);
        }

        /// <summary>
        /// Rolls hit dice for the companion during a short rest to heal.
        /// </summary>
        /// <param name="hitDie">The type of hit dice to roll.</param>
        public void RollShortRestHitDice(Dice hitDie)
        {
            var threshold = this.Health.MaxHealth - (int)Math.Ceiling(this.Health.MaxHealth * HealingRatio);
            while (this.Health.BaseHealth < threshold && this.HitDice.Increment(hitDie.ToString()).dice != Dice.None)
            {
                this.RollHitDice(hitDie);
            }
        }

        /// <summary>
        /// Creates a deep copy of the companion instance.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Companion"/> instance.</returns>
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
