// <copyright file="SensesDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling.Dtos.Definitions
{
    using Concierge.Character.Enums;

    public sealed class SensesDto
    {
        public SensesDto()
        {
            this.Movement = 30;
            this.VisionType = VisionTypes.Normal;
        }

        public SensesDto(int movement, VisionTypes visionType)
        {
            this.Movement = movement;
            this.VisionType = visionType;
        }

        public int Movement { get; set; }

        public VisionTypes VisionType { get; set; }
    }
}
