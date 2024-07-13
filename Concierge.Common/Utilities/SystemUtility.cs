// <copyright file="SystemUtility.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Utilities
{
    using System;
    using System.Diagnostics;
    using System.Management;
    using System.Net.NetworkInformation;

    using Concierge.Common.Dtos;
    using Concierge.Common.Enums;

    /// <summary>
    /// Provides utility methods for system-related operations.
    /// </summary>
    public static class SystemUtility
    {
        private const int Windows7 = 7601;
        private const int Windows8 = 9200;
        private const int Windows10 = 10240;
        private const int Windows11 = 22000;

        private const int Timeout = 1000;
        private const string Host = "google.com";

        private static OSVersion osVersion = OSVersion.Unknown;

        /// <summary>
        /// Gets a value indicating whether the system has an active Internet connection.
        /// </summary>
        public static bool HasInternet
        {
            get
            {
                try
                {
                    return new Ping().Send(Host, Timeout, new byte[32], new PingOptions()).Status == IPStatus.Success;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Retrieves information about the current Internet status.
        /// </summary>
        /// <returns>An instance of <see cref="InternetInfo"/> containing information about the Internet status.</returns>
        public static InternetInfo GetInternetStatus() => new (string.Empty, HasInternet ? InternetStatus.Wireless : InternetStatus.Disconnected);

        /// <summary>
        /// Retrieves the battery status information.
        /// </summary>
        /// <returns>An instance of <see cref="BatteryInfo"/> containing information about the battery status.</returns>
        public static BatteryInfo GetBatteryStatus()
        {
            try
            {
                var wmi = new ManagementClass("Win32_Battery");
                var allBatteries = wmi.GetInstances();
                var status = BatteryStatus.Undefined;

                foreach (var battery in allBatteries)
                {
                    PropertyData pData = battery.Properties["BatteryStatus"];

                    if (pData != null && pData.Value != null && Enum.IsDefined(typeof(BatteryStatus), pData.Value))
                    {
                        status = (BatteryStatus)pData.Value;
                    }
                }

                return new BatteryInfo((int)GetBatteryPercent(), status);
            }
            catch (Exception)
            {
                return new BatteryInfo();
            }
        }

        /// <summary>
        /// Retrieves the battery percentage level.
        /// </summary>
        /// <returns>The battery percentage level.</returns>
        public static double GetBatteryPercent()
        {
            try
            {
                var wmi = new ManagementClass("Win32_Battery");
                var allBatteries = wmi.GetInstances();
                var batteryLevel = 0.0;

                foreach (var battery in allBatteries)
                {
                    batteryLevel = Convert.ToDouble(battery["EstimatedChargeRemaining"]);
                }

                return batteryLevel;
            }
            catch (Exception)
            {
                return 0.0;
            }
        }

        /// <summary>
        /// Opens the on-screen keyboard application.
        /// </summary>
        public static void OpenOnScreenKeyboard()
        {
            Process.Start(new ProcessStartInfo("osk.exe")
            {
                UseShellExecute = true,
            });
        }

        /// <summary>
        /// Retrieves the Windows version based on the current build number.
        /// </summary>
        /// <returns>The Windows version.</returns>
        public static OSVersion GetWindowsVersion()
        {
            if (osVersion != OSVersion.Unknown)
            {
                return osVersion;
            }

            var buildNumber = Environment.OSVersion.Version.Build;
            if (ConciergeMath.Between(buildNumber, Windows7, Windows8, Inclusivity.LeftInclusive))
            {
                osVersion = OSVersion.Windows7;
            }
            else if (ConciergeMath.Between(buildNumber, Windows8, Windows10, Inclusivity.LeftInclusive))
            {
                osVersion = OSVersion.Windows8;
            }
            else if (ConciergeMath.Between(buildNumber, Windows10, Windows11, Inclusivity.LeftInclusive))
            {
                osVersion = OSVersion.Windows10;
            }
            else if (buildNumber >= Windows11)
            {
                osVersion = OSVersion.Windows11;
            }

            return osVersion;
        }
    }
}
