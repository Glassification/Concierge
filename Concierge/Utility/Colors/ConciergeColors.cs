// <copyright file="ConciergeColors.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Colors
{
    using System.Windows;
    using System.Windows.Media;

    public static class ConciergeColors
    {
        static ConciergeColors()
        {
            ControlText = new SolidColorBrush(Color.FromArgb(255, 214, 214, 214));

            LightCarryCapacity = new SolidColorBrush(Color.FromArgb(255, 216, 228, 188));
            MediumCarryCapacity = new SolidColorBrush(Color.FromArgb(255, 252, 213, 180));
            HeavyCarryCapacity = new SolidColorBrush(Color.FromArgb(255, 187, 74, 67));

            UsedBoxBrush = new SolidColorBrush(Color.FromArgb(255, 62, 62, 66));
            TotalDarkBoxBrush = new SolidColorBrush(Color.FromArgb(255, 15, 15, 15));
            TotalLightBoxBrush = new SolidColorBrush(Color.FromArgb(255, 51, 51, 51));
            ControlBackgroundBrush = new SolidColorBrush(Color.FromArgb(255, 63, 63, 63));
            ToggleBoxBrush = new SolidColorBrush(Color.FromArgb(255, 6, 1, 31));
            RectangleBorderHighlight = new SolidColorBrush(Color.FromArgb(255, 34, 126, 169));
            RectangleBorder = new SolidColorBrush(Color.FromArgb(255, 0, 9, 23));
            Highlight = new SolidColorBrush(Color.FromArgb(255, 28, 57, 71));
            NoteTreeItemHover = new SolidColorBrush(Color.FromArgb(255, 130, 223, 251));
            FailedSaveBrush = ColorUtilities.GenerateGradientBrush(FailedDarkRed, FailedLightRed, new Point(0.5, 0), new Point(0.5, 1));
            SucceededSaveBrush = ColorUtilities.GenerateGradientBrush(SucceededDarkGreen, SucceededLightGreen, new Point(0.5, 0), new Point(0.5, 1));
            ProficiencyBrush = ColorUtilities.GenerateGradientBrush(ProficiencyDarkBlue, ProficiencyLightBlue, new Point(0, 0), new Point(1, 1));
        }

        public static SolidColorBrush ControlText { get; }

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

        public static SolidColorBrush NoteTreeItemHover { get; }

        public static SolidColorBrush LightCarryCapacity { get; }

        public static SolidColorBrush MediumCarryCapacity { get; }

        public static SolidColorBrush HeavyCarryCapacity { get; }

        private static Color FailedLightRed => Color.FromArgb(255, 245, 133, 143);

        private static Color FailedDarkRed => Color.FromArgb(255, 250, 74, 90);

        private static Color SucceededLightGreen => Color.FromArgb(255, 190, 236, 176);

        private static Color SucceededDarkGreen => Color.FromArgb(255, 123, 216, 96);

        private static Color ProficiencyLightBlue => Color.FromArgb(255, 0, 18, 51);

        private static Color ProficiencyDarkBlue => Color.FromArgb(255, 0, 9, 23);
    }
}