// <copyright file="ResolutionScaling.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common
{
    using System.Reflection;
    using System.Windows;

    /// <summary>
    /// Provides resolution scaling factors based on the current DPI (dots per inch).
    /// </summary>
    public static class ResolutionScaling
    {
        /// <summary>
        /// Gets the current DPI (dots per inch).
        /// </summary>
        public static int Dpi
        {
            get
            {
                var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);
                return (int?)dpiYProperty?.GetValue(null, null) ?? 96;
            }
        }

        /// <summary>
        /// Gets the DPI scaling factor for UI elements.
        /// </summary>
        public static double DpiFactor
        {
            get
            {
                return Dpi switch
                {
                    96 => 1.25,
                    120 => 1,
                    144 => 0.75,
                    168 => 0.50,
                    _ => 1,
                };
            }
        }

        /// <summary>
        /// Gets the DPI scaling factor for images.
        /// </summary>
        public static double ImageFactor
        {
            get
            {
                return Dpi switch
                {
                    96 => 1,
                    120 => 1.25,
                    144 => 1.50,
                    168 => 1.75,
                    _ => 1,
                };
            }
        }
    }
}
