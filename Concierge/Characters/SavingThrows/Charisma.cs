using Concierge.Utility;
using System;

namespace Concierge.SavingThrowsNamespace
{
    public class Charisma : SavingThrows
    {
        private int bonus;

        public Charisma(bool proficiency = false)
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

                bonus += Constants.CalculateBonus(Program.Character.Attributes.Charisma);

                return bonus;
            }
        }
    }
}
