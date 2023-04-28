// <copyright file="GlossaryGenerator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.DevTools
{
    using System.Collections.Generic;
    using System.IO;

    using Concierge.Tools.Glossary;
    using Newtonsoft.Json;

    public static class GlossaryGenerator
    {
        public static void Generate()
        {
            var entries = new List<GlossaryEntry>()
            {
                new GlossaryEntry("Ability Scores", LoadMarkdown("Ability Scores.md", "Ability Scores"))
                {
                    GlossaryEntries = new List<GlossaryEntry>()
                    {
                        new GlossaryEntry("Ability Checks", LoadMarkdown("Ability Checks.md", "Ability Scores")),
                        new GlossaryEntry("Advantage and Disadvantage", LoadMarkdown("Advantage and Disadvantage.md", "Ability Scores")),
                        new GlossaryEntry("Attributes", LoadMarkdown("Attributes.md", "Ability Scores"))
                        {
                            GlossaryEntries = new List<GlossaryEntry>()
                            {
                                new GlossaryEntry("Strength", LoadMarkdown("Strength.md", "Ability Scores\\Attributes")),
                                new GlossaryEntry("Dexterity", LoadMarkdown("Dexterity.md", "Ability Scores\\Attributes")),
                                new GlossaryEntry("Constitution", LoadMarkdown("Constitution.md", "Ability Scores\\Attributes")),
                                new GlossaryEntry("Intelligence", LoadMarkdown("Intelligence.md", "Ability Scores\\Attributes")),
                                new GlossaryEntry("Wisdom", LoadMarkdown("Wisdom.md", "Ability Scores\\Attributes")),
                                new GlossaryEntry("Charisma", LoadMarkdown("Charisma.md", "Ability Scores\\Attributes")),
                            },
                        },
                        new GlossaryEntry("Contests", LoadMarkdown("Contests.md", "Ability Scores")),
                        new GlossaryEntry("Passive Checks", LoadMarkdown("Passive Checks.md", "Ability Scores")),
                        new GlossaryEntry("Proficiency Bonus", LoadMarkdown("Proficiency Bonus.md", "Ability Scores")),
                        new GlossaryEntry("Saving Throws", LoadMarkdown("Saving Throws.md", "Ability Scores")),
                        new GlossaryEntry("Skills", LoadMarkdown("Skills.md", "Ability Scores"))
                        {
                            GlossaryEntries = new List<GlossaryEntry>()
                            {
                                new GlossaryEntry("Athletics", LoadMarkdown("Athletics.md", "Ability Scores\\Skills")),
                                new GlossaryEntry("Acrobatics", LoadMarkdown("Acrobatics.md", "Ability Scores\\Skills")),
                                new GlossaryEntry("Sleight of Hand", LoadMarkdown("Sleight of Hand.md", "Ability Scores\\Skills")),
                                new GlossaryEntry("Stealth", LoadMarkdown("Stealth.md", "Ability Scores\\Skills")),
                                new GlossaryEntry("Arcana", LoadMarkdown("Arcana.md", "Ability Scores\\Skills")),
                                new GlossaryEntry("History", LoadMarkdown("History.md", "Ability Scores\\Skills")),
                                new GlossaryEntry("Investigation", LoadMarkdown("Investigation.md", "Ability Scores\\Skills")),
                                new GlossaryEntry("Nature", LoadMarkdown("Nature.md", "Ability Scores\\Skills")),
                                new GlossaryEntry("Religion", LoadMarkdown("Religion.md", "Ability Scores\\Skills")),
                                new GlossaryEntry("Animal Handling", LoadMarkdown("Animal Handling.md", "Ability Scores\\Skills")),
                                new GlossaryEntry("Insight", LoadMarkdown("Insight.md", "Ability Scores\\Skills")),
                                new GlossaryEntry("Medicine", LoadMarkdown("Medicine.md", "Ability Scores\\Skills")),
                                new GlossaryEntry("Perception", LoadMarkdown("Perception.md", "Ability Scores\\Skills")),
                                new GlossaryEntry("Survival", LoadMarkdown("Survival.md", "Ability Scores\\Skills")),
                                new GlossaryEntry("Deception", LoadMarkdown("Deception.md", "Ability Scores\\Skills")),
                                new GlossaryEntry("Intimidation", LoadMarkdown("Intimidation.md", "Ability Scores\\Skills")),
                                new GlossaryEntry("Performance", LoadMarkdown("Performance.md", "Ability Scores\\Skills")),
                                new GlossaryEntry("Persuasion", LoadMarkdown("Persuasion.md", "Ability Scores\\Skills")),
                            },
                        },
                        new GlossaryEntry("Working Together", LoadMarkdown("Working Together.md", "Ability Scores")),
                    },
                },
                new GlossaryEntry("Adventuring", LoadMarkdown("Adventuring.md", "Adventuring"))
                {
                    GlossaryEntries = new List<GlossaryEntry>()
                    {
                        new GlossaryEntry("Adventuring", LoadMarkdown("Adventuring.md", "Adventuring")),
                        new GlossaryEntry("Food and Water", LoadMarkdown("Food and Water.md", "Adventuring")),
                        new GlossaryEntry("Movement", LoadMarkdown("Movement.md", "Adventuring"))
                        {
                            GlossaryEntries = new List<GlossaryEntry>()
                            {
                                new GlossaryEntry("Speed", LoadMarkdown("Speed.md", "Adventuring\\Movement")),
                                new GlossaryEntry("Traveling", LoadMarkdown("Traveling.md", "Adventuring\\Movement")),
                                new GlossaryEntry("Special Movement", LoadMarkdown("Special Movement.md", "Adventuring\\Movement")),
                            },
                        },
                        new GlossaryEntry("Resting", LoadMarkdown("Resting.md", "Adventuring")),
                        new GlossaryEntry("Suffocating", LoadMarkdown("Suffocating.md", "Adventuring")),
                        new GlossaryEntry("Time", LoadMarkdown("Time.md", "Adventuring")),
                        new GlossaryEntry("Vision and Light", LoadMarkdown("Vision and Light.md", "Adventuring"))
                        {
                            GlossaryEntries = new List<GlossaryEntry>()
                            {
                                new GlossaryEntry("Blindsight", LoadMarkdown("Blindsight.md", "Adventuring\\Vision")),
                                new GlossaryEntry("Darkvision", LoadMarkdown("Darkvision.md", "Adventuring\\Vision")),
                                new GlossaryEntry("Tremorsense", LoadMarkdown("Tremorsense.md", "Adventuring\\Vision")),
                                new GlossaryEntry("Truesight", LoadMarkdown("Truesight.md", "Adventuring\\Vision")),
                            },
                        },
                    },
                },
                new GlossaryEntry("Combat", LoadMarkdown("Combat.md", "Combat"))
                {
                    GlossaryEntries = new List<GlossaryEntry>()
                    {
                        new GlossaryEntry("Order of Combat", LoadMarkdown("The Order of Combat.md", "Combat"))
                        {
                            GlossaryEntries = new List<GlossaryEntry>()
                            {
                                new GlossaryEntry("Initiative", LoadMarkdown("Initiative.md", "Combat\\Order")),
                                new GlossaryEntry("Surprise", LoadMarkdown("Surprise.md", "Combat\\Order")),
                                new GlossaryEntry("Your Turn", LoadMarkdown("Your Turn.md", "Combat\\Order")),
                            },
                        },
                        new GlossaryEntry("Movement and Position", LoadMarkdown("Movement and Position.md", "Combat"))
                        {
                            GlossaryEntries = new List<GlossaryEntry>()
                            {
                                new GlossaryEntry("Breaking Up Your Move", LoadMarkdown("Breaking Up Your Move.md", "Combat\\Movement")),
                                new GlossaryEntry("Difficult Terrain", LoadMarkdown("Difficult Terrain.md", "Combat\\Movement")),
                                new GlossaryEntry("Being Prone", LoadMarkdown("Being Prone.md", "Combat\\Movement")),
                                new GlossaryEntry("Creature Size", LoadMarkdown("Creature Size.md", "Combat\\Movement")),
                            },
                        },
                        new GlossaryEntry("Actions", LoadMarkdown("Actions.md", "Combat"))
                        {
                            GlossaryEntries = new List<GlossaryEntry>()
                            {
                                new GlossaryEntry("Attack", LoadMarkdown("Attack.md", "Combat\\Actions")),
                                new GlossaryEntry("Cast a Spell", LoadMarkdown("Cast a Spell.md", "Combat\\Actions")),
                                new GlossaryEntry("Dash", LoadMarkdown("Dash.md", "Combat\\Actions")),
                                new GlossaryEntry("Disengage", LoadMarkdown("Disengage.md", "Combat\\Actions")),
                                new GlossaryEntry("Dodge", LoadMarkdown("Dodge.md", "Combat\\Actions")),
                                new GlossaryEntry("Help", LoadMarkdown("Help.md", "Combat\\Actions")),
                                new GlossaryEntry("Hide", LoadMarkdown("Hide.md", "Combat\\Actions")),
                                new GlossaryEntry("Ready", LoadMarkdown("Ready.md", "Combat\\Actions")),
                                new GlossaryEntry("Search", LoadMarkdown("Search.md", "Combat\\Actions")),
                                new GlossaryEntry("Use an Object", LoadMarkdown("Use an Object.md", "Combat\\Actions")),
                            },
                        },
                        new GlossaryEntry("Making an Attack", LoadMarkdown("Making an Attack.md", "Combat"))
                        {
                            GlossaryEntries = new List<GlossaryEntry>()
                            {
                                new GlossaryEntry("Attack Rolls", LoadMarkdown("Attack Rolls.md", "Combat\\Making Attack")),
                                new GlossaryEntry("Melee Attacks", LoadMarkdown("Melee Attacks.md", "Combat\\Making Attack")),
                                new GlossaryEntry("Ranged Attacks", LoadMarkdown("Ranged Attacks.md", "Combat\\Making Attack")),
                                new GlossaryEntry("Unseen Attackers and Targets", LoadMarkdown("Unseen Attackers and Targets.md", "Combat\\Making Attack")),
                            },
                        },
                        new GlossaryEntry("Cover", LoadMarkdown("Cover.md", "Combat")),
                        new GlossaryEntry("Damage and Healing", LoadMarkdown("Damage and Healing.md", "Combat"))
                        {
                            GlossaryEntries = new List<GlossaryEntry>()
                            {
                                new GlossaryEntry("Hit Points", LoadMarkdown("Hit Points.md", "Combat\\Damage and Healing")),
                                new GlossaryEntry("Damage Rolls", LoadMarkdown("Damage Rolls.md", "Combat\\Damage and Healing")),
                                new GlossaryEntry("Damage Types", LoadMarkdown("Damage Types.md", "Combat\\Damage and Healing")),
                                new GlossaryEntry("Resistance and Vulnerability", LoadMarkdown("Resistance and Vulnerability.md", "Combat\\Damage and Healing")),
                                new GlossaryEntry("Healing", LoadMarkdown("Healing.md", "Combat\\Damage and Healing")),
                                new GlossaryEntry("Dropping to 0 Hit Points", LoadMarkdown("Dropping to 0 Hit Points.md", "Combat\\Damage and Healing")),
                                new GlossaryEntry("Knocking a Creature Out", LoadMarkdown("Knocking a Creature Out.md", "Combat\\Damage and Healing")),
                                new GlossaryEntry("Temporary Hit Points", LoadMarkdown("Temporary Hit Points.md", "Combat\\Damage and Healing")),
                            },
                        },
                        new GlossaryEntry("Mounted Combat", LoadMarkdown("Mounted Combat.md", "Combat")),
                    },
                },
                new GlossaryEntry("Equipment", LoadMarkdown("Equipment.md", "Equipment"))
                {
                    GlossaryEntries = new List<GlossaryEntry>()
                    {
                        new GlossaryEntry("Armor", LoadMarkdown("Armor.md", "Equipment"))
                        {
                            GlossaryEntries = new List<GlossaryEntry>()
                            {
                                new GlossaryEntry("Light", LoadMarkdown("Light Armor.md", "Equipment\\Armor")),
                                new GlossaryEntry("Medium", LoadMarkdown("Medium Armor.md", "Equipment\\Armor")),
                                new GlossaryEntry("Heavy", LoadMarkdown("Heavy Armor.md", "Equipment\\Armor")),
                            },
                        },
                        new GlossaryEntry("Mounts and Vehicles", LoadMarkdown("Mounts and Vehicles.md", "Equipment")),
                        new GlossaryEntry("Selling Treasure", LoadMarkdown("Selling Treasure.md", "Equipment")),
                        new GlossaryEntry("Services", LoadMarkdown("Services.md", "Equipment")),
                        new GlossaryEntry("Trade Goods", LoadMarkdown("Trade Goods.md", "Equipment")),
                        new GlossaryEntry("Wealth", LoadMarkdown("Wealth.md", "Equipment")),
                        new GlossaryEntry("Weapons", LoadMarkdown("Weapons.md", "Equipment"))
                        {
                            GlossaryEntries = new List<GlossaryEntry>()
                            {
                                new GlossaryEntry("Traits", LoadMarkdown("Improvised.md", "Equipment\\Weapons")),
                                new GlossaryEntry("Proficiency", LoadMarkdown("Proficiency.md", "Equipment\\Weapons")),
                                new GlossaryEntry("Properties", LoadMarkdown("Properties.md", "Equipment\\Weapons")),
                                new GlossaryEntry("Silvered", LoadMarkdown("Silvered.md", "Equipment\\Weapons")),
                                new GlossaryEntry("Special", LoadMarkdown("Special.md", "Equipment\\Weapons")),
                            },
                        },
                    },
                },
                new GlossaryEntry("Personality", LoadMarkdown("Personality.md", "Personality"))
                {
                    GlossaryEntries = new List<GlossaryEntry>()
                    {
                        new GlossaryEntry("Alignment", LoadMarkdown("Alignment.md", "Personality")),
                        new GlossaryEntry("Characteristics", LoadMarkdown("Characteristics.md", "Personality"))
                        {
                            GlossaryEntries = new List<GlossaryEntry>()
                            {
                                new GlossaryEntry("Bonds", LoadMarkdown("Bonds.md", "Personality\\Characteristics")),
                                new GlossaryEntry("Flaws", LoadMarkdown("Flaws.md", "Personality\\Characteristics")),
                                new GlossaryEntry("Ideals", LoadMarkdown("Ideals.md", "Personality\\Characteristics")),
                                new GlossaryEntry("Traits", LoadMarkdown("Traits.md", "Personality\\Characteristics")),
                            },
                        },
                        new GlossaryEntry("Inspiration", LoadMarkdown("Inspiration.md", "Personality")),
                        new GlossaryEntry("Languages", LoadMarkdown("Languages.md", "Personality")),
                    },
                },
                new GlossaryEntry("Spellcasting", LoadMarkdown("Spellcasting.md", "Spellcasting"))
                {
                    GlossaryEntries = new List<GlossaryEntry>()
                    {
                        new GlossaryEntry("Spell Level", LoadMarkdown("Spell Level.md", "Spellcasting")),
                        new GlossaryEntry("Known and Prepared Spells", LoadMarkdown("Known and Prepared Spells.md", "Spellcasting")),
                        new GlossaryEntry("Spell Slots", LoadMarkdown("Spell Slots.md", "Spellcasting"))
                        {
                            GlossaryEntries = new List<GlossaryEntry>()
                            {
                                new GlossaryEntry("Casting at Higher Levels", LoadMarkdown("Casting at Higher Levels.md", "Spellcasting\\Spell Slots")),
                                new GlossaryEntry("Casting in Armor", LoadMarkdown("Casting in Armor.md", "Spellcasting\\Spell Slots")),
                                new GlossaryEntry("Cantrips", LoadMarkdown("Cantrips.md", "Spellcasting\\Spell Slots")),
                            },
                        },
                        new GlossaryEntry("Rituals", LoadMarkdown("Rituals.md", "Spellcasting")),
                        new GlossaryEntry("Casting a Spell", LoadMarkdown("Casting a Spell.md", "Spellcasting"))
                        {
                            GlossaryEntries = new List<GlossaryEntry>()
                            {
                                new GlossaryEntry("Casting Time", LoadMarkdown("Casting Time.md", "Spellcasting\\Casting a Spell")),
                                new GlossaryEntry("Range", LoadMarkdown("Range.md", "Spellcasting\\Casting a Spell")),
                                new GlossaryEntry("Components", LoadMarkdown("Components.md", "Spellcasting\\Casting a Spell")),
                                new GlossaryEntry("Duration", LoadMarkdown("Duration.md", "Spellcasting\\Casting a Spell")),
                                new GlossaryEntry("Targets", LoadMarkdown("Targets.md", "Spellcasting\\Casting a Spell")),
                                new GlossaryEntry("Areas of Effect", LoadMarkdown("Areas of Effect.md", "Spellcasting\\Casting a Spell")),
                                new GlossaryEntry("Saving Throws", LoadMarkdown("Saving Throws.md", "Spellcasting\\Casting a Spell")),
                                new GlossaryEntry("Attack Rolls", LoadMarkdown("Attack Rolls.md", "Spellcasting\\Casting a Spell")),
                                new GlossaryEntry("Combining Magical Effects", LoadMarkdown("Combining Magical Effects.md", "Spellcasting\\Casting a Spell")),
                            },
                        },
                        new GlossaryEntry("Schools of Magic", LoadMarkdown("Schools of Magic.md", "Spellcasting")),
                    },
                },
            };

            var rawJson = JsonConvert.SerializeObject(entries, Formatting.Indented);
            File.WriteAllText("C:\\Users\\TomBe\\source\\repos\\Markdown\\Glossary.json", rawJson);
        }

        private static string LoadMarkdown(string name, string folder)
        {
            return File.ReadAllText(Path.Combine("C:\\Users\\TomBe\\source\\repos\\Markdown", Path.Combine(folder, name)));
        }
    }
}
