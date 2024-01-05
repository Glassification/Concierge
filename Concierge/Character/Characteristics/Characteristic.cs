// <copyright file="Characteristic.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using System.Collections.Generic;

    using Concierge.Common;
    using Concierge.Common.Extensions;

    public sealed class Characteristic : ICopyable<Characteristic>
    {
        public Characteristic()
        {
            this.Abilities = [];
            this.Appearance = new Appearance();
            this.Attributes = new Attributes();
            this.Languages = [];
            this.Personality = new Personality();
            this.Proficiencies = [];
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
                Abilities = [.. this.Abilities.DeepCopy()],
                Appearance = this.Appearance.DeepCopy(),
                Attributes = this.Attributes.DeepCopy(),
                Languages = [.. this.Languages.DeepCopy()],
                Personality = this.Personality.DeepCopy(),
                Proficiencies = [.. this.Proficiencies.DeepCopy()],
                Senses = this.Senses.DeepCopy(),
            };
        }
    }
}
