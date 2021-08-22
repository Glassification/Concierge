// <copyright file="Details.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Characteristics
{
    using System.Collections.Generic;

    using Concierge.Character.Enums;
    using Newtonsoft.Json;

    public class Details
    {
        public Details()
        {
            this.Name = string.Empty;
            this.Race = string.Empty;
            this.Background = string.Empty;
            this.Alignment = string.Empty;
            this.Experience = string.Empty;
            this.Languages = new List<Language>();
            this.InitiativeBonus = 0;
            this.PerceptionBonus = 0;
            this.BaseMovement = 0;
            this.Vision = VisionTypes.Normal;
        }

        public string Name { get; set; }

        public string Race { get; set; }

        public string Background { get; set; }

        public string Alignment { get; set; }

        public string Experience { get; set; }

        public List<Language> Languages { get; set; }

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
                if (conditions.Encumbrance.Equals("Encumbered"))
                {
                    baseMovement -= 10;
                }
                else if (conditions.Encumbrance.Equals("Heavily Encumbered"))
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
    }
}
