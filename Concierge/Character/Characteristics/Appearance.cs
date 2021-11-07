// <copyright file="Appearance.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using Concierge.Primatives;
    using Concierge.Utility;
    using Concierge.Utility.Enums;

    public class Appearance : ICopyable
    {
        public Appearance()
        {
            this.Gender = string.Empty;
            this.Age = 0;
            this.Height = new ConvertableDouble(0, UnitTypes.Imperial, Measurements.Height);
            this.Weight = new ConvertableDouble(0, UnitTypes.Imperial, Measurements.Weight);
            this.SkinColour = string.Empty;
            this.EyeColour = string.Empty;
            this.HairColour = string.Empty;
            this.DistinguishingMarks = string.Empty;
        }

        public string Gender { get; set; }

        public int Age { get; set; }

        public ConvertableDouble Height { get; set; }

        public ConvertableDouble Weight { get; set; }

        public string SkinColour { get; set; }

        public string EyeColour { get; set; }

        public string HairColour { get; set; }

        public string DistinguishingMarks { get; set; }

        public ICopyable DeepCopy()
        {
            return new Appearance()
            {
                Gender = this.Gender,
                Age = this.Age,
                Height = this.Height.DeepCopy() as ConvertableDouble,
                Weight = this.Weight.DeepCopy() as ConvertableDouble,
                SkinColour = this.SkinColour,
                EyeColour = this.EyeColour,
                HairColour = this.HairColour,
                DistinguishingMarks = this.DistinguishingMarks,
            };
        }
    }
}
