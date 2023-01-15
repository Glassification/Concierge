// <copyright file="UnknownScriptService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Services
{
    public class UnknownScriptService : IScriptService
    {
        public UnknownScriptService()
        {
        }

        public ConsoleResult Run(string command, string name)
        {
            return ConsoleResult.Default(command);
        }
    }
}
