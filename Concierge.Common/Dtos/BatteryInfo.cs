// <copyright file="BatteryInfo.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Dtos
{
    using Concierge.Common.Enums;
    using Concierge.Common.Extensions;
    using MaterialDesignThemes.Wpf;

    /// <summary>
    /// Represents the status of a battery with an icon and a corresponding percentage value.
    /// </summary>
    public sealed class BatteryInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BatteryInfo"/> class with default values.
        /// </summary>
        public BatteryInfo()
            : this(0, BatteryStatus.Undefined)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BatteryInfo"/> class with the specified percentage and status.
        /// </summary>
        /// <param name="percentage">The percentage value indicating the battery level.</param>
        /// <param name="status">The current status of the battery.</param>
        public BatteryInfo(int percentage, BatteryStatus status)
        {
            this.Percentage = percentage;
            this.Status = status;
        }

        /// <summary>
        /// Gets the icon representing the battery status.
        /// </summary>
        public PackIconKind Icon
        {
            get
            {
                if (this.Status == BatteryStatus.Undefined)
                {
                    return PackIconKind.PowerPlug;
                }

                var isCharging = this.Status == BatteryStatus.AcConnected;

                if (this.Percentage < 10)
                {
                    return isCharging ? PackIconKind.BatteryChargingOutline : PackIconKind.Battery0;
                }
                else if (this.Percentage < 20)
                {
                    return isCharging ? PackIconKind.BatteryCharging10 : PackIconKind.Battery10;
                }
                else if (this.Percentage < 30)
                {
                    return isCharging ? PackIconKind.BatteryCharging20 : PackIconKind.Battery20;
                }
                else if (this.Percentage < 40)
                {
                    return isCharging ? PackIconKind.BatteryCharging30 : PackIconKind.Battery30;
                }
                else if (this.Percentage < 50)
                {
                    return isCharging ? PackIconKind.BatteryCharging40 : PackIconKind.Battery40;
                }
                else if (this.Percentage < 60)
                {
                    return isCharging ? PackIconKind.BatteryCharging50 : PackIconKind.Battery50;
                }
                else if (this.Percentage < 70)
                {
                    return isCharging ? PackIconKind.BatteryCharging60 : PackIconKind.Battery60;
                }
                else if (this.Percentage < 80)
                {
                    return isCharging ? PackIconKind.BatteryCharging70 : PackIconKind.Battery70;
                }
                else if (this.Percentage < 90)
                {
                    return isCharging ? PackIconKind.BatteryCharging80 : PackIconKind.Battery80;
                }
                else if (this.Percentage < 100)
                {
                    return isCharging ? PackIconKind.BatteryCharging90 : PackIconKind.Battery90;
                }
                else
                {
                    return isCharging ? PackIconKind.BatteryCharging100 : PackIconKind.Battery100;
                }
            }
        }

        /// <summary>
        /// Gets or sets the percentage value indicating the battery level.
        /// </summary>
        public int Percentage { get; set; }

        /// <summary>
        /// Gets or sets the enum value indicating the battery status.
        /// </summary>
        public BatteryStatus Status { get; set; }

        public override string ToString()
        {
            return $"Battery Status: {this.Status.ToString().FormatFromEnum()}, {this.Percentage}%";
        }
    }
}
