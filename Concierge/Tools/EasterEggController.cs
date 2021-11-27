// <copyright file="EasterEggController.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools
{
    using System.Windows.Input;

    using Concierge.Interfaces.UtilityInterface;

    public static class EasterEggController
    {
        private static readonly Key[] KonamiCodeSequence = new Key[] { Key.Up, Key.Up, Key.Down, Key.Down, Key.Left, Key.Right, Key.Left, Key.Right, Key.B, Key.A };

        private static readonly KonamiCodeWindow konamiCodeWindow = new ();

        private static int KonamiCodeIndex { get; set; }

        private static bool IsKonamiCodeValid => KonamiCodeSequence.Length == KonamiCodeIndex;

        public static void KonamiCode(Key key)
        {
            if (key == KonamiCodeSequence[KonamiCodeIndex])
            {
                KonamiCodeIndex++;

                if (IsKonamiCodeValid)
                {
                    konamiCodeWindow.ShowWindow();
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
