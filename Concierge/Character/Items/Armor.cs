// <copyright file="Armor.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Items
{
    using System;

    using Concierge.Character.Enums;
    using Concierge.Primatives;
    using Concierge.Utility;
    using Newtonsoft.Json;

    public class Armor : ICopyable<Armor>
    {
        public Armor()
        {
            this.Equiped = string.Empty;
            this.Type = ArmorType.None;
            this.ArmorClass = 0;
            this.Strength = 0;
            this.Weight = ConciergeDouble.Empty;
            this.Stealth = ArmorStealth.Normal;
            this.Shield = string.Empty;
            this.ShieldArmorClass = 0;
            this.ShieldWeight = ConciergeDouble.Empty;
            this.MiscArmorClass = 0;
            this.MagicArmorClass = 0;
        }

        public string Equiped { get; set; }

        public ArmorType Type { get; set; }

        public int ArmorClass { get; set; }

        public int Strength { get; set; }

        public ConciergeDouble Weight { get; set; }

        public ArmorStealth Stealth { get; set; }

        public string Shield { get; set; }

        public int ShieldArmorClass { get; set; }

        public ConciergeDouble ShieldWeight { get; set; }

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

        public Armor DeepCopy()
        {
            return new Armor()
            {
                Equiped = this.Equiped,
                Type = this.Type,
                ArmorClass = this.ArmorClass,
                Strength = this.Strength,
                Weight = this.Weight.DeepCopy(),
                Stealth = this.Stealth,
                Shield = this.Shield,
                ShieldArmorClass = this.ShieldArmorClass,
                ShieldWeight = this.ShieldWeight.DeepCopy(),
                MiscArmorClass = this.MiscArmorClass,
                MagicArmorClass = this.MagicArmorClass,
            };
        }
    }
}
