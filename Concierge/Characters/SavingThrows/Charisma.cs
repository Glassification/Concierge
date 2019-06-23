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
