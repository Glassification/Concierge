// <copyright file="ConciergeBrushes.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common
{
    using System.Windows.Media;

    /// <summary>
    /// Provides a collection of pre-defined SolidColorBrush objects used for styling the Concierge application.
    /// </summary>
    public static class ConciergeBrushes
    {
        static ConciergeBrushes()
        {
            WindowBackground = new SolidColorBrush(ConciergeColors.WindowBackground);
            WindowBackground.Freeze();

            LightCarryCapacity = new SolidColorBrush(ConciergeColors.LightCarryCapacity);
            LightCarryCapacity.Freeze();

            MediumCarryCapacity = new SolidColorBrush(ConciergeColors.MediumCarryCapacity);
            MediumCarryCapacity.Freeze();

            HeavyCarryCapacity = new SolidColorBrush(ConciergeColors.HeavyCarryCapacity);
            HeavyCarryCapacity.Freeze();

            UsedBox = new SolidColorBrush(ConciergeColors.UsedBox);
            UsedBox.Freeze();

            DarkGreyBox = new SolidColorBrush(ConciergeColors.DarkGreyBox);
            DarkGreyBox.Freeze();

            BorderHighlight = new SolidColorBrush(ConciergeColors.BorderHighlight);
            BorderHighlight.Freeze();

            Highlight = new SolidColorBrush(ConciergeColors.Highlight);
            Highlight.Freeze();

            TreeItemHover = new SolidColorBrush(ConciergeColors.TreeItemHover);
            TreeItemHover.Freeze();

            ControlForeBlue = new SolidColorBrush(ConciergeColors.ControlForeBlue);
            ControlForeBlue.Freeze();

            DisabledControlForeBlue = new SolidColorBrush(ConciergeColors.DisabledControlForeBlue);
            DisabledControlForeBlue.Freeze();

            ControlBackBlue = new SolidColorBrush(ConciergeColors.ControlBackBlue);
            ControlBackBlue.Freeze();

            ControlForeDarkBlue = new SolidColorBrush(ConciergeColors.ControlForeDarkBlue);
            ControlForeDarkBlue.Freeze();

            Verdigris = new SolidColorBrush(ConciergeColors.Verdigris);
            Verdigris.Freeze();

            Mint = new SolidColorBrush(ConciergeColors.Mint);
            Mint.Freeze();

            Deer = new SolidColorBrush(ConciergeColors.Deer);
            Deer.Freeze();

            DarkPink = new SolidColorBrush(ConciergeColors.DarkPink);
            DarkPink.Freeze();

            Copper = new SolidColorBrush(ConciergeColors.Copper);
            Copper.Freeze();

            Silver = new SolidColorBrush(ConciergeColors.Silver);
            Silver.Freeze();

            Electrum = new SolidColorBrush(ConciergeColors.Electrum);
            Electrum.Freeze();

            Gold = new SolidColorBrush(ConciergeColors.Gold);
            Gold.Freeze();

            Platinum = new SolidColorBrush(ConciergeColors.Platinum);
            Platinum.Freeze();

            Border = new SolidColorBrush(ConciergeColors.Border);
            Border.Freeze();

            Separator = new SolidColorBrush(ConciergeColors.Separator);
            Separator.Freeze();
        }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FF121626.
        /// </summary>
        public static SolidColorBrush WindowBackground { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FF48B3B9.
        /// </summary>
        public static SolidColorBrush Verdigris { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FF0F0F0F.
        /// </summary>
        public static SolidColorBrush DarkGreyBox { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FF3E3E42.
        /// </summary>
        public static SolidColorBrush UsedBox { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FF227EA9.
        /// </summary>
        public static SolidColorBrush BorderHighlight { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FF1C3947.
        /// </summary>
        public static SolidColorBrush Highlight { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FF82DFFB.
        /// </summary>
        public static SolidColorBrush TreeItemHover { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FFD8E4BC.
        /// </summary>
        public static SolidColorBrush LightCarryCapacity { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FFFCD5B4.
        /// </summary>
        public static SolidColorBrush MediumCarryCapacity { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FFBB4A43.
        /// </summary>
        public static SolidColorBrush HeavyCarryCapacity { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FF262E42.
        /// </summary>
        public static SolidColorBrush ControlForeBlue { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FF1D2436.
        /// </summary>
        public static SolidColorBrush DisabledControlForeBlue { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FF1F2535.
        /// </summary>
        public static SolidColorBrush ControlForeDarkBlue { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FF171B24.
        /// </summary>
        public static SolidColorBrush ControlBackBlue { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FF48B987.
        /// </summary>
        public static SolidColorBrush Mint { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FFB98748.
        /// </summary>
        public static SolidColorBrush Deer { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FFB9487B.
        /// </summary>
        public static SolidColorBrush DarkPink { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FFB87333.
        /// </summary>
        public static SolidColorBrush Copper { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FFC0C0C0.
        /// </summary>
        public static SolidColorBrush Silver { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FF50C878.
        /// </summary>
        public static SolidColorBrush Electrum { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FFCFB53B.
        /// </summary>
        public static SolidColorBrush Gold { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FFE5E4E2.
        /// </summary>
        public static SolidColorBrush Platinum { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FF4A4A4A.
        /// </summary>
        public static SolidColorBrush Border { get; }

        /// <summary>
        /// Gets the Concierge-defined brush that has an ARGB value of #FF161A26.
        /// </summary>
        public static SolidColorBrush Separator { get; }
    }
}
