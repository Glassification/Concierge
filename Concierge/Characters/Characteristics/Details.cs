// <copyright file="Details.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters.Characteristics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Characters.Enums;
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
        public int Movement
        {
            get
            {
                int newMovement = this.BaseMovement;

                if (Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Five") ||
                    Program.CcsFile.Character.Vitality.Conditions.Grappled.Equals("Grappled") ||
                    Program.CcsFile.Character.Vitality.Conditions.Restrained.Equals("Restrained"))
                {
                    return 0;
                }
                else
                {
                    if (Program.CcsFile.Character.Vitality.Conditions.Encumbrance.Equals("Encumbered"))
                    {
                        newMovement -= 10;
                    }
                    else if (Program.CcsFile.Character.Vitality.Conditions.Encumbrance.Equals("Heavily Encumbered"))
                    {
                        newMovement -= 20;
                    }

                    if (Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Two") ||
                        Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Three") ||
                        Program.CcsFile.Character.Vitality.Conditions.Fatigued.Equals("Four"))
                    {
                        newMovement /= 2;
                    }

                    return newMovement;
                }
            }
        }

        public Language GetLanguageById(Guid id)
        {
            return this.Languages.Single(x => x.Id.Equals(id));
        }
    }
}
