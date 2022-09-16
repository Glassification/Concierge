// <copyright file="AppSettings.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    using System.Collections.Generic;

    public sealed class AppSettings
    {
        public AppSettings()
        {
            this.ColorPicker = new ColorPicker();
            this.CustomColors = new Dictionary<string, string>();
            this.StartUp = new StartUp();
            this.UserSettings = new UserSettings();
        }

        public ColorPicker ColorPicker { get; set; }

        public Dictionary<string, string> CustomColors { get; set; }

        public StartUp StartUp { get; set; }

        public UserSettings UserSettings { get; set; }
    }
}
