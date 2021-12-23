// <copyright file="AppSettings.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    using System.Collections.Generic;

    public class AppSettings
    {
        public AppSettings()
        {
        }

        public Settings Settings { get; set; }

        public Dictionary<string, string> CustomColors { get; set; }
    }
}
