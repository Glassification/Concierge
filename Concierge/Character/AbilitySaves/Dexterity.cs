// <copyright file="Dexterity.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.AbilitySaves
{
    using Concierge.Character.Enums;
    using Concierge.Common;

    public sealed class Dexterity : SavingThrow
    {
        private int bonus;

        public Dexterity(bool proficiency = false, StatusChecks checkOverride = StatusChecks.None)
        {
            this.Proficiency = proficiency;
            this.CheckOverride = checkOverride;
        }

        public override StatusChecks StatusChecks =>
            this.CheckOverride != StatusChecks.None
            ? this.CheckOverride
            : Program.CcsFile.Character.Vitality.Conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Three ||
                Program.CcsFile.Character.Vitality.Conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Four ||
                Program.CcsFile.Character.Vitality.Conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Five ||
                Program.CcsFile.Character.Vitality.Conditions.Restrained.Afflicted
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

                this.bonus += Constants.CalculateBonus(Program.CcsFile.Character.Characteristic.Attributes.Dexterity);

                return this.bonus;
            }
        }

        public override SavingThrow DeepCopy()
        {
            return new Dexterity(this.Proficiency, this.CheckOverride);
        }
    }
}
