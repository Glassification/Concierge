// <copyright file="Vitality.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Vitals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Character.Aspects;
    using Concierge.Character.Enums;
    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Common.Extensions;
    using Concierge.Configuration;
    using Concierge.Tools.DiceRoller;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the vital statistics and conditions of a character.
    /// </summary>
    public sealed class Vitality : ICopyable<Vitality>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vitality"/> class with default values.
        /// </summary>
        public Vitality()
        {
            this.ClassResources = [];
            this.DeathSavingThrows = new DeathSavingThrows();
            this.Health = new Health();
            this.HitDice = new HitDice();
            this.Status = new Status();
        }

        /// <summary>
        /// Gets or sets the class resources of the character.
        /// </summary>
        public List<ClassResource> ClassResources { get; set; }

        /// <summary>
        /// Gets or sets the death saving throws of the character.
        /// </summary>
        public DeathSavingThrows DeathSavingThrows { get; set; }

        /// <summary>
        /// Gets or sets the health of the character.
        /// </summary>
        public Health Health { get; set; }

        /// <summary>
        /// Gets or sets the hit dice of the character.
        /// </summary>
        public HitDice HitDice { get; set; }

        /// <summary>
        /// Gets or sets the status conditions of the character.
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Gets the current health of the character, considering temporary health and exhaustion levels.
        /// </summary>
        [JsonIgnore]
        public int CurrentHealth
        {
            get
            {
                if (this.Status.Exhaustion.Status == ConditionStatus.Exhaustion4 || this.Status.Exhaustion.Status == ConditionStatus.Exhaustion5)
                {
                    if (this.Health.BaseHealth > this.Health.MaxHealth / 2)
                    {
                        return this.Health.MaxHealth / 2;
                    }
                }
                else if (this.Status.Exhaustion.Status == ConditionStatus.Exhaustion6)
                {
                    return 0;
                }

                return this.Health.BaseHealth + this.Health.TemporaryHealth;
            }
        }

        /// <summary>
        /// Resets the death saving throws of the character.
        /// </summary>
        public void ResetDeathSaves()
        {
            this.DeathSavingThrows.ResetDeathSaves();
        }

        /// <summary>
        /// Inflicts damage on the character.
        /// </summary>
        /// <param name="damage">The amount of damage to inflict.</param>
        public void Damage(int damage)
        {
            int oldTempHealth = this.Health.TemporaryHealth;

            this.Health.TemporaryHealth -= damage;

            if (this.Health.TemporaryHealth < 0)
            {
                this.Health.TemporaryHealth = 0;
                damage -= oldTempHealth;
                this.Health.BaseHealth -= damage;
            }
        }

        /// <summary>
        /// Heals the character by a specified amount.
        /// </summary>
        /// <param name="heal">The amount of healing.</param>
        public void Heal(int heal)
        {
            this.Health.BaseHealth += heal;
        }

        /// <summary>
        /// Creates a deep copy of the <see cref="Vitality"/> object.
        /// </summary>
        /// <returns>A new instance of the <see cref="Vitality"/> class with the same property values as the original.</returns>
        public Vitality DeepCopy()
        {
            return new Vitality()
            {
                ClassResources = [.. this.ClassResources.DeepCopy()],
                DeathSavingThrows = this.DeathSavingThrows.DeepCopy(),
                Health = this.Health.DeepCopy(),
                HitDice = this.HitDice.DeepCopy(),
                Status = this.Status.DeepCopy(),
            };
        }

        /// <summary>
        /// Rolls hit dice for the character during a short rest.
        /// </summary>
        /// <param name="hitDie">The type of hit die to roll.</param>
        /// <param name="constitution">The character's Constitution modifier.</param>
        /// <returns>The result of the hit dice roll.</returns>
        public DiceRoll RollHitDice(Dice hitDie, Constitution constitution)
        {
            var roll = DiceRoll.RollDice(1, (int)hitDie);
            var modifier = constitution.Bonus;

            this.Heal(roll.FirstOrDefault(0) + modifier);

            return new DiceRoll((int)hitDie, roll, modifier);
        }

        /// <summary>
        /// Rolls hit dice for the character during a short rest, healing up to a certain threshold.
        /// </summary>
        /// <param name="hitDie">The type of hit die to roll.</param>
        /// <param name="constitution">The character's Constitution modifier.</param>
        public void RollShortRestHitDice(Dice hitDie, Constitution constitution)
        {
            var ratio = Math.Abs(AppSettingsManager.UserSettings.HealingThreshold - 100) / 100.0;
            var threshold = this.Health.MaxHealth - (int)Math.Ceiling(this.Health.MaxHealth * ratio);
            while (this.Health.BaseHealth < threshold && this.HitDice.Increment(hitDie.ToString()).dice != Dice.None)
            {
                this.RollHitDice(hitDie, constitution);
            }
        }
    }
}
