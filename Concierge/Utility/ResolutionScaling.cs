// <copyright file="ResolutionScaling.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System.Reflection;
    using System.Windows;

    public static class ResolutionScaling
    {
        public static double DpiFactor
        {
            get
            {
                var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);
                var dpiY = (int?)dpiYProperty?.GetValue(null, null) ?? 96;

                return dpiY switch
                {
                    96 => 1.25,
                    120 => 1,
                    144 => 0.75,
                    168 => 0.50,
                    _ => 1,
                };
            }
        }
    }
}
