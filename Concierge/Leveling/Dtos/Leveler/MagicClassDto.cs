// <copyright file="MagicClassDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling.Dtos.Leveler
{
    using Concierge.Character.Magic;

    public sealed class MagicClassDto
    {
        public MagicClassDto(MagicalClass? oldMagicClass, MagicalClass? newMagicClass)
        {
            this.Old = oldMagicClass;
            this.New = newMagicClass;
        }

        public MagicalClass? Old { get; set; }

        public MagicalClass? New { get; set; }
    }
}
