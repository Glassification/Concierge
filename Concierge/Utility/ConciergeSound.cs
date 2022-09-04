// <copyright file="ConciergeSound.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System;
    using System.Media;

    using Concierge.Configuration;

    public static class ConciergeSound
    {
        private static int skipClickCount;

        static ConciergeSound()
        {
            try
            {
                HighPitchTapSound = new SoundPlayer(Properties.Resources.HighPitchTapSoundLoud);
                TapSound = new SoundPlayer(Properties.Resources.GenericTapSoundLoud);
                WarningSound = new SoundPlayer(Properties.Resources.GenericWarning);
            }
            catch (Exception)
            {
                // We are currently handling this error. Just swallow it.
            }
        }

        private static SoundPlayer? HighPitchTapSound { get; set; }

        private static SoundPlayer? TapSound { get; set; }

        private static SoundPlayer? WarningSound { get; set; }

        private static bool SkipClick
        {
            get
            {
                if (skipClickCount == 0)
                {
                    skipClickCount++;
                    return true;
                }

                return false;
            }
        }

        public static void ResetSkipClick()
        {
            skipClickCount = 0;
        }

        public static void TapNavigation()
        {
            if (AppSettingsManager.UserSettings.MuteSounds || SkipClick)
            {
                return;
            }

            TapSound?.Play();
        }

        public static void UpdateValue()
        {
            if (AppSettingsManager.UserSettings.MuteSounds)
            {
                return;
            }

            HighPitchTapSound?.Play();
        }

        public static void Warning()
        {
            if (AppSettingsManager.UserSettings.MuteSounds)
            {
                return;
            }

            WarningSound?.Play();
        }
    }
}
