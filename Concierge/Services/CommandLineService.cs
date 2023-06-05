// <copyright file="CommandLineService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System;

    using Concierge.Configuration;
    using Concierge.Persistence.ReadWriters;

    public sealed class CommandLineService
    {
        private readonly string[] commandLineArgs;

        public CommandLineService()
        {
            this.commandLineArgs = Environment.GetCommandLineArgs();
        }

        public void ReadCommandLineArgs()
        {
            if (this.commandLineArgs.Length < 2)
            {
                return;
            }

            var characterReadWriter = new CharacterReadWriter(Program.ErrorService, Program.Logger);
            var ccsFile = characterReadWriter.ReadJson<CcsFile>(this.commandLineArgs[1]);

            ccsFile.AbsolutePath = this.commandLineArgs[1];
            ccsFile.Initialize();
            if (!ccsFile.CheckHash() || (AppSettingsManager.UserSettings.CheckVersion && !ccsFile.CheckVersion()))
            {
                Program.CcsFile = new CcsFile();
            }
            else
            {
                Program.CcsFile = ccsFile;
            }
        }
    }
}
