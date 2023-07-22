// <copyright file="StatusEffectGenerator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.DevTools.Generators
{
    using System.Collections.Generic;
    using System.IO;

    using Concierge.Character.Enums;
    using Concierge.Character.Vitals;
    using Newtonsoft.Json;

    public static class StatusEffectGenerator
    {
        public static void Generate(string file)
        {
            var list = new List<StatusEffect>()
            {
                new StatusEffect()
                {
                    Name = "Acid",
                    Type = StatusEffectTypes.None,
                },
                new StatusEffect()
                {
                    Name = "Bludgeoning",
                    Type = StatusEffectTypes.None,
                },
                new StatusEffect()
                {
                    Name = "Cold",
                    Type = StatusEffectTypes.None,
                },
                new StatusEffect()
                {
                    Name = "Fire",
                    Type = StatusEffectTypes.None,
                },
                new StatusEffect()
                {
                    Name = "Force",
                    Type = StatusEffectTypes.None,
                },
                new StatusEffect()
                {
                    Name = "Lightning",
                    Type = StatusEffectTypes.None,
                },
                new StatusEffect()
                {
                    Name = "Magic Weapons",
                    Type = StatusEffectTypes.None,
                },
                new StatusEffect()
                {
                    Name = "Necrotic",
                    Type = StatusEffectTypes.None,
                },
                new StatusEffect()
                {
                    Name = "Nonmagical",
                    Type = StatusEffectTypes.None,
                },
                new StatusEffect()
                {
                    Name = "Piercing",
                    Type = StatusEffectTypes.None,
                },
                new StatusEffect()
                {
                    Name = "Poison",
                    Type = StatusEffectTypes.None,
                },
                new StatusEffect()
                {
                    Name = "Psychic",
                    Type = StatusEffectTypes.None,
                },
                new StatusEffect()
                {
                    Name = "Radiant",
                    Type = StatusEffectTypes.None,
                },
                new StatusEffect()
                {
                    Name = "Slashing",
                    Type = StatusEffectTypes.None,
                },
                new StatusEffect()
                {
                    Name = "Spells",
                    Type = StatusEffectTypes.None,
                },
                new StatusEffect()
                {
                    Name = "AThundercid",
                    Type = StatusEffectTypes.None,
                },
            };

            var rawJson = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(file, rawJson);
        }
    }
}
