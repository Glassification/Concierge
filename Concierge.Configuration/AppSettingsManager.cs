﻿// <copyright file="AppSettingsManager.cs" company="Thomas Beckett">
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

        public static void UpdateSettings(UserSettingsDto userSettingsDto, bool isDebug)
        {
            if (UserSettings.UnitOfMeasurement != userSettingsDto.UnitOfMeasurement)
            {
                RefreshUnits(userSettingsDto);
            }

            UserSettings.Autosaving.Set(userSettingsDto.Autosaving);
            UserSettings.DefaultFolder.Set(userSettingsDto.DefaultFolder);

            UserSettings.CheckVersion = userSettingsDto.CheckVersion;
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

        public static void RefreshUnits(UserSettingsDto? userSettingsDto = null)
        {
            UnitsChanged?.Invoke(userSettingsDto is null ? ToUserSettingsDto() : userSettingsDto, new EventArgs());
        }

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