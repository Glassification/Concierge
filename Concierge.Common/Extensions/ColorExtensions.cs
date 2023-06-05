// <copyright file="ColorExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Extensions
{
    using System;
    using System.Linq;
    using System.Windows.Media;

    public static class ColorExtensions
    {
        public static string GetName(this Color color)
        {
            var properties = typeof(Colors).GetProperties().FirstOrDefault(p =>
            {
                Color color1 = (Color)(p.GetValue(null) ?? Colors.Transparent);
                return Color.AreClose(color1, color);
            });
            return properties != null ? properties.Name.FormatColorName() : color.ToHexWithoutAlpha();
        }

        public static SolidColorBrush GetForeColor(this Color color)
        {
            var brightness = (int)Math.Sqrt((color.R * color.R * 0.241) + (color.G * color.G * 0.691) + (color.B * color.B * 0.068));
            return brightness < Constants.BrightnessTransition ? Brushes.White : Brushes.Black;
        }

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

        public static string ToHexWithoutAlpha(this Color color)
        {
            return color.ToString().Remove(1, 2);
        }

        public static Color Saturate(this Color color, double percent)
        {
            percent = Math.Max(0, Math.Min(1, percent));

            var r = (byte)(color.R - (percent / 100.0 * color.R));
            var g = (byte)(color.G - (percent / 100.0 * color.G));
            var b = (byte)(color.B - (percent / 100.0 * color.B));

            return Color.FromRgb(r, g, b);
        }

        public static Color Desaturate(this Color color, double percent)
        {
            var greayscale = (0.3 * color.R) + (0.6 * color.G) + (0.1 * color.B);
            percent = Math.Max(0, Math.Min(1, percent));

            var r = (byte)(color.R + (percent * (greayscale - color.R)));
            var g = (byte)(color.G + (percent * (greayscale - color.G)));
            var b = (byte)(color.B + (percent * (greayscale - color.B)));

            return Color.FromRgb(r, g, b);
        }
    }
}
