// <copyright file="Skills.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.CharacterSkills.SkillTypes
{
    using Concierge.Characters.Enums;
    using Newtonsoft.Json;

    public abstract class Skills
    {
        public bool Proficiency
        {
            get;
            set;
        }

        public bool Expertise
        {
            get;
            set;
        }

        [JsonIgnore]
        public abstract StatusChecks Checks
        {
            get;
        }

        [JsonIgnore]
        public abstract int Bonus
        {
            get;
        }
    }
}
