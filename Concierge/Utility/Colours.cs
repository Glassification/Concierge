// <copyright file="Colours.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    using Concierge.Utility.Extensions;

    public static class Colours
    {
        public const byte ColourSpace = 255;

        static Colours()
        {
            UsedBoxBrush = new SolidColorBrush(Color.FromArgb(255, 62, 62, 66));
            TotalDarkBoxBrush = new SolidColorBrush(Color.FromArgb(255, 15, 15, 15));
            TotalLightBoxBrush = new SolidColorBrush(Color.FromArgb(255, 51, 51, 51));
            ControlBackgroundBrush = new SolidColorBrush(Color.FromArgb(255, 63, 63, 63));
            ToggleBoxBrush = new SolidColorBrush(Color.FromArgb(255, 6, 1, 31));
            RectangleBorderHighlight = new SolidColorBrush(Color.FromArgb(255, 34, 126, 169));
            RectangleBorder = new SolidColorBrush(Color.FromArgb(255, 0, 9, 23));
            Highlight = new SolidColorBrush(Color.FromArgb(255, 28, 57, 71));
            FailedSaveBrush = GenerateGradientBrush(FailedDarkRed, FailedLightRed, new Point(0.5, 0), new Point(0.5, 1));
            SucceededSaveBrush = GenerateGradientBrush(SucceededDarkGreen, SucceededLightGreen, new Point(0.5, 0), new Point(0.5, 1));
            ProficiencyBrush = GenerateGradientBrush(ProficiencyDarkPurple, ProficiencyLightPurple, new Point(0, 0), new Point(1, 1));
        }

        public static LinearGradientBrush FailedSaveBrush { get; }

        public static LinearGradientBrush SucceededSaveBrush { get; }

        public static LinearGradientBrush ProficiencyBrush { get; }

        public static SolidColorBrush TotalDarkBoxBrush { get; }

        public static SolidColorBrush TotalLightBoxBrush { get; }

        public static SolidColorBrush ControlBackgroundBrush { get; }

        public static SolidColorBrush UsedBoxBrush { get; }

        public static SolidColorBrush ToggleBoxBrush { get; }

        public static SolidColorBrush RectangleBorderHighlight { get; }

        public static SolidColorBrush RectangleBorder { get; }

        public static SolidColorBrush Highlight { get; }

        public static Color LightGreen => Color.FromArgb(255, 216, 228, 188);

        public static Color LightYellow => Color.FromArgb(255, 252, 213, 180);

        public static Color MediumRed => Color.FromArgb(255, 187, 74, 67);

        public static Color FailedLightRed => Color.FromArgb(255, 245, 133, 143);

        public static Color FailedDarkRed => Color.FromArgb(255, 250, 74, 90);

        public static Color SucceededLightGreen => Color.FromArgb(255, 190, 236, 176);

        public static Color SucceededDarkGreen => Color.FromArgb(255, 123, 216, 96);

        public static Color ProficiencyLightPurple => Color.FromArgb(255, 0, 18, 51);

        public static Color ProficiencyDarkPurple => Color.FromArgb(255, 0, 9, 23);

        public static HsvColour ToHsv(Color color)
        {
            var max = Math.Max(color.R, Math.Max(color.G, color.B));
            var min = Math.Min(color.R, Math.Min(color.G, color.B));

            var hue = color.GetHue();
            var saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            var value = max / 255d;

            return new HsvColour(hue, saturation, value);
        }

        public static Color FromHsv(HsvColour hsvColour)
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

        private static LinearGradientBrush GenerateGradientBrush(Color startColor, Color endColor, Point startPoint, Point endPoint)
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