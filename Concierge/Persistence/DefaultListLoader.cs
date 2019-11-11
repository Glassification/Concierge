using Concierge.Characters.Collections;
using Concierge.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml.Linq;

namespace Concierge.Persistence
{
    public static class DefaultListLoader
    {
        public static List<Weapon> LoadWeaponList()
        {
            List<Weapon> weapons = new List<Weapon>();

            try
            {
                XDocument xml = XDocument.Parse(Properties.Resources.WeaponList);
                XElement root = xml.Element("Weapons");

                var SimpleMelee = root.Elements("Weapon");
                foreach (XElement elem in SimpleMelee)
                {
                    Weapon weapon = new Weapon(Guid.Empty)
                    {
                        Name = (string)elem.Attribute("name"),
                        Ability = Constants.Abilities.NONE,
                        Damage = (string)elem.Attribute("damage"),
                        Misc = "",
                        DamageType = (Constants.DamageTypes)Enum.Parse(typeof(Constants.DamageTypes), (string)elem.Attribute("type")),
                        Range = (string)elem.Attribute("range"),
                        Note = (string)elem.Attribute("notes"),
                        Weight = (double)elem.Attribute("weight"),
                        WeaponType = (Constants.WeaponTypes)Enum.Parse(typeof(Constants.WeaponTypes), ((string)elem.Attribute("name")).Replace(" ", ""))
                    };

                    weapons.Add(weapon);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                MessageBox.Show("Error: Default weapon list not loaded successfully", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return weapons;
        }

        public static List<Ammunition> LoadAmmunitionList()
        {
            List<Ammunition> ammunitions = new List<Ammunition>();

            try
            {
                XDocument xml = XDocument.Parse(Properties.Resources.AmmoList);
                XElement root = xml.Element("Ammunitions");

                var Ammunitions = root.Elements("Ammunition");
                foreach (XElement elem in Ammunitions)
                {
                    Ammunition ammunition = new Ammunition(Guid.Empty)
                    {
                        Name = (string)elem.Attribute("name"),
                        Quantity = (int)elem.Attribute("qty"),
                        Bonus = "",
                        DamageType = Constants.DamageTypes.None,
                        Used = 0
                    };

                    ammunitions.Add(ammunition);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                MessageBox.Show("Error: Default ammunition list not loaded successfully", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return ammunitions;
        }

        public static List<Spell> LoadSpellList()
        {
            List<Spell> spells = new List<Spell>();

            try
            {
                XDocument xml = XDocument.Parse(Properties.Resources.SpellList);
                XElement root = xml.Element("Spells");

                var Spells = root.Elements("Spell");
                foreach (XElement elem in Spells)
                {
                    Spell spell = new Spell(Guid.Empty)
                    {
                        Name = (string)elem.Attribute("name"),
                        Level = int.Parse((string)elem.Attribute("level")),
                        Page = int.Parse((string)elem.Attribute("page")),
                        School = (Constants.ArcaneSchools)Enum.Parse(typeof(Constants.ArcaneSchools), (string)elem.Attribute("school")),
                        Ritual = bool.Parse((string)elem.Attribute("ritual")),
                        Components = ((string)elem.Attribute("comp")).Equals("") ? "N/A" : (string)elem.Attribute("comp"),
                        Concentration = ((string)elem.Attribute("concen")).Equals("Yes") ? true : false,
                        Range = ((string)elem.Attribute("range")).Equals("") ? "N/A" : (string)elem.Attribute("range"),
                        Duration = (string)elem.Attribute("duration"),
                        Area = ((string)elem.Attribute("area")).Equals("") ? "N/A" : (string)elem.Attribute("area"),
                        Save = ((string)elem.Attribute("save")).Equals("") ? "N/A" : (string)elem.Attribute("save"),
                        Damage = ((string)elem.Attribute("damage")).Equals("") ? "N/A" : (string)elem.Attribute("damage"),
                        Description = (string)elem.Attribute("description"),
                        Prepared = ((string)elem.Attribute("prepared")).Equals("Yes") ? true : false
                    };

                    spells.Add(spell);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                MessageBox.Show("Error: Default spell list not loaded successfully", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return spells;
        }

        public static List<Inventory> LoadInventoryList()
        {
            List<Inventory> inventories = new List<Inventory>();

            try
            {
                XDocument xml = XDocument.Parse(Properties.Resources.ItemList);
                XElement root = xml.Element("Items");

                var AdventuringGear = root.Elements("Item");
                foreach (XElement elem in AdventuringGear)
                {
                    Inventory inventory = new Inventory(Guid.Empty)
                    {
                        Name = (string)elem.Attribute("name"),
                        Amount = 1,
                        Weight = (double)elem.Attribute("weight"),
                        Note = (string)elem.Attribute("notes")
                    };

                    inventories.Add(inventory);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                MessageBox.Show("Error: Default item list not loaded successfully", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return inventories;
        }

        public static List<Language> LoadLanguageList()
        {
            List<Language> languages = new List<Language>();

            try
            {
                XDocument xml = XDocument.Parse(Properties.Resources.LanguageList);
                XElement root = xml.Element("Languages");

                var Languages = root.Elements("Language");
                foreach (XElement elem in Languages)
                {
                    Language l = new Language(Guid.Empty)
                    {
                        Name = (string)elem.Attribute("Name"),
                        Script = (string)elem.Attribute("Script"),
                        Speakers = (string)elem.Attribute("Speakers")
                    };

                    languages.Add(l);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                MessageBox.Show("Error: Default language list not loaded successfully", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return languages;
        }
    }
}
