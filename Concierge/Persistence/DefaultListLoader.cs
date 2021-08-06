// <copyright file="DefaultListLoader.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Concierge.Characters.Characteristics;
    using Concierge.Characters.Items;
    using Concierge.Characters.Spellcasting;
    using Concierge.Exceptions.Enums;
    using Newtonsoft.Json;

    public static class DefaultListLoader
    {
        public static List<Ability> LoadAbilityList()
        {
            var abilities = new List<Ability>();

            try
            {
                var rawJson = Encoding.Default.GetString(Properties.Resources.Ability);
                abilities = JsonConvert.DeserializeObject<List<Ability>>(rawJson);

                Program.Logger.Info("Languages loaded successfully.");
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex, Severity.Release);
            }

            return abilities;
        }

        public static List<Ammunition> LoadAmmunitionList()
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
                Program.ErrorService.LogError(ex, Severity.Release);
            }

            return ammunitions;
        }

        public static List<Inventory> LoadInventoryList()
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
                Program.ErrorService.LogError(ex, Severity.Release);
            }

            return inventories;
        }

        public static List<Language> LoadLanguageList()
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
                Program.ErrorService.LogError(ex, Severity.Release);
            }

            return languages;
        }

        public static List<Spell> LoadSpellList()
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
                Program.ErrorService.LogError(ex, Severity.Release);
            }

            return spells;
        }

        public static List<Weapon> LoadWeaponList()
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
                Program.ErrorService.LogError(ex, Severity.Release);
            }

            return weapons;
        }
    }
}
