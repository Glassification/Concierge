// <copyright file="UnknownScriptService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Services
{
    using System;

    public class UnknownScriptService : ScriptService
    {
        public UnknownScriptService()
        {
        }

        public override string[] Names => throw new NotImplementedException();

        public override string[] Actions => throw new NotImplementedException();

        public override ConsoleResult Run(ConsoleCommand command)
        {
            return ConsoleResult.Default(command.Command);
        }

        public override string List()
        {
            return string.Empty;
        }
    }
}
