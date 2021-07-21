// <copyright file="HitDice.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Status
{
    public class HitDice
    {
        private int spentD6;
        private int spentD8;
        private int spentD10;
        private int spentD12;

        public HitDice()
        {
            this.TotalD6 = 0;
            this.TotalD8 = 0;
            this.TotalD10 = 0;
            this.TotalD12 = 0;
            this.spentD6 = 0;
            this.spentD8 = 0;
            this.spentD10 = 0;
            this.spentD12 = 0;
        }

        public int TotalD6 { get; set; }

        public int TotalD8 { get; set; }

        public int TotalD10 { get; set; }

        public int TotalD12 { get; set; }

        public int SpentD6
        {
            get => this.spentD6;
            set
            {
                if (value <= this.TotalD6)
                {
                    this.spentD6 = value;
                }
            }
        }

        public int SpentD8
        {
            get => this.spentD8;
            set
            {
                if (value <= this.TotalD8)
                {
                    this.spentD8 = value;
                }
            }
        }

        public int SpentD10
        {
            get => this.spentD10;
            set
            {
                if (value <= this.TotalD10)
                {
                    this.spentD10 = value;
                }
            }
        }

        public int SpentD12
        {
            get => this.spentD12;
            set
            {
                if (value <= this.TotalD12)
                {
                    this.spentD12 = value;
                }
            }
        }
    }
}
