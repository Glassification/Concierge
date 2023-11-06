// <copyright file="SavingThrow.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.AbilitySaves
{
    using Concierge.Character.Enums;
    using Concierge.Common;
    using Newtonsoft.Json;

    public abstract class SavingThrow : ICopyable<SavingThrow>, IAbility
    {
        public static SavingThrow Empty => new Strength();

        public bool Proficiency { get; set; }

        public StatusChecks CheckOverride { get; set; }

        [JsonIgnore]
        public abstract StatusChecks StatusChecks { get; }

        [JsonIgnore]
        public abstract int Bonus { get; }

        public abstract SavingThrow DeepCopy();
    }
}
