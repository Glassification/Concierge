// <copyright file="Athletics.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.SkillsNamespace
{
    using Concierge.Characters.Enums;
    using Concierge.Utility;

    public class Athletics : Skills
    {
        private int bonus;

        public Athletics(bool proficiency = false, bool expertise = false)
        {
            this.Proficiency = proficiency;
            this.Expertise = expertise;
        }

        public override StatusChecks Checks => Program.Character.Vitality.Conditions.Fatigued.Equals("One") ||
                    Program.Character.Vitality.Conditions.Fatigued.Equals("Two") ||
                    Program.Character.Vitality.Conditions.Fatigued.Equals("Three") ||
                    Program.Character.Vitality.Conditions.Fatigued.Equals("Four") ||
                    Program.Character.Vitality.Conditions.Fatigued.Equals("Five") ||
                    Program.Character.Vitality.Conditions.Frightened.Equals("Frightened") ||
                    Program.Character.Vitality.Conditions.Poisoned.Equals("Poisoned")
                    ? StatusChecks.Disadvantage
                    : Program.Character.Vitality.Conditions.Blinded.Equals("Blinded") ? StatusChecks.Fail : StatusChecks.Normal;

        public override int Bonus
        {
            get
            {
                this.bonus = 0;

                if (this.Proficiency)
                {
                    this.bonus += Program.Character.ProficiencyBonus;
                }

                if (this.Expertise)
                {
                    this.bonus += Program.Character.ProficiencyBonus;
                }

                this.bonus += Utilities.CalculateBonus(Program.Character.Attributes.Strength);

                return this.bonus;
            }
        }
    }
}
