// <copyright file="Charisma.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.AbilitySavingThrows.SavingThrowTypes
{
    using Concierge.Character.Enums;
    using Concierge.Utility.Utilities;

    public class Charisma : SavingThrows
    {
        private int bonus;

        public Charisma(bool proficiency = false, StatusChecks checkOverride = StatusChecks.None)
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

                this.bonus += CharacterUtility.CalculateBonus(Program.CcsFile.Character.Attributes.Charisma);

                return this.bonus;
            }
        }

        public override SavingThrows DeepCopy()
        {
            return new Charisma(this.Proficiency, this.CheckOverride);
        }
    }
}
