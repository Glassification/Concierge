// <copyright file="ComboBoxGenerator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;

    using Concierge.Character.Enums;
    using Concierge.Character.Equipable;
    using Concierge.Common;
    using Concierge.Common.Enums;
    using Concierge.Display.Controls;
    using Concierge.Search.Enums;
    using MaterialDesignThemes.Wpf;

    public static class ComboBoxGenerator
    {
        private static readonly double[] fontSizes = [8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72];
        private static readonly FontFamily[] fontFamilies = [.. Fonts.SystemFontFamilies.OrderBy(f => f.Source)];

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

        private static readonly Brush[] classBrushes = [
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
            Brushes.SteelBlue,
            Brushes.Yellow,
            Brushes.MediumSeaGreen,
        ];

        /// <summary>
        /// Generates a list of ComboBoxItems using default items and custom items, sorted by their name.
        /// </summary>
        /// <typeparam name="T">The type of items that implement the IUnique interface.</typeparam>
        /// <param name="defaultItems">A collection of default items.</param>
        /// <param name="customItems">A list of custom items to be combined with default items.</param>
        /// <returns>A list of ComboBoxItems with content, foreground color, and tag set based on the provided items.</returns>
        public static List<ComboBoxItemControl> SelectorComboBox<T>(ReadOnlyCollection<T> defaultItems, List<T> customItems)
            where T : IUnique
        {
            var combinedItems = new List<T>();
            combinedItems.AddRange(defaultItems);
            combinedItems.AddRange(customItems);
            combinedItems.Sort(new UniqueComparer<T>());

            var comboBoxItems = new List<ComboBoxItemControl>();
            foreach (var item in combinedItems)
            {
                comboBoxItems.Add(new ComboBoxItemControl(item));
            }

            return comboBoxItems;
        }

        public static List<ComboBoxItemControl> AbilityTypesComboBox()
        {
            return
            [
                new (PackIconKind.ArrangeSendBackward, Brushes.LightBlue, AbilityTypes.None.ToString()),
                new (PackIconKind.StarCircleOutline, Brushes.MediumPurple, AbilityTypes.Background.ToString()),
                new (PackIconKind.BookVariant, Brushes.Orange, AbilityTypes.Feat.ToString()),
                new (PackIconKind.BookVariant, Brushes.IndianRed, AbilityTypes.ClassFeature.ToString()),
                new (PackIconKind.BorderNone, Brushes.SlateGray, AbilityTypes.RaceFeature.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> DamageTypesComboBox()
        {
            return
            [
                new (PackIconKind.ListStatus, Brushes.MediumPurple,  DamageTypes.None.ToString()),
                new (PackIconKind.Flask, Brushes.LightGreen,  DamageTypes.Acid.ToString()),
                new (PackIconKind.AxeBattle, Brushes.IndianRed,  DamageTypes.Bludgeoning.ToString()),
                new (PackIconKind.Snowflake, Brushes.Silver,  DamageTypes.Cold.ToString()),
                new (PackIconKind.Fire, Brushes.OrangeRed,  DamageTypes.Fire.ToString()),
                new (PackIconKind.Triforce, Brushes.IndianRed,  DamageTypes.Force.ToString()),
                new (PackIconKind.LightningBolt, Brushes.Goldenrod,  DamageTypes.Lightning.ToString()),
                new (PackIconKind.Coffin, Brushes.YellowGreen,  DamageTypes.Necrotic.ToString()),
                new (PackIconKind.ArrowProjectile, Brushes.IndianRed,  DamageTypes.Piercing.ToString()),
                new (PackIconKind.Poison, Brushes.LightGreen,  DamageTypes.Poison.ToString()),
                new (PackIconKind.CrystalBall, Brushes.Cyan,  DamageTypes.Psychic.ToString()),
                new (PackIconKind.WeatherSunny, Brushes.Goldenrod,  DamageTypes.Radiant.ToString()),
                new (PackIconKind.Sword, Brushes.IndianRed,  DamageTypes.Slashing.ToString()),
                new (PackIconKind.WeatherLightning, Brushes.Goldenrod,  DamageTypes.Thunder.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> CoinTypesComboBox()
        {
            return
            [
                new (PackIconKind.CircleMultiple, ConciergeBrushes.Copper, CoinType.Copper.ToString()),
                new (PackIconKind.CircleMultiple, ConciergeBrushes.Silver, CoinType.Silver.ToString()),
                new (PackIconKind.CircleMultiple, ConciergeBrushes.Electrum, CoinType.Electrum.ToString()),
                new (PackIconKind.CircleMultiple, ConciergeBrushes.Gold, CoinType.Gold.ToString()),
                new (PackIconKind.CircleMultiple, ConciergeBrushes.Platinum, CoinType.Platinum.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> GenderComboBox()
        {
            return
            [
                new (PackIconKind.GenderFemale, Brushes.Pink, Gender.Female.ToString()),
                new (PackIconKind.GenderMale, Brushes.SteelBlue, Gender.Male.ToString()),
                new (PackIconKind.GenderNonBinary, Brushes.MediumPurple, Gender.Other.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> ArmorTypeComboBox()
        {
            return
            [
                new (PackIconKind.ShieldSword, ConciergeBrushes.Silver, ArmorType.None.ToString()),
                new (PackIconKind.ShieldSword, ConciergeBrushes.LightCarryCapacity, ArmorType.Light.ToString()),
                new (PackIconKind.ShieldSword, ConciergeBrushes.MediumCarryCapacity, ArmorType.Medium.ToString()),
                new (PackIconKind.ShieldSword, ConciergeBrushes.HeavyCarryCapacity, ArmorType.Heavy.ToString()),
                new (PackIconKind.ShieldSword, ConciergeBrushes.Verdigris, ArmorType.Massive.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> StealthComboBox()
        {
            return
            [
                new (PackIconKind.EyeOff, ConciergeBrushes.Mint, ArmorStealth.Normal.ToString()),
                new (PackIconKind.Eye, Brushes.IndianRed, ArmorStealth.Disadvantage.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> ArmorStatusComboBox()
        {
            return
            [
                new (PackIconKind.ToggleSwitch, ConciergeBrushes.Mint, ArmorStatus.Donned.ToString()),
                new (PackIconKind.ToggleSwitchOff, Brushes.IndianRed, ArmorStatus.Doffed.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> AbilitiesComboBox()
        {
            return
            [
                new (PackIconKind.Human, Brushes.Silver, Abilities.NONE.ToString()),
                new (PackIconKind.WeightLifter, Brushes.IndianRed, Abilities.STR.ToString()),
                new (PackIconKind.Karate, Brushes.Orange, Abilities.DEX.ToString()),
                new (PackIconKind.Run, Brushes.Yellow, Abilities.CON.ToString()),
                new (PackIconKind.HumanMale, Brushes.LightGreen, Abilities.INT.ToString()),
                new (PackIconKind.Meditation, Brushes.Cyan, Abilities.WIS.ToString()),
                new (PackIconKind.HumanGreeting, Brushes.MediumPurple, Abilities.CHA.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> WeaponTypesComboBox()
        {
            return
            [
                new (PackIconKind.BorderNone, Brushes.SlateGray, WeaponTypes.None.ToString()),
                new (PackIconKind.AxeBattle, Brushes.IndianRed, WeaponTypes.Battleaxe.ToString()),
                new (PackIconKind.SignPole, Brushes.Magenta, WeaponTypes.Blowgun.ToString()),
                new (PackIconKind.Oar, Brushes.Cyan, WeaponTypes.Club.ToString()),
                new (PackIconKind.KnifeMilitary, Brushes.LightGreen, WeaponTypes.Dagger.ToString()),
                new (PackIconKind.SignPole, Brushes.Magenta, WeaponTypes.Dart.ToString()),
                new (PackIconKind.Mace, Brushes.LightPink, WeaponTypes.Flail.ToString()),
                new (PackIconKind.AxeBattle, Brushes.IndianRed, WeaponTypes.Glaive.ToString()),
                new (PackIconKind.AxeBattle, Brushes.IndianRed, WeaponTypes.Greataxe.ToString()),
                new (PackIconKind.Oar, Brushes.Cyan, WeaponTypes.Greatclub.ToString()),
                new (PackIconKind.Sword, Brushes.LightGreen, WeaponTypes.Greatsword.ToString()),
                new (PackIconKind.AxeBattle, Brushes.IndianRed, WeaponTypes.Halberd.ToString()),
                new (PackIconKind.Axe, Brushes.IndianRed, WeaponTypes.Handaxe.ToString()),
                new (PackIconKind.BowArrow, Brushes.Orange, WeaponTypes.HandCrossbow.ToString()),
                new (PackIconKind.BowArrow, Brushes.Orange, WeaponTypes.HeavyCrossbow.ToString()),
                new (PackIconKind.Spear, Brushes.LightBlue, WeaponTypes.Javelin.ToString()),
                new (PackIconKind.Spear, Brushes.LightBlue, WeaponTypes.Lance.ToString()),
                new (PackIconKind.BowArrow, Brushes.Orange, WeaponTypes.LightCrossbow.ToString()),
                new (PackIconKind.Hammer, Brushes.Cyan, WeaponTypes.LightHammer.ToString()),
                new (PackIconKind.BowArrow, Brushes.Orange, WeaponTypes.Longbow.ToString()),
                new (PackIconKind.Sword, Brushes.LightGreen, WeaponTypes.Longsword.ToString()),
                new (PackIconKind.Mace, Brushes.LightPink, WeaponTypes.Mace.ToString()),
                new (PackIconKind.Mace, Brushes.LightPink, WeaponTypes.Maul.ToString()),
                new (PackIconKind.Mace, Brushes.LightPink, WeaponTypes.Morningstar.ToString()),
                new (PackIconKind.SpiderWeb, Brushes.MediumPurple, WeaponTypes.Net.ToString()),
                new (PackIconKind.Spear, Brushes.LightBlue, WeaponTypes.Pike.ToString()),
                new (PackIconKind.MagicStaff, Brushes.Cyan, WeaponTypes.Quarterstaff.ToString()),
                new (PackIconKind.Fencing, Brushes.LightGreen, WeaponTypes.Rapier.ToString()),
                new (PackIconKind.Sword, Brushes.LightGreen, WeaponTypes.Scimitar.ToString()),
                new (PackIconKind.BowArrow, Brushes.Orange, WeaponTypes.Shortbow.ToString()),
                new (PackIconKind.Sword, Brushes.LightGreen, WeaponTypes.Shortsword.ToString()),
                new (PackIconKind.Sickle, Brushes.MediumPurple, WeaponTypes.Sickle.ToString()),
                new (PackIconKind.Gesture, Brushes.MediumPurple, WeaponTypes.Sling.ToString()),
                new (PackIconKind.Spear, Brushes.LightBlue, WeaponTypes.Spear.ToString()),
                new (PackIconKind.SilverwareFork, Brushes.MediumPurple, WeaponTypes.Trident.ToString()),
                new (PackIconKind.Hammer, Brushes.Cyan, WeaponTypes.Warhammer.ToString()),
                new (PackIconKind.Pickaxe, Brushes.MediumPurple, WeaponTypes.WarPick.ToString()),
                new (PackIconKind.JumpRope, Brushes.MediumPurple, WeaponTypes.Whip.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> RecoveryComboBox()
        {
            return
            [
                new (PackIconKind.Run, ConciergeBrushes.Mint, Recovery.None.ToString()),
                new (PackIconKind.Sleep, ConciergeBrushes.HeavyCarryCapacity, Recovery.LongRest.ToString()),
                new (PackIconKind.Bed, ConciergeBrushes.LightCarryCapacity, Recovery.ShortRest.ToString()),
                new (PackIconKind.BedClock, ConciergeBrushes.MediumCarryCapacity, Recovery.ShortOrLongRest.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> VisionComboBox()
        {
            return
            [
                new (PackIconKind.Eye, ConciergeBrushes.Mint, VisionTypes.Normal.ToString()),
                new (PackIconKind.EyeClosed, Brushes.Silver, VisionTypes.Blindsight.ToString()),
                new (PackIconKind.EyeOutline, Brushes.MediumPurple, VisionTypes.Darkvision.ToString()),
                new (PackIconKind.EyePlusOutline, Brushes.MediumPurple, VisionTypes.SuperiorDarkvision.ToString()),
                new (PackIconKind.Leak, Brushes.SaddleBrown, VisionTypes.Tremrsense.ToString()),
                new (PackIconKind.EyeSettings, Brushes.Goldenrod, VisionTypes.Truesight.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> CreatureSizesComboBox()
        {
            return
            [
                new (PackIconKind.SizeXxs, Brushes.SlateBlue, CreatureSizes.Fine.ToString()),
                new (PackIconKind.SizeXs, Brushes.LightBlue, CreatureSizes.Diminutive.ToString()),
                new (PackIconKind.SizeS, Brushes.Cyan, CreatureSizes.Tiny.ToString()),
                new (PackIconKind.SizeS, Brushes.Cyan, CreatureSizes.Small.ToString()),
                new (PackIconKind.SizeM, Brushes.Yellow, CreatureSizes.Medium.ToString()),
                new (PackIconKind.SizeL, Brushes.Goldenrod, CreatureSizes.Large.ToString()),
                new (PackIconKind.SizeXl, Brushes.Orange, CreatureSizes.Huge.ToString()),
                new (PackIconKind.SizeXxl, Brushes.OrangeRed, CreatureSizes.Gargantuan.ToString()),
                new (PackIconKind.SizeXxxl, Brushes.IndianRed, CreatureSizes.Colossal.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> ExhaustionLevelComboBox()
        {
            return
            [
                new (PackIconKind.Numeric0BoxMultiple, ConciergeBrushes.Mint, ExhaustionLevel.Normal.ToString()),
                new (PackIconKind.Numeric1BoxMultiple, Brushes.LightBlue, ExhaustionLevel.One.ToString()),
                new (PackIconKind.Numeric2BoxMultiple, Brushes.Yellow, ExhaustionLevel.Two.ToString()),
                new (PackIconKind.Numeric3BoxMultiple, Brushes.Goldenrod, ExhaustionLevel.Three.ToString()),
                new (PackIconKind.Numeric4BoxMultiple, Brushes.Orange, ExhaustionLevel.Four.ToString()),
                new (PackIconKind.Numeric5BoxMultiple, Brushes.OrangeRed, ExhaustionLevel.Five.ToString()),
                new (PackIconKind.Numeric6BoxMultiple, Brushes.IndianRed, ExhaustionLevel.Six.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> EncumbranceLevelComboBox()
        {
            return
            [
                new (PackIconKind.Weight, ConciergeBrushes.Mint, EncumbranceLevel.Normal.ToString()),
                new (PackIconKind.WeightGram, Brushes.Orange, EncumbranceLevel.Encumbered.ToString()),
                new (PackIconKind.WeightKg, Brushes.IndianRed, EncumbranceLevel.HeavilyEncumbered.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> EquipmentSlotLevelComboBox()
        {
            return
            [
                new (PackIconKind.Head, ConciergeBrushes.Mint, EquipmentSlot.Head.ToString()),
                new (PackIconKind.TshirtCrew, Brushes.LightBlue, EquipmentSlot.Torso.ToString()),
                new (PackIconKind.HandBackLeft, Brushes.Goldenrod, EquipmentSlot.Hands.ToString()),
                new (PackIconKind.HumanHandsdown, Brushes.OrangeRed, EquipmentSlot.Legs.ToString()),
                new (PackIconKind.ShoeFormal, Brushes.IndianRed, EquipmentSlot.Feet.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> StretchLevelComboBox()
        {
            return
            [
                new (PackIconKind.RelativeScale, Brushes.LightBlue, Stretch.None.ToString()),
                new (PackIconKind.RelativeScale, Brushes.Goldenrod, Stretch.Fill.ToString()),
                new (PackIconKind.RelativeScale, Brushes.OrangeRed, Stretch.Uniform.ToString()),
                new (PackIconKind.RelativeScale, Brushes.IndianRed, Stretch.UniformToFill.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> ProficiencyTypesComboBox()
        {
            return
            [
                new (PackIconKind.Wall, Brushes.LightBlue, ProficiencyTypes.Armor.ToString()),
                new (PackIconKind.Tools, Brushes.Goldenrod, ProficiencyTypes.Tool.ToString()),
                new (PackIconKind.SwordCross, Brushes.OrangeRed, ProficiencyTypes.Weapon.ToString()),
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

        public static List<ComboBoxItemControl> RacesComboBox()
        {
            var items = new List<ComboBoxItemControl>();
            for (int i = 0; i < Defaults.Races.Count; i++)
            {
                items.Add(new ComboBoxItemControl(PackIconKind.FaceWomanProfile, raceBrushes[i], Defaults.Races[i]));
            }

            return items;
        }

        public static List<ComboBoxItemControl> SubRacesComboBox()
        {
            var items = new List<ComboBoxItemControl>();
            for (int i = 0; i < Defaults.Races.Count; i++)
            {
                foreach (var subRace in Defaults.Subrace)
                {
                    if (subRace.Name.Equals(Defaults.Races[i]))
                    {
                        items.Add(new ComboBoxItemControl(PackIconKind.FaceWomanProfile, raceBrushes[i], subRace.ToString()));
                    }
                }
            }

            return items;
        }

        public static List<ComboBoxItemControl> ClassesComboBox()
        {
            var items = new List<ComboBoxItemControl>();
            for (int i = 0; i < Defaults.Classes.Count; i++)
            {
                items.Add(new ComboBoxItemControl(PackIconKind.Notebook, classBrushes[i], Defaults.Classes[i]));
            }

            return items;
        }

        public static List<ComboBoxItemControl> SubClassesComboBox()
        {
            var items = new List<ComboBoxItemControl>();
            for (int i = 0; i < Defaults.Classes.Count; i++)
            {
                foreach (var subClass in Defaults.Subclass)
                {
                    if (subClass.Name.Equals(Defaults.Classes[i]))
                    {
                        items.Add(new ComboBoxItemControl(PackIconKind.Notebook, classBrushes[i], subClass.ToString()));
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
                new (PackIconKind.CheckboxMarkedCircleOutline, Brushes.LightBlue, StatusChecks.Normal.ToString()),
                new (PackIconKind.CheckboxMarkedCirclePlusOutline, Brushes.LightGreen, StatusChecks.Advantage.ToString()),
                new (PackIconKind.CheckboxMarkedCircleMinusOutline, Brushes.IndianRed, StatusChecks.Disadvantage.ToString()),
                new (PackIconKind.CircleOffOutline, Brushes.Silver, StatusChecks.Fail.ToString()),
                new (PackIconKind.CheckboxMarkedCircleAutoOutline, Brushes.Goldenrod, StatusChecks.Auto.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> ArcaneSchoolsComboBox()
        {
            return
            [
                new (PackIconKind.ShieldSun, Brushes.LightBlue, ArcaneSchools.Abjuration.ToString()),
                new (PackIconKind.Flare, Brushes.LightYellow, ArcaneSchools.Conjuration.ToString()),
                new (PackIconKind.EyeCircle, Brushes.SlateGray, ArcaneSchools.Divination.ToString()),
                new (PackIconKind.HeadCog, Brushes.LightPink, ArcaneSchools.Enchantment.ToString()),
                new (PackIconKind.Flash, Brushes.IndianRed, ArcaneSchools.Evocation.ToString()),
                new (PackIconKind.AppleIcloud, Brushes.MediumPurple, ArcaneSchools.Illusion.ToString()),
                new (PackIconKind.Coffin, Brushes.LightGreen, ArcaneSchools.Necromancy.ToString()),
                new (PackIconKind.CircleOpacity, Brushes.Orange, ArcaneSchools.Transmutation.ToString()),
                new (PackIconKind.Earth, Brushes.White, ArcaneSchools.Universal.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> StatusEffectTypesComboBox()
        {
            return
            [
                new (PackIconKind.ShieldOff, Brushes.Silver, StatusEffectTypes.None.ToString()),
                new (PackIconKind.ShieldStar, Brushes.IndianRed, StatusEffectTypes.Immunity.ToString()),
                new (PackIconKind.ShieldPlus, Brushes.Orange, StatusEffectTypes.Resistance.ToString()),
                new (PackIconKind.ShieldRemove, Brushes.LightGreen, StatusEffectTypes.Vulnerability.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> DiceComboBox()
        {
            return
            [
                new (PackIconKind.SquareRounded, Brushes.Silver, Dice.None.ToString()),
                new (PackIconKind.DiceD4, Brushes.LightBlue, Dice.D4.ToString()),
                new (PackIconKind.DiceD6, Brushes.LightPink, Dice.D6.ToString()),
                new (PackIconKind.DiceD8, Brushes.IndianRed, Dice.D8.ToString()),
                new (PackIconKind.DiceD10, Brushes.LightGreen, Dice.D10.ToString()),
                new (PackIconKind.DiceD12, Brushes.Orange, Dice.D12.ToString()),
                new (PackIconKind.DiceD20, Brushes.MediumPurple, Dice.D20.ToString()),
                new (PackIconKind.DiceMultiple, Brushes.LightYellow, Dice.D100.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> SearchDomainComboBox()
        {
            return
            [
                new (PackIconKind.File, ConciergeBrushes.Deer, SearchDomain.CurrentPage.ToString()),
                new (PackIconKind.BookOpenVariant, ConciergeBrushes.Mint, SearchDomain.EntireSheet.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> UnitTypesComboBox()
        {
            return
            [
                new (PackIconKind.TapeMeasure, ConciergeBrushes.Deer, UnitTypes.Imperial.ToString()),
                new (PackIconKind.TapeMeasure, ConciergeBrushes.Mint, UnitTypes.Metric.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> HorizontalAlignmentComboBox()
        {
            return
            [
                new (PackIconKind.FormatAlignLeft, ConciergeBrushes.Mint, HorizontalAlignment.Left.ToString()),
                new (PackIconKind.FormatAlignCenter, ConciergeBrushes.Deer, HorizontalAlignment.Center.ToString()),
                new (PackIconKind.FormatAlignRight, Brushes.IndianRed, HorizontalAlignment.Right.ToString()),
                new (PackIconKind.StretchToPage, Brushes.Goldenrod, HorizontalAlignment.Stretch.ToString()),
            ];
        }

        public static List<ComboBoxItemControl> FontSizeComboBox()
        {
            var items = new List<ComboBoxItemControl>();
            foreach (var size in fontSizes)
            {
                items.Add(new ComboBoxItemControl(PackIconKind.FormatSize, Brushes.LightBlue, size.ToString()));
            }

            return items;
        }

        public static List<ComboBoxItemControl> FontFamilyComboBox()
        {
            var items = new List<ComboBoxItemControl>();
            foreach (var font in fontFamilies)
            {
                items.Add(new ComboBoxItemControl(PackIconKind.FormatFont, Brushes.LightBlue, font.ToString(), font));
            }

            return items;
        }
    }
}
