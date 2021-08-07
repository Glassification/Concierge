// <copyright file="ConciergeSound.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System.Media;

    public static class ConciergeSound
    {
        private static int skipClickCount = 0;

        static ConciergeSound()
        {
            ButtonSound = new SoundPlayer(Properties.Resources.GenericTapSound);
            WarningSound = new SoundPlayer(Properties.Resources.GenericWarning);
        }

        private static SoundPlayer ButtonSound { get; set; }

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

        public static void ButtonClick()
        {
            if (Program.CcsFile.MuteSound || SkipClick)
            {
                return;
            }

            ButtonSound.Play();
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
