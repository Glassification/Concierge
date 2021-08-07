// <copyright file="HitDice.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Status
{
    public class HitDice
    {
        public HitDice()
        {
            this.TotalD6 = 0;
            this.TotalD8 = 0;
            this.TotalD10 = 0;
            this.TotalD12 = 0;
            this.SpentD6 = 0;
            this.SpentD8 = 0;
            this.SpentD10 = 0;
            this.SpentD12 = 0;
        }

        public int TotalD6 { get; set; }

        public int TotalD8 { get; set; }

        public int TotalD10 { get; set; }

        public int TotalD12 { get; set; }

        public int SpentD6 { get; set; }

        public int SpentD8 { get; set; }

        public int SpentD10 { get; set; }

        public int SpentD12 { get; set; }
    }
}
