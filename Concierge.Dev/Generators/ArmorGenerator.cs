// <copyright file="ArmorGenerator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.DevTools.Generators
{
    using System.Collections.Generic;
    using System.IO;
    using Concierge.Character.Enums;
    using Concierge.Character.Equipable;
    using Concierge.Common.Enums;
    using Concierge.Data;
    using Newtonsoft.Json;

    public static class ArmorGenerator
    {
        public static void Generate(string file)
        {
            var items = new List<Armor>()
            {
                new Armor()
                {
                    Name = "Padded",
                    Ac = 11,
                    Stealth = ArmorStealth.Disadvantage,
                    Weight = new UnitDouble(8, UnitTypes.Imperial, Measurements.Weight),
                    Strength = 0,
                    Type = ArmorType.Light,
                    IsCustom = true,
                },
                new Armor()
                {
                    Name = "Leather",
                    Ac = 11,
                    Stealth = ArmorStealth.Normal,
                    Weight = new UnitDouble(10, UnitTypes.Imperial, Measurements.Weight),
                    Strength = 0,
                    Type = ArmorType.Light,
                    IsCustom = true,
                },
                new Armor()
                {
                    Name = "Studded Leather",
                    Ac = 12,
                    Stealth = ArmorStealth.Normal,
                    Weight = new UnitDouble(13, UnitTypes.Imperial, Measurements.Weight),
                    Strength = 0,
                    Type = ArmorType.Light,
                    IsCustom = true,
                },
                new Armor()
                {
                    Name = "Hide",
                    Ac = 12,
                    Stealth = ArmorStealth.Normal,
                    Weight = new UnitDouble(12, UnitTypes.Imperial, Measurements.Weight),
                    Strength = 0,
                    Type = ArmorType.Medium,
                    IsCustom = true,
                },
                new Armor()
                {
                    Name = "Chain Shirt",
                    Ac = 13,
                    Stealth = ArmorStealth.Normal,
                    Weight = new UnitDouble(20, UnitTypes.Imperial, Measurements.Weight),
                    Strength = 0,
                    Type = ArmorType.Medium,
                    IsCustom = true,
                },
                new Armor()
                {
                    Name = "Scale Mail",
                    Ac = 14,
                    Stealth = ArmorStealth.Disadvantage,
                    Weight = new UnitDouble(45, UnitTypes.Imperial, Measurements.Weight),
                    Strength = 0,
                    Type = ArmorType.Medium,
                    IsCustom = true,
                },
                new Armor()
                {
                    Name = "Breastplate",
                    Ac = 14,
                    Stealth = ArmorStealth.Normal,
                    Weight = new UnitDouble(20, UnitTypes.Imperial, Measurements.Weight),
                    Strength = 0,
                    Type = ArmorType.Medium,
                    IsCustom = true,
                },
                new Armor()
                {
                    Name = "Half Plate",
                    Ac = 15,
                    Stealth = ArmorStealth.Disadvantage,
                    Weight = new UnitDouble(40, UnitTypes.Imperial, Measurements.Weight),
                    Strength = 0,
                    Type = ArmorType.Medium,
                    IsCustom = true,
                },
                new Armor()
                {
                    Name = "Ring Mail",
                    Ac = 14,
                    Stealth = ArmorStealth.Disadvantage,
                    Weight = new UnitDouble(40, UnitTypes.Imperial, Measurements.Weight),
                    Strength = 0,
                    Type = ArmorType.Heavy,
                    IsCustom = true,
                },
                new Armor()
                {
                    Name = "Chain Mail",
                    Ac = 16,
                    Stealth = ArmorStealth.Disadvantage,
                    Weight = new UnitDouble(55, UnitTypes.Imperial, Measurements.Weight),
                    Strength = 13,
                    Type = ArmorType.Heavy,
                    IsCustom = true,
                },
                new Armor()
                {
                    Name = "Splint",
                    Ac = 17,
                    Stealth = ArmorStealth.Disadvantage,
                    Weight = new UnitDouble(60, UnitTypes.Imperial, Measurements.Weight),
                    Strength = 15,
                    Type = ArmorType.Heavy,
                    IsCustom = true,
                },
                new Armor()
                {
                    Name = "Plate",
                    Ac = 18,
                    Stealth = ArmorStealth.Disadvantage,
                    Weight = new UnitDouble(65, UnitTypes.Imperial, Measurements.Weight),
                    Strength = 15,
                    Type = ArmorType.Heavy,
                    IsCustom = true,
                },
            };

            var rawJson = JsonConvert.SerializeObject(items, Formatting.Indented);
            File.WriteAllText(file, rawJson);
        }
    }
}
