// <copyright file="ClassResourceDefinitions.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Leveling.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class ClassResourceDefinitions
    {
        public static int GetArtificerResourceIncrease(int level)
        {
            return level switch
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
        }

        public static int GetBarbarianResourceIncrease(int level)
        {
            return level switch
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
        }

        public static int GetBardResourceIncrease(int level)
        {
            return level switch
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
        }
    }
}
