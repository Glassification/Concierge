// <copyright file="StartUp.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    /// <summary>
    /// Represents startup settings for the application.
    /// </summary>
    public sealed class StartUp
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartUp"/> class with default settings.
        /// </summary>
        public StartUp()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating how many backup slots to maintain.
        /// </summary>
        public int MaxBackups { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to compress the character sheet when saving.
        /// </summary>
        public bool CompressCharacterSheet { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable the console.
        /// </summary>
        public bool EnableConsole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable network access.
        /// </summary>
        public bool EnableNetworkAccess { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the splash screen during startup.
        /// </summary>
        public bool ShowSplashScreen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable advanced features.
        /// </summary>
        public bool ExpertMode { get; set; }
    }
}
