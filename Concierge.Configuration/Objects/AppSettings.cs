﻿// <copyright file="AppSettings.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    public sealed class AppSettings
    {
        public AppSettings()
        {
            this.StartUp = new StartUp();
            this.UserSettings = new UserSettings();
        }

        public StartUp StartUp { get; set; }

        public UserSettings UserSettings { get; set; }
    }
}