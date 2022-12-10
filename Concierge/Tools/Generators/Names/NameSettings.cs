// <copyright file="NameSettings.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Generators.Names
{
    using Concierge.Character.Enums;

    public sealed class NameSettings : IGeneratorSettings
    {
        public NameSettings(bool filterGender, Gender gender, bool filterRace, string race)
        {
            this.FilterGender = filterGender;
            this.FilterRace = filterRace;
            this.Gender = gender;
            this.Race = race;
        }

        public bool FilterGender { get; set; }

        public bool FilterRace { get; set; }

        public Gender Gender { get; set; }

        public string Race { get; set; }
    }
}
