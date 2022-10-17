// <copyright file="MagicClassDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling.Dtos.Leveler
{
    using Concierge.Character.Spellcasting;

    public sealed class MagicClassDto
    {
        public MagicClassDto(MagicClass? oldMagicClass, MagicClass? newMagicClass)
        {
            this.Old = oldMagicClass;
            this.New = newMagicClass;
        }

        public MagicClass? Old { get; set; }

        public MagicClass? New { get; set; }
    }
}
