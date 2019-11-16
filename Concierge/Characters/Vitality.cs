using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters
{
    public class Vitality
    {
        private int iBaseHealthField;

        public Vitality()
        {
            MaxHealth = 0;
            BaseHealth = 0;
            TemporaryHealth = 0;
            HitDice = new HitDice();
            Conditions = new Conditions();
        }

        public void ResetHealth()
        {
            BaseHealth = MaxHealth;
        }

        public void RegainHitDice()
        {
            int temp;

            temp = HitDice.SpentD6;
            temp -= Math.Max(HitDice.SpentD6 / 2, 1);
            HitDice.SpentD6 = Math.Max(temp, 0);

            temp = HitDice.SpentD8;
            temp -= Math.Max(HitDice.SpentD8 / 2, 1);
            HitDice.SpentD8 = Math.Max(temp, 0);

            temp = HitDice.SpentD10;
            temp -= Math.Max(HitDice.SpentD10 / 2, 1);
            HitDice.SpentD10 = Math.Max(temp, 0);

            temp = HitDice.SpentD12;
            temp -= Math.Max(HitDice.SpentD12 / 2, 1);
            HitDice.SpentD12 = Math.Max(temp, 0);
        }

        public void Damage(int damage)
        {
            int oldTempHealth = TemporaryHealth;

            TemporaryHealth -= damage;

            if (TemporaryHealth < 0)
            {
                TemporaryHealth = 0;
                damage -= oldTempHealth;
                BaseHealth -= damage;
            }
        }

        public void Heal(int heal)
        {
            BaseHealth += heal;
        }

        public int MaxHealth { get; set; }

        public int CurrentHealth
        {
            get
            {
                if (Conditions.Fatigued.Equals("Four") || Conditions.Fatigued.Equals("Five"))
                {
                    if (BaseHealth > MaxHealth/2)
                    {
                        return MaxHealth / 2;
                    }
                }
                else if (Conditions.Fatigued.Equals("Six"))
                {
                    return 0;
                }

                return BaseHealth + TemporaryHealth;
            }
        }

        public int BaseHealth
        {
            get
            {
                return iBaseHealthField;
            }
            set
            {
                iBaseHealthField = Math.Min(value, MaxHealth);
                iBaseHealthField = Math.Max(value, 0);
            }
        }

        public int TemporaryHealth { get; set; }

        public HitDice HitDice { get; set; }

        public Conditions Conditions { get; set; }
    }
}
