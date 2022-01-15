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

            section = config.GetSection(nameof(CustomColors));
            CustomColors = section.Get<Dictionary<string, string>>();
            CustomColors = new Dictionary<string, string>(CustomColors, StringComparer.InvariantCultureIgnoreCase);

            section = config.GetSection(nameof(StartUp));
            StartUp = section.Get<StartUp>();

            section = config.GetSection(nameof(UserSettings));
            UserSettings = section.Get<UserSettings>();
        }

        public delegate void UnitsChangedEventHandler(object sender, EventArgs e);

        public static event UnitsChangedEventHandler UnitsChanged;

        public static Dictionary<string, string> CustomColors { get; private set; }

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
            UserSettings.MuteSounds = userSettingsDto.MuteSounds;
            UserSettings.UseCoinWeight = userSettingsDto.UseCoinWeight;
            UserSettings.UseEncumbrance = userSettingsDto.UseEncumbrance;
            UserSettings.UnitOfMeasurement = userSettingsDto.UnitOfMeasurement;
            UserSettings.AttemptToCenterWindows = userSettingsDto.AttemptToCenterWindows;

            if (Program.IsDebug)
            {
                return;
            }

            var appSettings = new AppSettings()
            {
                UserSettings = UserSettings,
                CustomColors = CustomColors,
                StartUp = StartUp,
            };

            var config = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
            var appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            File.WriteAllText(appSettingsPath, config);
        }

        public static void RefreshUnits(UserSettingsDto userSettingsDto = null)
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
                MuteSounds = UserSettings.MuteSounds,
                UseCoinWeight = UserSettings.UseCoinWeight,
                UseEncumbrance = UserSettings.UseEncumbrance,
                UnitOfMeasurement = UserSettings.UnitOfMeasurement,
                AttemptToCenterWindows = UserSettings.AttemptToCenterWindows,
            };
        }
    }
}
