// <copyright file="Wisdom.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.AbilitySaves
{
    using Concierge.Character.Enums;
    using Concierge.Common;

    public sealed class Wisdom : SavingThrow
    {
        private int bonus;

        public Wisdom(bool proficiency = false, StatusChecks checkOverride = StatusChecks.Auto)
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

                this.bonus += Constants.Bonus(Program.CcsFile.Character.Characteristic.Attributes.Wisdom);

                return this.bonus;
            }
        }

        public override SavingThrow DeepCopy()
        {
            return new Wisdom(this.Proficiency, this.CheckOverride);
        }
    }
}
