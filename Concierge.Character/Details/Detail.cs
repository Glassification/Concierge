// <copyright file="Detail.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Details
{
    using System.Collections.Generic;

    using Concierge.Common;
    using Concierge.Common.Extensions;

    public sealed class Detail : ICopyable<Detail>
    {
        public Detail()
        {
            this.Abilities = [];
            this.Appearance = new Appearance();
            this.Languages = [];
            this.Personality = new Personality();
            this.Portrait = new Portrait();
            this.Proficiencies = [];
            this.Senses = new Senses();
        }

        public List<Ability> Abilities { get; set; }

        public Appearance Appearance { get; set; }

        public List<Language> Languages { get; set; }

        public Personality Personality { get; set; }

        public Portrait Portrait { get; set; }

        public List<Proficiency> Proficiencies { get; set; }

        public Senses Senses { get; set; }

        public Detail DeepCopy()
        {
            return new Detail()
            {
                Abilities = [.. this.Abilities.DeepCopy()],
                Appearance = this.Appearance.DeepCopy(),
                Languages = [.. this.Languages.DeepCopy()],
                Personality = this.Personality.DeepCopy(),
                Portrait = this.Portrait.DeepCopy(),
                Proficiencies = [.. this.Proficiencies.DeepCopy()],
                Senses = this.Senses.DeepCopy(),
            };
        }
    }
}
