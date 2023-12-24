// <copyright file="DetailedItemGenerator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.DevTools.Generators
{
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Media;

    using Concierge.Data;
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json;

    public static class DetailedItemGenerator
    {
        private static readonly Brush[] classBrushes = [
            Brushes.MediumPurple,
            Brushes.IndianRed,
            Brushes.Cyan,
            Brushes.OrangeRed,
            Brushes.Goldenrod,
            Brushes.MediumSeaGreen,
            Brushes.Orange,
            Brushes.YellowGreen,
            Brushes.SteelBlue,
            Brushes.Orange,
            Brushes.Coral,
            Brushes.SteelBlue,
            Brushes.Yellow,
            Brushes.MediumSeaGreen,
        ];

        private static readonly Brush[] raceBrushes = [
            Brushes.MediumPurple,
            Brushes.LightGreen,
            Brushes.IndianRed,
            Brushes.Goldenrod,
            Brushes.Cyan,
            Brushes.OrangeRed,
            Brushes.LightPink,
            Brushes.YellowGreen,
            Brushes.Magenta,
            Brushes.Orange,
            Brushes.Coral,
            Brushes.Silver,
            Brushes.SteelBlue,
            Brushes.Yellow,
            Brushes.Silver,
            Brushes.Silver,
            Brushes.MediumSeaGreen,
            Brushes.Silver,
            Brushes.Silver,
            Brushes.Silver,
            Brushes.Silver,
        ];

        private static readonly string[] classInfo = [
            "INT based - Masters of invention",
            "STR based - Fighters with primal ferocity",
            "CHA based - Masters of song and speech",
            "DEX,INT based - Unending determination to destroy evils",
            "WIS based - Intermediaries between mortals and gods",
            "WIS based - Extensions of nature's indomitable will",
            "STR,DEX based - Unparalleled martial mastery",
            "DEX,WIS based - Deadly martial artist",
            "STR,CHA based - An oath is a powerful bond",
            "DEX,WIS based - Keeping an unending watch",
            "DEX based - Cornerstone of any adventuring party",
            "CHA based - The power chooses the sorcerer",
            "CHA based - Unlock magic both subtle and spectacular",
            "INT based - The supreme magic-users",
        ];

        private static readonly string[] raceInfo = [
            "A winged people who originated on the Elemental Plane of Air",
            "Mortals who carry a spark of the Upper Planes within their souls",
            "Neither bugs nor bears, they are the cousins of goblins and hobgoblins",
            "Humanoids shaped by draconic gods or the dragons themselves",
            "Commitment to clan, and a burning hatred of goblins and orcs",
            "A magical people of otherworldly grace and ethereal beauty",
            "Planetouched humans, infused with the power of the elements",
            "Enjoy every moment of invention, exploration, investigation, and creation",
            "A subterranean folk, they can be found in every corner of the multiverse",
            "Distantly related to giants and infused with supernatural essence",
            "Walking in two worlds but truly belonging to neither",
            "When alliances between humans and orcs are sealed by marriages",
            "The comforts of home are the goals of most halflings' lives",
            "The youngest of the common races, late to arrive on the world scene",
            "Feathered folk who resemble ravens, blessed with keen observation",
            "Created by the Cat Lord to blend the qualities of humanoids and cats",
            "Their appearance and nature are not their fault but the result of an ancient sin",
            "Tortles have a saying: “We wear our homes on our backs.” ",
            "Originally from the Elemental Plane of Water",
        ];

        private static readonly string[] classNames = [
            "Artificer",
            "Barbarian",
            "Bard",
            "Blood Hunter",
            "Cleric",
            "Druid",
            "Fighter",
            "Monk",
            "Paladin",
            "Ranger",
            "Rogue",
            "Sorcerer",
            "Warlock",
            "Wizard",
        ];

        private static readonly string[] raceNames = [
            "Aarakocra",
            "Aasimar",
            "Bugbear",
            "Dragonborn",
            "Dwarf",
            "Elf",
            "Genasi",
            "Gnome",
            "Goblin",
            "Goliath",
            "Half-Elf",
            "Half-Orc",
            "Halfling",
            "Human",
            "Kenku",
            "Tabaxi",
            "Tiefling",
            "Tortle",
            "Triton",
        ];

        private static readonly PackIconKind[] classIcons = [
            PackIconKind.HammerScrewdriver,
            PackIconKind.AxeBattle,
            PackIconKind.Music,
            PackIconKind.WaterOpacity,
            PackIconKind.Dharmachakra,
            PackIconKind.Grass,
            PackIconKind.Sword,
            PackIconKind.HandFrontRight,
            PackIconKind.ShieldSun,
            PackIconKind.BowArrow,
            PackIconKind.Fencing,
            PackIconKind.Creation,
            PackIconKind.LightningBolt,
            PackIconKind.WizardHat,
        ];

        public static void Generate()
        {
            var list = new List<DetailedItem>();
            for (int i = 0; i < 14; i++)
            {
                list.Add(new DetailedItem(classNames[i], classInfo[i], classIcons[i], classBrushes[i]));
            }

            var race = new List<DetailedItem>();
            for (int i = 0; i < 19; i++)
            {
                race.Add(new DetailedItem(raceNames[i], raceInfo[i], PackIconKind.FaceWomanProfile, raceBrushes[i]));
            }

            File.WriteAllText("C:\\Users\\TomBe\\source\\repos\\detailedRace.json", JsonConvert.SerializeObject(race, Formatting.Indented));
            File.WriteAllText("C:\\Users\\TomBe\\source\\repos\\detailedClass.json", JsonConvert.SerializeObject(list, Formatting.Indented));
        }
    }
}
