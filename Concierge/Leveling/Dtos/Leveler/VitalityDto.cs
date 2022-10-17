// <copyright file="VitalityDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling.Dtos.Leveler
{
    using Concierge.Character.Statuses;

    public sealed class VitalityDto
    {
        public VitalityDto(Vitality oldVitality, Vitality newVitality)
        {
            this.Old = oldVitality;
            this.New = newVitality;
        }

        public Vitality Old { get; set; }

        public Vitality New { get; set; }
    }
}
