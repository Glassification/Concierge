// <copyright file="Skill.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.AbilitySkills
{
    using Concierge.Character.Enums;
    using Concierge.Common;
    using Newtonsoft.Json;

    public abstract class Skill : ICopyable<Skill>, IAbility
    {
        public bool Proficiency { get; set; }

        public bool Expertise { get; set; }

        public StatusChecks CheckOverride { get; set; }

        [JsonIgnore]
        public abstract StatusChecks StatusChecks { get; }

        [JsonIgnore]
        public abstract int Bonus { get; }

        public abstract Skill DeepCopy();
    }
}
