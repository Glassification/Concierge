// <copyright file="Intelligence.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.AbilitySavingThrows.SavingThrowTypes
{
    using Concierge.Character.Enums;
    using Concierge.Utility;

    public class Intelligence : SavingThrows
    {
        private int bonus;

        public Intelligence(bool proficiency = false)
        {
            this.Proficiency = proficiency;
        }

        public override StatusChecks StatusChecks => Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Three") ||
                    Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Four") ||
                    Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Five")
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

                this.bonus += Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Intelligence);

                return this.bonus;
            }
        }

        public override SavingThrows DeepCopy()
        {
            return new Intelligence(this.Proficiency);
        }
    }
}
