// <copyright file="UserSettingsDto.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Dtos
{
    using System.Windows;

    using Concierge.Common.Enums;
    using Concierge.Configuration.Objects;

    /// <summary>
    /// Represents a data transfer object (DTO) for storing user settings.
    /// </summary>
    public sealed class UserSettingsDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSettingsDto"/> class with default settings.
        /// </summary>
        public UserSettingsDto()
        {
            this.Autosaving = new Autosave();
            this.DefaultFolder = new DefaultFolders();
        }

        /// <summary>
        /// Gets or sets the autosave settings associated with the user.
        /// </summary>
        public Autosave Autosaving { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to check for application updates or versions automatically.
        /// </summary>
        public bool CheckVersion { get; set; }

        /// <summary>
        /// Gets or sets the default folders used for saving data.
        /// </summary>
        public DefaultFolders DefaultFolder { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment of the header in the user interface.
        /// </summary>
        public HorizontalAlignment HeaderAlignment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to mute sounds in the application.
        /// </summary>
        public bool MuteSounds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use coin weight in calculations.
        /// </summary>
        public bool UseCoinWeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use encumbrance in calculations.
        /// </summary>
        public bool UseEncumbrance { get; set; }

        /// <summary>
        /// Gets or sets the unit of measurement preferred by the user, such as Metric or Imperial.
        /// </summary>
        public UnitTypes UnitOfMeasurement { get; set; }

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
