// <copyright file="GenerateNames.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.DevTools
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Concierge.Common.Enums;
    using Concierge.Common.Utilities;
    using Concierge.Persistence;
    using Concierge.Tools.Enums;
    using Concierge.Tools.Generators.Names;
    using Newtonsoft.Json;

    public static class GenerateNames
    {
        private static readonly string[] postfixes = new string[] { "Male", "Female", "Last" };

        public static void Generate()
        {
            var list = new List<Name>();
            var fileNames = CreateFileNames();

            foreach (var name in fileNames)
            {
                TryAddNames(list, name);
            }

            var sortedList = list.OrderBy(x => x.Race).ThenBy(y => y.Gender).ThenBy(z => z.Value).ToList();

            var rawJson = JsonConvert.SerializeObject(sortedList, Formatting.Indented);
            File.WriteAllText(@"C:\Users\TomBe\source\repos\Names.json", rawJson);
        }

        private static List<string> CreateFileNames()
        {
            var fileNames = new List<string>();
            foreach (var race in Defaults.Races)
            {
                foreach (var postfix in postfixes)
                {
                    fileNames.Add($"{race}{postfix}.txt");
                }
            }

            return fileNames;
        }

        private static void TryAddNames(List<Name> list, string fileName)
        {
            try
            {
                var names = File.ReadAllLines(@$"C:\Users\TomBe\source\repos\{fileName}");
                var gender = fileName.Contains("Male") ? Gender.Male : fileName.Contains("Female") ? Gender.Female : Gender.Other;
                var nameType = fileName.Contains("Last") ? NameType.Last : NameType.First;
                var tokens = StringUtility.FormatName(fileName).Split(' ');
                var race = tokens[0];
                if (race.Contains('-'))
                {
                    race = $"{race}{tokens[1]}";
                }

                AddNames(names, gender, nameType, race, list);
            }
            catch (Exception)
            {
                File.AppendAllText(@"C:\Users\TomBe\source\repos\UnknownRaces.txt", $"{fileName}\n");
            }
        }

        private static void AddNames(string[] names, Gender gender, NameType nameType, string race, List<Name> list)
        {
            foreach (var name in names)
            {
                list.Add(new Name(name, nameType, gender, race));
            }
        }
    }
}
