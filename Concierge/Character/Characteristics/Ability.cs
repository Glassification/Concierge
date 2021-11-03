// <copyright file="Ability.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using System;

    public class Ability : ICopyable
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

        public ICopyable DeepCopy()
        {
            return new Ability()
            {
                Name = this.Name,
                Level = this.Level,
                Uses = this.Uses,
                Recovery = this.Recovery,
                Requirements = this.Requirements,
                Action = this.Action,
                Description = this.Description,
                Id = this.Id,
            };
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
