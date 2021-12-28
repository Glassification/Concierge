// <copyright file="SavingThrow.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.AbilitySavingThrows
{
    using Concierge.Character.AbilitySavingThrows.SavingThrowTypes;
    using Concierge.Utility;

    public class SavingThrow : ICopyable<SavingThrow>
    {
        public SavingThrow()
        {
            this.Strength = new Strength();
            this.Dexterity = new Dexterity();
            this.Constitution = new Constitution();
            this.Intelligence = new Intelligence();
            this.Wisdom = new Wisdom();
            this.Charisma = new Charisma();
        }

        public Strength Strength { get; set; }

        public Dexterity Dexterity { get; set; }

        public Constitution Constitution { get; set; }

        public Intelligence Intelligence { get; set; }

        public Wisdom Wisdom { get; set; }

        public Charisma Charisma { get; set; }

        public SavingThrow DeepCopy()
        {
            return new SavingThrow()
            {
                Strength = (Strength)this.Strength.DeepCopy(),
                Dexterity = (Dexterity)this.Dexterity.DeepCopy(),
                Constitution = (Constitution)this.Constitution.DeepCopy(),
                Intelligence = (Intelligence)this.Intelligence.DeepCopy(),
                Wisdom = (Wisdom)this.Wisdom.DeepCopy(),
                Charisma = (Charisma)this.Charisma.DeepCopy(),
            };
        }
    }
}
