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
    using Concierge.Tools.DiceRoller;
    using Newtonsoft.Json;

    public sealed class Vitality : ICopyable<Vitality>
    {
        private const double HealingRatio = 0.1;

        public Vitality()
        {
            this.ClassResources = [];
            this.DeathSavingThrows = new DeathSavingThrows();
            this.Health = new Health();
            this.HitDice = new HitDice();
            this.Status = new Status();
        }

        public List<ClassResource> ClassResources { get; set; }

        public DeathSavingThrows DeathSavingThrows { get; set; }

        public Health Health { get; set; }

        public HitDice HitDice { get; set; }

        public Status Status { get; set; }

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

        public void ResetHealth()
        {
            this.Health.BaseHealth = this.Health.MaxHealth;
        }

        public void RegainHitDice()
        {
            this.HitDice.SpentD6 = Constants.Regain(this.HitDice.SpentD6);
            this.HitDice.SpentD8 = Constants.Regain(this.HitDice.SpentD8);
            this.HitDice.SpentD10 = Constants.Regain(this.HitDice.SpentD10);
            this.HitDice.SpentD12 = Constants.Regain(this.HitDice.SpentD12);
        }

        public void ResetDeathSaves()
        {
            this.DeathSavingThrows.ResetDeathSaves();
        }

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

        public void Heal(int heal)
        {
            this.Health.BaseHealth += heal;
        }

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

        public DiceRoll RollHitDice(Dice hitDie, Constitution constitution)
        {
            var roll = DiceRoll.RollDice(1, (int)hitDie);
            var modifier = constitution.Bonus;

            this.Heal(roll.FirstOrDefault(0) + modifier);

            return new DiceRoll((int)hitDie, roll, modifier);
        }

        public void RollShortRestHitDice(Dice hitDie, Constitution constitution)
        {
            var threshold = this.Health.MaxHealth - (int)Math.Ceiling(this.Health.MaxHealth * HealingRatio);
            while (this.Health.BaseHealth < threshold && this.HitDice.Increment(hitDie.ToString()).dice != Dice.None)
            {
                this.RollHitDice(hitDie, constitution);
            }
        }
    }
}
