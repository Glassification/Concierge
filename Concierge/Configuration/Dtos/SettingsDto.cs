// <copyright file="SettingsDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Dtos
{
    using Concierge.Utility.Units.Enums;

    public class SettingsDto
    {
        public SettingsDto()
        {
        }

        public bool AutosaveEnabled { get; init; }

        public int AutosaveInterval { get; init; }

        public bool CheckVersion { get; init; }

        public bool MuteSounds { get; init; }

        public bool UseCoinWeight { get; init; }

        public bool UseEncumbrance { get; init; }

        public UnitTypes UnitOfMeasurement { get; init; }

        public bool AttemptToCenterWindows { get; init; }

        public override bool Equals(object obj)
        {
            if (obj is not SettingsDto)
            {
                return false;
            }

            var settings = obj as SettingsDto;

            return
                settings.AutosaveInterval == this.AutosaveInterval &&
                settings.AutosaveEnabled == this.AutosaveEnabled &&
                settings.CheckVersion == this.CheckVersion &&
                settings.MuteSounds == this.MuteSounds &&
                settings.UseCoinWeight == this.UseCoinWeight &&
                settings.UseEncumbrance == this.UseEncumbrance &&
                settings.UnitOfMeasurement == this.UnitOfMeasurement &&
                settings.AttemptToCenterWindows == this.AttemptToCenterWindows;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
