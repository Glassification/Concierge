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
            DisabledControlForeBlue = Color.FromArgb(255, 29, 36, 54);
            ControlForeDarkBlue = Color.FromArgb(255, 31, 37, 53);
            ControlBackBlue = Color.FromArgb(255, 23, 27, 36);
            Verdigris = Color.FromArgb(255, 72, 179, 185);
            Mint = Color.FromArgb(255, 72, 185, 135);
            Deer = Color.FromArgb(255, 185, 135, 72);
            BlueMagenta = Color.FromArgb(255, 135, 72, 185);
            DarkPink = Color.FromArgb(255, 185, 72, 123);
            Copper = Color.FromArgb(255, 184, 115, 51);
            Silver = Colors.Silver;
            Electrum = Color.FromArgb(255, 80, 200, 120);
            Gold = Color.FromArgb(255, 207, 181, 59);
            Platinum = Color.FromArgb(255, 229, 228, 226);
            Border = Color.FromArgb(255, 74, 74, 74);
            Separator = Color.FromArgb(255, 22, 26, 38);
        }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FF121626.
        /// </summary>
        public static Color WindowBackground { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FF48B3B9.
        /// </summary>
        public static Color Verdigris { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FF0F0F0F.
        /// </summary>
        public static Color DarkGreyBox { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FF3E3E42.
        /// </summary>
        public static Color UsedBox { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FF227EA9.
        /// </summary>
        public static Color BorderHighlight { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FF1C3947.
        /// </summary>
        public static Color Highlight { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FF9932CC.
        /// </summary>
        public static Color HighlightRichTextBox { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FF82DFFB.
        /// </summary>
        public static Color TreeItemHover { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FFD8E4BC.
        /// </summary>
        public static Color LightCarryCapacity { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FFFCD5B4.
        /// </summary>
        public static Color MediumCarryCapacity { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FFBB4A43.
        /// </summary>
        public static Color HeavyCarryCapacity { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FF262E42.
        /// </summary>
        public static Color ControlForeBlue { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FF1D2436.
        /// </summary>
        public static Color DisabledControlForeBlue { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FF1F2535.
        /// </summary>
        public static Color ControlForeDarkBlue { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FF171B24.
        /// </summary>
        public static Color ControlBackBlue { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FF48B987.
        /// </summary>
        public static Color Mint { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FFB98748.
        /// </summary>
        public static Color Deer { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FF8748b9.
        /// </summary>
        public static Color BlueMagenta { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FFB9487B.
        /// </summary>
        public static Color DarkPink { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FFB87333.
        /// </summary>
        public static Color Copper { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FFC0C0C0.
        /// </summary>
        public static Color Silver { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FF50C878.
        /// </summary>
        public static Color Electrum { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FFCFB53B.
        /// </summary>
        public static Color Gold { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FFE5E4E2.
        /// </summary>
        public static Color Platinum { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FF4A4A4A.
        /// </summary>
        public static Color Border { get; }

        /// <summary>
        /// Gets the Concierge-defined color that has an ARGB value of #FF161A26.
        /// </summary>
        public static Color Separator { get; }
    }
}