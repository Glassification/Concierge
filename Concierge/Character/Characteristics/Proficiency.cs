// <copyright file="Proficiency.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using System;

    using Concierge.Character.Enums;
    using Concierge.Utility;

    public class Proficiency : ICopyable<Proficiency>
    {
        public const string MartialMelee = "Martial Melee Weapons";
        public const string MartialRanged = "Martial Ranged Weapons";
        public const string SimpleMelee = "Simple Melee Weapons";
        public const string SimpleRanged = "Simple Ranged Weapons";

        public Proficiency()
        {
            this.Name = string.Empty;
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public ProficiencyTypes ProficiencyType { get; set; }

        public Guid Id { get; init; }

        public Proficiency DeepCopy()
        {
            return new Proficiency()
            {
                Name = this.Name,
                ProficiencyType = this.ProficiencyType,
                Id = this.Id,
            };
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
