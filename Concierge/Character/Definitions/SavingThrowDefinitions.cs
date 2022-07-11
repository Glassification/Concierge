// <copyright file="SavingThrowDefinitions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Character.Definitions
{
    using Concierge.Character.Dtos;

    public static class SavingThrowDefinitions
    {
        public static SavingThrowDto GetArtificerSavingThrows()
        {
            return new SavingThrowDto(constitution: true, intelligence: true);
        }

        public static SavingThrowDto GetBarbarianSavingThrows()
        {
            return new SavingThrowDto(strength: true, constitution: true);
        }

        public static SavingThrowDto GetBardSavingThrows()
        {
            return new SavingThrowDto(dexterity: true, charisma: true);
        }

        public static SavingThrowDto GetBlooodHunterSavingThrows()
        {
            return new SavingThrowDto(dexterity: true, intelligence: true);
        }

        public static SavingThrowDto GetClericSavingThrows()
        {
            return new SavingThrowDto(wisdom: true, charisma: true);
        }

        public static SavingThrowDto GetDruidSavingThrows()
        {
            return new SavingThrowDto(intelligence: true, wisdom: true);
        }

        public static SavingThrowDto GetFighterSavingThrows()
        {
            return new SavingThrowDto(strength: true, constitution: true);
        }

        public static SavingThrowDto GetMonkSavingThrows()
        {
            return new SavingThrowDto(strength: true, dexterity: true);
        }

        public static SavingThrowDto GetPaladinSavingThrows()
        {
            return new SavingThrowDto(wisdom: true, charisma: true);
        }

        public static SavingThrowDto GetRangerSavingThrows()
        {
            return new SavingThrowDto(strength: true, dexterity: true);
        }

        public static SavingThrowDto GetRogueSavingThrows()
        {
            return new SavingThrowDto(dexterity: true, intelligence: true);
        }

        public static SavingThrowDto GetSorcererSavingThrows()
        {
            return new SavingThrowDto(constitution: true, charisma: true);
        }

        public static SavingThrowDto GetWarlockSavingThrows()
        {
            return new SavingThrowDto(wisdom: true, charisma: true);
        }

        public static SavingThrowDto GetWizardSavingThrows()
        {
            return new SavingThrowDto(intelligence: true, wisdom: true);
        }
    }
}
