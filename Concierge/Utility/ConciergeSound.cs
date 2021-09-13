// <copyright file="ConciergeSound.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System.Media;

    public static class ConciergeSound
    {
        private static int skipClickCount;

        static ConciergeSound()
        {
            HighPitchTapSound = new SoundPlayer(Properties.Resources.HighPitchTapSoundLoud);
            TapSound = new SoundPlayer(Properties.Resources.GenericTapSoundLoud);
            WarningSound = new SoundPlayer(Properties.Resources.GenericWarning);
            ConmanSong = new SoundPlayer(Properties.Resources.I_m_a_Conman);
        }

        private static SoundPlayer HighPitchTapSound { get; set; }

        private static SoundPlayer TapSound { get; set; }

        private static SoundPlayer WarningSound { get; set; }

        private static SoundPlayer ConmanSong { get; set; }

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
            if (ConciergeSettings.MuteSounds || SkipClick)
            {
                return;
            }

            TapSound.Play();
        }

        public static void UpdateValue()
        {
            if (ConciergeSettings.MuteSounds)
            {
                return;
            }

            HighPitchTapSound.Play();
        }

        public static void Warning()
        {
            if (ConciergeSettings.MuteSounds)
            {
                return;
            }

            WarningSound.Play();
        }

        public static void KonamiCodeSound()
        {
            ConmanSong.Play();
        }
    }
}
