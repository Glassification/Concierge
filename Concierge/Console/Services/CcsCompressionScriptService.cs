// <copyright file="CcsCompressionScriptService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Services
{
    using Concierge.Console.Scripts;

    public sealed class CcsCompressionScriptService : ScriptService
    {
        private static readonly string[] names = new string[]
        {
            "Character",
        };

        private static readonly string[] actions = new string[]
        {
            "Zip",
            "Unzip",
        };

        public CcsCompressionScriptService()
            : this(true)
        {
        }

        public CcsCompressionScriptService(bool isFirst)
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
