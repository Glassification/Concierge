// <copyright file="AutoSelection.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.ImportService
{
    using System.IO;

    using Concierge.Common.Extensions;
    using Concierge.Services.Enums;

    public static class AutoSelection
    {
        private static readonly string[] AbilityMatch = new string[] { "ability", "abilities", "abilitys" };
        private static readonly string[] AmmunitionMatch = new string[] { "ammunition", "ammunitions", "ammo", "ammos" };
        private static readonly string[] InventoryMatch = new string[] { "inventory", "inventorys", "inventories" };
        private static readonly string[] JournalMatch = new string[] { "journal", "journals", "chapter", "chapters", "note", "notes" };
        private static readonly string[] LanguageMatch = new string[] { "language", "languages" };
        private static readonly string[] ProficiencyMatch = new string[] { "proficiency", "proficiencys", "proficiencies", "proficiancy", "proficiancys", "proficiancies" };
        private static readonly string[] SpellMatch = new string[] { "spell", "spells", "magic" };
        private static readonly string[] WeaponMatch = new string[] { "weapon", "weapons", "attack", "attacks" };

        public static bool FuzzySearch(string text, SelectionType selectionType)
        {
            if (text.IsNullOrWhiteSpace())
            {
                return false;
            }

            var fileName = Path.GetFileNameWithoutExtension(text).ToLower();
            return selectionType switch
            {
                SelectionType.Ability => ListContains(AbilityMatch, fileName),
                SelectionType.Ammunition => ListContains(AmmunitionMatch, fileName),
                SelectionType.Inventory => ListContains(InventoryMatch, fileName),
                SelectionType.Journal => ListContains(JournalMatch, fileName),
                SelectionType.Language => ListContains(LanguageMatch, fileName),
                SelectionType.Proficiency => ListContains(ProficiencyMatch, fileName),
                SelectionType.Spell => ListContains(SpellMatch, fileName),
                SelectionType.Weapon => ListContains(WeaponMatch, fileName),
                _ => false,
            };
        }

        private static bool ListContains(string[] list, string text)
        {
            foreach (var item in list)
            {
                if (item.Contains(text))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
