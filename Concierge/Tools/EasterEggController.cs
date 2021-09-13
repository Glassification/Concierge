// <copyright file="EasterEggController.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools
{
    using System.Windows.Input;

    using Concierge.Utility;

    public static class EasterEggController
    {
        private static readonly Key[] KonamiCodeSequence = new Key[] { Key.Up, Key.Up, Key.Down, Key.Down, Key.Left, Key.Right, Key.Left, Key.Right, Key.A, Key.B };

        private static int KonamiCodeIndex { get; set; }

        public static void KonamiCode(Key key)
        {
            if (key == KonamiCodeSequence[KonamiCodeIndex])
            {
                KonamiCodeIndex++;

                if (KonamiCodeSequence.Length == KonamiCodeIndex)
                {
                    ConciergeSound.KonamiCodeSound();
                    KonamiCodeIndex = 0;
                }
            }
            else
            {
                KonamiCodeIndex = 0;
            }
        }
    }
}
