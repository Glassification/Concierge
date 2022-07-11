// <copyright file="CharacterUtility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Utilities
{
    using System;
    using System.Collections.Generic;

    using Concierge.Character;
    using Concierge.Character.Characteristics;
    using Concierge.Character.Definitions;
    using Concierge.Character.Dtos;
    using Concierge.Character.Enums;

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0066:Convert switch statement to expression", Justification = "Improve Readability.")]
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
                "Artificer" => SpellSlotDefinitions.GetArtificerSpellSlotIncrease(level),
                "Bard" => SpellSlotDefinitions.GetBardSpellSlotIncrease(level),
                "Blood Hunter" => SpellSlotDefinitions.GetBloodHunterSpellSlotIncrease(level),
                "Cleric" => SpellSlotDefinitions.GetClericSpellSlotIncrease(level),
                "Druid" => SpellSlotDefinitions.GetDruidSpellSlotIncrease(level),
                "Paladin" => SpellSlotDefinitions.GetPaladinSpellSlotIncrease(level),
                "Ranger" => SpellSlotDefinitions.GetRangerSpellSlotIncrease(level),
                "Rogue" => SpellSlotDefinitions.GetRogueSpellSlotIncrease(level),
                "Sorcerer" => SpellSlotDefinitions.GetSorcererSpellSlotIncrease(level),
                "Warlock" => SpellSlotDefinitions.GetWarlockSpellSlotIncrease(level),
                "Wizard" => SpellSlotDefinitions.GetWizardSpellSlotIncrease(level),
                _ => new SpellSlotDto(),
            };
        }

        public static List<Proficiency> GetProficiencies(string className, bool multiClass)
        {
            return className switch
            {
                "Artificer" => ClassProficiencyDefinitions.GetArtificerProficiencies(multiClass),
                "Barbarian" => ClassProficiencyDefinitions.GetBarbarianProficiencies(multiClass),
                "Bard" => ClassProficiencyDefinitions.GetBardProficiencies(multiClass),
                "Blood Hunter" => ClassProficiencyDefinitions.GetBloodHunterProficiencies(multiClass),
                "Cleric" => ClassProficiencyDefinitions.GetClericProficiencies(multiClass),
                "Druid" => ClassProficiencyDefinitions.GetDruidProficiencies(multiClass),
                "Fighter" => ClassProficiencyDefinitions.GetFighterProficiencies(multiClass),
                "Monk" => ClassProficiencyDefinitions.GetMonkProficiencies(multiClass),
                "Paladin" => ClassProficiencyDefinitions.GetPaladinProficiencies(multiClass),
                "Ranger" => ClassProficiencyDefinitions.GetRangerProficiencies(multiClass),
                "Rogue" => ClassProficiencyDefinitions.GetRogueProficiencies(multiClass),
                "Sorcerer" => ClassProficiencyDefinitions.GetSorcererProficiencies(multiClass),
                "Warlock" => ClassProficiencyDefinitions.GetWarlockProficiencies(multiClass),
                "Wizard" => ClassProficiencyDefinitions.GetWizardProficiencies(multiClass),
                _ => new List<Proficiency>(),
            };
        }

        public static SavingThrowDto GetSavingThrows(string className)
        {
            return className switch
            {
                "Artificer" => SavingThrowDefinitions.GetArtificerSavingThrows(),
                "Barbarian" => SavingThrowDefinitions.GetBarbarianSavingThrows(),
                "Bard" => SavingThrowDefinitions.GetBardSavingThrows(),
                "Blood Hunter" => SavingThrowDefinitions.GetBlooodHunterSavingThrows(),
                "Cleric" => SavingThrowDefinitions.GetClericSavingThrows(),
                "Druid" => SavingThrowDefinitions.GetDruidSavingThrows(),
                "Fighter" => SavingThrowDefinitions.GetFighterSavingThrows(),
                "Monk" => SavingThrowDefinitions.GetMonkSavingThrows(),
                "Paladin" => SavingThrowDefinitions.GetPaladinSavingThrows(),
                "Ranger" => SavingThrowDefinitions.GetRangerSavingThrows(),
                "Rogue" => SavingThrowDefinitions.GetRogueSavingThrows(),
                "Sorcerer" => SavingThrowDefinitions.GetSorcererSavingThrows(),
                "Warlock" => SavingThrowDefinitions.GetWarlockSavingThrows(),
                "Wizard" => SavingThrowDefinitions.GetWizardSavingThrows(),
                _ => new SavingThrowDto(),
            };
        }
    }
}
