// <copyright file="SpellSlotsDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling.Dtos
{
    using Concierge.Character.Spellcasting;

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
