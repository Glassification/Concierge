// <copyright file="ColorExtensions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Extensions
{
    using System;
    using System.Linq;
    using System.Windows.Media;

    /// <summary>
    /// Provides extension methods for working with Color objects.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Retrieves the name of the color, or the HEX value if name is undefined.
        /// </summary>
        /// <param name="color">The Color object.</param>
        /// <returns>The name of the color, formatted.</returns>
        public static string GetName(this Color color)
        {
            var properties = typeof(Colors).GetProperties().FirstOrDefault(p =>
            {
                Color color1 = (Color)(p.GetValue(null) ?? Colors.Transparent);
                return Color.AreClose(color1, color);
            });
            return properties is not null ? properties.Name.FormatColorName() : color.ToHexWithoutAlpha();
        }

        /// <summary>
        /// Retrieves the foreground color (either white or black) based on the brightness of the color.
        /// </summary>
        /// <param name="color">The Color object.</param>
        /// <returns>A SolidColorBrush representing the foreground color.</returns>
        public static SolidColorBrush GetForeColor(this Color color)
        {
            return color.GetBrightness() < Constants.BrightnessTransition ? Brushes.White : Brushes.Black;
        }

        /// <summary>
        /// Calculates the brightness value of the specified color.
        /// </summary>
        /// <param name="color">The color to calculate the brightness from.</param>
        /// <returns>The brightness value of the color, ranging from 0 to 255.</returns>
        public static int GetBrightness(this Color color)
        {
            return (int)Math.Sqrt((color.R * color.R * 0.241) + (color.G * color.G * 0.691) + (color.B * color.B * 0.068));
        }

        /// <summary>
        /// Retrieves the hue value of the color.
        /// </summary>
        /// <param name="color">The Color object.</param>
        /// <returns>The hue value of the color.</returns>
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

        /// <summary>
        /// Converts the color to a hexadecimal string representation without the alpha channel.
        /// </summary>
        /// <param name="color">The Color object.</param>
        /// <returns>A hexadecimal string representation of the color without the alpha channel.</returns>
        public static string ToHexWithoutAlpha(this Color color)
        {
            return color.ToString().Remove(1, 2);
        }

        /// <summary>
        /// Saturates the color by a given percentage.
        /// </summary>
        /// <param name="color">The Color object.</param>
        /// <param name="percent">The percentage to saturate the color by.</param>
        /// <returns>A new saturated Color object.</returns>
        public static Color Saturate(this Color color, double percent)
        {
            percent = Math.Max(0, Math.Min(1, percent));

            var r = (byte)(color.R - (percent / 100.0 * color.R));
            var g = (byte)(color.G - (percent / 100.0 * color.G));
            var b = (byte)(color.B - (percent / 100.0 * color.B));

            return Color.FromRgb(r, g, b);
        }

        /// <summary>
        /// Desaturates the color by a given percentage.
        /// </summary>
        /// <param name="color">The Color object.</param>
        /// <param name="percent">The percentage to desaturate the color by.</param>
        /// <returns>A new desaturated Color object.</returns>
        public static Color Desaturate(this Color color, double percent)
        {
            var greyscale = (0.3 * color.R) + (0.6 * color.G) + (0.1 * color.B);
            percent = Math.Max(0, Math.Min(1, percent));

            var r = (byte)(color.R + (percent * (greyscale - color.R)));
            var g = (byte)(color.G + (percent * (greyscale - color.G)));
            var b = (byte)(color.B + (percent * (greyscale - color.B)));

            return Color.FromRgb(r, g, b);
        }
    }
}
