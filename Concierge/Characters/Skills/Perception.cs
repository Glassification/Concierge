using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.SkillsNamespace
{
    public class Perception : Skills
    {
        private int bonus;

        public Perception(bool proficiency = false, bool expertise = false)
        {
            Proficiency = proficiency;
            Expertise = expertise;
        }

        public override Constants.Checks Checks
        {
            get
            {
                if (Program.Character.Vitality.Conditions.Fatigued.Equals("One") ||
                    Program.Character.Vitality.Conditions.Fatigued.Equals("Two") ||
                    Program.Character.Vitality.Conditions.Fatigued.Equals("Three") ||
                    Program.Character.Vitality.Conditions.Fatigued.Equals("Four") ||
                    Program.Character.Vitality.Conditions.Fatigued.Equals("Five") ||
                    Program.Character.Vitality.Conditions.Frightened.Equals("Frightened") ||
                    Program.Character.Vitality.Conditions.Poisoned.Equals("Poisoned"))
                    return Constants.Checks.Disadvantage;
                else if (Program.Character.Vitality.Conditions.Blinded.Equals("Blinded"))
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

                bonus += Utilities.CalculateBonus(Program.Character.Attributes.Wisdom);

                return bonus;
            }
        }
    }
}