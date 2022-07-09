// <copyright file="SpellSlotMapper.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility.Helpers
{
    using Concierge.Utility.Dtos;

    public static class SpellSlotMapper
    {
        public static SpellSlotDto GetBardSpellSlotIncrease(int level)
        {
            return $"Bard {level}" switch
            {
                "Bard 1" => new SpellSlotDto(known: 4, slots: 2, cantrip: 2, first: 2),
                "Bard 2" => new SpellSlotDto(known: 1, slots: 1, first: 1),
                "Bard 3" => new SpellSlotDto(known: 1, slots: 3, second: 1, third: 2),
                "Bard 4" => new SpellSlotDto(known: 1, slots: 1, cantrip: 1, second: 1),
                "Bard 5" => new SpellSlotDto(known: 1, slots: 2, third: 2),
                "Bard 6" => new SpellSlotDto(known: 1, slots: 1, third: 1),
                "Bard 7" => new SpellSlotDto(known: 1, slots: 1, fourth: 1),
                "Bard 8" => new SpellSlotDto(known: 1, slots: 1, fourth: 1),
                "Bard 9" => new SpellSlotDto(known: 1, slots: 2, fourth: 1, fifth: 1),
                "Bard 10" => new SpellSlotDto(known: 1, slots: 1, cantrip: 1, fifth: 1),
                "Bard 11" => new SpellSlotDto(known: 1, slots: 1, sixth: 1),
                "Bard 12" => new SpellSlotDto(),
                "Bard 13" => new SpellSlotDto(known: 1, slots: 1, seventh: 1),
                "Bard 14" => new SpellSlotDto(known: 2),
                "Bard 15" => new SpellSlotDto(known: 1, slots: 1, eighth: 1),
                "Bard 16" => new SpellSlotDto(),
                "Bard 17" => new SpellSlotDto(known: 1, slots: 1, nineth: 1),
                "Bard 18" => new SpellSlotDto(known: 2, slots: 1, fifth: 1),
                "Bard 19" => new SpellSlotDto(slots: 1, sixth: 1),
                "Bard 20" => new SpellSlotDto(slots: 1, seventh: 1),
                _ => new SpellSlotDto(),
            };
        }

        public static SpellSlotDto GetClericSpellSlotIncrease(int level)
        {
            return $"Cleric {level}" switch
            {
                "Cleric 1" => new SpellSlotDto(known: 1, cantrip: 3, slots: 2, first: 2),
                "Cleric 2" => new SpellSlotDto(),
                "Cleric 3" => new SpellSlotDto(),
                "Cleric 4" => new SpellSlotDto(),
                "Cleric 5" => new SpellSlotDto(),
                "Cleric 6" => new SpellSlotDto(),
                "Cleric 7" => new SpellSlotDto(),
                "Cleric 8" => new SpellSlotDto(),
                "Cleric 9" => new SpellSlotDto(),
                "Cleric 10" => new SpellSlotDto(),
                "Cleric 11" => new SpellSlotDto(),
                "Cleric 12" => new SpellSlotDto(),
                "Cleric 13" => new SpellSlotDto(),
                "Cleric 14" => new SpellSlotDto(),
                "Cleric 15" => new SpellSlotDto(),
                "Cleric 16" => new SpellSlotDto(),
                "Cleric 17" => new SpellSlotDto(),
                "Cleric 18" => new SpellSlotDto(),
                "Cleric 19" => new SpellSlotDto(),
                "Cleric 20" => new SpellSlotDto(),
                _ => new SpellSlotDto(),
            };
        }

        public static SpellSlotDto GetDruidSpellSlotIncrease(int level)
        {
            return $"Druid {level}" switch
            {
                "Druid 1" => new SpellSlotDto(),
                "Druid 2" => new SpellSlotDto(),
                "Druid 3" => new SpellSlotDto(),
                "Druid 4" => new SpellSlotDto(),
                "Druid 5" => new SpellSlotDto(),
                "Druid 6" => new SpellSlotDto(),
                "Druid 7" => new SpellSlotDto(),
                "Druid 8" => new SpellSlotDto(),
                "Druid 9" => new SpellSlotDto(),
                "Druid 10" => new SpellSlotDto(),
                "Druid 11" => new SpellSlotDto(),
                "Druid 12" => new SpellSlotDto(),
                "Druid 13" => new SpellSlotDto(),
                "Druid 14" => new SpellSlotDto(),
                "Druid 15" => new SpellSlotDto(),
                "Druid 16" => new SpellSlotDto(),
                "Druid 17" => new SpellSlotDto(),
                "Druid 18" => new SpellSlotDto(),
                "Druid 19" => new SpellSlotDto(),
                "Druid 20" => new SpellSlotDto(),
                _ => new SpellSlotDto(),
            };
        }

        public static SpellSlotDto GetPaladinSpellSlotIncrease(int level)
        {
            return $"Paladin {level}" switch
            {
                "Paladin 1" => new SpellSlotDto(),
                "Paladin 2" => new SpellSlotDto(),
                "Paladin 3" => new SpellSlotDto(),
                "Paladin 4" => new SpellSlotDto(),
                "Paladin 5" => new SpellSlotDto(),
                "Paladin 6" => new SpellSlotDto(),
                "Paladin 7" => new SpellSlotDto(),
                "Paladin 8" => new SpellSlotDto(),
                "Paladin 9" => new SpellSlotDto(),
                "Paladin 10" => new SpellSlotDto(),
                "Paladin 11" => new SpellSlotDto(),
                "Paladin 12" => new SpellSlotDto(),
                "Paladin 13" => new SpellSlotDto(),
                "Paladin 14" => new SpellSlotDto(),
                "Paladin 15" => new SpellSlotDto(),
                "Paladin 16" => new SpellSlotDto(),
                "Paladin 17" => new SpellSlotDto(),
                "Paladin 18" => new SpellSlotDto(),
                "Paladin 19" => new SpellSlotDto(),
                "Paladin 20" => new SpellSlotDto(),
                _ => new SpellSlotDto(),
            };
        }

        public static SpellSlotDto GetRangerSpellSlotIncrease(int level)
        {
            return $"Ranger {level}" switch
            {
                "Ranger 1" => new SpellSlotDto(),
                "Ranger 2" => new SpellSlotDto(),
                "Ranger 3" => new SpellSlotDto(),
                "Ranger 4" => new SpellSlotDto(),
                "Ranger 5" => new SpellSlotDto(),
                "Ranger 6" => new SpellSlotDto(),
                "Ranger 7" => new SpellSlotDto(),
                "Ranger 8" => new SpellSlotDto(),
                "Ranger 9" => new SpellSlotDto(),
                "Ranger 10" => new SpellSlotDto(),
                "Ranger 11" => new SpellSlotDto(),
                "Ranger 12" => new SpellSlotDto(),
                "Ranger 13" => new SpellSlotDto(),
                "Ranger 14" => new SpellSlotDto(),
                "Ranger 15" => new SpellSlotDto(),
                "Ranger 16" => new SpellSlotDto(),
                "Ranger 17" => new SpellSlotDto(),
                "Ranger 18" => new SpellSlotDto(),
                "Ranger 19" => new SpellSlotDto(),
                "Ranger 20" => new SpellSlotDto(),
                _ => new SpellSlotDto(),
            };
        }

        public static SpellSlotDto GetRogueSpellSlotIncrease(int level)
        {
            return $"Rogue {level}" switch
            {
                "Rogue 1" => new SpellSlotDto(),
                "Rogue 2" => new SpellSlotDto(),
                "Rogue 3" => new SpellSlotDto(),
                "Rogue 4" => new SpellSlotDto(),
                "Rogue 5" => new SpellSlotDto(),
                "Rogue 6" => new SpellSlotDto(),
                "Rogue 7" => new SpellSlotDto(),
                "Rogue 8" => new SpellSlotDto(),
                "Rogue 9" => new SpellSlotDto(),
                "Rogue 10" => new SpellSlotDto(),
                "Rogue 11" => new SpellSlotDto(),
                "Rogue 12" => new SpellSlotDto(),
                "Rogue 13" => new SpellSlotDto(),
                "Rogue 14" => new SpellSlotDto(),
                "Rogue 15" => new SpellSlotDto(),
                "Rogue 16" => new SpellSlotDto(),
                "Rogue 17" => new SpellSlotDto(),
                "Rogue 18" => new SpellSlotDto(),
                "Rogue 19" => new SpellSlotDto(),
                "Rogue 20" => new SpellSlotDto(),
                _ => new SpellSlotDto(),
            };
        }

        public static SpellSlotDto GetSorcererSpellSlotIncrease(int level)
        {
            return $"Sorcerer {level}" switch
            {
                "Sorcerer 1" => new SpellSlotDto(),
                "Sorcerer 2" => new SpellSlotDto(),
                "Sorcerer 3" => new SpellSlotDto(),
                "Sorcerer 4" => new SpellSlotDto(),
                "Sorcerer 5" => new SpellSlotDto(),
                "Sorcerer 6" => new SpellSlotDto(),
                "Sorcerer 7" => new SpellSlotDto(),
                "Sorcerer 8" => new SpellSlotDto(),
                "Sorcerer 9" => new SpellSlotDto(),
                "Sorcerer 10" => new SpellSlotDto(),
                "Sorcerer 11" => new SpellSlotDto(),
                "Sorcerer 12" => new SpellSlotDto(),
                "Sorcerer 13" => new SpellSlotDto(),
                "Sorcerer 14" => new SpellSlotDto(),
                "Sorcerer 15" => new SpellSlotDto(),
                "Sorcerer 16" => new SpellSlotDto(),
                "Sorcerer 17" => new SpellSlotDto(),
                "Sorcerer 18" => new SpellSlotDto(),
                "Sorcerer 19" => new SpellSlotDto(),
                "Sorcerer 20" => new SpellSlotDto(),
                _ => new SpellSlotDto(),
            };
        }

        public static SpellSlotDto GetWarlockSpellSlotIncrease(int level)
        {
            return $"Warlock {level}" switch
            {
                "Warlock 1" => new SpellSlotDto(),
                "Warlock 2" => new SpellSlotDto(),
                "Warlock 3" => new SpellSlotDto(),
                "Warlock 4" => new SpellSlotDto(),
                "Warlock 5" => new SpellSlotDto(),
                "Warlock 6" => new SpellSlotDto(),
                "Warlock 7" => new SpellSlotDto(),
                "Warlock 8" => new SpellSlotDto(),
                "Warlock 9" => new SpellSlotDto(),
                "Warlock 10" => new SpellSlotDto(),
                "Warlock 11" => new SpellSlotDto(),
                "Warlock 12" => new SpellSlotDto(),
                "Warlock 13" => new SpellSlotDto(),
                "Warlock 14" => new SpellSlotDto(),
                "Warlock 15" => new SpellSlotDto(),
                "Warlock 16" => new SpellSlotDto(),
                "Warlock 17" => new SpellSlotDto(),
                "Warlock 18" => new SpellSlotDto(),
                "Warlock 19" => new SpellSlotDto(),
                "Warlock 20" => new SpellSlotDto(),
                _ => new SpellSlotDto(),
            };
        }

        public static SpellSlotDto GetWizardSpellSlotIncrease(int level)
        {
            return $"Wizard {level}" switch
            {
                "Wizard 1" => new SpellSlotDto(),
                "Wizard 2" => new SpellSlotDto(),
                "Wizard 3" => new SpellSlotDto(),
                "Wizard 4" => new SpellSlotDto(),
                "Wizard 5" => new SpellSlotDto(),
                "Wizard 6" => new SpellSlotDto(),
                "Wizard 7" => new SpellSlotDto(),
                "Wizard 8" => new SpellSlotDto(),
                "Wizard 9" => new SpellSlotDto(),
                "Wizard 10" => new SpellSlotDto(),
                "Wizard 11" => new SpellSlotDto(),
                "Wizard 12" => new SpellSlotDto(),
                "Wizard 13" => new SpellSlotDto(),
                "Wizard 14" => new SpellSlotDto(),
                "Wizard 15" => new SpellSlotDto(),
                "Wizard 16" => new SpellSlotDto(),
                "Wizard 17" => new SpellSlotDto(),
                "Wizard 18" => new SpellSlotDto(),
                "Wizard 19" => new SpellSlotDto(),
                "Wizard 20" => new SpellSlotDto(),
                _ => new SpellSlotDto(),
            };
        }
    }
}
