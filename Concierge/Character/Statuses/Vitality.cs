// <copyright file="Vitality.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Statuses
{
    using System;

    using Newtonsoft.Json;

    public class Vitality
    {
        private int baseHealthField;

        public Vitality()
        {
            this.MaxHealth = 0;
            this.BaseHealth = 0;
            this.TemporaryHealth = 0;
            this.HitDice = new HitDice();
            this.Conditions = new Conditions();
            this.DeathSavingThrows = new DeathSavingThrows();
        }

        public int MaxHealth { get; set; }

        [JsonIgnore]
        public int CurrentHealth
        {
            get
            {
                if (this.Conditions.Fatigued.Equals("Four") || this.Conditions.Fatigued.Equals("Five"))
                {
                    if (this.BaseHealth > this.MaxHealth / 2)
                    {
                        return this.MaxHealth / 2;
                    }
                }
                else if (this.Conditions.Fatigued.Equals("Six"))
                {
                    return 0;
                }

                return this.BaseHealth + this.TemporaryHealth;
            }
        }

        [JsonIgnore]
        public bool IsDead => this.CurrentHealth == -this.MaxHealth;

        public int BaseHealth
        {
            get => this.baseHealthField;
            set
            {
                this.baseHealthField = Math.Min(value, this.MaxHealth);
                this.baseHealthField = Math.Max(value, -this.MaxHealth);
            }
        }

        public int TemporaryHealth { get; set; }

        public HitDice HitDice { get; set; }

        public Conditions Conditions { get; set; }

        public DeathSavingThrows DeathSavingThrows { get; set; }

        public void ResetHealth()
        {
            this.BaseHealth = this.MaxHealth;
        }

        public void RegainHitDice()
        {
            this.HitDice.SpentD6 = RegainHitDie(this.HitDice.SpentD6);
            this.HitDice.SpentD8 = RegainHitDie(this.HitDice.SpentD8);
            this.HitDice.SpentD10 = RegainHitDie(this.HitDice.SpentD10);
            this.HitDice.SpentD12 = RegainHitDie(this.HitDice.SpentD12);
        }

        public void Damage(int damage)
        {
            int oldTempHealth = this.TemporaryHealth;

            this.TemporaryHealth -= damage;

            if (this.TemporaryHealth < 0)
            {
                this.TemporaryHealth = 0;
                damage -= oldTempHealth;
                this.BaseHealth -= damage;
            }
        }

        public void Heal(int heal)
        {
            this.BaseHealth += heal;
        }

        private static int RegainHitDie(int spent)
        {
            var temp = spent;
            temp -= Math.Max(spent / 2, 1);

            return Math.Max(temp, 0);
        }
    }
}
