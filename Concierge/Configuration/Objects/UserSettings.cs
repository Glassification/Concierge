﻿// <copyright file="UserSettings.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    using Concierge.Utility.Units.Enums;

    public class UserSettings
    {
        public UserSettings()
        {
        }

        public bool AttemptToCenterWindows { get; set; }

        public bool AutosaveEnabled { get; set; }

        public int AutosaveInterval { get; set; }

        public bool CheckVersion { get; set; }

        public bool MuteSounds { get; set; }

        public UnitTypes UnitOfMeasurement { get; set; }

        public bool UseCoinWeight { get; set; }

        public bool UseEncumbrance { get; set; }
    }
}