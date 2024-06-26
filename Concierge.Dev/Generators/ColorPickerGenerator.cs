﻿// <copyright file="ColorPickerGenerator.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.DevTools.Generators
{
    using System.IO;

    using Concierge.Data;
    using Concierge.Services;
    using Newtonsoft.Json;

    public static class ColorPickerGenerator
    {
        public static void Save(string file)
        {
            var colorPicker = new CustomColorService()
            {
                DefaultColors =
                [
                    new CustomColor("Red", 255, 0, 0),
                    new CustomColor("Green", 0, 255, 0),
                    new CustomColor("Blue", 0, 0, 255),
                    new CustomColor("Yellow", 255, 255, 0),
                    new CustomColor("Orange", 255, 165, 0),
                    new CustomColor("White", 255, 255, 255),
                    new CustomColor("Black", 0, 0, 0),
                ],
                RecentColors =
                [
                    new CustomColor("Peru", 205, 133, 63),
                    new CustomColor("Medium Violet Red", 199, 21, 133),
                    new CustomColor("Goldenrod", 238, 232, 170),
                    new CustomColor("Dark Green", 21, 71, 52),
                    new CustomColor("Purple", 128, 0, 128),
                    new CustomColor("Steel Blue", 70, 130, 180),
                    new CustomColor("Firebrick", 178, 34, 34),
                ],
                CustomColors =
                [
                    new CustomColor("White", 255, 233, 220),
                    new CustomColor("Ice Blue", 65, 130, 255),
                ],
            };

            var rawString = JsonConvert.SerializeObject(colorPicker, Formatting.Indented);
            File.WriteAllText(file, rawString);
        }
    }
}
