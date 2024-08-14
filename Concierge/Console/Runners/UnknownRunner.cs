// <copyright file="UnknownRunner.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Runners
{
    using System;

    public sealed class UnknownRunner : Runner
    {
        public UnknownRunner()
        {
        }

        public override string[] Names => throw new NotImplementedException();

        public override string[] Actions => throw new NotImplementedException();

        public override ConsoleResult Run(ConsoleCommand command)
        {
            return ConsoleResult.DefaultError(command.Command);
        }

        public override string List()
        {
            return string.Empty;
        }
    }
}
