// <copyright file="ListCommandsRunner.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Runners
{
    using System;
    using System.IO;
    using System.Text;

    using Concierge.Common;
    using Concierge.Common.Extensions;
    using Concierge.Console.Enums;
    using Concierge.Persistence.ReadWriters;

    public sealed class ListCommandsRunner : Runner
    {
        private static readonly string[] names =
        [
            "List",
        ];

        private static readonly string[] actions =
        [
            "Commands",
            "History",
            "Log",
        ];

        private readonly HistoryReadWriter historyReadWriter;
        private readonly string consoleHistoryFile = Path.Combine(ConciergeFiles.HistoryDirectory, ConciergeFiles.ConsoleHistoryName);

        public ListCommandsRunner()
        {
            this.historyReadWriter = new HistoryReadWriter(Program.ErrorService);
        }

        public override string[] Names => names;

        public override string[] Actions => actions;

        public override ConsoleResult Run(ConsoleCommand command)
        {
            if (command.Action.Equals("Commands", StringComparison.InvariantCultureIgnoreCase))
            {
                return new ConsoleResult(this.List(), ResultType.Success);
            }

            if (command.Action.Equals("History", StringComparison.InvariantCultureIgnoreCase))
            {
                return new ConsoleResult(this.History(), ResultType.Success);
            }

            if (command.Action.Equals("Log", StringComparison.InvariantCultureIgnoreCase))
            {
                return new ConsoleResult(Log(), ResultType.Success);
            }

            return new ConsoleResult($"Implementation for '{command.Action}' not found.", ResultType.NotImplemented);
        }

        public override string List()
        {
            var builder = new StringBuilder();

            builder.Append(new ListRunner().List());
            builder.Append(new WealthRunner(false).List());
            builder.Append(new ReadWriterRunner(false).List());
            builder.Append(new CcsCompressionRunner(false).List());
            builder.Append(base.List());

            return builder.ToString();
        }

        private static string Log()
        {
            var builder = new StringBuilder();

            for (int i = 0; i < Program.Logger.SessionLog.Count; i++)
            {
                var indent = i > 0 ? Indent : string.Empty;
                builder.AppendLine($"{indent}{Program.Logger.SessionLog[i]}");
            }

            return builder.ToString();
        }

        private string History()
        {
            var history = this.historyReadWriter.ReadList<string>(this.consoleHistoryFile);
            var builder = new StringBuilder();

            for (int i = history.Count - 1; i >= 0; i--)
            {
                var indent = i < history.Count - 1 ? Indent : string.Empty;
                builder.AppendLine($"{indent}{history[i].Strip(Constants.ConsolePrompt)}");
            }

            return builder.ToString();
        }
    }
}
