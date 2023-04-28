// <copyright file="ColorUtility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

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
