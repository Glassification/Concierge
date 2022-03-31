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
        public const string AppSettingsProdcutionName = "appsettings.production.json";

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
            var fullPath = Path.Combine(fileLocation, fileName);

            return File.Exists(fullPath);
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
    }
}
