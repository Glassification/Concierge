// <copyright file="Strength.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.AbilitySaves
{
    using Concierge.Character.Enums;
    using Concierge.Common;

    public sealed class Strength : SavingThrow
    {
        private int bonus;

        public Strength(bool proficiency = false, StatusChecks checkOverride = StatusChecks.None)
        {
            this.Proficiency = proficiency;
            this.CheckOverride = checkOverride;
        }

        public override StatusChecks StatusChecks =>
            this.CheckOverride != StatusChecks.None
            ? this.CheckOverride
            : Program.CcsFile.Character.Vitality.Conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Three ||
                Program.CcsFile.Character.Vitality.Conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Four ||
                Program.CcsFile.Character.Vitality.Conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Five
                ? StatusChecks.Disadvantage
                : Program.CcsFile.Character.Vitality.Conditions.Paralyzed.Afflicted ||
                  Program.CcsFile.Character.Vitality.Conditions.Stunned.Afflicted
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

                this.bonus += Constants.Bonus(Program.CcsFile.Character.Characteristic.Attributes.Strength);

                return this.bonus;
            }
        }

        public override SavingThrow DeepCopy()
        {
            return new Strength(this.Proficiency, this.CheckOverride);
        }
    }
}
