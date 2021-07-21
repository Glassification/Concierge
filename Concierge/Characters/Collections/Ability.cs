// <copyright file="Ability.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Collections
{
    using System;

    public class Ability
    {
        public Ability()
        {
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public string Level { get; set; }

        public string Uses { get; set; }

        public string Recovery { get; set; }

        public string Action { get; set; }

        public string Note { get; set; }

        public Guid Id { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
