// <copyright file="ConciergeFiles.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common
{
    using System;
    using System.IO;
    using System.Reflection;

    public static class ConciergeFiles
    {
        public const string AppSettingsName = "appsettings.json";
        public const string AppSettingsTestName = "appsettings.test.json";
        public const string AppSettingsProductionName = "appsettings.production.json";

        public const string CustomColorsName = "CustomColors.json";
        public const string ConsoleOutput = "ConsoleOutput.json";

        public const string DiceHistoryName = "DiceHistory.txt";
        public const string ConsoleHistoryName = "ConsoleHistory.txt";
        public const string NameGeneratorHistoryName = "NameHistory.txt";

        private static readonly string applicationData;

        static ConciergeFiles()
        {
            applicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            Directory.CreateDirectory(AppDataDirectory);
            Directory.CreateDirectory(HistoryDirectory);
            Directory.CreateDirectory(LoggingDirectory);
        }

        public static string AppDataDirectory => Path.Combine(applicationData, "Concierge");

        public static string HistoryDirectory => Path.Combine(applicationData, @"Concierge\History");

        public static string LoggingDirectory => Path.Combine(applicationData, @"Concierge\Logging");

        public static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;

        public static string ExecutingDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        public static bool FileExistsAtLocation(string fileName, string fileLocation)
        {
            return File.Exists(Path.Combine(fileLocation, fileName));
        }

        public static string GetCorrectAppSettingsPath()
        {
            return FileExistsAtLocation(AppSettingsName, AppDataDirectory) ?
                AppDataDirectory :
                BaseDirectory;
        }

        public static string GetCorrectCustomColorsPath()
        {
            return FileExistsAtLocation(CustomColorsName, AppDataDirectory) ?
                AppDataDirectory :
                BaseDirectory;
        }
    }
}
