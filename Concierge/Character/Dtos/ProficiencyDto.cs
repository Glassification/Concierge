// <copyright file="ProficiencyDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Dtos
{
    using Concierge.Character.Enums;

    public class ProficiencyDto
    {
        public ProficiencyDto(string proficiency, ProficiencyTypes proficiencyTypes)
        {
            this.Proficiency = proficiency;
            this.ProficiencyTypes = proficiencyTypes;
        }

        public string Proficiency { get; set; }

        public ProficiencyTypes ProficiencyTypes { get; set; }
    }
}
