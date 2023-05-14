// <copyright file="Characteristic.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Common;
    using Concierge.Common.Extensions;

    public class Characteristic : ICopyable<Characteristic>
    {
        public Characteristic()
        {
            this.Abilities = new List<Ability>();
            this.Appearance = new Appearance();
            this.Attributes = new Attributes();
            this.Languages = new List<Language>();
            this.Personality = new Personality();
            this.Proficiencies = new List<Proficiency>();
            this.Senses = new Senses();
        }

        public List<Ability> Abilities { get; set; }

        public Appearance Appearance { get; set; }

        public Attributes Attributes { get; set; }

        public List<Language> Languages { get; set; }

        public Personality Personality { get; set; }

        public List<Proficiency> Proficiencies { get; set; }

        public Senses Senses { get; set; }

        public Characteristic DeepCopy()
        {
            return new Characteristic()
            {
                Abilities = this.Abilities.DeepCopy().ToList(),
                Appearance = this.Appearance.DeepCopy(),
                Attributes = this.Attributes.DeepCopy(),
                Languages = this.Languages.DeepCopy().ToList(),
                Personality = this.Personality.DeepCopy(),
                Proficiencies = this.Proficiencies.DeepCopy().ToList(),
                Senses = this.Senses.DeepCopy(),
            };
        }
    }
}
