// <copyright file="StringResourceService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System.Globalization;

    public sealed class StringResourceService
    {
        private readonly CultureInfo cultureInfo;

        public StringResourceService()
            : this(CultureInfo.InvariantCulture)
        {
        }

        public StringResourceService(CultureInfo cultureInfo)
        {
            this.cultureInfo = cultureInfo;
        }

        public string GetPropertyDescription(string window, string controlName, string defaultDescription = "")
        {
            return Properties.Resources.ResourceManager.GetString($"{window}_{controlName}", this.cultureInfo) ?? defaultDescription;
        }
    }
}
