// <copyright file="ConciergeColors.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System.Windows.Media;

    public static class ConciergeColors
    {
        static ConciergeColors()
        {
            WindowBackground = Color.FromArgb(255, 32, 32, 32);

            LightCarryCapacity = Color.FromArgb(255, 216, 228, 188);

            MediumCarryCapacity = Color.FromArgb(255, 252, 213, 180);

            HeavyCarryCapacity = Color.FromArgb(255, 187, 74, 67);

            UsedBox = Color.FromArgb(255, 62, 62, 66);

            TotalDarkBox = Color.FromArgb(255, 15, 15, 15);

            TotalLightBox = Color.FromArgb(255, 55, 55, 55);

            ControlBackground = Color.FromArgb(255, 55, 55, 55);

            BorderHighlight = Color.FromArgb(255, 34, 126, 169);

            Highlight = Color.FromArgb(255, 28, 57, 71);

            HighlightRichTextBox = Colors.DarkOrchid;

            TreeItemHover = Color.FromArgb(255, 130, 223, 251);

            ControlForeGray = Color.FromArgb(255, 51, 51, 51);

            ControlBackGray = Color.FromArgb(255, 43, 43, 43);

            Verdigris = Color.FromArgb(255, 72, 179, 185);

            Mint = Color.FromArgb(255, 72, 185, 135);

            Deer = Color.FromArgb(255, 185, 135, 72);
        }

        public static Color WindowBackground { get; }

        public static Color Verdigris { get; }

        public static Color TotalDarkBox { get; }

        public static Color TotalLightBox { get; }

        public static Color ControlBackground { get; }

        public static Color UsedBox { get; }

        public static Color BorderHighlight { get; }

        public static Color Highlight { get; }

        public static Color HighlightRichTextBox { get; }

        public static Color TreeItemHover { get; }

        public static Color LightCarryCapacity { get; }

        public static Color MediumCarryCapacity { get; }

        public static Color HeavyCarryCapacity { get; }

        public static Color ControlForeGray { get; }

        public static Color ControlBackGray { get; }

        public static Color Mint { get; }

        public static Color Deer { get; }
    }
}