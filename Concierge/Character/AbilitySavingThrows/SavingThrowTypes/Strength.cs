// <copyright file="Strength.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.AbilitySavingThrows.SavingThrowTypes
{
    using Concierge.Character.Enums;
    using Concierge.Utility;

    public class Strength : SavingThrows
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
            : Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Three") ||
                Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Four") ||
                Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Five")
                ? StatusChecks.Disadvantage
                : Program.CcsFile.Character.Vitality.Conditions.Paralyzed.Equals("Paralyzed") ||
                  Program.CcsFile.Character.Vitality.Conditions.Stunned.Equals("Stunned")
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

                this.bonus += Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Strength);

                return this.bonus;
            }
        }

        public override SavingThrows DeepCopy()
        {
            return new Strength(this.Proficiency, this.CheckOverride);
        }
    }
}
