// <copyright file="AppSettingsManager.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration
{
    using System;
    using System.IO;

    using Concierge.Configuration.Dtos;
    using Concierge.Configuration.Objects;
    using Concierge.Persistence;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    public static class AppSettingsManager
    {
        static AppSettingsManager()
        {
            IConfigurationSection section;

            var config = new ConfigurationBuilder()
                .SetBasePath(ConciergeFiles.GetCorrectAppSettingsPath())
                .AddJsonFile(ConciergeFiles.AppSettingsName)
                .Build();

            section = config.GetSection(nameof(StartUp));
            StartUp = section.Get<StartUp>() ?? new StartUp();

            section = config.GetSection(nameof(UserSettings));
            UserSettings = section.Get<UserSettings>() ?? new UserSettings();
        }

        public delegate void UnitsChangedEventHandler(object sender, EventArgs e);

        public static event UnitsChangedEventHandler? UnitsChanged;

        public static StartUp StartUp { get; private set; }

        public static UserSettings UserSettings { get; private set; }

        public static void UpdateSettings(UserSettingsDto userSettingsDto)
        {
            if (UserSettings.UnitOfMeasurement != userSettingsDto.UnitOfMeasurement)
            {
                RefreshUnits(userSettingsDto);
            }

            UserSettings.AutosaveEnabled = userSettingsDto.AutosaveEnabled;
            UserSettings.AutosaveInterval = userSettingsDto.AutosaveInterval;
            UserSettings.CheckVersion = userSettingsDto.CheckVersion;
            UserSettings.MaxCustomColors = userSettingsDto.MaxCustomColors;
            UserSettings.MuteSounds = userSettingsDto.MuteSounds;
            UserSettings.UseCoinWeight = userSettingsDto.UseCoinWeight;
            UserSettings.UseEncumbrance = userSettingsDto.UseEncumbrance;
            UserSettings.UnitOfMeasurement = userSettingsDto.UnitOfMeasurement;

            if (Program.IsDebug)
            {
                return;
            }

            WriteUpdatedSettingsToFile();
        }

        public static void RefreshUnits(UserSettingsDto? userSettingsDto = null)
        {
            UnitsChanged?.Invoke(userSettingsDto is null ? ToUserSettingsDto() : userSettingsDto, new EventArgs());
        }

        public static UserSettingsDto ToUserSettingsDto()
        {
            return new UserSettingsDto()
            {
                AutosaveEnabled = UserSettings.AutosaveEnabled,
                AutosaveInterval = UserSettings.AutosaveInterval,
                CheckVersion = UserSettings.CheckVersion,
                MaxCustomColors = UserSettings.MaxCustomColors,
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
