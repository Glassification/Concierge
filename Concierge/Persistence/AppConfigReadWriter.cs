// <copyright file="AppConfigReadWriter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System;
    using System.Configuration;
    using System.Globalization;

    using Concierge.Utility.Extensions;

    public static class AppConfigReadWriter
    {
        /// =========================================
        /// SaveSetting()
        /// =========================================
        public static void SaveSetting(string settingName, string value)
        {
            if (settingName.IsNullOrWhiteSpace())
            {
                Program.Logger.Error($"Setting name is empty, cannot save the value '{value}'.");
                return;
            }

            if (value.IsNullOrWhiteSpace())
            {
                Program.Logger.Error($"'{settingName}' has an invalid value.");
                return;
            }

            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings[settingName].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex);
            }
        }

        /// =========================================
        /// GetEnumSetting()
        /// =========================================
        public static T GetEnumSetting<T>(string settingName)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            try
            {
                return !Enum.TryParse(ConfigurationManager.AppSettings[settingName], out T value) ? default : value;
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex);
                return default;
            }
        }

        /// =========================================
        /// GetBoolSetting()
        /// =========================================
        public static bool GetBoolSetting(string settingName, bool defaultValue)
        {
            try
            {
                return !bool.TryParse(ConfigurationManager.AppSettings[settingName], out bool value) ? defaultValue : value;
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex);
                return defaultValue;
            }
        }

        /// =========================================
        /// GetStringSetting()
        /// =========================================
        public static string GetStringSetting(string settingName, string defaultValue)
        {
            try
            {
                string value;

                return (value = ConfigurationManager.AppSettings[settingName]).IsNullOrWhiteSpace() ? defaultValue : value;
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex);
                return defaultValue;
            }
        }

        /// =========================================
        /// GetIntSetting()
        /// =========================================
        public static int GetIntSetting(string settingName, int defaultValue)
        {
            try
            {
                return !int.TryParse(
                    ConfigurationManager.AppSettings[settingName],
                    NumberStyles.Float,
                    CultureInfo.CurrentCulture,
                    out int value)
                    ? defaultValue
                    : value;
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex);
                return defaultValue;
            }
        }

        /// =========================================
        /// GetDoubleSetting()
        /// =========================================
        public static double GetDoubleSetting(string settingName, double defaultValue)
        {
            try
            {
                return !double.TryParse(
                    ConfigurationManager.AppSettings[settingName],
                    NumberStyles.Float,
                    CultureInfo.CurrentCulture,
                    out double value)
                    ? defaultValue
                    : value;
            }
            catch (Exception ex)
            {
                Program.Logger.Error(ex);
                return defaultValue;
            }
        }
    }
}
