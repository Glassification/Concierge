// <copyright file="DefaultListLoader.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Xml.Linq;

    using Concierge.Characters.Collections;
    using Concierge.Characters.Enums;
    using Concierge.Utility;

    public static class DefaultListLoader
    {
        public static List<Weapon> LoadWeaponList()
        {
            var weapons = new List<Weapon>();

            try
            {
                var xml = XDocument.Parse(Properties.Resources.WeaponList);
                var root = xml.Element("Weapons");

                var SimpleMelee = root.Elements("Weapon");
                foreach (var elem in SimpleMelee)
                {
                    var weapon = new Weapon(Guid.Empty)
                    {
                        Name = (string)elem.Attribute("name"),
                        Ability = Abilities.NONE,
                        Damage = (string)elem.Attribute("damage"),
                        Misc = string.Empty,
                        DamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), (string)elem.Attribute("type")),
                        Range = (string)elem.Attribute("range"),
                        Note = (string)elem.Attribute("notes"),
                        Weight = (double)elem.Attribute("weight"),
                        WeaponType = (WeaponTypes)Enum.Parse(typeof(WeaponTypes), ((string)elem.Attribute("name")).Replace(" ", string.Empty)),
                    };

                    weapons.Add(weapon);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                _ = MessageBox.Show("Error: Default weapon list not loaded successfully", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return weapons;
        }

        public static List<Ammunition> LoadAmmunitionList()
        {
            var ammunitions = new List<Ammunition>();

            try
            {
                var xml = XDocument.Parse(Properties.Resources.AmmoList);
                var root = xml.Element("Ammunitions");

                var Ammunitions = root.Elements("Ammunition");
                foreach (var elem in Ammunitions)
                {
                    var ammunition = new Ammunition(Guid.Empty)
                    {
                        Name = (string)elem.Attribute("name"),
                        Quantity = (int)elem.Attribute("qty"),
                        Bonus = string.Empty,
                        DamageType = DamageTypes.None,
                        Used = 0,
                    };

                    ammunitions.Add(ammunition);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                _ = MessageBox.Show("Error: Default ammunition list not loaded successfully", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return ammunitions;
        }

        public static List<Spell> LoadSpellList()
        {
            var spells = new List<Spell>();

            try
            {
                var xml = XDocument.Parse(Properties.Resources.SpellList);
                var root = xml.Element("Spells");

                var Spells = root.Elements("Spell");
                foreach (var elem in Spells)
                {
                    var spell = new Spell(Guid.Empty)
                    {
                        Name = (string)elem.Attribute("name"),
                        Level = int.Parse((string)elem.Attribute("level")),
                        Page = int.Parse((string)elem.Attribute("page")),
                        School = (ArcaneSchools)Enum.Parse(typeof(ArcaneSchools), (string)elem.Attribute("school")),
                        Ritual = ((string)elem.Attribute("ritual")).Equals("Yes") ? true : false,
                        Components = ((string)elem.Attribute("comp")).Equals(string.Empty) ? "N/A" : (string)elem.Attribute("comp"),
                        Concentration = ((string)elem.Attribute("concen")).Equals("Yes") ? true : false,
                        Range = ((string)elem.Attribute("range")).Equals(string.Empty) ? "N/A" : (string)elem.Attribute("range"),
                        Duration = (string)elem.Attribute("duration"),
                        Area = ((string)elem.Attribute("area")).Equals(string.Empty) ? "N/A" : (string)elem.Attribute("area"),
                        Save = ((string)elem.Attribute("save")).Equals(string.Empty) ? "N/A" : (string)elem.Attribute("save"),
                        Damage = ((string)elem.Attribute("damage")).Equals(string.Empty) ? "N/A" : (string)elem.Attribute("damage"),
                        Description = (string)elem.Attribute("description"),
                        Prepared = ((string)elem.Attribute("prepared")).Equals("Yes") ? true : false,
                    };

                    spells.Add(spell);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                _ = MessageBox.Show("Error: Default spell list not loaded successfully", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return spells;
        }

        public static List<Inventory> LoadInventoryList()
        {
            var inventories = new List<Inventory>();

            try
            {
                var xml = XDocument.Parse(Properties.Resources.ItemList);
                var root = xml.Element("Items");

                var AdventuringGear = root.Elements("Item");
                foreach (var elem in AdventuringGear)
                {
                    var inventory = new Inventory(Guid.Empty)
                    {
                        Name = (string)elem.Attribute("name"),
                        Amount = 1,
                        Weight = (double)elem.Attribute("weight"),
                        Note = (string)elem.Attribute("notes"),
                    };

                    inventories.Add(inventory);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                _ = MessageBox.Show("Error: Default item list not loaded successfully", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return inventories;
        }

        public static List<Language> LoadLanguageList()
        {
            var languages = new List<Language>();

            try
            {
                var xml = XDocument.Parse(Properties.Resources.LanguageList);
                var root = xml.Element("Languages");

                var Languages = root.Elements("Language");
                foreach (var elem in Languages)
                {
                    var language = new Language(Guid.Empty)
                    {
                        Name = (string)elem.Attribute("Name"),
                        Script = (string)elem.Attribute("Script"),
                        Speakers = (string)elem.Attribute("Speakers"),
                    };

                    languages.Add(language);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                _ = MessageBox.Show("Error: Default language list not loaded successfully", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return languages;
        }
    }
}
