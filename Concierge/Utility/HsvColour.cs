﻿// <copyright file="HsvColour.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    public class HsvColour
    {
        public HsvColour(double hue, double saturation, double value)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Value = value;
        }

        public double Hue { get; set; }

        public double Saturation { get; set; }

        public double Value { get; set; }

        public void Invert()
        {
            this.Hue = (this.Hue + 180) % 360;
        }
    }
}