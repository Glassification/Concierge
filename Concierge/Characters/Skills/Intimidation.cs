using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.SkillsNamespace
{
    public class Intimidation : Skills
    {
        private int bonus;

        public Intimidation(bool proficiency = false, bool expertise = false)
        {
            Proficiency = proficiency;
            Expertise = expertise;
        }

        public override Constants.Checks Checks
        {
            get
            {
                if (Program.Character.Vitality.Conditions.Fatigued.Equals("Exhaustion 1") ||
                    Program.Character.Vitality.Conditions.Fatigued.Equals("Exhaustion 2") ||
                    Program.Character.Vitality.Conditions.Fatigued.Equals("Exhaustion 3") ||
                    Program.Character.Vitality.Conditions.Fatigued.Equals("Exhaustion 4") ||
                    Program.Character.Vitality.Conditions.Fatigued.Equals("Exhaustion 5") ||
                    Program.Character.Vitality.Conditions.Frightened.Equals("Afflicted") ||
                    Program.Character.Vitality.Conditions.Poisoned.Equals("Afflicted"))
                    return Constants.Checks.Disadvantage;
                else if (Program.Character.Vitality.Conditions.Blinded.Equals("Afflicted"))
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
                if (Expertise)
                    bonus += Program.Character.ProficiencyBonus;

                bonus += Constants.CalculateBonus(Program.Character.Attributes.Charisma);

                return bonus;
            }
        }
    }
}