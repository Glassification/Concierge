// <copyright file="AppSettingsManager.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Media;

    using Concierge.Configuration.Dtos;
    using Concierge.Configuration.Objects;
    using Concierge.Persistence;
    using Concierge.Utility.Extensions;
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

            section = config.GetSection(nameof(ColorPicker));
            ColorPicker = section.Get<ColorPicker>() ?? new ColorPicker();

            section = config.GetSection(nameof(CustomColors));
            CustomColors = section.Get<Dictionary<string, string>>() ?? new Dictionary<string, string>();
            CustomColors = new Dictionary<string, string>(CustomColors, StringComparer.InvariantCultureIgnoreCase);

            section = config.GetSection(nameof(StartUp));
            StartUp = section.Get<StartUp>() ?? new StartUp();

            section = config.GetSection(nameof(UserSettings));
            UserSettings = section.Get<UserSettings>() ?? new UserSettings();
        }

        public delegate void UnitsChangedEventHandler(object sender, EventArgs e);

        public static event UnitsChangedEventHandler? UnitsChanged;

        public static ColorPicker ColorPicker { get; private set; }

        public static Dictionary<string, string> CustomColors { get; private set; }

        public static StartUp StartUp { get; private set; }

        public static UserSettings UserSettings { get; private set; }

        public static void UpdateRecentColors(List<Color> colors)
        {
            for (int i = 0; i < colors.Count; i++)
            {
                ColorPicker.RecentColors[i] = colors[i].GetName();
            }

            if (Program.IsDebug)
            {
                return;
            }

            WriteUpdatedSettingsToFile();
        }

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
                ColorPicker = ColorPicker,
                CustomColors = CustomColors,
                StartUp = StartUp,
                UserSettings = UserSettings,
            };

            var config = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
            var appSettingsPath = Path.Combine(ConciergeFiles.GetCorrectAppSettingsPath(), ConciergeFiles.AppSettingsName);
            File.WriteAllText(appSettingsPath, config);
        }
    }
}
