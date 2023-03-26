// <copyright file="HistoryReadWriter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.ReadWriters
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Concierge.Utility;
    using Concierge.Utility.Extensions;

    public static class HistoryReadWriter
    {
        public static List<string> Read(string filePath)
        {
            var list = new List<string>();

            try
            {
                CreateFileIfMissing(filePath);

                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var history = line.Split(']')[1].Trim();
                    list.Add(history);
                }
            }
            catch (Exception ex)
            {
                ex = ex.TryConvertToReadWriterException(filePath);
                Program.ErrorService.LogError(ex);
            }

            list.Reverse();
            return list;
        }

        public static void Write(string filePath, string history)
        {
            try
            {
                File.AppendAllText(filePath, $"[{ConciergeDateTime.LoggingNow}] {history}\n");
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }
        }

        private static void CreateFileIfMissing(string filePath)
        {
            if (!File.Exists(filePath))
            {
                using var createStream = File.Create(filePath);
            }
        }
    }
}
