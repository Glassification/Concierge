// <copyright file="MagicClassGenerator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.DevTools.Generators
{
    using System.Collections.Generic;
    using System.IO;
    using Concierge.Character.Enums;
    using Concierge.Character.Spellcasting;
    using Newtonsoft.Json;

    public static class MagicClassGenerator
    {
        public static void Generate(string file)
        {
            var list = new List<MagicClass>()
            {
                new MagicClass()
                {
                    Name = "Arcane Trickster",
                    Ability = Abilities.INT,
                    Level = 3,
                    KnownCantrips = 3,
                    KnownSpells = 3,
                    SpellSlots = 2,
                },
                new MagicClass()
                {
                    Name = "Artificer",
                    Ability = Abilities.INT,
                    Level = 1,
                    KnownCantrips = 2,
                    KnownSpells = 0,
                    SpellSlots = 2,
                },
                new MagicClass()
                {
                    Name = "Barbarian",
                    Ability = Abilities.NONE,
                    Level = 1,
                    KnownCantrips = 0,
                    KnownSpells = 0,
                    SpellSlots = 0,
                },
                new MagicClass()
                {
                    Name = "Bard",
                    Ability = Abilities.CHA,
                    Level = 1,
                    KnownCantrips = 2,
                    KnownSpells = 4,
                    SpellSlots = 2,
                },
                new MagicClass()
                {
                    Name = "Blood Hunter",
                    Ability = Abilities.INT,
                    Level = 1,
                    KnownCantrips = 1,
                    KnownSpells = 0,
                    SpellSlots = 0,
                },
                new MagicClass()
                {
                    Name = "Cleric",
                    Ability = Abilities.WIS,
                    Level = 1,
                    KnownCantrips = 3,
                    KnownSpells = 1,
                    SpellSlots = 2,
                },
                new MagicClass()
                {
                    Name = "Druid",
                    Ability = Abilities.WIS,
                    Level = 1,
                    KnownCantrips = 3,
                    KnownSpells = 1,
                    SpellSlots = 2,
                },
                new MagicClass()
                {
                    Name = "Fighter",
                    Ability = Abilities.NONE,
                    Level = 1,
                    KnownCantrips = 0,
                    KnownSpells = 0,
                    SpellSlots = 0,
                },
                new MagicClass()
                {
                    Name = "Monk",
                    Ability = Abilities.WIS,
                    Level = 1,
                    KnownCantrips = 0,
                    KnownSpells = 0,
                    SpellSlots = 0,
                },
                new MagicClass()
                {
                    Name = "Paladin",
                    Ability = Abilities.CHA,
                    Level = 2,
                    KnownCantrips = 0,
                    KnownSpells = 1,
                    SpellSlots = 2,
                },
                new MagicClass()
                {
                    Name = "Ranger",
                    Ability = Abilities.WIS,
                    Level = 2,
                    KnownCantrips = 0,
                    KnownSpells = 2,
                    SpellSlots = 2,
                },
                new MagicClass()
                {
                    Name = "Rogue",
                    Ability = Abilities.NONE,
                    Level = 1,
                    KnownCantrips = 0,
                    KnownSpells = 0,
                    SpellSlots = 0,
                },
                new MagicClass()
                {
                    Name = "Sorcerer",
                    Ability = Abilities.CHA,
                    Level = 1,
                    KnownCantrips = 4,
                    KnownSpells = 2,
                    SpellSlots = 2,
                },
                new MagicClass()
                {
                    Name = "Warlock",
                    Ability = Abilities.CHA,
                    Level = 1,
                    KnownCantrips = 2,
                    KnownSpells = 2,
                    SpellSlots = 1,
                },
                new MagicClass()
                {
                    Name = "Wizard",
                    Ability = Abilities.INT,
                    Level = 1,
                    KnownCantrips = 3,
                    KnownSpells = 1,
                    SpellSlots = 2,
                },
            };

            var rawJson = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(file, rawJson);
        }
    }
}
