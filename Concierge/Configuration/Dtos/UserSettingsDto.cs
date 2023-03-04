// <copyright file="UserSettingsDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Dtos
{
    using Concierge.Utility.Units.Enums;

    public sealed class UserSettingsDto
    {
        public UserSettingsDto()
        {
        }

        public bool AutosaveEnabled { get; init; }

        public int AutosaveInterval { get; init; }

        public bool CheckVersion { get; init; }

        public bool MuteSounds { get; init; }

        public bool UseCoinWeight { get; init; }

        public bool UseEncumbrance { get; init; }

        public UnitTypes UnitOfMeasurement { get; init; }

        public override bool Equals(object? obj)
        {
            if (obj is not UserSettingsDto settings)
            {
                return false;
            }

            return
                settings.AutosaveInterval == this.AutosaveInterval &&
                settings.AutosaveEnabled == this.AutosaveEnabled &&
                settings.CheckVersion == this.CheckVersion &&
                settings.MuteSounds == this.MuteSounds &&
                settings.UseCoinWeight == this.UseCoinWeight &&
                settings.UseEncumbrance == this.UseEncumbrance &&
                settings.UnitOfMeasurement == this.UnitOfMeasurement;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
