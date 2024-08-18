// <copyright file="EchoRunner.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Runners
{
    using System;

    using Concierge.Common.Extensions;
    using Concierge.Console.Enums;

    public sealed class EchoRunner : Runner
    {
        public EchoRunner()
        {
        }

        public override string[] Names => throw new NotImplementedException();

        public override string[] Actions => throw new NotImplementedException();

        public override ConsoleResult Run(ConsoleCommand command)
        {
            return new ConsoleResult(command.Argument, ResultType.Information);
        }

        public override string List()
        {
            return string.Empty;
        }

        public override bool Contains(string name)
        {
            return name.ContainsIgnoreCase("Echo");
        }
    }
}
