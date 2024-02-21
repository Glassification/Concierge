// <copyright file="LevelingMap.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling
{
    using System.Collections.Generic;

    using Concierge.Character.Details;
    using Concierge.Character.Dispositions;
    using Concierge.Leveling.Definitions;
    using Concierge.Leveling.Dtos.Definitions;

    public static class LevelingMap
    {
        public static SpellSlotDto GetSpellSlotIncrease(string className, string subclassName, int level)
        {
            var spellSlots = className switch
            {
                "Artificer" => SpellSlotDefinitions.GetArtificerSpellSlotIncrease(level),
                "Bard" => SpellSlotDefinitions.GetBardSpellSlotIncrease(level),
                "Blood Hunter" => SpellSlotDefinitions.GetBloodHunterSpellSlotIncrease(level),
                "Cleric" => SpellSlotDefinitions.GetClericSpellSlotIncrease(level),
                "Druid" => SpellSlotDefinitions.GetDruidSpellSlotIncrease(level),
                "Paladin" => SpellSlotDefinitions.GetPaladinSpellSlotIncrease(level),
                "Ranger" => SpellSlotDefinitions.GetRangerSpellSlotIncrease(level),
                "Arcane Trickster" => SpellSlotDefinitions.GetRogueSpellSlotIncrease(level),
                "Sorcerer" => SpellSlotDefinitions.GetSorcererSpellSlotIncrease(level),
                "Warlock" => SpellSlotDefinitions.GetWarlockSpellSlotIncrease(level),
                "Wizard" => SpellSlotDefinitions.GetWizardSpellSlotIncrease(level),
                _ => new SpellSlotDto(),
            };

            spellSlots = subclassName switch
            {
                "Artificer" => SpellSlotDefinitions.GetArtificerSpellSlotIncrease(level),
                "Bard" => SpellSlotDefinitions.GetBardSpellSlotIncrease(level),
                "Blood Hunter" => SpellSlotDefinitions.GetBloodHunterSpellSlotIncrease(level),
                "Cleric" => SpellSlotDefinitions.GetClericSpellSlotIncrease(level),
                "Druid" => SpellSlotDefinitions.GetDruidSpellSlotIncrease(level),
                "Paladin" => SpellSlotDefinitions.GetPaladinSpellSlotIncrease(level),
                "Ranger" => SpellSlotDefinitions.GetRangerSpellSlotIncrease(level),
                "Arcane Trickster" => SpellSlotDefinitions.GetRogueSpellSlotIncrease(level),
                "Sorcerer" => SpellSlotDefinitions.GetSorcererSpellSlotIncrease(level),
                "Warlock" => SpellSlotDefinitions.GetWarlockSpellSlotIncrease(level),
                "Wizard" => SpellSlotDefinitions.GetWizardSpellSlotIncrease(level),
                _ => spellSlots,
            };

            return spellSlots;
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
                _ => [],
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

        public static ClassResourceDto GetResourceIncrease(string className, string subclass, int level)
        {
            return className switch
            {
                "Artificer" => ClassResourceDefinitions.GetArtificerResourceIncrease(level),
                "Barbarian" => ClassResourceDefinitions.GetBarbarianResourceIncrease(level),
                "Bard" => ClassResourceDefinitions.GetBardResourceIncrease(level),
                "Fighter" => subclass.Equals("Champion") ? ClassResourceDefinitions.GetFighterResourceIncrease(level) : new ClassResourceDto(),
                "Monk" => ClassResourceDefinitions.GetMonkResourceIncrease(level),
                "Rogue" => ClassResourceDefinitions.GetRogueResourceIncrease(level),
                "Sorcerer" => ClassResourceDefinitions.GetSorcererResourceIncrease(level),
                _ => new ClassResourceDto(),
            };
        }

        public static SensesDto GetRaceSenses(Race race)
        {
            return race.Name switch
            {
                "Dwarf" => RaceSensesDefinition.GetDwarfSenses(),
                "Elf" => RaceSensesDefinition.GetElfSenses(race.Subrace),
                "Halfling" => RaceSensesDefinition.GetHalflingSenses(),
                "Human" => RaceSensesDefinition.GetHumanSenses(),
                "Dragonborn" => RaceSensesDefinition.GetDragonbornSenses(),
                "Gnome" => RaceSensesDefinition.GetGnomeSenses(),
                "Half-Elf" => RaceSensesDefinition.GetHalfElfSenses(),
                "Half-Orc" => RaceSensesDefinition.GetHalfOrcSenses(),
                "Tiefling" => RaceSensesDefinition.GetTieflingSenses(),
                _ => new SensesDto(),
            };
        }
    }
}
