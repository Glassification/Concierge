// <copyright file="GenerateNames.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace ConciergeDevTools
{
    using System.Collections.Generic;

    using Concierge.Character.Enums;
    using Concierge.Tools;
    using Newtonsoft.Json;

    public static class GenerateNames
    {
        public static void Generate()
        {
            var maleNames = File.ReadAllLines(@"C:\Users\TomBe\source\repos\MaleNames.txt");
            var femaleNames = File.ReadAllLines(@"C:\Users\TomBe\source\repos\FemaleNames.txt");
            var list = new List<Name>();

            AddNames(maleNames, Gender.Male, list);
            AddNames(femaleNames, Gender.Female, list);

            var sortedList = list.OrderBy(x => x.FirstName);

            var rawJson = JsonConvert.SerializeObject(sortedList, Formatting.Indented);
            File.WriteAllText(@"C:\Users\TomBe\source\repos\Names.json", rawJson);
        }

        private static void AddNames(string[] names, Gender gender, List<Name> list)
        {
            foreach (var name in names)
            {
                var token = name.Split(' ');
                list.Add(new Name(token[0], token[1], gender));
            }
        }
    }
}
