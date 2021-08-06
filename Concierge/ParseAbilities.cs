// <copyright file="ParseAbilities.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

    using Concierge.Characters.Characteristics;
    using Newtonsoft.Json;

    public static class ParseAbilities
    {
        private static readonly Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)", RegexOptions.Compiled);

        public static void ParseAbilitiesCsv()
        {
            var lines = File.ReadAllLines("C:\\Users\\TomBe\\Desktop\\Dnd.csv");
            var abilities = new List<Ability>();

            foreach (var line in lines)
            {
                var splitLine = SplitLine(line);

                abilities.Add(new Ability()
                {
                    Name = splitLine[0],
                    Requirements = splitLine[1],
                    Description = splitLine[2],
                });
            }

            var rawJson = JsonConvert.SerializeObject(abilities, Formatting.Indented);

            File.WriteAllText("C:\\Users\\TomBe\\Desktop\\Abilities.json", rawJson);
        }

        private static List<string> SplitLine(string line)
        {
            var list = new List<string>();

            foreach (Match match in csvSplit.Matches(line))
            {
                var curr = match.Value;
                if (curr.Length == 0)
                {
                    list.Add(string.Empty);
                }

                list.Add(curr.TrimStart(','));
            }

            return list;
        }
    }
}
