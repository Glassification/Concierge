// <copyright file="Stealth.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.AbilitySkills.SkillTypes
{
    using Concierge.Character.Enums;
    using Concierge.Utility;

    public class Stealth : Skills
    {
        private int bonus;

        public Stealth(bool proficiency = false, bool expertise = false)
        {
            this.Proficiency = proficiency;
            this.Expertise = expertise;
        }

        public override StatusChecks Checks => Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("One") ||
                    Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Two") ||
                    Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Three") ||
                    Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Four") ||
                    Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Five") ||
                    Program.CcsFile.Character.Vitality.Conditions.Frightened.Equals("Frightened") ||
                    Program.CcsFile.Character.Vitality.Conditions.Poisoned.Equals("Poisoned")
                    ? StatusChecks.Disadvantage
                    : Program.CcsFile.Character.Vitality.Conditions.Blinded.Equals("Blinded") ? StatusChecks.Fail : StatusChecks.Normal;

        public override int Bonus
        {
            get
            {
                this.bonus = 0;

                if (this.Proficiency)
                {
                    this.bonus += Program.CcsFile.Character.ProficiencyBonus;
                }

                if (this.Expertise)
                {
                    this.bonus += Program.CcsFile.Character.ProficiencyBonus;
                }

                this.bonus += Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Dexterity);

                return this.bonus;
            }
        }
    }
}