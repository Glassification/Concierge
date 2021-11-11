// <copyright file="Spell.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Spellcasting
{
    using System;

    using Concierge.Character.Enums;
    using Concierge.Utility;
    using Newtonsoft.Json;

    public class Spell : ICopyable<Spell>
    {
        public Spell()
        {
            this.Id = Guid.NewGuid();
        }

        public string Name { get; set; }

        public bool Prepared { get; set; }

        [JsonIgnore]
        public string PreparedDisplay => this.Prepared ? "Yes" : "No";

        public int Level { get; set; }

        public int Page { get; set; }

        public ArcaneSchools School { get; set; }

        public bool Ritual { get; set; }

        [JsonIgnore]
        public string RitualDisplay => this.Ritual ? "Yes" : "No";

        public string Components { get; set; }

        public bool Concentration { get; set; }

        [JsonIgnore]
        public string ConcentrationDisplay => this.Concentration ? "Yes" : "No";

        public string Range { get; set; }

        public string Duration { get; set; }

        public string Area { get; set; }

        public string Save { get; set; }

        public string Damage { get; set; }

        public string Description { get; set; }

        public string Class { get; set; }

        public Guid Id { get; init; }

        public Spell DeepCopy()
        {
            return new Spell()
            {
                Prepared = this.Prepared,
                Level = this.Level,
                Page = this.Page,
                School = this.School,
                Ritual = this.Ritual,
                Components = this.Components,
                Concentration = this.Concentration,
                Range = this.Range,
                Duration = this.Duration,
                Area = this.Area,
                Save = this.Save,
                Damage = this.Damage,
                Description = this.Description,
                Class = this.Class,
                Id = this.Id,
            };
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
