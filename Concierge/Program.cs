// <copyright file="Program.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge
{
    using System.Reflection;
    using System.Windows;

    using Concierge.Interfaces;
    using Concierge.Interfaces.UtilityInterface;
    using Concierge.Logging;
    using Concierge.Persistence;
    using Concierge.Services;
    using Concierge.Utility;
    using Concierge.Utility.Dtos;
    using Concierge.Utility.Extensions;

    public static class Program
    {
        static Program()
        {
#if DEBUG
            IsDebug = true;
#else
            IsDebug = false;
#endif

            InitializeLogger();

            SaveStatusWindow = new SaveStatusWindow();
            Modified = true;
            Typing = false;
            ErrorService = new ErrorService(Logger);
            UndoRedoService = new UndoRedoService();
            CcsFile = new CcsFile();
            MainWindow = null;
        }

        public static bool IsDebug { get; }

        public static CcsFile CcsFile { get; set; }

        public static SaveStatusWindow SaveStatusWindow { get; }

        public static bool Typing { get; set; }

        public static bool Modified { get; private set; }

        public static Logger Logger { get; private set; }

        public static ErrorService ErrorService { get; private set; }

        public static UndoRedoService UndoRedoService { get; private set; }

        public static string AssemblyVersion
        {
            get
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return $"{version.Major}.{version.Minor}.{version.Build}";
            }
        }

        private static MainWindow MainWindow { get; set; }

        public static void InitializeMainWindow(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }

        public static MainWindowDto GetMainWindowProperties()
        {
            return new MainWindowDto()
            {
                Center = MainWindow is null ? new Point(0, 0) : new Point((int)(MainWindow.Width / 2), (int)(MainWindow.Height / 2)),
                Location = MainWindow is null ? new Point(0, 0) : MainWindow.GetAbsolutePosition(),
                ActualWidth = (int)MainWindow.ActualWidth,
                WindowState = MainWindow.WindowState,
            };
        }

        public static void Modify()
        {
            Modified = true;
        }

        public static void Unmodify()
        {
            Modified = false;
        }

        private static void InitializeLogger()
        {
            Logger = new LocalLogger(IsDebug);

            Logger.NewLine();
            Logger.Info($"Starting Concierge v{AssemblyVersion}");
        }
    }
}
