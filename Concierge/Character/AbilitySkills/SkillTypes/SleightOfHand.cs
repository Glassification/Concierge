// <copyright file="SleightOfHand.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.AbilitySkills.SkillTypes
{
    using Concierge.Character.Enums;
    using Concierge.Utility;

    public class SleightOfHand : Skills
    {
        private int bonus;

        public SleightOfHand(bool proficiency = false, bool expertise = false, StatusChecks checkOverride = StatusChecks.None)
        {
            this.Proficiency = proficiency;
            this.Expertise = expertise;
            this.CheckOverride = checkOverride;
        }

        public override StatusChecks Checks =>
            this.CheckOverride != StatusChecks.None
            ? this.CheckOverride
            : Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("One") ||
                Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Two") ||
                Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Three") ||
                Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Four") ||
                Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Five") ||
                Program.CcsFile.Character.Vitality.Conditions.Frightened.Equals("Frightened") ||
                Program.CcsFile.Character.Vitality.Conditions.Poisoned.Equals("Poisoned")
                ? StatusChecks.Disadvantage
                : Program.CcsFile.Character.Vitality.Conditions.Blinded.Equals("Blinded")
                    ? StatusChecks.Fail
                    : StatusChecks.Normal;

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

        public override Skills DeepCopy()
        {
            return new SleightOfHand(this.Proficiency, this.Expertise, this.CheckOverride);
        }
    }
}