// <copyright file="Senses.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Details
{
    using Concierge.Character.Enums;
    using Concierge.Common;

    public sealed class Senses : ICopyable<Senses>
    {
        public Senses()
        {
            this.InitiativeBonus = 0;
            this.PerceptionBonus = 0;
            this.MovementBonus = 0;
            this.BaseMovement = 30;
            this.Vision = VisionTypes.Normal;
        }

        public int InitiativeBonus { get; set; }

        public bool Inspiration { get; set; }

        public int PerceptionBonus { get; set; }

        public int BaseMovement { get; set; }

        public int MovementBonus { get; set; }

        public VisionTypes Vision { get; set; }

        public Senses DeepCopy()
        {
            return new Senses()
            {
                InitiativeBonus = this.InitiativeBonus,
                Inspiration = this.Inspiration,
                PerceptionBonus = this.PerceptionBonus,
                MovementBonus = this.MovementBonus,
                BaseMovement = this.BaseMovement,
                Vision = this.Vision,
            };
        }
    }
}
