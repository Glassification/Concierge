using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.SavingThrowsNamespace
{
    public class Strength : SavingThrows
    {
        private int bonus;

        public Strength(bool proficiency = false)
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
                else if (Program.Character.Vitality.Conditions.Paralyzed.Equals("Afflicted") ||
                         Program.Character.Vitality.Conditions.Stunned.Equals("Afflicted"))
                    return Constants.Checks.Fail;
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

                bonus += Constants.CalculateBonus(Program.Character.Attributes.Strength);

                return bonus;
            }
        }
    }
}
