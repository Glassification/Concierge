// <copyright file="ReadWriterScript.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Scripts
{
    using System;
    using System.IO;

    using Concierge.Common.Extensions;
    using Concierge.Console.Enums;
    using Newtonsoft.Json;

    public sealed class ReadWriterScript<T> : IScript
    {
        public ReadWriterScript(T item)
        {
            this.Item = item;
        }

        private T Item { get; set; }

        public ConsoleResult Evaluate(ConsoleCommand command)
        {
            if (command.Action.Equals("Read", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.Read(command);
            }

            if (command.Action.Equals("Write", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.Write(command);
            }

            if (command.Action.Equals("New", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.New(command);
            }

            return new ConsoleResult($"Implementation for '{command.Action}' not found.", ResultType.NotImplemented);
        }

        private ConsoleResult Read(ConsoleCommand command)
        {
            if (command.Argument.IsNullOrWhiteSpace())
            {
                return new ConsoleResult($"No file specified to read {command.Name} from.", ResultType.Error);
            }

            if (this.Item is not CcsFile)
            {
                return new ConsoleResult($"{this.Item?.GetType()?.Name} does not support reading.", ResultType.Error);
            }

            try
            {
                Program.MainWindow?.OpenCharacterSheet(command.Argument);
                return new ConsoleResult($"Read {command.Name} from {command.Argument}.", ResultType.Success);
            }
            catch (Exception ex)
            {
                return new ConsoleResult(ex.Message, ResultType.Error);
            }
        }

        private ConsoleResult Write(ConsoleCommand command)
        {
            if (command.Argument.IsNullOrWhiteSpace())
            {
                return new ConsoleResult($"No file specified to write {command.Name} to.", ResultType.Error);
            }

            try
            {
                var rawItem = JsonConvert.SerializeObject(this.Item, Formatting.Indented);
                File.WriteAllText(command.Argument, rawItem);

                return new ConsoleResult($"Wrote {command.Name} to {command.Argument}.", ResultType.Success);
            }
            catch (Exception ex)
            {
                return new ConsoleResult(ex.Message, ResultType.Error);
            }
        }

        private ConsoleResult New(ConsoleCommand command)
        {
            if (this.Item is not CcsFile)
            {
                return new ConsoleResult($"{this.Item?.GetType()?.Name} does not support new.", ResultType.Error);
            }

            try
            {
                Program.MainWindow?.NewCharacterSheet();
                return new ConsoleResult($"New {command.Name} sheet.", ResultType.Success);
            }
            catch (Exception ex)
            {
                return new ConsoleResult(ex.Message, ResultType.Error);
            }
        }
    }
}
