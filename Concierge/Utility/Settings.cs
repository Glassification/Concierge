// <copyright file="Settings.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    public static class Settings
    {
        static Settings()
        {
            Default();
        }

        public static bool AutosaveEnable { get; set; }

        public static int AutosaveInterval { get; set; }

        public static bool UseCoinWeight { get; set; }

        public static bool UseEncumbrance { get; set; }

        public static void Default()
        {
            AutosaveEnable = false;
            AutosaveInterval = 1;
            UseCoinWeight = false;
            UseEncumbrance = false;
        }
    }
}
