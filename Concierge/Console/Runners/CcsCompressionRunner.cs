// <copyright file="CcsCompressionRunner.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Runners
{
    using Concierge.Console.Scripts;

    public sealed class CcsCompressionRunner : Runner
    {
        private static readonly string[] names =
        [
            "Character",
        ];

        private static readonly string[] actions =
        [
            "Zip",
            "Unzip",
        ];

        public CcsCompressionRunner()
            : this(true)
        {
        }

        public CcsCompressionRunner(bool isFirst)
        {
            this.IsFirstList = isFirst;
        }

        public override string[] Names => names;

        public override string[] Actions => actions;

        public override ConsoleResult Run(ConsoleCommand command)
        {
            return new CcsCompressionScript().Evaluate(command);
        }
    }
}
