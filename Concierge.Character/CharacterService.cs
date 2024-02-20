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
    using Concierge.Common.Extensions;

    public sealed class CharacterService
    {
        private readonly CharacterSheet characterSheet;

        public CharacterService(CharacterSheet characterSheet)
        {
            this.characterSheet = characterSheet;
        }

        public int CalculateBonus(Abilities ability)
        {
            return ability switch
            {
                Abilities.STR => this.characterSheet.Attributes.Strength.Bonus + this.characterSheet.ProficiencyBonus,
                Abilities.DEX => this.characterSheet.Attributes.Dexterity.Bonus + this.characterSheet.ProficiencyBonus,
                Abilities.CON => this.characterSheet.Attributes.Constitution.Bonus + this.characterSheet.ProficiencyBonus,
                Abilities.INT => this.characterSheet.Attributes.Intelligence.Bonus + this.characterSheet.ProficiencyBonus,
                Abilities.WIS => this.characterSheet.Attributes.Wisdom.Bonus + this.characterSheet.ProficiencyBonus,
                Abilities.CHA => this.characterSheet.Attributes.Charisma.Bonus + this.characterSheet.ProficiencyBonus,
                Abilities.NONE => this.characterSheet.ProficiencyBonus,
                _ => this.characterSheet.ProficiencyBonus,
            };
        }

        public List<Spell> ListPreparedSpells(string @class)
        {
            return this.characterSheet.SpellCasting.Spells.Where(x => x.Class.Equals(@class) && x.Level > 0 && x.Prepared).ToList();
        }

        public int GetProficiencyBonus(CompanionWeapon weapon)
        {
            var bonus = this.characterSheet.ProficiencyBonus;
            if (weapon.ProficiencyOverride)
            {
                return bonus;
            }

            return 0;
        }

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
