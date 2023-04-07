// <copyright file="ConsoleReadWriter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.ReadWriters
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Concierge.Console;
    using Concierge.Utility.Extensions;
    using Newtonsoft.Json;

    public static class ConsoleReadWriter
    {
        public static List<ConsoleResult> Read(string fileName)
        {
            var list = new List<ConsoleResult>();

            try
            {
                var rawJson = File.ReadAllLines(fileName);
                foreach (var line in rawJson)
                {
                    var json = JsonConvert.DeserializeObject<ConsoleResult>(line);
                    if (json is not null)
                    {
                        list.Add(json);
                    }
                }
            }
            catch (Exception ex)
            {
                ex = ex.TryConvertToReadWriterException(fileName);
                Program.ErrorService.LogError(ex);
            }

            return list;
        }

        public static void Write(string fileName, ConsoleResult result)
        {
            try
            {
                var json = JsonConvert.SerializeObject(result);
                File.AppendAllText(fileName, json);
                File.AppendAllText(fileName, "\n");
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }
        }

        public static void Clear(string fileName)
        {
            try
            {
                File.WriteAllText(fileName, string.Empty);
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }
        }
    }
}
