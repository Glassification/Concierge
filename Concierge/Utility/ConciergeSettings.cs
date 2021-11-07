// <copyright file="ConciergeSettings.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System;

    using Concierge.Persistence;
    using Concierge.Utility.Dtos;
    using Concierge.Utility.Enums;

    public static class ConciergeSettings
    {
        private const bool DefaultAutosaveEnabled = false;
        private const int DefaultAutosaveInterval = 1;
        private const bool DefaultCheckVersion = true;
        private const bool DefaultMuteSounds = false;
        private const bool DefaultUseCoinWeight = false;
        private const bool DefaultUseEncumbrance = false;
        private const bool DefaultDisplayWindowInCentre = false;

        static ConciergeSettings()
        {
            AutosaveEnabled = AppConfigReadWriter.Read(nameof(AutosaveEnabled), DefaultAutosaveEnabled);
            AutosaveInterval = AppConfigReadWriter.Read(nameof(AutosaveInterval), DefaultAutosaveInterval);
            CheckVersion = AppConfigReadWriter.Read(nameof(CheckVersion), DefaultCheckVersion);
            MuteSounds = AppConfigReadWriter.Read(nameof(MuteSounds), DefaultMuteSounds);
            UseCoinWeight = AppConfigReadWriter.Read(nameof(UseCoinWeight), DefaultUseCoinWeight);
            UseEncumbrance = AppConfigReadWriter.Read(nameof(UseEncumbrance), DefaultUseEncumbrance);
            UnitOfMeasurement = AppConfigReadWriter.Read<UnitTypes>(nameof(UnitOfMeasurement));
            DisplayWindowInCentre = AppConfigReadWriter.Read(nameof(DisplayWindowInCentre), DefaultDisplayWindowInCentre);
        }

        public delegate void UnitsChangedEventHandler(object sender, EventArgs e);

        public static event UnitsChangedEventHandler UnitsChanged;

        public static bool AutosaveEnabled { get; private set; }

        public static int AutosaveInterval { get; private set; }

        public static bool CheckVersion { get; private set; }

        public static bool MuteSounds { get; private set; }

        public static bool UseCoinWeight { get; private set; }

        public static bool UseEncumbrance { get; private set; }

        public static UnitTypes UnitOfMeasurement { get; private set; }

        public static bool DisplayWindowInCentre { get; private set; }

        public static void UpdateSettings(ConciergeSettingsDto conciergeSettings)
        {
            if (UnitOfMeasurement != conciergeSettings.UnitOfMeasurement)
            {
                UnitsChanged?.Invoke(conciergeSettings, new EventArgs());
            }

            AutosaveEnabled = conciergeSettings.AutosaveEnabled;
            AutosaveInterval = conciergeSettings.AutosaveInterval;
            CheckVersion = conciergeSettings.CheckVersion;
            MuteSounds = conciergeSettings.MuteSounds;
            UseCoinWeight = conciergeSettings.UseCoinWeight;
            UseEncumbrance = conciergeSettings.UseEncumbrance;
            UnitOfMeasurement = conciergeSettings.UnitOfMeasurement;
            DisplayWindowInCentre = conciergeSettings.DisplayWindowInCentre;

            // Can't change app.config during debug mode
            if (Program.IsDebug)
            {
                return;
            }

            AppConfigReadWriter.Write(nameof(AutosaveEnabled), AutosaveEnabled.ToString());
            AppConfigReadWriter.Write(nameof(AutosaveInterval), AutosaveInterval.ToString());
            AppConfigReadWriter.Write(nameof(CheckVersion), CheckVersion.ToString());
            AppConfigReadWriter.Write(nameof(MuteSounds), MuteSounds.ToString());
            AppConfigReadWriter.Write(nameof(UseCoinWeight), UseCoinWeight.ToString());
            AppConfigReadWriter.Write(nameof(UseEncumbrance), UseEncumbrance.ToString());
            AppConfigReadWriter.Write(nameof(UnitOfMeasurement), UnitOfMeasurement.ToString());
            AppConfigReadWriter.Write(nameof(DisplayWindowInCentre), DisplayWindowInCentre.ToString());
        }

        public static void RefreshUnits()
        {
            UnitsChanged?.Invoke(ToConciergeSettingsDto(), new EventArgs());
        }

        public static ConciergeSettingsDto ToConciergeSettingsDto()
        {
            return new ConciergeSettingsDto()
            {
                AutosaveEnabled = AutosaveEnabled,
                AutosaveInterval = AutosaveInterval,
                CheckVersion = CheckVersion,
                MuteSounds = MuteSounds,
                UseCoinWeight = UseCoinWeight,
                UseEncumbrance = UseEncumbrance,
                UnitOfMeasurement = UnitOfMeasurement,
            };
        }

        public static string GetCurrentState()
        {
            return $@"AutosaveEnabled:[{AutosaveEnabled}], 
                        AutosaveInterval:[{AutosaveInterval}],
                        IsDebug:[{Program.IsDebug}],
                        MuteSounds:[{MuteSounds}],
                        UseCoinWeight:[{UseCoinWeight}],
                        UseEncumbrance:[{UseEncumbrance}]
                        UnitOfMeasurement:[{UnitOfMeasurement}]
                        DisplayWindowInCentre:[{DisplayWindowInCentre}]";
        }
    }
}
