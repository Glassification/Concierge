﻿// <copyright file="CustomBlobGenerator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.DevTools.Generators
{
    using System.Collections.Generic;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Equipable;
    using Concierge.Character.Spellcasting;
    using Concierge.Character.Vitals;
    using Concierge.Logging;
    using Concierge.Persistence;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Services;
    using Newtonsoft.Json;

    public static class CustomBlobGenerator
    {
        public static void Generate(string filePath)
        {
            var ability = new Ability()
            {
                Name = "Gun Fu",
                IsCustom = true,
            };

            var ammunition = new Ammunition()
            {
                Name = "9mm",
                IsCustom = true,
            };

            var classResource = new ClassResource()
            {
                Name = "Plot Armor",
                IsCustom = true,
            };

            var inventory = new Inventory()
            {
                Name = "Holy Hand Grenade",
                Notes = "Count to 3.",
                Weight = new Data.UnitDouble(6.6, Common.Enums.UnitTypes.Imperial, Common.Enums.Measurements.Weight),
                IsCustom = true,
            };

            var language = new Language()
            {
                Name = "Spanish",
                IsCustom = true,
            };

            var magicClass = new MagicClass()
            {
                Name = "Psylock",
                IsCustom = true,
            };

            var proficiency = new Proficiency()
            {
                Name = "Driving",
                IsCustom = true,
            };

            var spell = new Spell()
            {
                Name = "Hydro Flame",
                IsCustom = true,
            };

            var statusEffect = new StatusEffect()
            {
                Name = "High",
                IsCustom = true,
            };

            var blobList = new List<CustomBlob>()
            {
                new CustomBlob(ability),
                new CustomBlob(ammunition),
                new CustomBlob(classResource),
                new CustomBlob(inventory),
                new CustomBlob(language),
                new CustomBlob(magicClass),
                new CustomBlob(proficiency),
                new CustomBlob(spell),
                new CustomBlob(statusEffect),
            };

            var readwriter = new CustomItemReadWriter(new ErrorService(new LocalLogger(@"C:\Users\TomBe\AppData\Roaming\Concierge\asd.txt")));
            readwriter.WriteList(@"C:\Users\TomBe\AppData\Roaming\Concierge\CustomItems.txt", blobList);
        }
    }
}