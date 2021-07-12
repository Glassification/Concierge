// <copyright file="Dexterity.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.SavingThrowsNamespace
{
    using Concierge.Characters.Enums;
    using Concierge.Utility;

    public class Dexterity : SavingThrows
    {
        private int bonus;

        public Dexterity(bool proficiency = false)
        {
            this.Proficiency = proficiency;
        }

        public override StatusChecks StatusChecks => Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Three") ||
                    Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Four") ||
                    Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Five") ||
                    Program.CcsFile.Character.Vitality.Conditions.Restrained.Equals("Restrained")
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

                this.bonus += Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Dexterity);

                return this.bonus;
            }
        }
    }
}
