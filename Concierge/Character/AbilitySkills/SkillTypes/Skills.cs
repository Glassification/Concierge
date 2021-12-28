// <copyright file="Skills.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.AbilitySkills.SkillTypes
{
    using Concierge.Character.Enums;
    using Concierge.Utility;
    using Newtonsoft.Json;

    public abstract class Skills : ICopyable<Skills>
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

        public abstract Skills DeepCopy();
    }
}
