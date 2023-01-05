// <copyright file="RaceSensesDefinition.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling.Definitions
{
    using System;

    using Concierge.Character.Enums;
    using Concierge.Leveling.Dtos.Definitions;

    public static class RaceSensesDefinition
    {
        public static SensesDto GetDwarfSenses()
        {
            return new SensesDto(25, VisionTypes.Darkvision);
        }

        public static SensesDto GetElfSenses(string subrace)
        {
            return new SensesDto(30, subrace.Contains("Drow", StringComparison.InvariantCultureIgnoreCase) ? VisionTypes.SuperiorDarkvision : VisionTypes.Darkvision);
        }

        public static SensesDto GetHalflingSenses()
        {
            return new SensesDto(25, VisionTypes.Normal);
        }

        public static SensesDto GetHumanSenses()
        {
            return new SensesDto(30, VisionTypes.Normal);
        }

        public static SensesDto GetDragonbornSenses()
        {
            return new SensesDto(30, VisionTypes.Normal);
        }

        public static SensesDto GetGnomeSenses()
        {
            return new SensesDto(25, VisionTypes.Darkvision);
        }

        public static SensesDto GetHalfElfSenses()
        {
            return new SensesDto(30, VisionTypes.Darkvision);
        }

        public static SensesDto GetHalfOrcSenses()
        {
            return new SensesDto(30, VisionTypes.Darkvision);
        }

        public static SensesDto GetTieflingSenses()
        {
            return new SensesDto(30, VisionTypes.Darkvision);
        }
    }
}
