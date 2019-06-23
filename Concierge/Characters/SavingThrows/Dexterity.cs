using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.SavingThrowsNamespace
{
    public class Dexterity : SavingThrows
    {
        private int bonus;

        public Dexterity(bool proficiency = false)
        {
            Proficiency = proficiency;
        }

        public override int Bonus
        {
            get
            {
                bonus = 0;

                if (Proficiency)
                    bonus += Program.Character.ProficiencyBonus;

                bonus += Constants.CalculateBonus(Program.Character.Attributes.Dexterity);

                return bonus;
            }
        }
    }
}
