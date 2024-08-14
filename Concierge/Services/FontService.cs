// <copyright file="FontService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System;
    using System.Linq;
    using System.Windows.Media;

    /// <summary>
    /// Provides services for listing system fonts.
    /// </summary>
    public static class FontService
    {
        private static readonly string[] invalidFonts =
        [
            "HoloLens MDL2 Assets",
            "MS Outlook",
            "MS Reference Specialty",
            "MT Extra",
            "Segoe Fluent Icons",
            "Segoe MDL2 Assets",
        ];

        /// <summary>
        /// Gets the default font family "Calibri".
        /// </summary>
        public static FontFamily DefaultFont => new ("Calibri");

        /// <summary>
        /// Gets the default font size of 20.
        /// </summary>
        public static double DefaultSize => 20;

        /// <summary>
        /// Lists all system font families ordered by their source name.
        /// </summary>
        /// <returns>An array of all system <see cref="FontFamily"/> objects ordered by their source name.</returns>
        public static FontFamily[] ListFonts()
        {
            return [.. Fonts.SystemFontFamilies.OrderBy(f => f.Source)];
        }

        /// <summary>
        /// Lists all system font families, excluding those specified as invalid, ordered by their source name.
        /// </summary>
        /// <returns>An array of valid system <see cref="FontFamily"/> objects ordered by their source name.</returns>
        public static FontFamily[] ListValidFonts()
        {
            return [.. Fonts.SystemFontFamilies.Where(x => !invalidFonts.Contains(x.Source)).OrderBy(y => y.Source)];
        }

        /// <summary>
        /// Returns a predefined list of commonly used font sizes.
        /// </summary>
        /// <returns>An array of double values representing commonly used font sizes.</returns>
        public static double[] ListFontSizes()
        {
            return [8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72];
        }
    }
}
