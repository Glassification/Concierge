// <copyright file="CcsCompressionScript.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Scripts
{
    using System;
    using System.IO;

    using Concierge.Console.Enums;
    using Concierge.Persistence;
    using Concierge.Utility.Extensions;

    public sealed class CcsCompressionScript : IScript
    {
        public CcsCompressionScript()
        {
        }

        public ConsoleResult Evaluate(ConsoleCommand command)
        {
            if (command.Action.Equals("Zip", StringComparison.InvariantCultureIgnoreCase))
            {
                return Zip(command);
            }

            if (command.Action.Equals("Unzip", StringComparison.InvariantCultureIgnoreCase))
            {
                return Unzip(command);
            }

            return new ConsoleResult($"Implementation for '{command.Action}' not found.", ResultType.NotImplemented);
        }

        private static string CreateOutputFilePath(string path, string action)
        {
            return $"{Path.GetDirectoryName(path)}\\{Path.GetFileNameWithoutExtension(path)}_{action}{Path.GetExtension(path)}";
        }

        private static ConsoleResult Zip(ConsoleCommand command)
        {
            if (command.Argument.IsNullOrWhiteSpace())
            {
                return new ConsoleResult($"No file specified to Zip.", ResultType.Error);
            }

            try
            {
                var file = File.ReadAllText(command.Argument);
                var zipped = CcsCompression.Zip(file);

                File.WriteAllBytes(CreateOutputFilePath(command.Argument, "Zip"), zipped);
                return new ConsoleResult($"Zipped {command.Name} to '{command.Argument}'.", ResultType.Success);
            }
            catch (Exception ex)
            {
                return new ConsoleResult(ex.Message, ResultType.Error);
            }
        }

        private static ConsoleResult Unzip(ConsoleCommand command)
        {
            if (command.Argument.IsNullOrWhiteSpace())
            {
                return new ConsoleResult($"No file specified to Unzip.", ResultType.Error);
            }

            try
            {
                var file = File.ReadAllBytes(command.Argument);
                var unzipped = CcsCompression.Unzip(file);

                File.WriteAllText(CreateOutputFilePath(command.Argument, "Unzip"), unzipped);
                return new ConsoleResult($"Unzipped {command.Name} to '{command.Argument}'.", ResultType.Success);
            }
            catch (Exception ex)
            {
                return new ConsoleResult(ex.Message, ResultType.Error);
            }
        }
    }
}
