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
            Directory.CreateDirectory(BackupDirectory);
            Directory.CreateDirectory(LoggingDirectory);

            SetupAppsettings();
            SetupCustomColors();
            SetupCustomItems();
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
        /// Gets the path to the directory where backup files are stored.
        /// </summary>
        public static string BackupDirectory => Path.Combine(applicationData, @"Concierge\Backups");

        /// <summary>
        /// Gets the path to the directory where logging files are stored.
        /// </summary>
        public static string LoggingDirectory => Path.Combine(applicationData, @"Concierge\Logging");

        /// <summary>
        /// Gets the base directory of the application.
        /// </summary>
        public static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// Gets the path to the appsettings file.
        /// </summary>
        public static string AppsettingsPath { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the path to the custom colors file.
        /// </summary>
        public static string CustomColorsPath { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the path to the custom items file.
        /// </summary>
        public static string CustomItemsPath { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the directory where the executing assembly is located.
        /// </summary>
        public static string ExecutingDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        private static void SetupAppsettings()
        {
            var baseAppsettings = Path.Combine(BaseDirectory, AppSettingsName);
            var appDataAppsettings = Path.Combine(AppDataDirectory, AppSettingsName);
            if (!File.Exists(appDataAppsettings))
            {
                File.Copy(baseAppsettings, appDataAppsettings);
            }

            AppsettingsPath = AppDataDirectory;
        }

        private static void SetupCustomColors()
        {
            var baseCustomColors = Path.Combine(BaseDirectory, CustomColorsName);
            var appDataCustomColors = Path.Combine(AppDataDirectory, CustomColorsName);
            if (!File.Exists(appDataCustomColors))
            {
                File.Copy(baseCustomColors, appDataCustomColors);
            }

            CustomColorsPath = AppDataDirectory;
        }

        private static void SetupCustomItems()
        {
            var appDataCustomItems = Path.Combine(AppDataDirectory, CustomItemsName);
            if (!File.Exists(appDataCustomItems))
            {
                File.Create(appDataCustomItems);
            }

            CustomItemsPath = AppDataDirectory;
        }
    }
}
