// <copyright file="SpellSlotDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling.Dtos
{
    public sealed class SpellSlotDto
    {
        public SpellSlotDto(
            int known = 0,
            int slots = 0,
            int cantrip = 0,
            int pact = 0,
            int first = 0,
            int second = 0,
            int third = 0,
            int fourth = 0,
            int fifth = 0,
            int sixth = 0,
            int seventh = 0,
            int eighth = 0,
            int nineth = 0)
        {
            this.Known = known;
            this.Slots = slots;
            this.Cantrip = cantrip;
            this.Pact = pact;
            this.First = first;
            this.Second = second;
            this.Third = third;
            this.Fourth = fourth;
            this.Fifth = fifth;
            this.Sixth = sixth;
            this.Seventh = seventh;
            this.Eighth = eighth;
            this.Nineth = nineth;
        }

        public int Known { get; set; }

        public int Slots { get; set; }

        public int Cantrip { get; set; }

        public int Pact { get; set; }

        public int First { get; set; }

        public int Second { get; set; }

        public int Third { get; set; }

        public int Fourth { get; set; }

        public int Fifth { get; set; }

        public int Sixth { get; set; }

        public int Seventh { get; set; }

        public int Eighth { get; set; }

        public int Nineth { get; set; }
    }
}
