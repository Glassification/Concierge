using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.SavingThrowsNamespace
{
    public class Constitution : SavingThrows
    {
        private int bonus;

        public Constitution(bool proficiency = false)
        {
            Proficiency = proficiency;
        }

        public override Constants.Checks Checks
        {
            get
            {
                if (Program.Character.Vitality.Conditions.Fatigued.Equals("Exhaustion 3") ||
                    Program.Character.Vitality.Conditions.Fatigued.Equals("Exhaustion 4") ||
                    Program.Character.Vitality.Conditions.Fatigued.Equals("Exhaustion 5"))
                    return Constants.Checks.Disadvantage;
                else
                    return Constants.Checks.Normal;
            }
        }

        public override int Bonus
        {
            get
            {
                bonus = 0;

                if (Proficiency)
                    bonus += Program.Character.ProficiencyBonus;

                bonus += Constants.CalculateBonus(Program.Character.Attributes.Constitution);

                return bonus;
            }
        }
    }
}
