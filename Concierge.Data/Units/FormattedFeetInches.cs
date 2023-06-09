// <copyright file="FormattedFeetInches.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Data.Units
{
    /// <summary>
    /// Represents a measurement in feet and inches in a formatted manner.
    /// </summary>
    public sealed class FormattedFeetInches
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormattedFeetInches"/> class with the specified number of feet and inches.
        /// </summary>
        /// <param name="feet">The number of feet.</param>
        /// <param name="inches">The number of inches.</param>
        public FormattedFeetInches(int feet, double inches)
        {
            this.Feet = feet;
            this.Inches = inches;
        }

        /// <summary>
        /// Gets or sets the number of feet.
        /// </summary>
        public int Feet { get; set; }

        /// <summary>
        /// Gets or sets the number of inches.
        /// </summary>
        public double Inches { get; set; }
    }
}
