// <copyright file="Program.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace ConciergeDevTools
{
    using System.CommandLine;

    public class Program
    {
        public static void Main(string[] args)
        {
            /*
            var scrubbedDataOption = new Option<string>(
                name: "--ScrubbedData",
                description: "The file to parse the data from.");

            var decodeFileOption = new Option<string>(
                name: "--EncodedFile",
                description: "The encoded file to decode.");

            var encodeFileOption = new Option<string>(
                name: "--DecodedFile",
                description: "The decoded file to encode.");

            var rootCommand = new RootCommand("Dev tools for Concierge");
            rootCommand.AddOption(scrubbedDataOption);
            rootCommand.AddOption(decodeFileOption);
            rootCommand.AddOption(encodeFileOption);

            rootCommand.SetHandler(
                (file) => { ParseScrubbedDataHandler(file); },
                scrubbedDataOption);
            rootCommand.SetHandler(
                (file) => { DecodeFile(file); },
                decodeFileOption);
            rootCommand.SetHandler(
                (file) => { EncodeFile(file); },
                encodeFileOption);

            rootCommand.Invoke(args);
            */
            GenerateNames.Generate();
        }

        private static void ParseScrubbedDataHandler(string fileName)
        {
            ParseScrubbedData.Parse(fileName);
        }

        private static void DecodeFile(string fileName)
        {
        }

        private static void EncodeFile(string fileName)
        {
        }
    }
}