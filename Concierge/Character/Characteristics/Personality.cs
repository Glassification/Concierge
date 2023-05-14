// <copyright file="Personality.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using Concierge.Common;

    public sealed class Personality : ICopyable<Personality>
    {
        public Personality()
        {
            this.Trait1 = string.Empty;
            this.Trait2 = string.Empty;
            this.Ideal = string.Empty;
            this.Bond = string.Empty;
            this.Flaw = string.Empty;
            this.Notes = string.Empty;
        }

        public string Trait1 { get; set; }

        public string Trait2 { get; set; }

        public string Ideal { get; set; }

        public string Bond { get; set; }

        public string Flaw { get; set; }

        public string Notes { get; set; }

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
