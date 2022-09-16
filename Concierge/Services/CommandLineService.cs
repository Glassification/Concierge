// <copyright file="CommandLineService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System;

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

            Program.CcsFile = CharacterReadWriter.Read(this.commandLineArgs[1]);
        }
    }
}
