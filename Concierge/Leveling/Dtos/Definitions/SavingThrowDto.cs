// <copyright file="SavingThrowDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling.Dtos.Definitions
{
    using Concierge.Character.AbilitySavingThrows.SavingThrowTypes;

    public sealed class SavingThrowDto
    {
        public SavingThrowDto(
            bool strength = false,
            bool dexterity = false,
            bool constitution = false,
            bool intelligence = false,
            bool wisdom = false,
            bool charisma = false)
        {
            this.Strength = new Strength(strength);
            this.Dexterity = new Dexterity(dexterity);
            this.Constitution = new Constitution(constitution);
            this.Intelligence = new Intelligence(intelligence);
            this.Wisdom = new Wisdom(wisdom);
            this.Charisma = new Charisma(charisma);
        }

        public Strength Strength { get; set; }

        public Dexterity Dexterity { get; set; }

        public Constitution Constitution { get; set; }

        public Intelligence Intelligence { get; set; }

        public Wisdom Wisdom { get; set; }

        public Charisma Charisma { get; set; }
    }
}
