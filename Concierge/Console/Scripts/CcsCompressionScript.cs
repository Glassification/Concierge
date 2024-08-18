// <copyright file="CcsCompressionScript.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Console.Scripts
{
    using System;
    using System.IO;

    using Concierge.Common.Extensions;
    using Concierge.Console.Enums;
    using Concierge.Persistence;

    public sealed class CcsCompressionScript : IScript
    {
        public CcsCompressionScript()
        {
        }

        public ConsoleResult Evaluate(ConsoleCommand command)
        {
            if (command.Action.EqualsIgnoreCase("Zip"))
            {
                return Zip(command);
            }

            if (command.Action.EqualsIgnoreCase("Unzip"))
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
                var newFile = CreateOutputFilePath(command.Argument, "Zip");

                File.WriteAllBytes(newFile, zipped);
                return new ConsoleResult($"Zipped {command.Name} to '{newFile}'.", ResultType.Success);
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
                var newFile = CreateOutputFilePath(command.Argument, "Unzip");

                File.WriteAllText(newFile, unzipped);
                return new ConsoleResult($"Unzipped {command.Name} to '{newFile}'.", ResultType.Success);
            }
            catch (Exception ex)
            {
                return new ConsoleResult(ex.Message, ResultType.Error);
            }
        }
    }
}
