// <copyright file="Detail.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Details
{
    using System.Collections.Generic;

    using Concierge.Common;
    using Concierge.Common.Extensions;

    /// <summary>
    /// Represents detailed information about a character or entity, including abilities, appearance, languages, personality, portrait, proficiencies, and senses.
    /// </summary>
    public sealed class Detail : ICopyable<Detail>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Detail"/> class.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the abilities of the character or entity.
        /// </summary>
        public List<Ability> Abilities { get; set; }

        /// <summary>
        /// Gets or sets the appearance details of the character or entity.
        /// </summary>
        public Appearance Appearance { get; set; }

        // <summary>
        /// Gets or sets the languages spoken by the character or entity.
        /// </summary>
        public List<Language> Languages { get; set; }

        /// <summary>
        /// Gets or sets the personality traits of the character or entity.
        /// </summary>
        public Personality Personality { get; set; }

        /// <summary>
        /// Gets or sets the portrait of the character or entity.
        /// </summary>
        public Portrait Portrait { get; set; }

        /// <summary>
        /// Gets or sets the proficiencies of the character or entity.
        /// </summary>
        public List<Proficiency> Proficiencies { get; set; }

        /// <summary>
        /// Gets or sets the senses of the character or entity.
        /// </summary>
        public Senses Senses { get; set; }

        /// <summary>
        /// Creates a deep copy of the <see cref="Detail"/> object.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Detail"/> object.</returns>
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
