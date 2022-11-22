// <copyright file="ConciergeFiles.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System;
    using System.IO;

    public static class ConciergeFiles
    {
        public const string AppSettingsName = "appsettings.json";
        public const string AppSettingsTestName = "appsettings.test.json";
        public const string AppSettingsProductionName = "appsettings.production.json";
        public const string CustomColorsName = "CustomColors.json";

        private static readonly string applicationData;

        static ConciergeFiles()
        {
            applicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }

        public static string AppDataDirectory => Path.Combine(applicationData, "Concierge");

        public static string LoggingDirectory => Path.Combine(applicationData, @"Concierge\Logging");

        public static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;

        public static bool FileExistsAtLocation(string fileName, string fileLocation)
        {
            return File.Exists(Path.Combine(fileLocation, fileName));
        }

        public static string GetCorrectAppSettingsPath()
        {
            if (Program.IsDebug)
            {
                return BaseDirectory;
            }

            return FileExistsAtLocation(AppSettingsName, AppDataDirectory) ?
                AppDataDirectory :
                BaseDirectory;
        }

        public static string GetCorrectCustomColorsPath()
        {
            if (Program.IsDebug)
            {
                return BaseDirectory;
            }

            return FileExistsAtLocation(CustomColorsName, AppDataDirectory) ?
                AppDataDirectory :
                BaseDirectory;
        }
    }
}
