// <copyright file="AppSettings.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    public sealed class AppSettings
    {
        public AppSettings()
        {
            this.ColorPicker = new ColorPicker();
            this.StartUp = new StartUp();
            this.UserSettings = new UserSettings();
        }

        public ColorPicker ColorPicker { get; set; }

        public StartUp StartUp { get; set; }

        public UserSettings UserSettings { get; set; }
    }
}
