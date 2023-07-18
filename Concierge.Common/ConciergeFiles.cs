// <copyright file="ConciergeFiles.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common
{
    using System;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Provides utility methods and constants for working with files and directories in the Concierge application.
    /// </summary>
    public static class ConciergeFiles
    {
        public const string AppSettingsName = "appsettings.json";
        public const string AppSettingsTestName = "appsettings.test.json";
        public const string AppSettingsProductionName = "appsettings.production.json";

        public const string CustomColorsName = "CustomColors.json";
        public const string ConsoleOutput = "ConsoleOutput.json";
        public const string CustomItemsName = "CustomItems.txt";

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

        /// <summary>
        /// Gets the path to the directory where application data files are stored.
        /// </summary>
        public static string AppDataDirectory => Path.Combine(applicationData, "Concierge");

        /// <summary>
        /// Gets the path to the directory where history files are stored.
        /// </summary>
        public static string HistoryDirectory => Path.Combine(applicationData, @"Concierge\History");

        /// <summary>
        /// Gets the path to the directory where logging files are stored.
        /// </summary>
        public static string LoggingDirectory => Path.Combine(applicationData, @"Concierge\Logging");

        /// <summary>
        /// Gets the base directory of the application.
        /// </summary>
        public static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// Gets the directory where the executing assembly is located.
        /// </summary>
        public static string ExecutingDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        /// <summary>
        /// Checks if a file exists at the specified location.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="fileLocation">The location of the file.</param>
        /// <returns><c>true</c> if the file exists; otherwise, <c>false</c>.</returns>
        public static bool FileExistsAtLocation(string fileName, string fileLocation)
        {
            return File.Exists(Path.Combine(fileLocation, fileName));
        }

        /// <summary>
        /// Gets the correct path for the appsettings JSON file.
        /// </summary>
        /// <returns>The correct path for the appsettings JSON file.</returns>
        public static string GetCorrectAppSettingsPath()
        {
            return FileExistsAtLocation(AppSettingsName, AppDataDirectory) ?
                AppDataDirectory :
                BaseDirectory;
        }

        /// <summary>
        /// Gets the correct path for the custom colors JSON file.
        /// </summary>
        /// <returns>The correct path for the custom colors JSON file.</returns>
        public static string GetCorrectCustomColorsPath()
        {
            return FileExistsAtLocation(CustomColorsName, AppDataDirectory) ?
                AppDataDirectory :
                BaseDirectory;
        }

        /// <summary>
        /// Gets the correct path for the custom items text file.
        /// </summary>
        /// <returns>The correct path for the custom items text file.</returns>
        public static string GetCorrectCustomItemsPath()
        {
            return FileExistsAtLocation(CustomItemsName, AppDataDirectory) ?
                AppDataDirectory :
                BaseDirectory;
        }
    }
}
