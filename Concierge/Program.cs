// <copyright file="Program.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge
{
    using System;
    using System.IO;
    using System.Reflection;

    using Concierge.Character;
    using Concierge.Common;
    using Concierge.Common.Utilities;
    using Concierge.Display;
    using Concierge.Logging;
    using Concierge.Persistence;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Services;
    using Microsoft.Win32;

    public static class Program
    {
        private static int buildNumber = -1;

        static Program()
        {
#if DEBUG
            IsDebug = true;
#else
            IsDebug = false;
#endif

            Logger = new LocalLogger(IsDebug);
            Logger.Start(AssemblyVersion);

            IsTyping = false;
            ErrorService = new ErrorService(Logger);
            UndoRedoService = new UndoRedoService();
            CcsFile = new CcsFile();
            MainWindow = null;
            BaseState = new ConciergeCharacter();
            CustomColorService = CustomColorReadWriter.Read(Path.Combine(ConciergeFiles.GetCorrectCustomColorsPath(), ConciergeFiles.CustomColorsName));

            ConsoleReadWriter.Clear(Path.Combine(ConciergeFiles.AppDataDirectory, ConciergeFiles.ConsoleOutput));
        }

        public delegate void ModifiedChangedEventHandler(object sender, EventArgs e);

        public static event ModifiedChangedEventHandler? ModifiedChanged;

        public static bool IsDebug { get; }

        public static bool IsTyping { get; private set; }

        public static bool IsModified => !BaseState.Equals(CcsFile.Character);

        public static CcsFile CcsFile { get; set; }

        public static Logger Logger { get; private set; }

        public static ErrorService ErrorService { get; private set; }

        public static UndoRedoService UndoRedoService { get; private set; }

        public static CustomColorService CustomColorService { get; private set; }

        public static string AssemblyVersion
        {
            get
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return $"{version?.Major}.{version?.Minor}.{version?.Build}";
            }
        }

        public static bool IsWindows11
        {
            get
            {
                if (buildNumber >= 0)
                {
                    return buildNumber >= 22000;
                }

                var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

                var currentBuildStr = reg?.GetValue("CurrentBuild") as string;
                if (int.TryParse(currentBuildStr, out int currentBuild))
                {
                    buildNumber = currentBuild;
                    return currentBuild >= 22000;
                }

                return false;
            }
        }

        public static MainWindow? MainWindow { get; private set; }

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

        public static void Typing()
        {
            IsTyping = true;
        }

        public static void NotTyping()
        {
            IsTyping = false;
        }
    }
}
