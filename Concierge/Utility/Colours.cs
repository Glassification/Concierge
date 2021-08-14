// <copyright file="Colours.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System.Windows.Media;

    public static class Colours
    {
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
    }
}