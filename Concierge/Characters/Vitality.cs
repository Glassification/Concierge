using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters
{
    public class Vitality
    {
        public Vitality()
        {
            MaxHealth = 0;
            CurrentHealth = 0;
            TemporaryHealth = 0;
            HitDice = new HitDice();
            Conditions = new Conditions();
        }

        public void ResetHealth()
        {
            CurrentHealth = MaxHealth;
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

        public int MaxHealth { get; set; }

        public int CurrentHealth { get; set; }

        public int TemporaryHealth { get; set; }

        public HitDice HitDice { get; set; }

        public Conditions Conditions { get; set; }
    }
}
