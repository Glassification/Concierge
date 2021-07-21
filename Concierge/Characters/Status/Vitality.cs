// <copyright file="Vitality.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Status
{
    using System;

    using Newtonsoft.Json;

    public class Vitality
    {
        private int iBaseHealthField;

        public Vitality()
        {
            this.MaxHealth = 0;
            this.BaseHealth = 0;
            this.TemporaryHealth = 0;
            this.HitDice = new HitDice();
            this.Conditions = new Conditions();
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

        public int BaseHealth
        {
            get => this.iBaseHealthField;
            set
            {
                this.iBaseHealthField = Math.Min(value, this.MaxHealth);
                this.iBaseHealthField = Math.Max(value, 0);
            }
        }

        public int TemporaryHealth { get; set; }

        public HitDice HitDice { get; set; }

        public Conditions Conditions { get; set; }

        public void ResetHealth()
        {
            this.BaseHealth = this.MaxHealth;
        }

        public void RegainHitDice()
        {
            int temp;

            temp = this.HitDice.SpentD6;
            temp -= Math.Max(this.HitDice.SpentD6 / 2, 1);
            this.HitDice.SpentD6 = Math.Max(temp, 0);

            temp = this.HitDice.SpentD8;
            temp -= Math.Max(this.HitDice.SpentD8 / 2, 1);
            this.HitDice.SpentD8 = Math.Max(temp, 0);

            temp = this.HitDice.SpentD10;
            temp -= Math.Max(this.HitDice.SpentD10 / 2, 1);
            this.HitDice.SpentD10 = Math.Max(temp, 0);

            temp = this.HitDice.SpentD12;
            temp -= Math.Max(this.HitDice.SpentD12 / 2, 1);
            this.HitDice.SpentD12 = Math.Max(temp, 0);
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
    }
}
