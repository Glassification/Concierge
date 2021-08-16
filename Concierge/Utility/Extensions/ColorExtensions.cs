// <copyright file="ColorExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Extensions
{
    using System;
    using System.Windows.Media;

    public static class ColorExtensions
    {
        public static double GetHue(this Color color)
        {
            double hue;
            var min = Math.Min(Math.Min(color.R, color.G), color.B);
            var max = Math.Max(Math.Max(color.R, color.G), color.B);

            if (min == max)
            {
                return 0;
            }

            hue = max == color.R
                ? (color.G - color.B) / (max - min)
                : max == color.G ? 2 + ((color.B - color.R) / (max - min)) : 4 + ((color.R - color.G) / (max - min));

            hue *= 60;

            if (hue < 0)
            {
                hue += 360;
            }

            return Math.Round(hue);
        }

        public static Color Invert(this Color color)
        {
            var hsvColour = Colours.ToHsv(color);
            hsvColour.Invert();

            return Colours.FromHsv(hsvColour);
        }

        public static Color SimpleInvert(this Color color)
        {
            return Color.FromArgb(
                Colours.ColourSpace,
                (byte)(Colours.ColourSpace - color.R),
                (byte)(Colours.ColourSpace - color.G),
                (byte)(Colours.ColourSpace - color.B));
        }
    }
}
