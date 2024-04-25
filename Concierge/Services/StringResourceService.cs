// <copyright file="StringResourceService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System.Globalization;
    using System.Resources;

    using Concierge.Configuration;

    /// <summary>
    /// Provides functionality for managing string resources in the application.
    /// </summary>
    public sealed class StringResourceService
    {
        private readonly CultureInfo cultureInfo;
        private readonly ResourceManager resourceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringResourceService"/> class with the default culture and resource manager.
        /// </summary>
        public StringResourceService()
            : this(CultureInfo.InvariantCulture, Properties.Resources.ResourceManager)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringResourceService"/> class with the specified culture and resource manager.
        /// </summary>
        /// <param name="cultureInfo">The culture to use for resource retrieval.</param>
        /// <param name="resourceManager">The resource manager to use for accessing resources.</param>
        public StringResourceService(CultureInfo cultureInfo, ResourceManager resourceManager)
        {
            this.cultureInfo = cultureInfo;
            this.resourceManager = resourceManager;
        }

        /// <summary>
        /// Cleans a string by replacing specific substrings.
        /// </summary>
        /// <param name="value">The string to clean.</param>
        /// <returns>The cleaned string.</returns>
        public string CleanString(string value)
        {
            return value.Replace("TextBackground", "TextBox", false, this.cultureInfo);
        }

        /// <summary>
        /// Gets the description of a property based on the window and control name.
        /// </summary>
        /// <param name="window">The name of the window.</param>
        /// <param name="controlName">The name of the control.</param>
        /// <param name="defaultDescription">The default description to return if no specific description is found.</param>
        /// <returns>The description of the property.</returns>
        public string GetPropertyDescription(string window, string controlName, string defaultDescription = "")
        {
            var wildWasteland = AppSettingsManager.StartUp.WildWasteland ? "_WildWasteland" : string.Empty;
            var result = this.resourceManager.GetString($"{window}_{controlName}{wildWasteland}", this.cultureInfo) ?? defaultDescription;

            if (AppSettingsManager.StartUp.WildWasteland && result.Equals(defaultDescription))
            {
                result = this.resourceManager.GetString($"{window}_{controlName}", this.cultureInfo) ?? defaultDescription;
            }

            return result;
        }
    }
}
