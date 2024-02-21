// <copyright file="SpellSlotsDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling.Dtos.Leveler
{
    using Concierge.Character.Magic;

    public sealed class SpellSlotsDto
    {
        public SpellSlotsDto(SpellSlots oldSpellSlots, SpellSlots newSpellSlots)
        {
            this.Old = oldSpellSlots;
            this.New = newSpellSlots;
        }

        public SpellSlots Old { get; set; }

        public SpellSlots New { get; set; }
    }
}
