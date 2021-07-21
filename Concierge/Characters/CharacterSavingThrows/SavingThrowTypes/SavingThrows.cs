// <copyright file="SavingThrows.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.CharacterSavingThrows.SavingThrowTypes
{
    using Concierge.Characters.Enums;
    using Newtonsoft.Json;

    public abstract class SavingThrows
    {
        public bool Proficiency
        {
            get;
            set;
        }

        [JsonIgnore]
        public abstract StatusChecks StatusChecks
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
