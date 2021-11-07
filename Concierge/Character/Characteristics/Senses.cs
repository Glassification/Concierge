// <copyright file="Senses.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using Concierge.Character.Enums;
    using Concierge.Utility;
    using Newtonsoft.Json;

    public class Senses : ICopyable
    {
        public Senses()
        {
            this.InitiativeBonus = 0;
            this.PerceptionBonus = 0;
            this.BaseMovement = 0;
            this.Vision = VisionTypes.Normal;
        }

        public int InitiativeBonus { get; set; }

        public int PerceptionBonus { get; set; }

        public int BaseMovement { get; set; }

        public VisionTypes Vision { get; set; }

        [JsonIgnore]
        public int Movement => GetMovement(this.BaseMovement);

        public static int GetMovement(int baseMovement)
        {
            var conditions = Program.CcsFile.Character.Vitality.Conditions;

            if (conditions.Fatigued.Equals("Five") ||
                conditions.Grappled.Equals("Grappled") ||
                conditions.Restrained.Equals("Restrained"))
            {
                return 0;
            }
            else
            {
                if (Statuses.Conditions.Encumbrance.Equals("Encumbered"))
                {
                    baseMovement -= 10;
                }
                else if (Statuses.Conditions.Encumbrance.Equals("Heavily Encumbered"))
                {
                    baseMovement -= 20;
                }

                if (conditions.Fatigued.Equals("Two") ||
                    conditions.Fatigued.Equals("Three") ||
                    conditions.Fatigued.Equals("Four"))
                {
                    baseMovement /= 2;
                }

                return baseMovement;
            }
        }

        public ICopyable DeepCopy()
        {
            return new Senses()
            {
                InitiativeBonus = this.InitiativeBonus,
                PerceptionBonus = this.PerceptionBonus,
                BaseMovement = this.BaseMovement,
                Vision = this.Vision,
            };
        }
    }
}
