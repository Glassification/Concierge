// <copyright file="SpellSlots.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Spellcasting
{
    using Concierge.Common;
    using Concierge.Leveling.Dtos.Definitions;

    public sealed class SpellSlots : ICopyable<SpellSlots>
    {
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

        public void LevelUp(SpellSlotDto spellSlotDto)
        {
            this.PactTotal += spellSlotDto.Pact;
            this.FirstTotal += spellSlotDto.First;
            this.SecondTotal += spellSlotDto.Second;
            this.ThirdTotal += spellSlotDto.Third;
            this.FourthTotal += spellSlotDto.Fourth;
            this.FifthTotal += spellSlotDto.Fifth;
            this.SixthTotal += spellSlotDto.Sixth;
            this.SeventhTotal += spellSlotDto.Seventh;
            this.EighthTotal += spellSlotDto.Eighth;
            this.NinethTotal += spellSlotDto.Nineth;

            this.Reset();
        }

        public int Increment(string name)
        {
            if (name.Contains("pact", System.StringComparison.InvariantCultureIgnoreCase) && this.PactUsed < this.PactTotal)
            {
                this.PactUsed++;
                return this.PactUsed;
            }

            if (name.Contains("first", System.StringComparison.InvariantCultureIgnoreCase) && this.FirstUsed < this.FirstTotal)
            {
                this.FirstUsed++;
                return this.FirstUsed;
            }

            if (name.Contains("second", System.StringComparison.InvariantCultureIgnoreCase) && this.SecondUsed < this.SecondTotal)
            {
                this.SecondUsed++;
                return this.SecondUsed;
            }

            if (name.Contains("Third", System.StringComparison.InvariantCultureIgnoreCase) && this.ThirdUsed < this.ThirdTotal)
            {
                this.ThirdUsed++;
                return this.ThirdUsed;
            }

            if (name.Contains("Fourth", System.StringComparison.InvariantCultureIgnoreCase) && this.FourthUsed < this.FourthTotal)
            {
                this.FourthUsed++;
                return this.FourthUsed;
            }

            if (name.Contains("Fifth", System.StringComparison.InvariantCultureIgnoreCase) && this.FifthUsed < this.FifthTotal)
            {
                this.FifthUsed++;
                return this.FifthUsed;
            }

            if (name.Contains("Sixth", System.StringComparison.InvariantCultureIgnoreCase) && this.SixthUsed < this.SixthTotal)
            {
                this.SixthUsed++;
                return this.SixthUsed;
            }

            if (name.Contains("Seventh", System.StringComparison.InvariantCultureIgnoreCase) && this.SeventhUsed < this.SeventhTotal)
            {
                this.SeventhUsed++;
                return this.SeventhUsed;
            }

            if (name.Contains("Eighth", System.StringComparison.InvariantCultureIgnoreCase) && this.EighthUsed < this.EighthTotal)
            {
                this.EighthUsed++;
                return this.EighthUsed;
            }

            if (name.Contains("Nineth", System.StringComparison.InvariantCultureIgnoreCase) && this.NinethUsed < this.NinethTotal)
            {
                this.NinethUsed++;
                return this.NinethUsed;
            }

            return 0;
        }
    }
}
