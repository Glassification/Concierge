// <copyright file="UserSettings.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    using Concierge.Common.Enums;

    public sealed class UserSettings
    {
        public UserSettings()
        {
            this.Autosaving = new Autosave();
            this.DefaultFolder = new DefaultFolders();
        }

        public Autosave Autosaving { get; set; }

        public bool CheckVersion { get; set; }

        public DefaultFolders DefaultFolder { get; set; }

        public bool MuteSounds { get; set; }

        public UnitTypes UnitOfMeasurement { get; set; }

        public bool UseCoinWeight { get; set; }

        public bool UseEncumbrance { get; set; }
    }
}
