// <copyright file="Appearance.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Characteristics
{
    public class Appearance
    {
        public Appearance()
        {
            this.Gender = string.Empty;
            this.Age = string.Empty;
            this.Height = string.Empty;
            this.Weight = string.Empty;
            this.SkinColour = string.Empty;
            this.EyeColour = string.Empty;
            this.HairColour = string.Empty;
            this.DistinguishingMarks = string.Empty;
        }

        public string Gender { get; set; }

        public string Age { get; set; }

        public string Height { get; set; }

        public string Weight { get; set; }

        public string SkinColour { get; set; }

        public string EyeColour { get; set; }

        public string HairColour { get; set; }

        public string DistinguishingMarks { get; set; }
    }
}
