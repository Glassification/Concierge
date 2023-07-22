// <copyright file="Program.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.DevTools
{
    using Concierge.DevTools.Generators;

    public class Program
    {
        public static void Main(string[] args)
        {
            /*var scrubbedDataOption = new Option<string>(
                name: "--ScrubbedData",
                description: "A csv file to parse the data from. Writes a json file to the same directory.");

            var colorPickerGeneratorOption = new Option<string>(
                name: "--ColorPickerGenerator",
                description: "A file to generate a default color picker json object.");

            var glossaryGeneratorOption = new Option<string>(
                name: "--GlossaryGenerator",
                description: "A folder to generate a glossary json object. Folder structure must already be setup with correct markdown files.");

            var customBlobGeneratorOption = new Option<string>(
                name: "--CustomBlobGenerator",
                description: "A file to generate a custom item list to.");

            var compressFileOption = new Option<string>(
                name: "--CompressFile",
                description: "The decompressed file to compress.");

            var decompressFileOption = new Option<string>(
                name: "--DecompressFile",
                description: "The compressed file to decompress.");

            var rootCommand = new RootCommand("Dev tools for Concierge");
            rootCommand.AddOption(scrubbedDataOption);
            rootCommand.AddOption(colorPickerGeneratorOption);
            rootCommand.AddOption(glossaryGeneratorOption);
            rootCommand.AddOption(customBlobGeneratorOption);
            rootCommand.AddOption(compressFileOption);
            rootCommand.AddOption(decompressFileOption);

            rootCommand.SetHandler(
                ParseScrubbedData.Parse,
                scrubbedDataOption);

            rootCommand.SetHandler(
                ColorPickerGenerator.Save,
                colorPickerGeneratorOption);

            rootCommand.SetHandler(
                GlossaryGenerator.Generate,
                glossaryGeneratorOption);

            rootCommand.SetHandler(
                CustomBlobGenerator.Generate,
                customBlobGeneratorOption);

            rootCommand.SetHandler(
                FileHelper.Compress,
                compressFileOption);

            rootCommand.SetHandler(
                FileHelper.Decompress,
                decompressFileOption);

            rootCommand.Invoke(args);*/

            StatusEffectGenerator.Generate(args[0]);
        }
    }
}