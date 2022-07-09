// <copyright file="CharacterUtility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Utilities
{
    using System;

    using Concierge.Character;
    using Concierge.Character.Enums;
    using Concierge.Utility.Dtos;
    using Concierge.Utility.Helpers;

    public static class CharacterUtility
    {
        public static int CalculateBonus(int score)
        {
            return (int)Math.Floor((score - 10) / 2.0);
        }

        public static int CalculateBonusFromAbility(Abilities ability, ConciergeCharacter character)
        {
            int bonus = character.ProficiencyBonus;

            return ability switch
            {
                Abilities.STR => CalculateBonus(character.Attributes.Strength) + bonus,
                Abilities.DEX => CalculateBonus(character.Attributes.Dexterity) + bonus,
                Abilities.CON => CalculateBonus(character.Attributes.Constitution) + bonus,
                Abilities.INT => CalculateBonus(character.Attributes.Intelligence) + bonus,
                Abilities.WIS => CalculateBonus(character.Attributes.Wisdom) + bonus,
                Abilities.CHA => CalculateBonus(character.Attributes.Charisma) + bonus,
                Abilities.NONE => bonus,
                _ => throw new NotImplementedException(),
            };
        }

        public static bool ValidateClassLevel(ConciergeCharacter character, int number, int newValue)
        {
            var totalLevel =
                (character.Properties.Class1.ClassNumber == number ? 0 : character.Properties.Class1.Level) +
                (character.Properties.Class2.ClassNumber == number ? 0 : character.Properties.Class2.Level) +
                (character.Properties.Class3.ClassNumber == number ? 0 : character.Properties.Class3.Level);

            totalLevel += newValue;

            return totalLevel is <= Constants.MaxLevel and >= 0;
        }

        public static double GetGoldValue(double value, CoinType coinType)
        {
            return coinType switch
            {
                CoinType.Copper => value / 100.0,
                CoinType.Silver => value / 10.0,
                CoinType.Electrum => value / 2.0,
                CoinType.Gold => value,
                CoinType.Platinum => value * 10.0,
                _ => throw new NotImplementedException(),
            };
        }

        public static HitDie GetHitDice(string className)
        {
            switch (className)
            {
                case "Sorcerer":
                case "Wizard":
                default:
                    return HitDie.D6;
                case "Artificer":
                case "Bard":
                case "Cleric":
                case "Druid":
                case "Gunslinger":
                case "Monk":
                case "Rogue":
                case "Warlock":
                    return HitDie.D8;
                case "Blood Hunter":
                case "Fighter":
                case "Paladin":
                case "Ranger":
                    return HitDie.D10;
                case "Barbarian":
                    return HitDie.D12;
            }
        }

        public static SpellSlotDto GetSpellSlotIncrease(string className, int level)
        {
            return className switch
            {
                "Bard" => SpellSlotMapper.GetBardSpellSlotIncrease(level),
                "Cleric" => SpellSlotMapper.GetClericSpellSlotIncrease(level),
                "Druid" => SpellSlotMapper.GetDruidSpellSlotIncrease(level),
                "Paladin" => SpellSlotMapper.GetPaladinSpellSlotIncrease(level),
                "Ranger" => SpellSlotMapper.GetRangerSpellSlotIncrease(level),
                "Rogue" => SpellSlotMapper.GetRogueSpellSlotIncrease(level),
                "Sorcerer" => SpellSlotMapper.GetSorcererSpellSlotIncrease(level),
                "Warlock" => SpellSlotMapper.GetWarlockSpellSlotIncrease(level),
                "Wizard" => SpellSlotMapper.GetWizardSpellSlotIncrease(level),
                _ => new SpellSlotDto(),
            };
        }
    }
}
