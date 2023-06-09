// <copyright file="ColorUtility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Provides utility methods for color-related operations.
    /// </summary>
    public static class ColorUtility
    {
        /// <summary>
        /// Retrieves a list of string representations of .NET colors from the <see cref="System.Drawing.KnownColor"/> enumeration.
        /// </summary>
        /// <returns>A list of color names.</returns>
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

        /// <summary>
        /// Generates a <see cref="LinearGradientBrush"/> with the specified start and end colors.
        /// </summary>
        /// <param name="startColor">The starting color of the gradient.</param>
        /// <param name="endColor">The ending color of the gradient.</param>
        /// <param name="startPoint">The starting point of the gradient.</param>
        /// <param name="endPoint">The ending point of the gradient.</param>
        /// <returns>The created <see cref="LinearGradientBrush"/> object.</returns>
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

        /// <summary>
        /// Formats the provided hexadecimal color string by adding a "#" symbol if it is missing.
        /// </summary>
        /// <param name="hex">The hexadecimal color string to format.</param>
        /// <returns>The formatted hexadecimal color string.</returns>
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
