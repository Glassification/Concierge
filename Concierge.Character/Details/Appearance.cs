// <copyright file="Appearance.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Details
{
    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Data;

    /// <summary>
    /// Represents the physical appearance of a character.
    /// </summary>
    public sealed class Appearance : ICopyable<Appearance>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Appearance"/> class with default values.
        /// </summary>
        public Appearance()
        {
            this.Gender = string.Empty;
            this.Age = 0;
            this.Height = new UnitDouble(0, UnitTypes.Imperial, Measurements.Height);
            this.Weight = new UnitDouble(0, UnitTypes.Imperial, Measurements.Weight);
            this.SkinColour = CustomColor.Invalid;
            this.EyeColour = CustomColor.Invalid;
            this.HairColour = CustomColor.Invalid;
            this.DistinguishingMarks = string.Empty;
        }

        /// <summary>
        /// Gets or sets the gender of the character.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the age of the character.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the height of the character.
        /// </summary>
        public UnitDouble Height { get; set; }

        /// <summary>
        /// Gets or sets the weight of the character.
        /// </summary>
        public UnitDouble Weight { get; set; }

        /// <summary>
        /// Gets or sets the skin color of the character.
        /// </summary>
        public CustomColor SkinColour { get; set; }

        /// <summary>
        /// Gets or sets the eye color of the character.
        /// </summary>
        public CustomColor EyeColour { get; set; }

        /// <summary>
        /// Gets or sets the hair color of the character.
        /// </summary>
        public CustomColor HairColour { get; set; }

        /// <summary>
        /// Gets or sets any distinguishing marks of the character.
        /// </summary>
        public string DistinguishingMarks { get; set; }

        /// <summary>
        /// Creates a deep copy of the appearance.
        /// </summary>
        /// <returns>A deep copy of the <see cref="Appearance"/>.</returns>
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
