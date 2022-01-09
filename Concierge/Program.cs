// <copyright file="Program.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge
{
    using System;
    using System.Reflection;
    using System.Windows;

    using Concierge.Character;
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
            IsTyping = false;
            ErrorService = new ErrorService(Logger);
            UndoRedoService = new UndoRedoService();
            CcsFile = new CcsFile();
            MainWindow = null;
        }

        public delegate void ModifiedChangedEventHandler(object sender, EventArgs e);

        public static event ModifiedChangedEventHandler ModifiedChanged;

        public static bool IsDebug { get; }

        public static bool IsTyping { get; set; }

        public static bool IsModified => !BaseState.Equals(CcsFile.Character);

        public static CcsFile CcsFile { get; set; }

        public static SaveStatusWindow SaveStatusWindow { get; }

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

        public static MainWindow MainWindow { get; private set; }

        private static ConciergeCharacter BaseState { get; set; }

        public static void InitializeMainWindow(MainWindow mainWindow)
        {
            if (MainWindow is null)
            {
                MainWindow = mainWindow;
                Logger.Info($"{nameof(mainWindow)} is initialized.");
            }
            else
            {
                Logger.Warning($"{nameof(mainWindow)} is already initialized.");
            }
        }

        public static MainWindowDto GetMainWindowProperties()
        {
            if (MainWindow == null)
            {
                return new MainWindowDto();
            }

            return new MainWindowDto()
            {
                Center = new Point((int)(MainWindow.Width / 2), (int)(MainWindow.Height / 2)),
                Location = MainWindow.GetAbsolutePosition(),
                ActualWidth = (int)MainWindow.ActualWidth,
                WindowState = MainWindow.WindowState,
            };
        }

        public static void Modify()
        {
            ModifiedChanged?.Invoke(IsModified, new EventArgs());
        }

        public static void Unmodify()
        {
            BaseState = CcsFile.Character.DeepCopy();
            ModifiedChanged?.Invoke(IsModified, new EventArgs());
            Logger.Info($"Updated Base State.");
        }

        private static void InitializeLogger()
        {
            Logger = new LocalLogger(IsDebug);

            Logger.NewLine();
            Logger.Info($"Starting Concierge v{AssemblyVersion}");
        }
    }
}
