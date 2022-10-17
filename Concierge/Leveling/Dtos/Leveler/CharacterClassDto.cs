// <copyright file="CharacterClassDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling.Dtos.Leveler
{
    using Concierge.Character;

    public sealed class CharacterClassDto
    {
        public CharacterClassDto(CharacterClass oldCharacterClass, CharacterClass newCharacterClass)
        {
            this.Old = oldCharacterClass;
            this.New = newCharacterClass;
        }

        public CharacterClass Old { get; set; }

        public CharacterClass New { get; set; }
    }
}
