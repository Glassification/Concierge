// <copyright file="Program.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge
{
    using Concierge.Characters;
    using Concierge.Persistence;

    public static class Program
    {
        static Program()
        {
            Character = new Character();
            CcsFile = null;
            Modified = true;
            Typing = false;
        }

        public static Character Character { get; private set; }

        public static CcsFile CcsFile { get; set; }

        public static bool Typing { get; set; }

        public static bool Modified { get; set; }
    }
}
