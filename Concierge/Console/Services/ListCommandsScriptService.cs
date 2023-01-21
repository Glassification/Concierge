// <copyright file="ListCommandsScriptService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Services
{
    using System;
    using System.Text;

    public class ListCommandsScriptService : ScriptService
    {
        public ListCommandsScriptService()
        {
        }

        public override string[] Names => throw new NotImplementedException();

        public override string[] Actions => throw new NotImplementedException();

        public override ConsoleResult Run(ConsoleCommand command)
        {
            return new ConsoleResult(this.List(), Enums.ResultType.Success);
        }

        public override string List()
        {
            var builder = new StringBuilder();

            builder.Append(new ListScriptService().List());
            builder.Append(new WealthScriptService(false).List());
            builder.Append(new ReadWriterScriptService(false).List());

            return builder.ToString();
        }
    }
}
