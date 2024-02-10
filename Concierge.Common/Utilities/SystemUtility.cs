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
        private const int Timeout = 1000;
        private const string Host = "google.com";

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

        public static InternetInfo GetInternetStatus()
        {
            return new InternetInfo(string.Empty, HasInternet ? InternetStatus.Wireless : InternetStatus.Disconnected);
        }

        /// <summary>
        /// Retrieves the battery status information.
        /// </summary>
        /// <returns>The battery status information.</returns>
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
    }
}
