// <copyright file="Name.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.Generators.Names
{
    using Concierge.Common.Enums;
    using Concierge.Tools.Enums;

    public sealed class Name
    {
        public Name()
        {
            this.Value = string.Empty;
            this.Race = string.Empty;
        }

        public Name(string name, NameType nameType, Gender gender, string race)
        {
            this.Value = name;
            this.NameType = nameType;
            this.Gender = gender;
            this.Race = race;
        }

        public string Value { get; set; }

        public NameType NameType { get; set; }

        public Gender Gender { get; set; }

        public string Race { get; set; }
    }
}
