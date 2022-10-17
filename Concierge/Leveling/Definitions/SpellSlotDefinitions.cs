// <copyright file="SpellSlotDefinitions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling.Definitions
{
    using Concierge.Leveling.Dtos.Definitions;

    public static class SpellSlotDefinitions
    {
        public static SpellSlotDto GetArtificerSpellSlotIncrease(int level)
        {
            return $"Artificer {level}" switch
            {
                "Artificer 1" => new SpellSlotDto(known: 2, slots: 2, cantrip: 2, first: 2),
                "Artificer 2" => new SpellSlotDto(),
                "Artificer 3" => new SpellSlotDto(known: 1, first: 1),
                "Artificer 4" => new SpellSlotDto(),
                "Artificer 5" => new SpellSlotDto(known: 3, first: 1, second: 2),
                "Artificer 6" => new SpellSlotDto(),
                "Artificer 7" => new SpellSlotDto(known: 1, first: 1),
                "Artificer 8" => new SpellSlotDto(),
                "Artificer 9" => new SpellSlotDto(known: 2, third: 2),
                "Artificer 10" => new SpellSlotDto(cantrip: 1),
                "Artificer 11" => new SpellSlotDto(known: 1, third: 1),
                "Artificer 12" => new SpellSlotDto(),
                "Artificer 13" => new SpellSlotDto(known: 1, fourth: 1),
                "Artificer 14" => new SpellSlotDto(cantrip: 1),
                "Artificer 15" => new SpellSlotDto(known: 1, fourth: 1),
                "Artificer 16" => new SpellSlotDto(),
                "Artificer 17" => new SpellSlotDto(known: 2, fourth: 1, fifth: 1),
                "Artificer 18" => new SpellSlotDto(),
                "Artificer 19" => new SpellSlotDto(known: 1, fifth: 1),
                "Artificer 20" => new SpellSlotDto(),
                _ => new SpellSlotDto(),
            };
        }

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

        public static SpellSlotDto GetBloodHunterSpellSlotIncrease(int level)
        {
            return $"Blood Hunter {level}" switch
            {
                "Blood Hunter 1" => new SpellSlotDto(known: 1),
                "Blood Hunter 2" => new SpellSlotDto(),
                "Blood Hunter 3" => new SpellSlotDto(),
                "Blood Hunter 4" => new SpellSlotDto(),
                "Blood Hunter 5" => new SpellSlotDto(),
                "Blood Hunter 6" => new SpellSlotDto(known: 1),
                "Blood Hunter 7" => new SpellSlotDto(),
                "Blood Hunter 8" => new SpellSlotDto(),
                "Blood Hunter 9" => new SpellSlotDto(),
                "Blood Hunter 10" => new SpellSlotDto(known: 1),
                "Blood Hunter 11" => new SpellSlotDto(),
                "Blood Hunter 12" => new SpellSlotDto(),
                "Blood Hunter 13" => new SpellSlotDto(),
                "Blood Hunter 14" => new SpellSlotDto(known: 1),
                "Blood Hunter 15" => new SpellSlotDto(),
                "Blood Hunter 16" => new SpellSlotDto(),
                "Blood Hunter 17" => new SpellSlotDto(),
                "Blood Hunter 18" => new SpellSlotDto(known: 1),
                "Blood Hunter 19" => new SpellSlotDto(),
                "Blood Hunter 20" => new SpellSlotDto(),
                _ => new SpellSlotDto(),
            };
        }

        public static SpellSlotDto GetClericSpellSlotIncrease(int level)
        {
            return $"Cleric {level}" switch
            {
                "Cleric 1" => new SpellSlotDto(known: 1, slots: 2, cantrip: 3, first: 2),
                "Cleric 2" => new SpellSlotDto(known: 1, slots: 1, first: 1),
                "Cleric 3" => new SpellSlotDto(known: 1, slots: 3, first: 1, second: 2),
                "Cleric 4" => new SpellSlotDto(known: 1, slots: 1, cantrip: 1, second: 1),
                "Cleric 5" => new SpellSlotDto(known: 1, slots: 2, third: 2),
                "Cleric 6" => new SpellSlotDto(known: 1, slots: 1, third: 1),
                "Cleric 7" => new SpellSlotDto(known: 1, slots: 1, fourth: 1),
                "Cleric 8" => new SpellSlotDto(known: 1, slots: 1, fourth: 1),
                "Cleric 9" => new SpellSlotDto(known: 1, slots: 2, fourth: 1, fifth: 1),
                "Cleric 10" => new SpellSlotDto(known: 1, slots: 1, cantrip: 1, fifth: 1),
                "Cleric 11" => new SpellSlotDto(known: 1, slots: 1, sixth: 1),
                "Cleric 12" => new SpellSlotDto(known: 1),
                "Cleric 13" => new SpellSlotDto(known: 1, slots: 1, seventh: 1),
                "Cleric 14" => new SpellSlotDto(known: 1),
                "Cleric 15" => new SpellSlotDto(known: 1, slots: 1, eighth: 1),
                "Cleric 16" => new SpellSlotDto(known: 1),
                "Cleric 17" => new SpellSlotDto(known: 1, slots: 1, nineth: 1),
                "Cleric 18" => new SpellSlotDto(known: 1, slots: 1, fifth: 1),
                "Cleric 19" => new SpellSlotDto(known: 1, slots: 1, sixth: 1),
                "Cleric 20" => new SpellSlotDto(known: 1, slots: 1, seventh: 1),
                _ => new SpellSlotDto(),
            };
        }

        public static SpellSlotDto GetDruidSpellSlotIncrease(int level)
        {
            return $"Druid {level}" switch
            {
                "Druid 1" => new SpellSlotDto(known: 1, slots: 2, cantrip: 2, first: 2),
                "Druid 2" => new SpellSlotDto(known: 1, slots: 1, first: 1),
                "Druid 3" => new SpellSlotDto(known: 1, slots: 3, first: 1, second: 2),
                "Druid 4" => new SpellSlotDto(known: 1, slots: 1, cantrip: 1, second: 1),
                "Druid 5" => new SpellSlotDto(known: 1, slots: 2, third: 2),
                "Druid 6" => new SpellSlotDto(known: 1, slots: 1, third: 1),
                "Druid 7" => new SpellSlotDto(known: 1, slots: 1, fourth: 1),
                "Druid 8" => new SpellSlotDto(known: 1, slots: 1, fourth: 1),
                "Druid 9" => new SpellSlotDto(known: 1, slots: 2, fourth: 1, fifth: 1),
                "Druid 10" => new SpellSlotDto(known: 1, slots: 1, cantrip: 1, fifth: 1),
                "Druid 11" => new SpellSlotDto(known: 1, slots: 1, sixth: 1),
                "Druid 12" => new SpellSlotDto(known: 1),
                "Druid 13" => new SpellSlotDto(known: 1, slots: 1, seventh: 1),
                "Druid 14" => new SpellSlotDto(known: 1),
                "Druid 15" => new SpellSlotDto(known: 1, slots: 1, eighth: 1),
                "Druid 16" => new SpellSlotDto(known: 1),
                "Druid 17" => new SpellSlotDto(known: 1, slots: 1, nineth: 1),
                "Druid 18" => new SpellSlotDto(known: 1, slots: 1, fifth: 1),
                "Druid 19" => new SpellSlotDto(known: 1, slots: 1, sixth: 1),
                "Druid 20" => new SpellSlotDto(known: 1, slots: 1, seventh: 1),
                _ => new SpellSlotDto(),
            };
        }

        public static SpellSlotDto GetPaladinSpellSlotIncrease(int level)
        {
            return $"Paladin {level}" switch
            {
                "Paladin 1" => new SpellSlotDto(),
                "Paladin 2" => new SpellSlotDto(known: 1, slots: 2, first: 2),
                "Paladin 3" => new SpellSlotDto(slots: 1, first: 1),
                "Paladin 4" => new SpellSlotDto(known: 1),
                "Paladin 5" => new SpellSlotDto(slots: 3, first: 1, second: 2),
                "Paladin 6" => new SpellSlotDto(known: 1),
                "Paladin 7" => new SpellSlotDto(slots: 1, second: 1),
                "Paladin 8" => new SpellSlotDto(known: 1),
                "Paladin 9" => new SpellSlotDto(slots: 2, third: 2),
                "Paladin 10" => new SpellSlotDto(known: 1),
                "Paladin 11" => new SpellSlotDto(slots: 1, third: 1),
                "Paladin 12" => new SpellSlotDto(known: 1),
                "Paladin 13" => new SpellSlotDto(slots: 1, fourth: 1),
                "Paladin 14" => new SpellSlotDto(known: 1),
                "Paladin 15" => new SpellSlotDto(slots: 1, fourth: 1),
                "Paladin 16" => new SpellSlotDto(known: 1),
                "Paladin 17" => new SpellSlotDto(slots: 2, fourth: 1, fifth: 1),
                "Paladin 18" => new SpellSlotDto(known: 1),
                "Paladin 19" => new SpellSlotDto(slots: 1, fifth: 1),
                "Paladin 20" => new SpellSlotDto(known: 1),
                _ => new SpellSlotDto(),
            };
        }

        public static SpellSlotDto GetRangerSpellSlotIncrease(int level)
        {
            return $"Ranger {level}" switch
            {
                "Ranger 1" => new SpellSlotDto(),
                "Ranger 2" => new SpellSlotDto(known: 2, slots: 1, first: 2),
                "Ranger 3" => new SpellSlotDto(known: 1, slots: 1, first: 1),
                "Ranger 4" => new SpellSlotDto(),
                "Ranger 5" => new SpellSlotDto(known: 1, slots: 3, first: 1, second: 2),
                "Ranger 6" => new SpellSlotDto(),
                "Ranger 7" => new SpellSlotDto(known: 1, slots: 1, second: 1),
                "Ranger 8" => new SpellSlotDto(),
                "Ranger 9" => new SpellSlotDto(known: 1, slots: 2, third: 2),
                "Ranger 10" => new SpellSlotDto(),
                "Ranger 11" => new SpellSlotDto(known: 1, slots: 1, third: 1),
                "Ranger 12" => new SpellSlotDto(),
                "Ranger 13" => new SpellSlotDto(known: 1, slots: 1, fourth: 1),
                "Ranger 14" => new SpellSlotDto(),
                "Ranger 15" => new SpellSlotDto(known: 1, slots: 1, fourth: 1),
                "Ranger 16" => new SpellSlotDto(),
                "Ranger 17" => new SpellSlotDto(known: 1, slots: 2, fourth: 1, fifth: 1),
                "Ranger 18" => new SpellSlotDto(),
                "Ranger 19" => new SpellSlotDto(known: 1, slots: 1, fifth: 1),
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
                "Rogue 3" => new SpellSlotDto(known: 3, slots: 2, cantrip: 3, first: 2),
                "Rogue 4" => new SpellSlotDto(known: 1, slots: 1, first: 1),
                "Rogue 5" => new SpellSlotDto(),
                "Rogue 6" => new SpellSlotDto(),
                "Rogue 7" => new SpellSlotDto(known: 1, slots: 3, first: 1, second: 2),
                "Rogue 8" => new SpellSlotDto(known: 1),
                "Rogue 9" => new SpellSlotDto(),
                "Rogue 10" => new SpellSlotDto(known: 1, slots: 1, cantrip: 1, second: 1),
                "Rogue 11" => new SpellSlotDto(known: 1),
                "Rogue 12" => new SpellSlotDto(),
                "Rogue 13" => new SpellSlotDto(known: 1, slots: 2, third: 2),
                "Rogue 14" => new SpellSlotDto(known: 1),
                "Rogue 15" => new SpellSlotDto(),
                "Rogue 16" => new SpellSlotDto(known: 1, slots: 1, third: 1),
                "Rogue 17" => new SpellSlotDto(),
                "Rogue 18" => new SpellSlotDto(),
                "Rogue 19" => new SpellSlotDto(known: 1, slots: 1, fourth: 1),
                "Rogue 20" => new SpellSlotDto(known: 1),
                _ => new SpellSlotDto(),
            };
        }

        public static SpellSlotDto GetSorcererSpellSlotIncrease(int level)
        {
            return $"Sorcerer {level}" switch
            {
                "Sorcerer 1" => new SpellSlotDto(known: 2, slots: 2, cantrip: 4, first: 2),
                "Sorcerer 2" => new SpellSlotDto(known: 1, slots: 1, first: 1),
                "Sorcerer 3" => new SpellSlotDto(known: 1, slots: 3, first: 1, second: 1),
                "Sorcerer 4" => new SpellSlotDto(known: 1, slots: 1, cantrip: 1, second: 1),
                "Sorcerer 5" => new SpellSlotDto(known: 1, slots: 1, third: 2),
                "Sorcerer 6" => new SpellSlotDto(known: 1, slots: 1, third: 1),
                "Sorcerer 7" => new SpellSlotDto(known: 1, slots: 1, fourth: 1),
                "Sorcerer 8" => new SpellSlotDto(known: 1, slots: 1, fourth: 1),
                "Sorcerer 9" => new SpellSlotDto(known: 1, slots: 2, fourth: 1, fifth: 1),
                "Sorcerer 10" => new SpellSlotDto(known: 1, slots: 1, cantrip: 1, fifth: 1),
                "Sorcerer 11" => new SpellSlotDto(known: 1, slots: 1, sixth: 1),
                "Sorcerer 12" => new SpellSlotDto(),
                "Sorcerer 13" => new SpellSlotDto(known: 1, slots: 1, seventh: 1),
                "Sorcerer 14" => new SpellSlotDto(),
                "Sorcerer 15" => new SpellSlotDto(known: 1, slots: 1, eighth: 1),
                "Sorcerer 16" => new SpellSlotDto(),
                "Sorcerer 17" => new SpellSlotDto(known: 1, slots: 1, nineth: 1),
                "Sorcerer 18" => new SpellSlotDto(slots: 1, fifth: 1),
                "Sorcerer 19" => new SpellSlotDto(slots: 1, sixth: 1),
                "Sorcerer 20" => new SpellSlotDto(slots: 1, seventh: 1),
                _ => new SpellSlotDto(),
            };
        }

        public static SpellSlotDto GetWarlockSpellSlotIncrease(int level)
        {
            return $"Warlock {level}" switch
            {
                "Warlock 1" => new SpellSlotDto(known: 2, slots: 1, cantrip: 1, first: 1),
                "Warlock 2" => new SpellSlotDto(known: 1, slots: 1, pact: 2, first: 1),
                "Warlock 3" => new SpellSlotDto(known: 1, second: 2),
                "Warlock 4" => new SpellSlotDto(known: 1, cantrip: 1),
                "Warlock 5" => new SpellSlotDto(known: 1, pact: 1, third: 2),
                "Warlock 6" => new SpellSlotDto(known: 1),
                "Warlock 7" => new SpellSlotDto(known: 1, pact: 1, fourth: 2),
                "Warlock 8" => new SpellSlotDto(known: 1),
                "Warlock 9" => new SpellSlotDto(known: 1, pact: 1, fifth: 2),
                "Warlock 10" => new SpellSlotDto(cantrip: 1),
                "Warlock 11" => new SpellSlotDto(known: 1, slots: 1, first: 1, second: 1, third: 1, fourth: 1, fifth: 1),
                "Warlock 12" => new SpellSlotDto(pact: 1),
                "Warlock 13" => new SpellSlotDto(known: 1),
                "Warlock 14" => new SpellSlotDto(),
                "Warlock 15" => new SpellSlotDto(known: 1, pact: 1),
                "Warlock 16" => new SpellSlotDto(),
                "Warlock 17" => new SpellSlotDto(known: 1, slots: 1, first: 1, second: 1, third: 1, fourth: 1, fifth: 1),
                "Warlock 18" => new SpellSlotDto(pact: 1),
                "Warlock 19" => new SpellSlotDto(known: 1),
                "Warlock 20" => new SpellSlotDto(),
                _ => new SpellSlotDto(),
            };
        }

        public static SpellSlotDto GetWizardSpellSlotIncrease(int level)
        {
            return $"Wizard {level}" switch
            {
                "Wizard 1" => new SpellSlotDto(known: 6, slots: 2, cantrip: 3, first: 1),
                "Wizard 2" => new SpellSlotDto(slots: 1, first: 1),
                "Wizard 3" => new SpellSlotDto(slots: 3, first: 1, second: 2),
                "Wizard 4" => new SpellSlotDto(slots: 1, cantrip: 1, second: 1),
                "Wizard 5" => new SpellSlotDto(slots: 2, third: 2),
                "Wizard 6" => new SpellSlotDto(slots: 1, third: 1),
                "Wizard 7" => new SpellSlotDto(slots: 1, fourth: 1),
                "Wizard 8" => new SpellSlotDto(slots: 1, fourth: 1),
                "Wizard 9" => new SpellSlotDto(slots: 2, fourth: 1, fifth: 1),
                "Wizard 10" => new SpellSlotDto(slots: 1, cantrip: 1, fifth: 1),
                "Wizard 11" => new SpellSlotDto(slots: 1, sixth: 1),
                "Wizard 12" => new SpellSlotDto(),
                "Wizard 13" => new SpellSlotDto(slots: 1, seventh: 1),
                "Wizard 14" => new SpellSlotDto(),
                "Wizard 15" => new SpellSlotDto(slots: 1, eighth: 1),
                "Wizard 16" => new SpellSlotDto(),
                "Wizard 17" => new SpellSlotDto(slots: 1, nineth: 1),
                "Wizard 18" => new SpellSlotDto(slots: 1, fifth: 1),
                "Wizard 19" => new SpellSlotDto(slots: 1, sixth: 1),
                "Wizard 20" => new SpellSlotDto(slots: 1, seventh: 1),
                _ => new SpellSlotDto(),
            };
        }
    }
}
