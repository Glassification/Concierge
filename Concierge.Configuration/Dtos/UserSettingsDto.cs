// <copyright file="UserSettingsDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Dtos
{
    using Concierge.Common.Enums;
    using Concierge.Configuration.Objects;

    public sealed class UserSettingsDto
    {
        public UserSettingsDto()
        {
            this.Autosaving = new Autosave();
            this.DefaultFolder = new DefaultFolders();
        }

        public Autosave Autosaving { get; set; }

        public bool CheckVersion { get; set; }

        public DefaultFolders DefaultFolder { get; set; }

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
                settings.Autosaving.Equals(this.Autosaving) &&
                settings.DefaultFolder.Equals(this.DefaultFolder) &&
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
