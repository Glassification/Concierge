// <copyright file="Details.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Characters.Collections;
    using Concierge.Characters.Enums;

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

        public int Movement
        {
            get
            {
                int newMovement = this.BaseMovement;

                if (Program.Character.Vitality.Conditions.Fatigued.Equals("Five") ||
                    Program.Character.Vitality.Conditions.Grappled.Equals("Grappled") ||
                    Program.Character.Vitality.Conditions.Restrained.Equals("Restrained"))
                {
                    return 0;
                }
                else
                {
                    if (Program.Character.Vitality.Conditions.Encumbrance.Equals("Encumbered"))
                    {
                        newMovement -= 10;
                    }
                    else if (Program.Character.Vitality.Conditions.Encumbrance.Equals("Heavily Encumbered"))
                    {
                        newMovement -= 20;
                    }

                    if (Program.Character.Vitality.Conditions.Fatigued.Equals("Two") ||
                        Program.Character.Vitality.Conditions.Fatigued.Equals("Three") ||
                        Program.Character.Vitality.Conditions.Fatigued.Equals("Four"))
                    {
                        newMovement /= 2;
                    }

                    return newMovement;
                }
            }
        }

        public Language GetLanguageById(Guid id)
        {
            return this.Languages.Single(x => x.ID.Equals(id));
        }
    }
}
