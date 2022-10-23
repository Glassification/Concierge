// <copyright file="DiceHistoryReadWriter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence.ReadWriters
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Concierge.Utility;

    public static class DiceHistoryReadWriter
    {
        private static readonly string diceHistoryFile;

        static DiceHistoryReadWriter()
        {
            diceHistoryFile = Path.Combine(ConciergeFiles.AppDataDirectory, @"DiceHistory.txt");
        }

        public static List<string> Read()
        {
            var list = new List<string>();

            try
            {
                CreateFileIfMissing();

                var lines = File.ReadAllLines(diceHistoryFile);
                foreach (var line in lines)
                {
                    var roll = line.Split(']')[1].Trim();
                    list.Add(roll);
                }
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }

            list.Reverse();
            return list;
        }

        public static void Write(string diceRoll)
        {
            try
            {
                File.AppendAllText(diceHistoryFile, $"[{ConciergeDateTime.LoggingNow}] {diceRoll}\n");
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(ex);
            }
        }

        private static void CreateFileIfMissing()
        {
            if (!File.Exists(diceHistoryFile))
            {
                using var createStream = File.Create(diceHistoryFile);
            }
        }
    }
}
