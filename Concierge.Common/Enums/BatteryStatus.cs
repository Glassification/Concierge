// <copyright file="BatteryStatus.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Common.Enums
{
    public enum BatteryStatus : ushort
    {
        Discharging = 1,
        AcConnected,
        FullyCharged,
        Low,
        Critical,
        Charging,
        ChargingAndHigh,
        ChargingAndLow,
        ChargingAndCritical,
        Undefined,
        PartiallyCharged,
    }
}
