// <copyright file="AppSettingsManager.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration
{
    using System;
    using System.IO;

    using Concierge.Common;
    using Concierge.Configuration.Dtos;
    using Concierge.Configuration.Objects;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    /// <summary>
    /// Provides management and access to application settings.
    /// </summary>
    public static class AppSettingsManager
    {
        static AppSettingsManager()
        {
            IConfigurationSection section;

            // Loads application settings from a JSON file
            var config = new ConfigurationBuilder()
                .SetBasePath(ConciergeFiles.GetCorrectAppSettingsPath())
                .AddJsonFile(ConciergeFiles.AppSettingsName)
                .Build();

            // Initializes startup settings from the configuration file or creates a new instance
            section = config.GetSection(nameof(StartUp));
            StartUp = section.Get<StartUp>() ?? new StartUp();

            // Initializes user settings from the configuration file or creates a new instance
            section = config.GetSection(nameof(UserSettings));
            UserSettings = section.Get<UserSettings>() ?? new UserSettings();
        }

        /// <summary>
        /// Represents the delegate for the event that is raised when the unit of measurement changes.
        /// </summary>
        public delegate void UnitsChangedEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Event that is raised when the unit of measurement changes.
        /// </summary>
        public static event UnitsChangedEventHandler? UnitsChanged;

        /// <summary>
        /// Gets the startup settings of the application.
        /// </summary>
        public static StartUp StartUp { get; private set; }

        /// <summary>
        /// Gets the user settings of the application.
        /// </summary>
        public static UserSettings UserSettings { get; private set; }

        /// <summary>
        /// Updates the user settings of the application with the provided data. Will not update appsettings while in debug mode.
        /// </summary>
        /// <param name="userSettingsDto">The <see cref="UserSettingsDto"/> containing the updated settings.</param>
        /// <param name="isDebug">Indicates whether the application is in debug mode.</param>
        public static void UpdateSettings(UserSettingsDto userSettingsDto, bool isDebug)
        {
            if (UserSettings.UnitOfMeasurement != userSettingsDto.UnitOfMeasurement)
            {
                RefreshUnits(userSettingsDto);
            }

            UserSettings.Autosaving.Set(userSettingsDto.Autosaving);
            UserSettings.DefaultFolder.Set(userSettingsDto.DefaultFolder);

            UserSettings.CheckVersion = userSettingsDto.CheckVersion;
            UserSettings.HeaderAlignment = userSettingsDto.HeaderAlignment;
            UserSettings.MuteSounds = userSettingsDto.MuteSounds;
            UserSettings.UseCoinWeight = userSettingsDto.UseCoinWeight;
            UserSettings.UseEncumbrance = userSettingsDto.UseEncumbrance;
            UserSettings.UnitOfMeasurement = userSettingsDto.UnitOfMeasurement;

            if (isDebug)
            {
                return;
            }

            WriteUpdatedSettingsToFile();
        }

        /// <summary>
        /// Triggers the UnitsChanged event with the provided user settings data.
        /// </summary>
        /// <param name="userSettingsDto">The <see cref="UserSettingsDto"/> containing the unit of measurement changes.</param>
        public static void RefreshUnits(UserSettingsDto? userSettingsDto = null)
        {
            UnitsChanged?.Invoke(userSettingsDto is null ? ToUserSettingsDto() : userSettingsDto, new EventArgs());
        }

        /// <summary>
        /// Converts the current user settings to a <see cref="UserSettingsDto"/> object.
        /// </summary>
        /// <returns>The <see cref="UserSettingsDto"/> object representing the current user settings.</returns>
        public static UserSettingsDto ToUserSettingsDto()
        {
            return new UserSettingsDto()
            {
                Autosaving = new Autosave()
                {
                    Enabled = UserSettings.Autosaving.Enabled,
                    Interval = UserSettings.Autosaving.Interval,
                },
                CheckVersion = UserSettings.CheckVersion,
                DefaultFolder = new DefaultFolders()
                {
                    OpenFolder = UserSettings.DefaultFolder.OpenFolder,
                    SaveFolder = UserSettings.DefaultFolder.SaveFolder,
                    UseOpenFolder = UserSettings.DefaultFolder.UseOpenFolder,
                    UseSaveFolder = UserSettings.DefaultFolder.UseSaveFolder,
                },
                HeaderAlignment = UserSettings.HeaderAlignment,
                MuteSounds = UserSettings.MuteSounds,
                UseCoinWeight = UserSettings.UseCoinWeight,
                UseEncumbrance = UserSettings.UseEncumbrance,
                UnitOfMeasurement = UserSettings.UnitOfMeasurement,
            };
        }

        private static void WriteUpdatedSettingsToFile()
        {
            var appSettings = new AppSettings()
            {
                StartUp = StartUp,
                UserSettings = UserSettings,
            };

            var config = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
            var appSettingsPath = Path.Combine(ConciergeFiles.GetCorrectAppSettingsPath(), ConciergeFiles.AppSettingsName);
            File.WriteAllText(appSettingsPath, config);
        }
    }
}
