// <copyright file="SavingThrowDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling.Dtos.Definitions
{
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
            this.Strength = strength;
            this.Dexterity = dexterity;
            this.Constitution = constitution;
            this.Intelligence = intelligence;
            this.Wisdom = wisdom;
            this.Charisma = charisma;
        }

        public bool Strength { get; set; }

        public bool Dexterity { get; set; }

        public bool Constitution { get; set; }

        public bool Intelligence { get; set; }

        public bool Wisdom { get; set; }

        public bool Charisma { get; set; }
    }
}
