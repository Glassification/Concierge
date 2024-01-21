// <copyright file="ConciergeColors.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common
{
    using System.Windows.Media;

    /// <summary>
    /// Provides a collection of pre-defined Color objects used for styling the Concierge application.
    /// </summary>
    public static class ConciergeColors
    {
        static ConciergeColors()
        {
            WindowBackground = Color.FromArgb(255, 18, 22, 38);

            LightCarryCapacity = Color.FromArgb(255, 216, 228, 188);

            MediumCarryCapacity = Color.FromArgb(255, 252, 213, 180);

            HeavyCarryCapacity = Color.FromArgb(255, 187, 74, 67);

            UsedBox = Color.FromArgb(255, 62, 62, 66);

            DarkGreyBox = Color.FromArgb(255, 15, 15, 15);

            BorderHighlight = Color.FromArgb(255, 34, 126, 169);

            Highlight = Color.FromArgb(255, 28, 57, 71);

            HighlightRichTextBox = Colors.DarkOrchid;

            TreeItemHover = Color.FromArgb(255, 130, 223, 251);

            ControlForeBlue = Color.FromArgb(255, 38, 46, 66);

            ControlBackBlue = Color.FromArgb(255, 23, 27, 36);

            Verdigris = Color.FromArgb(255, 72, 179, 185);

            Mint = Color.FromArgb(255, 72, 185, 135);

            Deer = Color.FromArgb(255, 185, 135, 72);

            Copper = Color.FromArgb(255, 184, 115, 51);

            Silver = Colors.Silver;

            Electrum = Color.FromArgb(255, 80, 200, 120);

            Gold = Color.FromArgb(255, 207, 181, 59);

            Platinum = Color.FromArgb(255, 229, 228, 226);

            Border = Color.FromArgb(255, 74, 74, 74);
        }

        public static Color WindowBackground { get; }

        public static Color Verdigris { get; }

        public static Color DarkGreyBox { get; }

        public static Color UsedBox { get; }

        public static Color BorderHighlight { get; }

        public static Color Highlight { get; }

        public static Color HighlightRichTextBox { get; }

        public static Color TreeItemHover { get; }

        public static Color LightCarryCapacity { get; }

        public static Color MediumCarryCapacity { get; }

        public static Color HeavyCarryCapacity { get; }

        public static Color ControlForeBlue { get; }

        public static Color ControlBackBlue { get; }

        public static Color Mint { get; }

        public static Color Deer { get; }

        public static Color Copper { get; }

        public static Color Silver { get; }

        public static Color Electrum { get; }

        public static Color Gold { get; }

        public static Color Platinum { get; }

        public static Color Border { get; }
    }
}