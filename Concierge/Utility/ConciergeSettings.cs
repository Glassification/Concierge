// <copyright file="ConciergeSettings.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using Concierge.Persistence;
    using Concierge.Utility.Dtos;

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
#if DEBUG
            IsDebug = true;
#else
            IsDebug = false;
#endif

            AutosaveEnabled = AppConfigReadWriter.Read(nameof(AutosaveEnabled), DefaultAutosaveEnabled);
            AutosaveInterval = AppConfigReadWriter.Read(nameof(AutosaveInterval), DefaultAutosaveInterval);
            CheckVersion = AppConfigReadWriter.Read(nameof(CheckVersion), DefaultCheckVersion);
            MuteSounds = AppConfigReadWriter.Read(nameof(MuteSounds), DefaultMuteSounds);
            UseCoinWeight = AppConfigReadWriter.Read(nameof(UseCoinWeight), DefaultUseCoinWeight);
            UseEncumbrance = AppConfigReadWriter.Read(nameof(UseEncumbrance), DefaultUseEncumbrance);
            DisplayWindowInCentre = AppConfigReadWriter.Read(nameof(DisplayWindowInCentre), DefaultDisplayWindowInCentre);
        }

        public static bool AutosaveEnabled { get; private set; }

        public static int AutosaveInterval { get; private set; }

        public static bool CheckVersion { get; private set; }

        public static bool IsDebug { get; private set; }

        public static bool MuteSounds { get; private set; }

        public static bool UseCoinWeight { get; private set; }

        public static bool UseEncumbrance { get; private set; }

        public static bool DisplayWindowInCentre { get; private set; }

        public static void UpdateSettings(ConciergeSettingsDto conciergeSettings)
        {
            AutosaveEnabled = conciergeSettings.AutosaveEnabled;
            AutosaveInterval = conciergeSettings.AutosaveInterval;
            CheckVersion = conciergeSettings.CheckVersion;
            MuteSounds = conciergeSettings.MuteSounds;
            UseCoinWeight = conciergeSettings.UseCoinWeight;
            UseEncumbrance = conciergeSettings.UseEncumbrance;
            DisplayWindowInCentre = conciergeSettings.DisplayWindowInCentre;

            // Can't change app.config during debug mode
            if (IsDebug)
            {
                return;
            }

            AppConfigReadWriter.Write(nameof(AutosaveEnabled), AutosaveEnabled.ToString());
            AppConfigReadWriter.Write(nameof(AutosaveInterval), AutosaveInterval.ToString());
            AppConfigReadWriter.Write(nameof(CheckVersion), CheckVersion.ToString());
            AppConfigReadWriter.Write(nameof(MuteSounds), MuteSounds.ToString());
            AppConfigReadWriter.Write(nameof(UseCoinWeight), UseCoinWeight.ToString());
            AppConfigReadWriter.Write(nameof(UseEncumbrance), UseEncumbrance.ToString());
            AppConfigReadWriter.Write(nameof(DisplayWindowInCentre), DisplayWindowInCentre.ToString());
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
            };
        }

        public static string GetCurrentState()
        {
            return $@"AutosaveEnabled:[{AutosaveEnabled}], 
                        AutosaveInterval:[{AutosaveInterval}],
                        IsDebug:[{IsDebug}],
                        MuteSounds:[{MuteSounds}],
                        UseCoinWeight:[{UseCoinWeight}],
                        UseEncumbrance:[{UseEncumbrance}]
                        DisplayWindowInCentre:[{DisplayWindowInCentre}]";
        }
    }
}
