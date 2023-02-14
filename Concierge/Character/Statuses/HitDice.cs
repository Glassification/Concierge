// <copyright file="HitDice.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Statuses
{
    using Concierge.Character.Enums;
    using Concierge.Utility;

    public sealed class HitDice : ICopyable<HitDice>
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

        public HitDie Increment(string name)
        {
            if (name.Contains("d6", System.StringComparison.InvariantCultureIgnoreCase) && this.SpentD6 < this.TotalD6)
            {
                this.SpentD6++;
                return HitDie.D6;
            }

            if (name.Contains("d8", System.StringComparison.InvariantCultureIgnoreCase) && this.SpentD8 < this.TotalD8)
            {
                this.SpentD8++;
                return HitDie.D8;
            }

            if (name.Contains("d10", System.StringComparison.InvariantCultureIgnoreCase) && this.SpentD10 < this.TotalD10)
            {
                this.SpentD10++;
                return HitDie.D10;
            }

            if (name.Contains("d12", System.StringComparison.InvariantCultureIgnoreCase) && this.SpentD12 < this.TotalD12)
            {
                this.SpentD12++;
                return HitDie.D12;
            }

            return HitDie.None;
        }

        public HitDice DeepCopy()
        {
            return new HitDice()
            {
                TotalD6 = this.TotalD6,
                TotalD8 = this.TotalD8,
                TotalD10 = this.TotalD10,
                TotalD12 = this.TotalD12,
                SpentD6 = this.SpentD6,
                SpentD8 = this.SpentD8,
                SpentD10 = this.SpentD10,
                SpentD12 = this.SpentD12,
            };
        }
    }
}
