// <copyright file="Spell.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Collections
{
    using System;

    using Concierge.Characters.Enums;
    using Newtonsoft.Json;

    public class Spell
    {
        public Spell()
        {
            this.Id = Guid.NewGuid();
        }

        public Spell(Guid id)
        {
            this.Id = id;
        }

        public string Name { get; set; }

        public bool Prepared { get; set; }

        [JsonIgnore]
        public string PreparedText => this.Prepared ? "Yes" : "No";

        public int Level { get; set; }

        public int Page { get; set; }

        public ArcaneSchools School { get; set; }

        public bool Ritual { get; set; }

        public string Components { get; set; }

        public bool Concentration { get; set; }

        public string Range { get; set; }

        public string Duration { get; set; }

        public string Area { get; set; }

        public string Save { get; set; }

        public string Damage { get; set; }

        public string Description { get; set; }

        public string Class { get; set; }

        public Guid Id { get; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
