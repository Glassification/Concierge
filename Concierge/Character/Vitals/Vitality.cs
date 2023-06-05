// <copyright file="Vitality.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Vitals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Concierge.Character.Vitals.ConditionStates;
    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Common.Extensions;
    using Concierge.Tools.DiceRoller;
    using Newtonsoft.Json;

    public sealed class Vitality : ICopyable<Vitality>
    {
        public Vitality()
        {
            this.Health = new Health();
            this.HitDice = new HitDice();
            this.Conditions = new Conditions();
            this.DeathSavingThrows = new DeathSavingThrows();
            this.StatusEffects = new List<StatusEffect>();
            this.ClassResources = new List<ClassResource>();
        }

        [JsonIgnore]
        public int CurrentHealth
        {
            get
            {
                if (this.Conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Four || this.Conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Five)
                {
                    if (this.Health.BaseHealth > this.Health.MaxHealth / 2)
                    {
                        return this.Health.MaxHealth / 2;
                    }
                }
                else if (this.Conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Six)
                {
                    return 0;
                }

                return this.Health.BaseHealth + this.Health.TemporaryHealth;
            }
        }

        public Health Health { get; set; }

        public HitDice HitDice { get; set; }

        public Conditions Conditions { get; set; }

        public DeathSavingThrows DeathSavingThrows { get; set; }

        public List<StatusEffect> StatusEffects { get; set; }

        public List<ClassResource> ClassResources { get; set; }

        public void ResetHealth()
        {
            this.Health.BaseHealth = this.Health.MaxHealth;
        }

        public void RegainHitDice()
        {
            this.HitDice.SpentD6 = RegainHitDie(this.HitDice.SpentD6);
            this.HitDice.SpentD8 = RegainHitDie(this.HitDice.SpentD8);
            this.HitDice.SpentD10 = RegainHitDie(this.HitDice.SpentD10);
            this.HitDice.SpentD12 = RegainHitDie(this.HitDice.SpentD12);
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
                Health = this.Health.DeepCopy(),
                HitDice = this.HitDice.DeepCopy(),
                Conditions = this.Conditions.DeepCopy(),
                DeathSavingThrows = this.DeathSavingThrows.DeepCopy(),
                StatusEffects = this.StatusEffects.DeepCopy().ToList(),
                ClassResources = this.ClassResources.DeepCopy().ToList(),
            };
        }

        public void LevelUp(HitDie hitDie, int newHp)
        {
            this.Health.MaxHealth += newHp;
            this.ResetHealth();
            this.RegainHitDice();
            switch (hitDie)
            {
                case HitDie.D6:
                    this.HitDice.TotalD6++;
                    break;
                case HitDie.D8:
                    this.HitDice.TotalD8++;
                    break;
                case HitDie.D10:
                    this.HitDice.TotalD10++;
                    break;
                case HitDie.D12:
                    this.HitDice.TotalD12++;
                    break;
            }
        }

        public DiceRoll RollHitDice(HitDie hitDie, Attributes attributes)
        {
            var roll = DiceRoll.RollDice(1, (int)hitDie);
            var modifier = Constants.Bonus(attributes.Constitution);

            this.Heal(roll.FirstOrDefault(0) + modifier);

            return new DiceRoll((int)hitDie, roll, modifier);
        }

        private static int RegainHitDie(int spent)
        {
            spent -= Math.Max(spent / 2, 1);
            return Math.Max(spent, 0);
        }
    }
}
