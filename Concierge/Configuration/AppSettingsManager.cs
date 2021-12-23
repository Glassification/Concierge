// <copyright file="AppSettingsManager.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;

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
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            section = config.GetSection(nameof(Settings));
            Settings = section.Get<Settings>();

            section = config.GetSection(nameof(CustomColors));
            CustomColors = section.Get<Dictionary<string, string>>();
        }

        public delegate void UnitsChangedEventHandler(object sender, EventArgs e);

        public static event UnitsChangedEventHandler UnitsChanged;

        public static Settings Settings { get; private set; }

        public static Dictionary<string, string> CustomColors { get; private set; }

        public static void UpdateSettings(SettingsDto settingsDto)
        {
            if (Settings.UnitOfMeasurement != settingsDto.UnitOfMeasurement)
            {
                UnitsChanged?.Invoke(settingsDto, new EventArgs());
            }

            Settings.AutosaveEnabled = settingsDto.AutosaveEnabled;
            Settings.AutosaveInterval = settingsDto.AutosaveInterval;
            Settings.CheckVersion = settingsDto.CheckVersion;
            Settings.MuteSounds = settingsDto.MuteSounds;
            Settings.UseCoinWeight = settingsDto.UseCoinWeight;
            Settings.UseEncumbrance = settingsDto.UseEncumbrance;
            Settings.UnitOfMeasurement = settingsDto.UnitOfMeasurement;

            if (Program.IsDebug)
            {
                return;
            }

            var appSettings = new AppSettings()
            {
                Settings = Settings,
                CustomColors = CustomColors,
            };

            var config = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
            var appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            File.WriteAllText(appSettingsPath, config);
        }

        public static void RefreshUnits()
        {
            UnitsChanged?.Invoke(ToSettingsDto(), new EventArgs());
        }

        public static SettingsDto ToSettingsDto()
        {
            return new SettingsDto()
            {
                AutosaveEnabled = Settings.AutosaveEnabled,
                AutosaveInterval = Settings.AutosaveInterval,
                CheckVersion = Settings.CheckVersion,
                MuteSounds = Settings.MuteSounds,
                UseCoinWeight = Settings.UseCoinWeight,
                UseEncumbrance = Settings.UseEncumbrance,
                UnitOfMeasurement = Settings.UnitOfMeasurement,
            };
        }
    }
}
