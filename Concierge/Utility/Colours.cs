// <copyright file="Colours.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System;
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
        }

        public static SolidColorBrush TotalDarkBoxBrush { get; }

        public static SolidColorBrush TotalLightBoxBrush { get; }

        public static SolidColorBrush ControlBackgroundBrush { get; }

        public static SolidColorBrush UsedBoxBrush { get; }

        public static SolidColorBrush ToggleBoxBrush { get; }

        public static Color LightGreen => Color.FromArgb(255, 216, 228, 188);

        public static Color LightYellow => Color.FromArgb(255, 252, 213, 180);

        public static Color MediumRed => Color.FromArgb(255, 187, 74, 67);

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

        public static Color Invert(Color color)
        {
            var hsvColour = ToHsv(color);
            hsvColour.Invert();

            return FromHsv(hsvColour);
        }
    }
}