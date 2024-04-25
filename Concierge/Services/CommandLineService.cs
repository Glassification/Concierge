// <copyright file="CommandLineService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System;

    using Concierge.Configuration;
    using Concierge.Persistence.ReadWriters;

    /// <summary>
    /// Service for reading command-line arguments and processing them.
    /// </summary>
    public sealed class CommandLineService
    {
        private readonly string[] commandLineArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineService"/> class.
        /// </summary>
        public CommandLineService()
        {
            this.commandLineArgs = Environment.GetCommandLineArgs();
        }

        /// <summary>
        /// Reads and processes the command-line arguments.
        /// </summary>
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
