// <copyright file="Ability.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using System;

    public class Ability : IConciergeList
    {
        public Ability()
        {
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public int Level { get; set; }

        public string Uses { get; set; }

        public string Recovery { get; set; }

        public string Requirements { get; set; }

        public string Action { get; set; }

        public string Description { get; set; }

        public Guid Id { get; init; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
