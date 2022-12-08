// <copyright file="MARGINS.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Structs
{
    using System.Runtime.InteropServices;
    using System.Windows;

    [StructLayout(LayoutKind.Sequential)]
    public struct MARGINS
    {
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;

        public MARGINS(Thickness t)
        {
            this.Left = (int)t.Left;
            this.Right = (int)t.Right;
            this.Top = (int)t.Top;
            this.Bottom = (int)t.Bottom;
        }
    }
}
