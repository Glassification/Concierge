// <copyright file="AppConfigReadWriter.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Persistence
{
    using System;
    using System.Configuration;

    using Concierge.Utility.Extensions;

    public static class AppConfigReadWriter
    {
        private delegate bool TryParseDelegate<T>(string s, out T result);

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

        public static T GetSetting<T>(string settingName, T defaultValue)
        {
            if (!typeof(T).IsPrimitive)
            {
                throw new ArgumentException("T must be a primitive type");
            }

            try
            {
                var tryParse = (TryParseDelegate<T>)Delegate.CreateDelegate(typeof(TryParseDelegate<T>), typeof(T), "TryParse", true);

                return !tryParse(ConfigurationManager.AppSettings[settingName], out T value) ? defaultValue : value;
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
    }
}
