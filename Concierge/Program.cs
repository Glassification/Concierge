// <copyright file="Program.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge
{
    using Concierge.Logging;
    using Concierge.Persistence;
    using Concierge.Presentation.HelperUi;
    using Concierge.Services;
    using Concierge.Utility;

    public static class Program
    {
        static Program()
        {
            InitializeLogger();

            ConciergeMessageWindow = new ConciergeMessageWindow();
            SaveStatusWindow = new SaveStatusWindow();
            Modified = true;
            Typing = false;
            ErrorService = new ErrorService(Logger);
        }

        public static CcsFile CcsFile { get; set; }

        public static ConciergeMessageWindow ConciergeMessageWindow { get; }

        public static SaveStatusWindow SaveStatusWindow { get; }

        public static bool Typing { get; set; }

        public static bool Modified { get; set; }

        public static LocalLogger Logger { get; private set; }

        public static ErrorService ErrorService { get; private set; }

        private static void InitializeLogger()
        {
            Logger = new LocalLogger();

            Logger.NewLine();
            Logger.Info($"Starting Concierge v{Constants.AssemblyVersion}");
        }
    }
}
