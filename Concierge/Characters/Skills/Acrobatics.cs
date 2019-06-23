using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.SkillsNamespace
{
    public class Acrobatics : Skills
    {
        private int bonus;

        public Acrobatics(bool proficiency = false, bool expertise = false)
        {
            Proficiency = proficiency;
            Expertise = expertise;
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

                bonus += Constants.CalculateBonus(Program.Character.Attributes.Dexterity);

                return bonus;
            }
        }
    }
}