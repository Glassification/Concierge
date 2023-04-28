// <copyright file="Senses.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using Concierge.Character.Enums;
    using Concierge.Common;
    using Newtonsoft.Json;

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

        [JsonIgnore]
        public int Movement => GetMovement(this.BaseMovement + this.MovementBonus);

        public static int GetMovement(int baseMovement)
        {
            var conditions = Program.CcsFile.Character.Vitality.Conditions;

            if (conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Five ||
                conditions.Grappled.Afflicted ||
                conditions.Restrained.Afflicted)
            {
                return 0;
            }
            else
            {
                if (conditions.Encumbered.EncumbranceLevel == EncumbranceLevel.Encumbered)
                {
                    baseMovement -= 10;
                }
                else if (conditions.Encumbered.EncumbranceLevel == EncumbranceLevel.HeavilyEncumbered)
                {
                    baseMovement -= 20;
                }

                if (conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Two ||
                    conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Three ||
                    conditions.Fatigued.ExhaustionLevel == ExhaustionLevel.Four)
                {
                    baseMovement /= 2;
                }

                return baseMovement;
            }
        }

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
