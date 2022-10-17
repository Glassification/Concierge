// <copyright file="ClassResourceDefinitions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Leveling.Definitions
{
    using Concierge.Leveling.Dtos.Definitions;

    public static class ClassResourceDefinitions
    {
        public static ClassResourceDto GetArtificerResourceIncrease(int level)
        {
            var increase = level switch
            {
                1 => 0,
                2 => 4,
                3 => 0,
                4 => 0,
                5 => 0,
                6 => 2,
                7 => 0,
                8 => 0,
                9 => 0,
                10 => 2,
                11 => 0,
                12 => 0,
                13 => 0,
                14 => 2,
                15 => 0,
                16 => 0,
                17 => 0,
                18 => 2,
                19 => 0,
                20 => 0,
                _ => 0,
            };

            return new ClassResourceDto(increase, "Infusions");
        }

        public static ClassResourceDto GetBarbarianResourceIncrease(int level)
        {
            var increase = level switch
            {
                1 => 0,
                2 => 2,
                3 => 1,
                4 => 0,
                5 => 1,
                6 => 0,
                7 => 0,
                8 => 0,
                9 => 0,
                10 => 0,
                11 => 0,
                12 => 0,
                13 => 0,
                14 => 0,
                15 => 0,
                16 => 0,
                17 => 0,
                18 => 0,
                19 => 0,
                20 => 0,
                _ => 0,
            };

            return new ClassResourceDto(increase, "Rages");
        }

        public static ClassResourceDto GetBardResourceIncrease(int level)
        {
            var increase = level switch
            {
                1 => 0,
                2 => 4,
                3 => 0,
                4 => 0,
                5 => 0,
                6 => 2,
                7 => 0,
                8 => 0,
                9 => 0,
                10 => 2,
                11 => 0,
                12 => 0,
                13 => 0,
                14 => 2,
                15 => 0,
                16 => 0,
                17 => 0,
                18 => 2,
                19 => 0,
                20 => 0,
                _ => 0,
            };

            return new ClassResourceDto(increase, "Bardic Inspiration");
        }

        public static ClassResourceDto GetFighterResourceIncrease(int level)
        {
            var increase = level switch
            {
                1 => 0,
                2 => 0,
                3 => 0,
                4 => 4,
                5 => 0,
                6 => 0,
                7 => 1,
                8 => 0,
                9 => 0,
                10 => 0,
                11 => 0,
                12 => 0,
                13 => 0,
                14 => 0,
                15 => 1,
                16 => 0,
                17 => 0,
                18 => 0,
                19 => 0,
                20 => 0,
                _ => 0,
            };

            return new ClassResourceDto(increase, "Superiority Dice");
        }

        public static ClassResourceDto GetMonkResourceIncrease(int level)
        {
            var increase = level switch
            {
                1 => 0,
                2 => 2,
                3 => 1,
                4 => 1,
                5 => 1,
                6 => 1,
                7 => 1,
                8 => 1,
                9 => 1,
                10 => 1,
                11 => 1,
                12 => 1,
                13 => 1,
                14 => 1,
                15 => 1,
                16 => 1,
                17 => 1,
                18 => 1,
                19 => 1,
                20 => 1,
                _ => 0,
            };

            return new ClassResourceDto(increase, "Ki Points");
        }

        public static ClassResourceDto GetRogueResourceIncrease(int level)
        {
            var increase = level switch
            {
                1 => 1,
                2 => 0,
                3 => 1,
                4 => 0,
                5 => 1,
                6 => 0,
                7 => 1,
                8 => 0,
                9 => 1,
                10 => 0,
                11 => 1,
                12 => 0,
                13 => 1,
                14 => 0,
                15 => 1,
                16 => 0,
                17 => 1,
                18 => 0,
                19 => 1,
                20 => 0,
                _ => 0,
            };

            return new ClassResourceDto(increase, "Sneak Attack Dice");
        }

        public static ClassResourceDto GetSorcererResourceIncrease(int level)
        {
            var increase = level switch
            {
                1 => 0,
                2 => 2,
                3 => 1,
                4 => 1,
                5 => 1,
                6 => 1,
                7 => 1,
                8 => 1,
                9 => 1,
                10 => 1,
                11 => 1,
                12 => 1,
                13 => 1,
                14 => 1,
                15 => 1,
                16 => 1,
                17 => 1,
                18 => 1,
                19 => 1,
                20 => 1,
                _ => 0,
            };

            return new ClassResourceDto(increase, "Sorcery Points");
        }
    }
}
