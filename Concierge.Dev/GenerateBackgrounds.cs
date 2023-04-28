// <copyright file="GenerateBackgrounds.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.DevTools
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Enums;
    using Newtonsoft.Json;

    public static class GenerateBackgrounds
    {
        public static void Generate()
        {
            var lines = File.ReadAllLines("C:\\Users\\TomBe\\source\\repos\\Untitled-1.txt");
            var list = new List<Ability>();

            foreach (var line in lines)
            {
                var tokens = line.Split('|');
                var ability = new Ability()
                {
                    Name = tokens[0].Trim(),
                    Description = tokens[1].Trim(),
                    Type = AbilityTypes.Background,
                };

                list.Add(ability);
            }

            var sortedList = list.OrderBy(x => x.Name).ToList();
            File.WriteAllText("C:\\Users\\TomBe\\source\\repos\\ability.json", JsonConvert.SerializeObject(sortedList, Formatting.Indented));
        }
    }
}
