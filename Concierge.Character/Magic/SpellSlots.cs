﻿// <copyright file="SpellSlots.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Magic
{
    using Concierge.Common;

    /// <summary>
    /// Represents the spell slots available for casting spells.
    /// </summary>
    public sealed class SpellSlots : ICopyable<SpellSlots>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpellSlots"/> class with default values.
        /// </summary>
        public SpellSlots()
        {
            this.PactTotal = 0;
            this.FirstTotal = 0;
            this.SecondTotal = 0;
            this.ThirdTotal = 0;
            this.FourthTotal = 0;
            this.FifthTotal = 0;
            this.SixthTotal = 0;
            this.SeventhTotal = 0;
            this.EighthTotal = 0;
            this.NinethTotal = 0;

            this.Reset();
        }

        public int PactTotal { get; set; }

        public int FirstTotal { get; set; }

        public int SecondTotal { get; set; }

        public int ThirdTotal { get; set; }

        public int FourthTotal { get; set; }

        public int FifthTotal { get; set; }

        public int SixthTotal { get; set; }

        public int SeventhTotal { get; set; }

        public int EighthTotal { get; set; }

        public int NinethTotal { get; set; }

        public int PactUsed { get; set; }

        public int FirstUsed { get; set; }

        public int SecondUsed { get; set; }

        public int ThirdUsed { get; set; }

        public int FourthUsed { get; set; }

        public int FifthUsed { get; set; }

        public int SixthUsed { get; set; }

        public int SeventhUsed { get; set; }

        public int EighthUsed { get; set; }

        public int NinethUsed { get; set; }

        /// <summary>
        /// Creates a deep copy of the spell slots.
        /// </summary>
        /// <returns>A deep copy of the <see cref="SpellSlots"/>.</returns>
        public SpellSlots DeepCopy()
        {
            return new SpellSlots()
            {
                PactTotal = this.PactTotal,
                FirstTotal = this.FirstTotal,
                SecondTotal = this.SecondTotal,
                ThirdTotal = this.ThirdTotal,
                FourthTotal = this.FourthTotal,
                FifthTotal = this.FifthTotal,
                SixthTotal = this.SixthTotal,
                SeventhTotal = this.SeventhTotal,
                EighthTotal = this.EighthTotal,
                NinethTotal = this.NinethTotal,
                PactUsed = this.PactUsed,
                FirstUsed = this.FirstUsed,
                SecondUsed = this.SecondUsed,
                ThirdUsed = this.ThirdUsed,
                FourthUsed = this.FourthUsed,
                FifthUsed = this.FifthUsed,
                SixthUsed = this.SixthUsed,
                SeventhUsed = this.SeventhUsed,
                EighthUsed = this.EighthUsed,
                NinethUsed = this.NinethUsed,
            };
        }

        /// <summary>
        /// Resets all used spell slots to zero.
        /// </summary>
        public void Reset()
        {
            this.PactUsed = 0;
            this.FirstUsed = 0;
            this.SecondUsed = 0;
            this.ThirdUsed = 0;
            this.FourthUsed = 0;
            this.FifthUsed = 0;
            this.SixthUsed = 0;
            this.SeventhUsed = 0;
            this.EighthUsed = 0;
            this.NinethUsed = 0;
        }

        /// <summary>
        /// Increments the used spell slots based on the spell slot name.
        /// </summary>
        /// <param name="name">The name of the spell slot.</param>
        /// <returns>A tuple containing the updated used and total spell slots.</returns>
        public (int used, int total) Increment(string name)
        {
            if (name.Contains("pact", System.StringComparison.InvariantCultureIgnoreCase) && this.PactUsed < this.PactTotal)
            {
                this.PactUsed++;
                return (this.PactUsed, this.PactTotal);
            }

            if (name.Contains("first", System.StringComparison.InvariantCultureIgnoreCase) && this.FirstUsed < this.FirstTotal)
            {
                this.FirstUsed++;
                return (this.FirstUsed, this.FirstTotal);
            }

            if (name.Contains("second", System.StringComparison.InvariantCultureIgnoreCase) && this.SecondUsed < this.SecondTotal)
            {
                this.SecondUsed++;
                return (this.SecondUsed, this.SecondTotal);
            }

            if (name.Contains("Third", System.StringComparison.InvariantCultureIgnoreCase) && this.ThirdUsed < this.ThirdTotal)
            {
                this.ThirdUsed++;
                return (this.ThirdUsed, this.ThirdTotal);
            }

            if (name.Contains("Fourth", System.StringComparison.InvariantCultureIgnoreCase) && this.FourthUsed < this.FourthTotal)
            {
                this.FourthUsed++;
                return (this.FourthUsed, this.FourthTotal);
            }

            if (name.Contains("Fifth", System.StringComparison.InvariantCultureIgnoreCase) && this.FifthUsed < this.FifthTotal)
            {
                this.FifthUsed++;
                return (this.FifthUsed, this.FifthTotal);
            }

            if (name.Contains("Sixth", System.StringComparison.InvariantCultureIgnoreCase) && this.SixthUsed < this.SixthTotal)
            {
                this.SixthUsed++;
                return (this.SixthUsed, this.SixthTotal);
            }

            if (name.Contains("Seventh", System.StringComparison.InvariantCultureIgnoreCase) && this.SeventhUsed < this.SeventhTotal)
            {
                this.SeventhUsed++;
                return (this.SeventhUsed, this.SeventhTotal);
            }

            if (name.Contains("Eighth", System.StringComparison.InvariantCultureIgnoreCase) && this.EighthUsed < this.EighthTotal)
            {
                this.EighthUsed++;
                return (this.EighthUsed, this.EighthTotal);
            }

            if (name.Contains("Ninth", System.StringComparison.InvariantCultureIgnoreCase) && this.NinethUsed < this.NinethTotal)
            {
                this.NinethUsed++;
                return (this.NinethUsed, this.NinethTotal);
            }

            return (0, 0);
        }
    }
}