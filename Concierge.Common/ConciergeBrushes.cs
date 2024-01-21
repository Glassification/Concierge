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

            ControlBackBlue = new SolidColorBrush(ConciergeColors.ControlBackBlue);
            ControlBackBlue.Freeze();

            Verdigris = new SolidColorBrush(ConciergeColors.Verdigris);
            Verdigris.Freeze();

            Mint = new SolidColorBrush(ConciergeColors.Mint);
            Mint.Freeze();

            Deer = new SolidColorBrush(ConciergeColors.Deer);
            Deer.Freeze();

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
        }

        public static SolidColorBrush WindowBackground { get; }

        public static SolidColorBrush Verdigris { get; }

        public static SolidColorBrush DarkGreyBox { get; }

        public static SolidColorBrush UsedBox { get; }

        public static SolidColorBrush BorderHighlight { get; }

        public static SolidColorBrush Highlight { get; }

        public static SolidColorBrush TreeItemHover { get; }

        public static SolidColorBrush LightCarryCapacity { get; }

        public static SolidColorBrush MediumCarryCapacity { get; }

        public static SolidColorBrush HeavyCarryCapacity { get; }

        public static SolidColorBrush ControlForeBlue { get; }

        public static SolidColorBrush ControlBackBlue { get; }

        public static SolidColorBrush Mint { get; }

        public static SolidColorBrush Deer { get; }

        public static SolidColorBrush Copper { get; }

        public static SolidColorBrush Silver { get; }

        public static SolidColorBrush Electrum { get; }

        public static SolidColorBrush Gold { get; }

        public static SolidColorBrush Platinum { get; }

        public static SolidColorBrush Border { get; }
    }
}
