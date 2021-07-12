// <copyright file="CommandLineService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System.IO;

    using Concierge.Persistence;

    public class CommandLineService
    {
        public CommandLineService()
        {
        }

        public void ReadCommandLineArgs(string[] args)
        {
            if (args.Length < 1)
            {
                return;
            }

            Program.CcsFile = CharacterLoader.LoadCharacterSheetJson(args[0]);
        }
    }
}
