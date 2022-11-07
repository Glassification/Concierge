// <copyright file="Appearance.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using Concierge.Primitives;
    using Concierge.Utility;
    using Concierge.Utility.Units.Enums;

    public sealed class Appearance : ICopyable<Appearance>
    {
        public Appearance()
        {
            this.Gender = string.Empty;
            this.Age = 0;
            this.Height = new UnitDouble(0, UnitTypes.Imperial, Measurements.Height);
            this.Weight = new UnitDouble(0, UnitTypes.Imperial, Measurements.Weight);
            this.SkinColour = CustomColor.Empty;
            this.EyeColour = CustomColor.Empty;
            this.HairColour = CustomColor.Empty;
            this.DistinguishingMarks = string.Empty;
        }

        public string Gender { get; set; }

        public int Age { get; set; }

        public UnitDouble Height { get; set; }

        public UnitDouble Weight { get; set; }

        public CustomColor SkinColour { get; set; }

        public CustomColor EyeColour { get; set; }

        public CustomColor HairColour { get; set; }

        public string DistinguishingMarks { get; set; }

        public Appearance DeepCopy()
        {
            return new Appearance()
            {
                Gender = this.Gender,
                Age = this.Age,
                Height = this.Height.DeepCopy(),
                Weight = this.Weight.DeepCopy(),
                SkinColour = this.SkinColour.DeepCopy(),
                EyeColour = this.EyeColour.DeepCopy(),
                HairColour = this.HairColour.DeepCopy(),
                DistinguishingMarks = this.DistinguishingMarks,
            };
        }
    }
}
