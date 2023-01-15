// <copyright file="IScript.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Scripts
{
    public interface IScript
    {
        ConsoleResult Evaluate(ConsoleCommand command);
    }
}
