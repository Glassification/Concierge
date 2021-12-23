// <copyright file="Settings.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    using Concierge.Utility.Units.Enums;

    public class Settings
    {
        public Settings()
        {
        }

        public bool AutosaveEnabled { get; set; }

        public int AutosaveInterval { get; set; }

        public bool CheckVersion { get; set; }

        public bool MuteSounds { get; set; }

        public bool UseCoinWeight { get; set; }

        public bool UseEncumbrance { get; set; }

        public UnitTypes UnitOfMeasurement { get; set; }

        public bool AttemptToCenterWindows { get; set; }
    }
}
