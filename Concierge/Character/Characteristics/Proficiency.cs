// <copyright file="Proficiency.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using System;

    using Concierge.Character.Enums;

    public class Proficiency
    {
        public Proficiency()
        {
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public ProficiencyTypes ProficiencyType { get; set; }

        public Guid Id { get; init; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
