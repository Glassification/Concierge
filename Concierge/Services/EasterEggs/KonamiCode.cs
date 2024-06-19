// <copyright file="KonamiCode.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.EasterEggs
{
    using System.Windows.Input;

    using Concierge.Display.Utility;

    /// <summary>
    /// Represents the Konami code Easter egg.
    /// </summary>
    public sealed class KonamiCode : IEasterEgg
    {
        private static readonly Key[] sequence =
        [
            Key.Up,
            Key.Up,
            Key.Down,
            Key.Down,
            Key.Left,
            Key.Right,
            Key.Left,
            Key.Right,
            Key.B,
            Key.A,
        ];

        public int CodeIndex { get; private set; }

        public void CheckCode(Key key)
        {
            if (this.CodeIndex == sequence.Length)
            {
                this.CodeIndex = 0;
            }

            if (key == sequence[this.CodeIndex])
            {
                this.CodeIndex++;

                if (sequence.Length == this.CodeIndex)
                {
                    ConciergeWindowService.ShowWindow(typeof(KonamiCodeWindow));
                    this.CodeIndex = 0;
                }
            }
            else if (key == sequence[0])
            {
                this.CodeIndex = 1;

                if (sequence.Length == this.CodeIndex)
                {
                    ConciergeWindowService.ShowWindow(typeof(KonamiCodeWindow));
                    this.CodeIndex = 0;
                }
            }
            else
            {
                this.CodeIndex = 0;
            }
        }
    }
}
