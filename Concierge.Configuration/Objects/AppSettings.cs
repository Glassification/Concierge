// <copyright file="AppSettings.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    /// <summary>
    /// Represents application settings that store both startup and user-specific settings.
    /// </summary>
    public sealed class AppSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettings"/> class with default settings.
        /// </summary>
        public AppSettings()
        {
            this.StartUp = new StartUp();
            this.UserSettings = new UserSettings();
        }

        /// <summary>
        /// Gets or sets the startup settings associated with the application.
        /// </summary>
        public StartUp StartUp { get; set; }

        /// <summary>
        /// Gets or sets the user-specific settings associated with the application.
        /// </summary>
        public UserSettings UserSettings { get; set; }
    }
}
