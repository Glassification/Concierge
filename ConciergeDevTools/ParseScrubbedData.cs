﻿// <copyright file="ParseScrubbedData.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace ConciergeDevTools
{
    using System.Text.RegularExpressions;

    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Primitives;
    using Newtonsoft.Json;

    public static class ParseScrubbedData
    {
        private static readonly Regex CommaSplit = new ("(?:^|,)(\"(?:[^\"])*\"|[^,]*)", RegexOptions.Compiled);

        public static void Parse(string file)
        {
            var lines = File.ReadAllLines(file);
            var items = new List<Inventory>();

            foreach (var line in lines)
            {
                var tokens = CommaSplit.Matches(line)
                    .Cast<Match>()
                    .Select(m => m.Value.Trim(new char[] { ',', '"' }))
                    .ToList();
                var value = tokens[2].Equals("--") ? 0 : int.Parse(tokens[2].Split(' ')[0]);
                var coinType = value == 0 ? CoinType.Copper : GetCoinType(tokens[2].Split(' ')[1]);
                var weight = tokens[3].Equals("--") ? 0 : double.Parse(tokens[3].Split(' ')[0]);

                items.Add(new Inventory()
                {
                    Name = tokens[0],
                    ItemCategory = tokens[1],
                    Value = value,
                    CoinType = coinType,
                    Weight = new UnitDouble(weight, Concierge.Utility.Units.Enums.UnitTypes.Imperial, Concierge.Utility.Units.Enums.Measurements.Weight),
                    Notes = tokens[4],
                    Description = tokens[5],
                    Amount = 1,
                    IgnoreWeight = false,
                    Attuned = false,
                    Index = 0,
                });
            }

            var rawJson = JsonConvert.SerializeObject(items, Formatting.Indented);
            File.WriteAllText(@"C:\Users\TomBe\Documents\Items.json", rawJson);
        }

        private static CoinType GetCoinType(string str)
        {
            return str switch
            {
                "cp" => CoinType.Copper,
                "sp" => CoinType.Silver,
                "ep" => CoinType.Electrum,
                "gp" => CoinType.Gold,
                "pp" => CoinType.Platinum,
                _ => CoinType.Copper,
            };
        }
    }
}