namespace MyApp
{
    using Concierge.Character.Enums;
    using Concierge.Character.Items;
    using Concierge.Primitives;
    using Newtonsoft.Json;
    using System.Text.RegularExpressions;

    internal class Program
    {
        private static readonly Regex CommaSplit = new ("(?:^|,)(\"(?:[^\"])*\"|[^,]*)", RegexOptions.Compiled);

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(@"C:\Users\TomBe\Documents\scrubbed data.csv");
            var items = new List<Inventory>();

            foreach (var line in lines)
            {
                var tokens = CommaSplit.Matches(line)
                    .Cast<Match>()
                    .Select(m => m.Value.Trim(new char[] { ',', '"' }))
                    .ToList();
                var value = tokens[2].Equals("--") ? 0 : int.Parse(tokens[2].Split(' ')[0]);
                var coinType = value == 0 ? CoinType.None : GetCoinType(tokens[2].Split(' ')[1]);
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
                "sp" => CoinType.Copper,
                "ep" => CoinType.Copper,
                "gp" => CoinType.Copper,
                "pp" => CoinType.Copper,
                _ => CoinType.None,
            };
        }
    }
}