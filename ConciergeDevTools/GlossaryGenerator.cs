// <copyright file="GlossaryGenerator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace ConciergeDevTools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Concierge.Tools.Glossary;
    using Newtonsoft.Json;

    public static class GlossaryGenerator
    {
        public static void Generate()
        {
            var entries = new List<GlossaryEntry>
            {
                new GlossaryEntry("Armor Class", LoadMarkdown("Armor Class.md")),
                new GlossaryEntry("Attributes", string.Empty)
                {
                    GlossaryEntries = new List<GlossaryEntry>()
                    {
                        new GlossaryEntry("Strength", LoadMarkdown("Strength.md")),
                        new GlossaryEntry("Dexterity", LoadMarkdown("Dexterity.md"))
                        {
                            GlossaryEntries = new List<GlossaryEntry>()
                            {
                                new GlossaryEntry("Hiding", LoadMarkdown("Hiding.md")),
                            },
                        },
                        new GlossaryEntry("Constitution", LoadMarkdown("Constitution.md")),
                        new GlossaryEntry("Intelligence", LoadMarkdown("Intelligence.md")),
                        new GlossaryEntry("Wisdom", LoadMarkdown("Wisdom.md"))
                        {
                            GlossaryEntries = new List<GlossaryEntry>()
                            {
                                new GlossaryEntry("Finding a Hidden Object", LoadMarkdown("Hidden Object.md")),
                            },
                        },
                        new GlossaryEntry("Charisma", LoadMarkdown("Charisma.md")),
                    },
                },
                new GlossaryEntry("Initiative", LoadMarkdown("Initiative.md")),
                new GlossaryEntry("Movement", LoadMarkdown("Movement.md"))
                {
                    GlossaryEntries = new List<GlossaryEntry>()
                    {
                        new GlossaryEntry("Long Jump", LoadMarkdown("Long Jump.md")),
                        new GlossaryEntry("High Jump", LoadMarkdown("High Jump.md")),
                    },
                },
                new GlossaryEntry("Proficiency", LoadMarkdown("Proficiency.md")),
                new GlossaryEntry("Resting", LoadMarkdown("Rests.md")),
                new GlossaryEntry("Saving Throws", LoadMarkdown("Saving Throws.md")),
                new GlossaryEntry("Senses", string.Empty)
                {
                    GlossaryEntries = new List<GlossaryEntry>()
                    {
                        new GlossaryEntry("Passive Checks", LoadMarkdown("Passive Checks.md")),
                        new GlossaryEntry("Vision", LoadMarkdown("Vision.md")),
                    },
                },
                new GlossaryEntry("Skills", string.Empty)
                {
                    GlossaryEntries = new List<GlossaryEntry>()
                    {
                        new GlossaryEntry("Athletics", LoadMarkdown("Athletics.md")),
                        new GlossaryEntry("Acrobatics", LoadMarkdown("Acrobatics.md")),
                        new GlossaryEntry("Sleight of Hand", LoadMarkdown("Sleight of Hand.md")),
                        new GlossaryEntry("Stealth", LoadMarkdown("Stealth.md")),
                        new GlossaryEntry("Arcana", LoadMarkdown("Arcana.md")),
                        new GlossaryEntry("History", LoadMarkdown("History.md")),
                        new GlossaryEntry("Investigation", LoadMarkdown("Investigation.md")),
                        new GlossaryEntry("Nature", LoadMarkdown("Nature.md")),
                        new GlossaryEntry("Religion", LoadMarkdown("Religion.md")),
                        new GlossaryEntry("Animal Handling", LoadMarkdown("Animal Handling.md")),
                        new GlossaryEntry("Insight", LoadMarkdown("Insight.md")),
                        new GlossaryEntry("Medicine", LoadMarkdown("Medicine.md")),
                        new GlossaryEntry("Perception", LoadMarkdown("Perception.md")),
                        new GlossaryEntry("Survival", LoadMarkdown("Survival.md")),
                        new GlossaryEntry("Deception", LoadMarkdown("Deception.md")),
                        new GlossaryEntry("Intimidation", LoadMarkdown("Intimidation.md")),
                        new GlossaryEntry("Performance", LoadMarkdown("Performance.md")),
                        new GlossaryEntry("Persuasion", LoadMarkdown("Persuasion.md")),
                    },
                },
            };

            var rawJson = JsonConvert.SerializeObject(entries, Formatting.Indented);
            File.WriteAllText("C:\\Users\\TomBe\\source\\repos\\Markdown\\Glossary.json", rawJson);
        }

        private static string LoadMarkdown(string name)
        {
            return File.ReadAllText(Path.Combine("C:\\Users\\TomBe\\source\\repos\\Markdown", name));
        }
    }
}
