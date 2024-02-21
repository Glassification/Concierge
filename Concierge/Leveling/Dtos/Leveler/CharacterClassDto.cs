// <copyright file="CharacterClassDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling.Dtos.Leveler
{
    using Concierge.Character.Dispositions;

    public sealed class CharacterClassDto
    {
        public CharacterClassDto(Class oldCharacterClass, Class newCharacterClass)
        {
            this.Old = oldCharacterClass;
            this.New = newCharacterClass;
        }

        public Class Old { get; set; }

        public Class New { get; set; }
    }
}
