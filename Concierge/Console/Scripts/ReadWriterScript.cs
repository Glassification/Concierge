// <copyright file="ReadWriterScript.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Concierge.Character;
    using Concierge.Console.Enums;
    using Concierge.Persistence;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Utility.Extensions;
    using Newtonsoft.Json;

    public class ReadWriterScript<T> : IScript
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

            return new ConsoleResult($"Implementation for '{command.Action}' not found.", ResultType.Error);
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
                var item = CharacterReadWriter.Read(command.Argument);

                return new ConsoleResult($"Read {command.Name} from {command.Argument}.", ResultType.Success, item);
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
    }
}
