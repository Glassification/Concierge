// <copyright file="StringResourceService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System.Globalization;
    using System.Resources;

    using Concierge.Configuration;

    public sealed class StringResourceService
    {
        private readonly CultureInfo cultureInfo;
        private readonly ResourceManager resourceManager;

        public StringResourceService()
            : this(CultureInfo.InvariantCulture, Properties.Resources.ResourceManager)
        {
        }

        public StringResourceService(CultureInfo cultureInfo, ResourceManager resourceManager)
        {
            this.cultureInfo = cultureInfo;
            this.resourceManager = resourceManager;
        }

        public string CleanString(string value)
        {
            return value.Replace("TextBackground", "TextBox", false, this.cultureInfo);
        }

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
