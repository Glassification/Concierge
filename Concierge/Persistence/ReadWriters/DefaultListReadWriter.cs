// <copyright file="DefaultListReadWriter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.ReadWriters
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Concierge.Character.Characteristics;
    using Concierge.Character.Items;
    using Concierge.Character.Spellcasting;
    using Newtonsoft.Json;

    public static class DefaultListReadWriter
    {
        public static List<Ability> ReadAbilityList()
        {
            var abilities = new List<Ability>();

            try
            {
                var rawJson = Encoding.Default.GetString(Properties.Resources.Ability);
                abilities = JsonConvert.DeserializeObject<List<Ability>>(rawJson);

                Program.Logger.Info("Abilities loaded successfully.");
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }
            finally
            {
                if (abilities is null)
                {
                    abilities = new List<Ability>();
                }
            }

            return abilities;
        }

        public static List<Ammunition> ReadAmmunitionList()
        {
            var ammunitions = new List<Ammunition>();

            try
            {
                var rawJson = Encoding.Default.GetString(Properties.Resources.Ammunition);
                ammunitions = JsonConvert.DeserializeObject<List<Ammunition>>(rawJson);

                Program.Logger.Info("Ammunition loaded successfully.");
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }
            finally
            {
                if (ammunitions is null)
                {
                    ammunitions = new List<Ammunition>();
                }
            }

            return ammunitions;
        }

        public static List<Inventory> ReadInventoryList()
        {
            var inventories = new List<Inventory>();

            try
            {
                var rawJson = Encoding.Default.GetString(Properties.Resources.Inventory);
                inventories = JsonConvert.DeserializeObject<List<Inventory>>(rawJson);

                Program.Logger.Info("Items loaded successfully.");
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }
            finally
            {
                if (inventories is null)
                {
                    inventories = new List<Inventory>();
                }
            }

            return inventories;
        }

        public static List<Language> ReadLanguageList()
        {
            var languages = new List<Language>();

            try
            {
                var rawJson = Encoding.Default.GetString(Properties.Resources.Language);
                languages = JsonConvert.DeserializeObject<List<Language>>(rawJson);

                Program.Logger.Info("Languages loaded successfully.");
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }
            finally
            {
                if (languages is null)
                {
                    languages = new List<Language>();
                }
            }

            return languages;
        }

        public static List<Spell> ReadSpellList()
        {
            var spells = new List<Spell>();

            try
            {
                var rawJson = Encoding.Default.GetString(Properties.Resources.Spell);
                spells = JsonConvert.DeserializeObject<List<Spell>>(rawJson);

                Program.Logger.Info("Spells loaded successfully.");
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }
            finally
            {
                if (spells is null)
                {
                    spells = new List<Spell>();
                }
            }

            return spells;
        }

        public static List<Weapon> ReadWeaponList()
        {
            var weapons = new List<Weapon>();

            try
            {
                var rawJson = Encoding.Default.GetString(Properties.Resources.Weapon);
                weapons = JsonConvert.DeserializeObject<List<Weapon>>(rawJson);

                Program.Logger.Info("Weapons loaded successfully.");
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }
            finally
            {
                if (weapons is null)
                {
                    weapons = new List<Weapon>();
                }
            }

            return weapons;
        }
    }
}
