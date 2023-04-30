// <copyright file="FormattedFeetInches.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Data.Units
{
    public sealed class FormattedFeetInches
    {
        public FormattedFeetInches(int feet, double inches)
        {
            this.Feet = feet;
            this.Inches = inches;
        }

        public int Feet { get; set; }

        public double Inches { get; set; }
    }
}
