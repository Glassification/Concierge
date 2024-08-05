// <copyright file="ComboBoxGenerator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Character.Equipable;
    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Configuration;
    using Concierge.Display.Controls;
    using Concierge.Display.Enums;
    using Concierge.Search.Enums;
    using Concierge.Services;
    using MaterialDesignThemes.Wpf;

    public static class ComboBoxGenerator
    {
        /// <summary>
        /// Generates a list of DetailedComboBoxItemControl using only custom items, sorted by their name.
        /// </summary>
        /// <typeparam name="T">The type of items that implement the IUnique interface.</typeparam>
        /// <param name="customItems">A list of custom items to be combined with default items.</param>
        /// <returns>A list of DetailedComboBoxItemControl with content, foreground color, and tag set based on the provided items.</returns>
        public static List<DetailedComboBoxItemControl> DetailedSelectorComboBox<T>(List<T> customItems)
            where T : IUnique
        {
            var combinedItems = new List<T>();
            combinedItems.AddRange(customItems);
            combinedItems.Sort(new UniqueComparer<T>());

            var comboBoxItems = new List<DetailedComboBoxItemControl>();
            combinedItems.ForEach(x => comboBoxItems.Add(new DetailedComboBoxItemControl(x)));

            return comboBoxItems;
        }

        /// <summary>
        /// Generates a list of DetailedComboBoxItemControl using default items and custom items, sorted by their name.
        /// </summary>
        /// <typeparam name="T">The type of items that implement the IUnique interface.</typeparam>
        /// <param name="defaultItems">A collection of default items.</param>
        /// <param name="customItems">A list of custom items to be combined with default items.</param>
        /// <returns>A list of DetailedComboBoxItemControl with content, foreground color, and tag set based on the provided items.</returns>
        public static List<DetailedComboBoxItemControl> DetailedSelectorComboBox<T>(ReadOnlyCollection<T> defaultItems, List<T> customItems)
            where T : IUnique
        {
            var combinedItems = new List<T>();
            combinedItems.AddRange(defaultItems);
            combinedItems.AddRange(customItems);
            combinedItems.Sort(new UniqueComparer<T>());

            var comboBoxItems = new List<DetailedComboBoxItemControl>();
            combinedItems.ForEach(x => comboBoxItems.Add(new DetailedComboBoxItemControl(x)));

            return comboBoxItems;
        }

        /// <summary>
        /// Generates a list of ComboBoxItemControl using default items and custom items, sorted by their name.
        /// </summary>
        /// <typeparam name="T">The type of items that implement the IUnique interface.</typeparam>
        /// <param name="defaultItems">A collection of default items.</param>
        /// <param name="customItems">A list of custom items to be combined with default items.</param>
        /// <returns>A list of ComboBoxItemControl with content, foreground color, and tag set based on the provided items.</returns>
        public static List<ComboBoxItemControl> SelectorComboBox<T>(ReadOnlyCollection<T> defaultItems, List<T> customItems)
            where T : IUnique
        {
            var combinedItems = new List<T>();
            combinedItems.AddRange(defaultItems);
            combinedItems.AddRange(customItems);
            combinedItems.Sort(new UniqueComparer<T>());

            var comboBoxItems = new List<ComboBoxItemControl>();
            combinedItems.ForEach(x => comboBoxItems.Add(new ComboBoxItemControl(x)));

            return comboBoxItems;
        }

        public static List<ComboBoxItemControl> AbilityTypesComboBox()
        {
            return
            [
                new (PackIconKind.BorderNone, Brushes.SlateGray, AbilityTypes.None),
                new (PackIconKind.ArrangeSendBackward, Brushes.LightBlue, AbilityTypes.Background),
                new (PackIconKind.StarCircleOutline, Brushes.MediumPurple, AbilityTypes.Feat),
                new (PackIconKind.BookVariant, Brushes.Orange, AbilityTypes.ClassFeature),
                new (PackIconKind.BookVariant, Brushes.IndianRed, AbilityTypes.RaceFeature),
            ];
        }

        public static List<ComboBoxItemControl> DamageTypesComboBox()
        {
            return
            [
                new (PackIconKind.TextureBox, Brushes.MediumPurple,  DamageTypes.None),
                new (PackIconKind.Flask, Brushes.LightGreen,  DamageTypes.Acid),
                new (PackIconKind.AxeBattle, Brushes.IndianRed,  DamageTypes.Bludgeoning),
                new (PackIconKind.Snowflake, Brushes.Silver,  DamageTypes.Cold),
                new (PackIconKind.Fire, Brushes.OrangeRed,  DamageTypes.Fire),
                new (PackIconKind.Triforce, Brushes.IndianRed,  DamageTypes.Force),
                new (PackIconKind.LightningBolt, Brushes.Goldenrod,  DamageTypes.Lightning),
                new (PackIconKind.Coffin, Brushes.YellowGreen,  DamageTypes.Necrotic),
                new (PackIconKind.ArrowProjectile, Brushes.IndianRed,  DamageTypes.Piercing),
                new (PackIconKind.Poison, Brushes.LightGreen,  DamageTypes.Poison),
                new (PackIconKind.CrystalBall, Brushes.Cyan,  DamageTypes.Psychic),
                new (PackIconKind.WeatherSunny, Brushes.Goldenrod,  DamageTypes.Radiant),
                new (PackIconKind.Sword, Brushes.IndianRed,  DamageTypes.Slashing),
                new (PackIconKind.WeatherLightning, Brushes.Goldenrod,  DamageTypes.Thunder),
                new (PackIconKind.HeartPlus, ConciergeBrushes.Mint, DamageTypes.Healing),
                new (PackIconKind.HeartMinus, Brushes.IndianRed, DamageTypes.Damage),
            ];
        }

        public static List<ComboBoxItemControl> CoinTypesComboBox()
        {
            return
            [
                new (PackIconKind.Gold, ConciergeBrushes.Copper, CoinType.Copper),
                new (PackIconKind.Gold, ConciergeBrushes.Silver, CoinType.Silver),
                new (PackIconKind.Gold, ConciergeBrushes.Electrum, CoinType.Electrum),
                new (PackIconKind.Gold, ConciergeBrushes.Gold, CoinType.Gold),
                new (PackIconKind.Gold, ConciergeBrushes.Platinum, CoinType.Platinum),
            ];
        }

        public static List<ComboBoxItemControl> DivideLootComboBox(Brush background)
        {
            return
            [
                new (PackIconKind.AccountGroup, Brushes.SteelBlue, DivideLootSelection.Players, background),
                new (PackIconKind.Gold, ConciergeBrushes.Copper, DivideLootSelection.Copper, background),
                new (PackIconKind.Gold, ConciergeBrushes.Silver, DivideLootSelection.Silver, background),
                new (PackIconKind.Gold, ConciergeBrushes.Electrum, DivideLootSelection.Electrum, background),
                new (PackIconKind.Gold, ConciergeBrushes.Gold, DivideLootSelection.Gold, background),
                new (PackIconKind.Gold, ConciergeBrushes.Platinum, DivideLootSelection.Platinum, background),
            ];
        }

        public static List<ComboBoxItemControl> GenderComboBox()
        {
            List<ComboBoxItemControl> list =
            [
                new (PackIconKind.GenderFemale, Brushes.Pink, Gender.Female),
                new (PackIconKind.GenderMale, Brushes.SteelBlue, Gender.Male),
                new (PackIconKind.GenderNonBinary, Brushes.MediumPurple, Gender.Other),
            ];

            if (AppSettingsManager.StartUp.WildWasteland)
            {
                list.Add(new (PackIconKind.Helicopter, Brushes.MediumPurple, Gender.AttackHelicopter));
            }

            return list;
        }

        public static List<ComboBoxItemControl> ArmorTypeComboBox()
        {
            return
            [
                new (PackIconKind.Wall, ConciergeBrushes.Silver, ArmorType.None),
                new (PackIconKind.Wall, ConciergeBrushes.LightCarryCapacity, ArmorType.Light),
                new (PackIconKind.Wall, ConciergeBrushes.MediumCarryCapacity, ArmorType.Medium),
                new (PackIconKind.Wall, ConciergeBrushes.HeavyCarryCapacity, ArmorType.Heavy),
                new (PackIconKind.Wall, ConciergeBrushes.Verdigris, ArmorType.Massive),
            ];
        }

        public static List<ComboBoxItemControl> StealthComboBox()
        {
            return
            [
                new (PackIconKind.Eye, ConciergeBrushes.Mint, ArmorStealth.Normal),
                new (PackIconKind.EyeOff, Brushes.IndianRed, ArmorStealth.Disadvantage),
            ];
        }

        public static List<ComboBoxItemControl> ArmorStatusComboBox()
        {
            return
            [
                new (PackIconKind.Shield, ConciergeBrushes.Mint, ArmorStatus.Donned),
                new (PackIconKind.ShieldOff, Brushes.IndianRed, ArmorStatus.Doffed),
            ];
        }

        public static List<ComboBoxItemControl> AbilitiesComboBox()
        {
            return
            [
                new (PackIconKind.Human, Brushes.Silver, Abilities.NONE),
                new (PackIconKind.WeightLifter, Brushes.IndianRed, Abilities.STR),
                new (PackIconKind.Karate, Brushes.Orange, Abilities.DEX),
                new (PackIconKind.Run, Brushes.Yellow, Abilities.CON),
                new (PackIconKind.HumanMale, Brushes.LightGreen, Abilities.INT),
                new (PackIconKind.Meditation, Brushes.Cyan, Abilities.WIS),
                new (PackIconKind.HumanGreeting, Brushes.MediumPurple, Abilities.CHA),
            ];
        }

        public static List<ComboBoxItemControl> WeaponTypesComboBox()
        {
            return
            [
                new (PackIconKind.BorderNone, Brushes.SlateGray, WeaponTypes.None),
                new (PackIconKind.AxeBattle, Brushes.IndianRed, WeaponTypes.Battleaxe),
                new (PackIconKind.SignPole, Brushes.Magenta, WeaponTypes.Blowgun),
                new (PackIconKind.Oar, Brushes.Cyan, WeaponTypes.Club),
                new (PackIconKind.KnifeMilitary, Brushes.LightGreen, WeaponTypes.Dagger),
                new (PackIconKind.SignPole, Brushes.Magenta, WeaponTypes.Dart),
                new (PackIconKind.Mace, Brushes.LightPink, WeaponTypes.Flail),
                new (PackIconKind.AxeBattle, Brushes.IndianRed, WeaponTypes.Glaive),
                new (PackIconKind.AxeBattle, Brushes.IndianRed, WeaponTypes.Greataxe),
                new (PackIconKind.Oar, Brushes.Cyan, WeaponTypes.Greatclub),
                new (PackIconKind.Sword, Brushes.LightGreen, WeaponTypes.Greatsword),
                new (PackIconKind.AxeBattle, Brushes.IndianRed, WeaponTypes.Halberd),
                new (PackIconKind.Axe, Brushes.IndianRed, WeaponTypes.Handaxe),
                new (PackIconKind.BowArrow, Brushes.Orange, WeaponTypes.HandCrossbow),
                new (PackIconKind.BowArrow, Brushes.Orange, WeaponTypes.HeavyCrossbow),
                new (PackIconKind.Spear, Brushes.LightBlue, WeaponTypes.Javelin),
                new (PackIconKind.Spear, Brushes.LightBlue, WeaponTypes.Lance),
                new (PackIconKind.BowArrow, Brushes.Orange, WeaponTypes.LightCrossbow),
                new (PackIconKind.Hammer, Brushes.Cyan, WeaponTypes.LightHammer),
                new (PackIconKind.BowArrow, Brushes.Orange, WeaponTypes.Longbow),
                new (PackIconKind.Sword, Brushes.LightGreen, WeaponTypes.Longsword),
                new (PackIconKind.Mace, Brushes.LightPink, WeaponTypes.Mace),
                new (PackIconKind.Mace, Brushes.LightPink, WeaponTypes.Maul),
                new (PackIconKind.Mace, Brushes.LightPink, WeaponTypes.Morningstar),
                new (PackIconKind.SpiderWeb, Brushes.MediumPurple, WeaponTypes.Net),
                new (PackIconKind.Spear, Brushes.LightBlue, WeaponTypes.Pike),
                new (PackIconKind.MagicStaff, Brushes.Cyan, WeaponTypes.Quarterstaff),
                new (PackIconKind.Fencing, Brushes.LightGreen, WeaponTypes.Rapier),
                new (PackIconKind.Sword, Brushes.LightGreen, WeaponTypes.Scimitar),
                new (PackIconKind.BowArrow, Brushes.Orange, WeaponTypes.Shortbow),
                new (PackIconKind.Sword, Brushes.LightGreen, WeaponTypes.Shortsword),
                new (PackIconKind.Sickle, Brushes.MediumPurple, WeaponTypes.Sickle),
                new (PackIconKind.Gesture, Brushes.MediumPurple, WeaponTypes.Sling),
                new (PackIconKind.Spear, Brushes.LightBlue, WeaponTypes.Spear),
                new (PackIconKind.SilverwareFork, Brushes.MediumPurple, WeaponTypes.Trident),
                new (PackIconKind.HandFrontLeft, Brushes.Cyan, WeaponTypes.Unarmed),
                new (PackIconKind.Hammer, Brushes.Cyan, WeaponTypes.Warhammer),
                new (PackIconKind.Pickaxe, Brushes.MediumPurple, WeaponTypes.WarPick),
                new (PackIconKind.JumpRope, Brushes.MediumPurple, WeaponTypes.Whip),
            ];
        }

        public static List<ComboBoxItemControl> RecoveryComboBox()
        {
            return
            [
                new (PackIconKind.Run, ConciergeBrushes.Mint, Recovery.None),
                new (PackIconKind.Sleep, ConciergeBrushes.HeavyCarryCapacity, Recovery.LongRest),
                new (PackIconKind.Bed, ConciergeBrushes.LightCarryCapacity, Recovery.ShortRest),
                new (PackIconKind.BedClock, ConciergeBrushes.MediumCarryCapacity, Recovery.ShortOrLongRest),
            ];
        }

        public static List<ComboBoxItemControl> VisionComboBox()
        {
            return
            [
                new (PackIconKind.Eye, ConciergeBrushes.Mint, VisionTypes.Normal),
                new (PackIconKind.EyeClosed, Brushes.Silver, VisionTypes.Blindsight),
                new (PackIconKind.EyeOutline, Brushes.MediumPurple, VisionTypes.Darkvision),
                new (PackIconKind.EyePlusOutline, Brushes.MediumPurple, VisionTypes.SuperiorDarkvision),
                new (PackIconKind.Leak, Brushes.SaddleBrown, VisionTypes.Tremrsense),
                new (PackIconKind.EyeSettings, Brushes.Goldenrod, VisionTypes.Truesight),
            ];
        }

        public static List<ComboBoxItemControl> CreatureSizesComboBox()
        {
            return
            [
                new (PackIconKind.SizeXxs, Brushes.SlateBlue, CreatureSizes.Fine),
                new (PackIconKind.SizeXs, Brushes.LightBlue, CreatureSizes.Diminutive),
                new (PackIconKind.SizeS, Brushes.Cyan, CreatureSizes.Tiny),
                new (PackIconKind.SizeS, Brushes.Cyan, CreatureSizes.Small),
                new (PackIconKind.SizeM, Brushes.Yellow, CreatureSizes.Medium),
                new (PackIconKind.SizeL, Brushes.Goldenrod, CreatureSizes.Large),
                new (PackIconKind.SizeXl, Brushes.Orange, CreatureSizes.Huge),
                new (PackIconKind.SizeXxl, Brushes.OrangeRed, CreatureSizes.Gargantuan),
                new (PackIconKind.SizeXxxl, Brushes.IndianRed, CreatureSizes.Colossal),
            ];
        }

        public static List<ComboBoxItemControl> ExhaustionLevelComboBox()
        {
            return
            [
                new (PackIconKind.Numeric0BoxMultiple, ConciergeBrushes.Mint, ConditionStatus.Normal),
                new (PackIconKind.Numeric1BoxMultiple, Brushes.LightBlue, ConditionStatus.Exhaustion1),
                new (PackIconKind.Numeric2BoxMultiple, Brushes.Yellow, ConditionStatus.Exhaustion2),
                new (PackIconKind.Numeric3BoxMultiple, Brushes.Goldenrod, ConditionStatus.Exhaustion3),
                new (PackIconKind.Numeric4BoxMultiple, Brushes.Orange, ConditionStatus.Exhaustion4),
                new (PackIconKind.Numeric5BoxMultiple, Brushes.OrangeRed, ConditionStatus.Exhaustion5),
                new (PackIconKind.Numeric6BoxMultiple, Brushes.IndianRed, ConditionStatus.Exhaustion6),
            ];
        }

        public static List<ComboBoxItemControl> EquipmentSlotLevelComboBox()
        {
            return
            [
                new (PackIconKind.Head, ConciergeBrushes.Mint, EquipmentSlot.Head),
                new (PackIconKind.TshirtCrew, Brushes.LightBlue, EquipmentSlot.Torso),
                new (PackIconKind.HandBackLeft, Brushes.Goldenrod, EquipmentSlot.Hands),
                new (PackIconKind.HumanHandsdown, Brushes.OrangeRed, EquipmentSlot.Legs),
                new (PackIconKind.ShoeFormal, Brushes.IndianRed, EquipmentSlot.Feet),
            ];
        }

        public static List<ComboBoxItemControl> StretchLevelComboBox()
        {
            return
            [
                new (PackIconKind.AspectRatio, Brushes.LightBlue, Stretch.None),
                new (PackIconKind.AspectRatio, Brushes.Goldenrod, Stretch.Fill),
                new (PackIconKind.AspectRatio, Brushes.OrangeRed, Stretch.Uniform),
                new (PackIconKind.AspectRatio, Brushes.IndianRed, Stretch.UniformToFill),
            ];
        }

        public static List<ComboBoxItemControl> ProficiencyTypesComboBox()
        {
            return
            [
                new (PackIconKind.Wall, Brushes.LightBlue, ProficiencyTypes.Armor, ProficiencyTypes.Armor),
                new (PackIconKind.Tools, Brushes.Goldenrod, ProficiencyTypes.Tool, ProficiencyTypes.Tool),
                new (PackIconKind.SwordCross, Brushes.OrangeRed, ProficiencyTypes.Weapon, ProficiencyTypes.Weapon),
            ];
        }

        public static List<ComboBoxItemControl> AlignmentTypesComboBox()
        {
            var items = new List<ComboBoxItemControl>();
            foreach (var alignment in Defaults.Alignment)
            {
                if (alignment.Contains("Good"))
                {
                    items.Add(new ComboBoxItemControl(PackIconKind.EmojiHappy, ConciergeBrushes.LightCarryCapacity, alignment));
                }
                else if (alignment.Contains("Neutral") && !alignment.Contains("Evil"))
                {
                    items.Add(new ComboBoxItemControl(PackIconKind.EmojiNeutral, ConciergeBrushes.MediumCarryCapacity, alignment));
                }
                else if (alignment.Contains("Evil"))
                {
                    items.Add(new ComboBoxItemControl(PackIconKind.EmojiDevil, ConciergeBrushes.HeavyCarryCapacity, alignment));
                }
            }

            return items;
        }

        public static List<ComboBoxItemControl> BackgroundsComboBox()
        {
            var items = new List<ComboBoxItemControl>();
            foreach (var background in Defaults.Backgrounds)
            {
                items.Add(new ComboBoxItemControl(PackIconKind.Book, Brushes.LightBlue, background));
            }

            return items;
        }

        public static List<DetailedComboBoxItemControl> RacesComboBox()
        {
            var items = new List<DetailedComboBoxItemControl>();
            foreach (var race in Defaults.Races)
            {
                items.Add(new DetailedComboBoxItemControl(race.Icon, race.IconColor, race.Name, race.Information));
            }

            return items;
        }

        public static List<ComboBoxItemControl> SubRacesComboBox(string raceFilter)
        {
            var items = new List<ComboBoxItemControl>();
            foreach (var race in Defaults.Races)
            {
                if (!race.Name.Equals(raceFilter))
                {
                    continue;
                }

                foreach (var subRace in Defaults.Subrace)
                {
                    if (subRace.Name.Equals(race.Name))
                    {
                        items.Add(new ComboBoxItemControl(PackIconKind.FaceWomanProfile, race.IconColor, subRace.Category));
                    }
                }
            }

            return items;
        }

        public static List<ComboBoxItemControl> ClassesComboBox()
        {
            var items = new List<ComboBoxItemControl>();
            foreach (var classes in Defaults.Classes)
            {
                items.Add(new ComboBoxItemControl(PackIconKind.Notebook, classes.IconColor, classes.Name));
            }

            return items;
        }

        public static List<DetailedComboBoxItemControl> DetailedClassesComboBox()
        {
            var items = new List<DetailedComboBoxItemControl>();
            foreach (var classes in Defaults.Classes)
            {
                items.Add(new DetailedComboBoxItemControl(classes.Icon, classes.IconColor, classes.Name, classes.Information));
            }

            return items;
        }

        public static List<ComboBoxItemControl> SubClassesComboBox(string classFilter)
        {
            var items = new List<ComboBoxItemControl>();
            foreach (var classes in Defaults.Classes)
            {
                if (!classes.Name.Equals(classFilter))
                {
                    continue;
                }

                foreach (var subClass in Defaults.Subclass)
                {
                    if (subClass.Name.Equals(classes.Name))
                    {
                        items.Add(new ComboBoxItemControl(PackIconKind.Notebook, classes.IconColor, subClass.Category));
                    }
                }
            }

            return items;
        }

        public static List<ComboBoxItemControl> ItemCategoriesComboBox()
        {
            var inventory = new Inventory();
            var items = new List<ComboBoxItemControl>();
            foreach (var item in Defaults.ItemCategories)
            {
                inventory.ItemCategory = item;
                var category = inventory.GetCategory();
                items.Add(new ComboBoxItemControl(category.IconKind, category.Brush, item));
            }

            return items;
        }

        public static List<ComboBoxItemControl> StatusChecksComboBox()
        {
            return
            [
                new (PackIconKind.CheckboxMarkedCircleOutline, Brushes.LightBlue, StatusChecks.Normal),
                new (PackIconKind.CheckboxMarkedCirclePlusOutline, Brushes.LightGreen, StatusChecks.Advantage),
                new (PackIconKind.CheckboxMarkedCircleMinusOutline, Brushes.IndianRed, StatusChecks.Disadvantage),
                new (PackIconKind.CircleOffOutline, Brushes.Silver, StatusChecks.Fail),
                new (PackIconKind.CheckboxMarkedCircleAutoOutline, Brushes.Goldenrod, StatusChecks.Auto),
            ];
        }

        public static List<ComboBoxItemControl> ArcaneSchoolsComboBox()
        {
            return
            [
                new (PackIconKind.ShieldSun, Brushes.LightBlue, ArcaneSchools.Abjuration),
                new (PackIconKind.Flare, Brushes.LightYellow, ArcaneSchools.Conjuration),
                new (PackIconKind.EyeCircle, Brushes.SlateGray, ArcaneSchools.Divination),
                new (PackIconKind.HeadCog, Brushes.LightPink, ArcaneSchools.Enchantment),
                new (PackIconKind.Flash, Brushes.IndianRed, ArcaneSchools.Evocation),
                new (PackIconKind.AppleIcloud, Brushes.MediumPurple, ArcaneSchools.Illusion),
                new (PackIconKind.Coffin, Brushes.LightGreen, ArcaneSchools.Necromancy),
                new (PackIconKind.CircleOpacity, Brushes.Orange, ArcaneSchools.Transmutation),
                new (PackIconKind.Earth, Brushes.White, ArcaneSchools.Universal),
            ];
        }

        public static List<ComboBoxItemControl> StatusEffectTypesComboBox()
        {
            return
            [
                new (PackIconKind.ShieldOff, Brushes.Silver, StatusEffectTypes.None),
                new (PackIconKind.ShieldStar, Brushes.IndianRed, StatusEffectTypes.Immunity),
                new (PackIconKind.ShieldPlus, Brushes.Orange, StatusEffectTypes.Resistance),
                new (PackIconKind.ShieldRemove, Brushes.LightGreen, StatusEffectTypes.Vulnerability),
            ];
        }

        public static List<ComboBoxItemControl> DiceComboBox()
        {
            return
            [
                new (PackIconKind.SquareRounded, Brushes.Silver, Dice.None),
                new (PackIconKind.DiceD4, Brushes.LightBlue, Dice.D4),
                new (PackIconKind.DiceD6, Brushes.LightPink, Dice.D6),
                new (PackIconKind.DiceD8, Brushes.IndianRed, Dice.D8),
                new (PackIconKind.DiceD10, Brushes.LightGreen, Dice.D10),
                new (PackIconKind.DiceD12, Brushes.Orange, Dice.D12),
                new (PackIconKind.DiceD20, Brushes.MediumPurple, Dice.D20),
                new (PackIconKind.DiceMultiple, Brushes.LightYellow, Dice.D100),
            ];
        }

        public static List<ComboBoxItemControl> SearchDomainComboBox()
        {
            return
            [
                new (PackIconKind.File, ConciergeBrushes.Deer, SearchDomain.CurrentPage),
                new (PackIconKind.BookOpenVariant, ConciergeBrushes.Mint, SearchDomain.EntireSheet),
            ];
        }

        public static List<ComboBoxItemControl> UnitTypesComboBox()
        {
            return
            [
                new (PackIconKind.WeightLb, ConciergeBrushes.Deer, UnitTypes.Imperial),
                new (PackIconKind.WeightKg, ConciergeBrushes.Mint, UnitTypes.Metric),
            ];
        }

        public static List<ComboBoxItemControl> HorizontalAlignmentComboBox()
        {
            return
            [
                new (PackIconKind.FormatAlignLeft, ConciergeBrushes.Mint, HorizontalAlignment.Left),
                new (PackIconKind.FormatAlignCenter, ConciergeBrushes.Deer, HorizontalAlignment.Center),
                new (PackIconKind.FormatAlignRight, Brushes.IndianRed, HorizontalAlignment.Right),
                new (PackIconKind.StretchToPage, Brushes.Goldenrod, HorizontalAlignment.Stretch),
            ];
        }

        public static List<ComboBoxItemControl> AugmentTypeComboBox()
        {
            return
            [
                new (PackIconKind.ArrowProjectileMultiple, Brushes.IndianRed, AugmentType.Ammunition),
                new (PackIconKind.StarCircleOutline, Brushes.MediumPurple, AugmentType.Feature),
                new (PackIconKind.Magic, Brushes.Plum, AugmentType.Spell),
                new (PackIconKind.TextureBox, Brushes.Silver, AugmentType.None),
            ];
        }

        public static List<ComboBoxItemControl> PartyTypeComboBox()
        {
            return
            [
                new (PackIconKind.AccountTie, Brushes.Goldenrod, PartyType.PlayerCharacter),
                new (PackIconKind.Account, ConciergeBrushes.DarkPink, PartyType.PartyMember),
                new (PackIconKind.AccountSchool, Brushes.SteelBlue, PartyType.Npc),
            ];
        }

        public static List<ComboBoxItemControl> PartyStatusComboBox()
        {
            return
            [
                new (PackIconKind.HeadCheck, ConciergeBrushes.Mint, PartyStatus.Alive),
                new (PackIconKind.HeadQuestion, ConciergeBrushes.Deer, PartyStatus.Missing),
                new (PackIconKind.HeadRemove, Brushes.IndianRed, PartyStatus.Dead),
            ];
        }

        public static List<ComboBoxItemControl> FontSizeComboBox()
        {
            var items = new List<ComboBoxItemControl>();
            var sizes = FontService.ListFontSizes();
            foreach (var size in sizes)
            {
                items.Add(new ComboBoxItemControl(PackIconKind.FormatSize, Brushes.LightBlue, size.ToString(), ConciergeBrushes.ControlForeDarkBlue));
            }

            return items;
        }

        public static List<ComboBoxItemControl> FontFamilyComboBox()
        {
            var items = new List<ComboBoxItemControl>();
            var fonts = FontService.ListValidFonts();
            foreach (var font in fonts)
            {
                items.Add(new ComboBoxItemControl(PackIconKind.FormatFont, Brushes.LightBlue, font, ConciergeBrushes.ControlForeDarkBlue));
            }

            return items;
        }
    }
}
