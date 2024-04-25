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

    /// <summary>
    /// Provides mappings and utility methods related to character leveling.
    /// </summary>
    public static class LevelingMap
    {
        /// <summary>
        /// Gets the spell slot increase for a given class, subclass, and level.
        /// </summary>
        /// <param name="className">The name of the character's class.</param>
        /// <param name="subclassName">The name of the character's subclass.</param>
        /// <param name="level">The level at which the spell slot increase occurs.</param>
        /// <returns>A <see cref="SpellSlotDto"/> representing the spell slot increase.</returns>
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

        /// <summary>
        /// Gets the proficiencies for a given class and multiclassing status.
        /// </summary>
        /// <param name="className">The name of the character's class.</param>
        /// <param name="multiClass">A boolean indicating if the character is multiclassing.</param>
        /// <returns>A list of <see cref="Proficiency"/> objects representing the character's proficiencies.</returns>
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

        /// <summary>
        /// Gets the saving throws for a given class.
        /// </summary>
        /// <param name="className">The name of the character's class.</param>
        /// <returns>A <see cref="SavingThrowDto"/> representing the character's saving throws.</returns>
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

        /// <summary>
        /// Gets the class resource increase for a given class and subclass at a specified level.
        /// </summary>
        /// <param name="className">The name of the character's class.</param>
        /// <param name="subclass">The subclass of the character's class.</param>
        /// <param name="level">The level at which to calculate the resource increase.</param>
        /// <returns>A <see cref="ClassResourceDto"/> representing the class resource increase.</returns>
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

        /// <summary>
        /// Gets the senses for a given race.
        /// </summary>
        /// <param name="race">The race of the character.</param>
        /// <returns>A <see cref="SensesDto"/> representing the race's senses.</returns>
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
