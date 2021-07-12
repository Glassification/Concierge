// <copyright file="Armor.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Characters
{
    using System;

    using Concierge.Characters.Enums;
    using Concierge.Utility;
    using Newtonsoft.Json;

    public class Armor
    {
        public Armor()
        {
            this.Equiped = string.Empty;
            this.Type = ArmorType.None;
            this.ArmorClass = 0;
            this.Strength = 0;
            this.Weight = 0.0;
            this.Stealth = ArmorStealth.Normal;
            this.Shield = string.Empty;
            this.ShieldArmorClass = 0;
            this.ShieldWeight = 0.0;
            this.MiscArmorClass = 0;
            this.MagicArmorClass = 0;
        }

        public string Equiped { get; set; }

        public ArmorType Type { get; set; }

        public int ArmorClass { get; set; }

        public int Strength { get; set; }

        public double Weight { get; set; }

        public ArmorStealth Stealth { get; set; }

        public string Shield { get; set; }

        public int ShieldArmorClass { get; set; }

        public double ShieldWeight { get; set; }

        public int MiscArmorClass { get; set; }

        public int MagicArmorClass { get; set; }

        [JsonIgnore]
        public int TotalArmorClass
        {
            get
            {
                var ac = this.ArmorClass + this.ShieldArmorClass + this.MiscArmorClass + this.MagicArmorClass;

                switch (this.Type)
                {
                    default:
                    case ArmorType.None:
                        if (this.ArmorClass == 0)
                        {
                            ac += 10;
                        }

                        break;
                    case ArmorType.Light:
                        ac += Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Dexterity);
                        break;
                    case ArmorType.Medium:
                        ac += Math.Min(2, Utilities.CalculateBonus(Program.CcsFile.Character.Attributes.Dexterity));
                        break;
                    case ArmorType.Heavy:
                    case ArmorType.Massive:
                        break;
                }

                return ac;
            }
        }
    }
}
