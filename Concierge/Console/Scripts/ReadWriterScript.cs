﻿// <copyright file="ReadWriterScript.cs" company="Thomas Beckett">
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
        private readonly T item;

        public ReadWriterScript(T item)
        {
            this.item = item;
        }

        public ConsoleResult Evaluate(ConsoleCommand command)
        {
            if (command.Name.EqualsIgnoreCase("Gamestate"))
            {
                return this.State(command);
            }

            if (command.Action.EqualsIgnoreCase("Read"))
            {
                return this.Read(command);
            }

            if (command.Action.EqualsIgnoreCase("Write"))
            {
                return this.Write(command);
            }

            if (command.Action.EqualsIgnoreCase("New"))
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

            if (this.item is not CcsFile)
            {
                return new ConsoleResult($"{this.item?.GetType()?.Name} does not support reading.", ResultType.Error);
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
                var rawItem = JsonConvert.SerializeObject(this.item, Formatting.Indented);
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
            if (this.item is not CcsFile)
            {
                return new ConsoleResult($"{this.item?.GetType()?.Name} does not support new.", ResultType.Error);
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "The way she goes.")]
        private ConsoleResult State(ConsoleCommand command)
        {
            if (command.Argument.IsNullOrWhiteSpace())
            {
                return new ConsoleResult($"No folder specified to write {command.Name} to.", ResultType.Error);
            }

            try
            {
                var character = JsonConvert.SerializeObject(Program.CcsFile.Character, Formatting.Indented);
                var state = Program.GetBaseState();
                File.WriteAllText(Path.Combine(command.Argument, "character.json"), character);
                File.WriteAllText(Path.Combine(command.Argument, "basestate.json"), state);

                return new ConsoleResult($"Wrote {command.Name} to {command.Argument}.", ResultType.Success);
            }
            catch (Exception ex)
            {
                return new ConsoleResult(ex.Message, ResultType.Error);
            }
        }
    }
}
