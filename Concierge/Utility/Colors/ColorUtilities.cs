// <copyright file="ColorUtilities.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Colors
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    using Concierge.Primatives;
    using Concierge.Utility.Extensions;

    public static class ColorUtilities
    {
        public const byte ColourSpace = 255;

        public static HsvColor ToHsv(Color color)
        {
            var max = Math.Max(color.R, Math.Max(color.G, color.B));
            var min = Math.Min(color.R, Math.Min(color.G, color.B));

            var hue = color.GetHue();
            var saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            var value = max / 255d;

            return new HsvColor(hue, saturation, value);
        }

        public static Color FromHsv(HsvColor hsvColour)
        {
            var hi = Convert.ToInt32(Math.Floor(hsvColour.Hue / 60)) % 6;
            var f = (hsvColour.Hue / 60) - Math.Floor(hsvColour.Hue / 60);

            hsvColour.Value *= 255;
            var v = Convert.ToInt32(hsvColour.Value);
            var p = Convert.ToInt32(hsvColour.Value * (1 - hsvColour.Saturation));
            var q = Convert.ToInt32(hsvColour.Value * (1 - (f * hsvColour.Saturation)));
            var t = Convert.ToInt32(hsvColour.Value * (1 - ((1 - f) * hsvColour.Saturation)));

            return hi == 0
                ? Color.FromArgb(255, (byte)v, (byte)t, (byte)p)
                : hi == 1
                    ? Color.FromArgb(255, (byte)q, (byte)v, (byte)p)
                    : hi == 2
                        ? Color.FromArgb(255, (byte)p, (byte)v, (byte)t)
                        : hi == 3
                            ? Color.FromArgb(255, (byte)p, (byte)q, (byte)v)
                            : hi == 4 ? Color.FromArgb(255, (byte)t, (byte)p, (byte)v) : Color.FromArgb(255, (byte)v, (byte)p, (byte)q);
        }

        public static LinearGradientBrush GenerateGradientBrush(Color startColor, Color endColor, Point startPoint, Point endPoint)
        {
            var gradientStopCollection = new GradientStopCollection()
            {
                new GradientStop(startColor, 0.0),
                new GradientStop(endColor, 1.0),
            };

            return new LinearGradientBrush(gradientStopCollection, startPoint, endPoint);
        }
    }
}
