// <copyright file="SpellSlots.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Spellcasting
{
    using Concierge.Leveling.Dtos;
    using Concierge.Utility;

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
    }
}
