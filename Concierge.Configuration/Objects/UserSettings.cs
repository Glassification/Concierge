// <copyright file="UserSettings.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    using System;
    using System.Windows;

    using Concierge.Common.Enums;
    using Concierge.Common.Extensions;

    /// <summary>
    /// Represents the user settings for an application.
    /// </summary>
    public sealed class UserSettings
    {
        private int healingThreshold;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSettings"/> class with default settings.
        /// </summary>
        public UserSettings()
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
        /// Gets or sets the hp to heal to during a short rest.
        /// </summary>
        public int HealingThreshold
        {
            get
            {
                return this.healingThreshold;
            }

            set
            {
                this.healingThreshold = Math.Max(Math.Min(value, 100), 0).NearestMultipleOfTen();
            }
        }

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
    }
}
