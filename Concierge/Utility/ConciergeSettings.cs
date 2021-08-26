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

        static ConciergeSettings()
        {
#if DEBUG
            IsDebug = true;
#else
            IsDebug = false;
#endif

            AutosaveEnabled = AppConfigReadWriter.GetBoolSetting(nameof(AutosaveEnabled), DefaultAutosaveEnabled);
            AutosaveInterval = AppConfigReadWriter.GetIntSetting(nameof(AutosaveInterval), DefaultAutosaveInterval);
            CheckVersion = AppConfigReadWriter.GetBoolSetting(nameof(CheckVersion), DefaultCheckVersion);
            MuteSounds = AppConfigReadWriter.GetBoolSetting(nameof(MuteSounds), DefaultMuteSounds);
            UseCoinWeight = AppConfigReadWriter.GetBoolSetting(nameof(UseCoinWeight), DefaultUseCoinWeight);
            UseEncumbrance = AppConfigReadWriter.GetBoolSetting(nameof(UseEncumbrance), DefaultUseEncumbrance);
        }

        public static bool AutosaveEnabled { get; private set; }

        public static int AutosaveInterval { get; private set; }

        public static bool CheckVersion { get; private set; }

        public static bool IsDebug { get; private set; }

        public static bool MuteSounds { get; private set; }

        public static bool UseCoinWeight { get; private set; }

        public static bool UseEncumbrance { get; private set; }

        public static void UpdateSettings(ConciergeSettingsDto conciergeSettings)
        {
            AutosaveEnabled = conciergeSettings.AutosaveEnabled;
            AutosaveInterval = conciergeSettings.AutosaveInterval;
            CheckVersion = conciergeSettings.CheckVersion;
            MuteSounds = conciergeSettings.MuteSounds;
            UseCoinWeight = conciergeSettings.UseCoinWeight;
            UseEncumbrance = conciergeSettings.UseEncumbrance;

            // Can't change app.config during debug mode
            if (IsDebug)
            {
                return;
            }

            AppConfigReadWriter.SaveSetting(nameof(AutosaveEnabled), AutosaveEnabled.ToString());
            AppConfigReadWriter.SaveSetting(nameof(AutosaveInterval), AutosaveInterval.ToString());
            AppConfigReadWriter.SaveSetting(nameof(CheckVersion), CheckVersion.ToString());
            AppConfigReadWriter.SaveSetting(nameof(MuteSounds), MuteSounds.ToString());
            AppConfigReadWriter.SaveSetting(nameof(UseCoinWeight), UseCoinWeight.ToString());
            AppConfigReadWriter.SaveSetting(nameof(UseEncumbrance), UseEncumbrance.ToString());
        }

        public static string GetCurrentState()
        {
            return $@"AutosaveEnabled:[{AutosaveEnabled}], 
                        AutosaveInterval:[{AutosaveInterval}],
                        IsDebug:[{IsDebug}],
                        MuteSounds:[{MuteSounds}],
                        UseCoinWeight:[{UseCoinWeight}],
                        UseEncumbrance:[{UseEncumbrance}]";
        }
    }
}
