// <copyright file="ColorUtility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    using Concierge.Primitives;
    using Concierge.Utility.Extensions;

    public static class ColorUtility
    {
        public static List<string> ListDotNetColors()
        {
            var colorList = new List<string>();
            var color = typeof(System.Drawing.KnownColor);
            var colors = Enum.GetValues(color);

            for (int i = 27; i < colors.Length - 8; i++)
            {
                var colorString = colors.GetValue(i)?.ToString();
                if (colorString is not null)
                {
                    colorList.Add(colorString);
                }
            }

            return colorList;
        }

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
            return new LinearGradientBrush(
                new GradientStopCollection()
                {
                    new GradientStop(startColor, 0.0),
                    new GradientStop(endColor, 1.0),
                },
                startPoint,
                endPoint);
        }

        public static string FormatHexString(string hex)
        {
            if (hex.Length < 1)
            {
                return string.Empty;
            }

            if (hex[0] == '#')
            {
                return hex;
            }

            return hex.Insert(0, "#");
        }
    }
}
