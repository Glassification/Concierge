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
            HighPitchTapSound = new SoundPlayer(Properties.Resources.HighPitchTapSound);
            TapSound = new SoundPlayer(Properties.Resources.GenericTapSound);
            WarningSound = new SoundPlayer(Properties.Resources.GenericWarning);
        }

        private static SoundPlayer HighPitchTapSound { get; set; }

        private static SoundPlayer TapSound { get; set; }

        private static SoundPlayer WarningSound { get; set; }

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

        public static void TapNavigation()
        {
            if (Program.CcsFile.MuteSound || SkipClick)
            {
                return;
            }

            TapSound.Play();
        }

        public static void UpdateValue()
        {
            if (Program.CcsFile.MuteSound)
            {
                return;
            }

            HighPitchTapSound.Play();
        }

        public static void Warning()
        {
            if (Program.CcsFile.MuteSound)
            {
                return;
            }

            WarningSound.Play();
        }
    }
}
