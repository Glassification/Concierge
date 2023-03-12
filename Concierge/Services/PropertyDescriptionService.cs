// <copyright file="PropertyDescriptionService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System.Collections.Generic;

    public class PropertyDescriptionService
    {
        private readonly Dictionary<string, string> descriptions;

        public PropertyDescriptionService()
        {
            this.descriptions = new Dictionary<string, string>();
        }

        public string GetDescription(string controlName, string defaultDescription = "")
        {
            if (this.descriptions.TryGetValue(controlName, out string? value))
            {
                return value ?? defaultDescription;
            }

            return defaultDescription;
        }
    }
}
