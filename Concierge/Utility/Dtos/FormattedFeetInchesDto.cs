// <copyright file="FormattedFeetInchesDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Dtos
{
    public class FormattedFeetInchesDto
    {
        public FormattedFeetInchesDto(int feet, double inches)
        {
            this.Feet = feet;
            this.Inches = inches;
        }

        public int Feet { get; set; }

        public double Inches { get; set; }
    }
}
