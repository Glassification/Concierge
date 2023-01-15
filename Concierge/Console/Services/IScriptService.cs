// <copyright file="IScriptService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Services
{
    public interface IScriptService
    {
        ConsoleResult Run(string command, string name);
    }
}
