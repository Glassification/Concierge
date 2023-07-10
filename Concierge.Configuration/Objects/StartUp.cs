// <copyright file="StartUp.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Configuration.Objects
{
    public sealed class StartUp
    {
        public StartUp()
        {
        }

        public bool CompressCharacterSheet { get; set; }

        public bool EnableConsole { get; set; }

        public bool EnableNetworkAccess { get; set; }

        public bool ShowSplashScreen { get; set; }
    }
}
