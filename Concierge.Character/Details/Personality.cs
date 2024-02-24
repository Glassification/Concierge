// <copyright file="Personality.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Details
{
    using Concierge.Common;

    /// <summary>
    /// Represents the personality traits, ideals, bonds, flaws, and additional notes of a character.
    /// </summary>
    public sealed class Personality : ICopyable<Personality>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Personality"/> class.
        /// </summary>
        public Personality()
        {
            this.Trait1 = string.Empty;
            this.Trait2 = string.Empty;
            this.Ideal = string.Empty;
            this.Bond = string.Empty;
            this.Flaw = string.Empty;
            this.Notes = string.Empty;
        }

        /// <summary>
        /// Gets or sets the first personality trait of the character.
        /// </summary>
        public string Trait1 { get; set; }

        /// <summary>
        /// Gets or sets the second personality trait of the character.
        /// </summary>
        public string Trait2 { get; set; }

        /// <summary>
        /// Gets or sets the character's ideal.
        /// </summary>
        public string Ideal { get; set; }

        /// <summary>
        /// Gets or sets the bond that the character holds.
        /// </summary>
        public string Bond { get; set; }

        /// <summary>
        /// Gets or sets the flaw or weakness of the character.
        /// </summary>
        public string Flaw { get; set; }

        /// <summary>
        /// Gets or sets additional notes about the character's personality.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Creates a deep copy of the <see cref="Personality"/> object.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Personality"/> object.</returns>
        public Personality DeepCopy()
        {
            return new Personality()
            {
                Trait1 = this.Trait1,
                Trait2 = this.Trait2,
                Ideal = this.Ideal,
                Bond = this.Bond,
                Flaw = this.Flaw,
                Notes = this.Notes,
            };
        }
    }
}
