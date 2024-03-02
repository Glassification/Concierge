// <copyright file="CharacterService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character
{
    using System.Collections.Generic;
    using System.Linq;

    using Concierge.Character.Companions;
    using Concierge.Character.Details;
    using Concierge.Character.Enums;
    using Concierge.Character.Equipable;
    using Concierge.Character.Magic;
    using Concierge.Common;
    using Concierge.Common.Extensions;

    /// <summary>
    /// Provides services for calculating character-related bonuses and accessing character information.
    /// </summary>
    public sealed class CharacterService
    {
        private readonly CharacterSheet characterSheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterService"/> class with the specified character sheet.
        /// </summary>
        /// <param name="characterSheet">The character sheet to be used by the service.</param>
        public CharacterService(CharacterSheet characterSheet)
        {
            this.characterSheet = characterSheet;
        }

        /// <summary>
        /// Calculates the bonus for the specified ability.
        /// </summary>
        /// <param name="ability">The ability for which to calculate the bonus.</param>
        /// <returns>The bonus value calculated for the ability.</returns>
        public int CalculateBonus(Abilities ability)
        {
            return ability switch
            {
                Abilities.STR => this.characterSheet.Attributes.Strength.Bonus,
                Abilities.DEX => this.characterSheet.Attributes.Dexterity.Bonus,
                Abilities.CON => this.characterSheet.Attributes.Constitution.Bonus,
                Abilities.INT => this.characterSheet.Attributes.Intelligence.Bonus,
                Abilities.WIS => this.characterSheet.Attributes.Wisdom.Bonus,
                Abilities.CHA => this.characterSheet.Attributes.Charisma.Bonus,
                Abilities.NONE => 0,
                _ => 0,
            };
        }

        /// <summary>
        /// Calculates the bonus for the specified ability, including proficiency bonus.
        /// </summary>
        /// <param name="ability">The ability for which to calculate the bonus.</param>
        /// <returns>The bonus value calculated for the ability, including proficiency bonus.</returns>
        public int CalculateBonusWithProficiency(Abilities ability)
        {
            return this.CalculateBonus(ability) + this.characterSheet.ProficiencyBonus;
        }

        /// <summary>
        /// Calculates the bonus for the specified ability of the companion.
        /// </summary>
        /// <param name="ability">The ability for which to calculate the bonus.</param>
        /// <returns>The bonus value calculated for the ability of the companion.</returns>
        public int CalculateCompanionBonus(Abilities ability)
        {
            return ability switch
            {
                Abilities.STR => Constants.Bonus(this.characterSheet.Companion.Attributes.Strength),
                Abilities.DEX => Constants.Bonus(this.characterSheet.Companion.Attributes.Dexterity),
                Abilities.CON => Constants.Bonus(this.characterSheet.Companion.Attributes.Constitution),
                Abilities.INT => Constants.Bonus(this.characterSheet.Companion.Attributes.Intelligence),
                Abilities.WIS => Constants.Bonus(this.characterSheet.Companion.Attributes.Wisdom),
                Abilities.CHA => Constants.Bonus(this.characterSheet.Companion.Attributes.Charisma),
                Abilities.NONE => 0,
                _ => 0,
            };
        }

        /// <summary>
        /// Lists the prepared spells of the specified class.
        /// </summary>
        /// <param name="class">The class for which to list the prepared spells.</param>
        /// <returns>A list of prepared spells for the specified class.</returns>
        public List<Spell> ListPreparedSpells(string @class)
        {
            return this.characterSheet.SpellCasting.Spells.Where(x => x.Class?.Equals(@class) ?? false && x.Level > 0 && x.Prepared).ToList();
        }

        /// <summary>
        /// Gets the proficiency bonus for the specified companion weapon.
        /// </summary>
        /// <param name="weapon">The companion weapon for which to get the proficiency bonus.</param>
        /// <returns>The proficiency bonus for the specified companion weapon.</returns>
        public int GetProficiencyBonus(CompanionWeapon weapon)
        {
            var bonus = this.characterSheet.ProficiencyBonus;
            if (weapon.ProficiencyOverride)
            {
                return bonus;
            }

            return 0;
        }

        /// <summary>
        /// Gets the proficiency bonus for the specified weapon.
        /// </summary>
        /// <param name="weapon">The weapon for which to get the proficiency bonus.</param>
        /// <returns>The proficiency bonus for the specified weapon.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0066:Convert switch statement to expression", Justification = "More readable.")]
        public int GetProficiencyBonus(Weapon weapon)
        {
            var bonus = this.characterSheet.ProficiencyBonus;
            var weaponName = weapon.Type.ToString().FormatFromPascalCase();
            if (weapon.ProficiencyOverride)
            {
                return bonus;
            }

            var proficiencies = this.characterSheet.Detail.Proficiencies;
            if (proficiencies.Any(x => x.Name.Equals(weaponName) && x.ProficiencyType == ProficiencyTypes.Weapon))
            {
                return bonus;
            }

            switch (weapon.Type)
            {
                case WeaponTypes.LightCrossbow:
                case WeaponTypes.Dart:
                case WeaponTypes.Shortbow:
                case WeaponTypes.Sling:
                    return proficiencies.Any(x => x.Name.Equals(Proficiency.SimpleRanged) && x.ProficiencyType == ProficiencyTypes.Weapon) ? bonus : 0;
                case WeaponTypes.Club:
                case WeaponTypes.Dagger:
                case WeaponTypes.Greatclub:
                case WeaponTypes.Handaxe:
                case WeaponTypes.Javelin:
                case WeaponTypes.LightHammer:
                case WeaponTypes.Mace:
                case WeaponTypes.Quarterstaff:
                case WeaponTypes.Sickle:
                case WeaponTypes.Spear:
                    return proficiencies.Any(x => x.Name.Equals(Proficiency.SimpleMelee) && x.ProficiencyType == ProficiencyTypes.Weapon) ? bonus : 0;
                case WeaponTypes.Blowgun:
                case WeaponTypes.HandCrossbow:
                case WeaponTypes.HeavyCrossbow:
                case WeaponTypes.Longbow:
                case WeaponTypes.Net:
                    return proficiencies.Any(x => x.Name.Equals(Proficiency.MartialRanged) && x.ProficiencyType == ProficiencyTypes.Weapon) ? bonus : 0;
                case WeaponTypes.Battleaxe:
                case WeaponTypes.Flail:
                case WeaponTypes.Glaive:
                case WeaponTypes.Greataxe:
                case WeaponTypes.Greatsword:
                case WeaponTypes.Halberd:
                case WeaponTypes.Lance:
                case WeaponTypes.Longsword:
                case WeaponTypes.Maul:
                case WeaponTypes.Morningstar:
                case WeaponTypes.Pike:
                case WeaponTypes.Rapier:
                case WeaponTypes.Scimitar:
                case WeaponTypes.Shortsword:
                case WeaponTypes.Trident:
                case WeaponTypes.WarPick:
                case WeaponTypes.Warhammer:
                case WeaponTypes.Whip:
                    return proficiencies.Any(x => x.Name.Equals(Proficiency.SimpleRanged) && x.ProficiencyType == ProficiencyTypes.Weapon) ? bonus : 0;
                default:
                    return 0;
            }
        }
    }
}
