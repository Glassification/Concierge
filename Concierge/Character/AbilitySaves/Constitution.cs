// <copyright file="Constitution.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.AbilitySaves
{
    using Concierge.Character.Enums;
    using Concierge.Common;

    public sealed class Constitution : SavingThrow
    {
        private int bonus;

        public Constitution(bool proficiency = false, StatusChecks checkOverride = StatusChecks.Auto)
        {
            this.Proficiency = proficiency;
            this.CheckOverride = checkOverride;
        }

        public override StatusChecks StatusChecks =>
            this.CheckOverride != StatusChecks.Auto
            ? this.CheckOverride
            : Program.CcsFile.Character.Vitality.Conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Three ||
                Program.CcsFile.Character.Vitality.Conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Four ||
                Program.CcsFile.Character.Vitality.Conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Five
                    ? StatusChecks.Disadvantage
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

                this.bonus += Constants.Bonus(Program.CcsFile.Character.Characteristic.Attributes.Constitution);

                return this.bonus;
            }
        }

        public override SavingThrow DeepCopy()
        {
            return new Constitution(this.Proficiency, this.CheckOverride);
        }
    }
}
