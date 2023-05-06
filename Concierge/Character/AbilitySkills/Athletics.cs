// <copyright file="Athletics.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.AbilitySkills
{
    using Concierge.Character.Enums;
    using Concierge.Common;

    public sealed class Athletics : Skill
    {
        private int bonus;

        public Athletics(bool proficiency = false, bool expertise = false, StatusChecks checkOverride = StatusChecks.None)
        {
            this.Proficiency = proficiency;
            this.Expertise = expertise;
            this.CheckOverride = checkOverride;
        }

        public override StatusChecks Checks =>
            this.CheckOverride != StatusChecks.None
            ? this.CheckOverride
            : Program.CcsFile.Character.Vitality.Conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.One ||
                Program.CcsFile.Character.Vitality.Conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Two ||
                Program.CcsFile.Character.Vitality.Conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Three ||
                Program.CcsFile.Character.Vitality.Conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Four ||
                Program.CcsFile.Character.Vitality.Conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Five ||
                Program.CcsFile.Character.Vitality.Conditions.Frightened.Afflicted ||
                Program.CcsFile.Character.Vitality.Conditions.Poisoned.Afflicted
                ? StatusChecks.Disadvantage
                : Program.CcsFile.Character.Vitality.Conditions.Blinded.Afflicted
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

                this.bonus += Constants.CalculateBonus(Program.CcsFile.Character.Characteristic.Attributes.Strength);

                return this.bonus;
            }
        }

        public override Skill DeepCopy()
        {
            return new Athletics(this.Proficiency, this.Expertise, this.CheckOverride);
        }
    }
}
