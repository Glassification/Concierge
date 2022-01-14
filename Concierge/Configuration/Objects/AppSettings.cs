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

        public Dictionary<string, string> CustomColors { get; set; }

        public StartUp StartUp { get; set; }

        public UserSettings UserSettings { get; set; }
    }
}
