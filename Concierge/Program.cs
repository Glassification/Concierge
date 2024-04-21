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
    using Concierge.Display;
    using Concierge.Logging;
    using Concierge.Persistence;
    using Concierge.Persistence.ReadWriters;
    using Concierge.Services;
    using Microsoft.Win32;

    public static class Program
    {
        private const int Windows11 = 22000;

        private static int buildNumber = -1;
        private static ConciergeVersion version = new ();

        static Program()
        {
#if DEBUG
            IsDebug = true;
#else
            IsDebug = false;
#endif

            Logger = new LocalLogger(ConciergeFiles.LoggingDirectory, IsDebug);
            Logger.Start(AssemblyVersion.ToString());

            IsTyping = false;
            ErrorService = new ErrorService(Logger);
            UndoRedoService = new UndoRedoService();
            MessageService = new MessageService();
            CcsFile = new CcsFile();
            MainWindow = null;
            BaseState = new CharacterSheet();

            SoundService.SetVolume();

            var colorReadWriter = new CustomColorReadWriter(ErrorService);
            var consolReadWrite = new ConsoleReadWriter(ErrorService);

            CustomItemService = new CustomItemService();
            CustomColorService = colorReadWriter.ReadJson<CustomColorService>(Path.Combine(ConciergeFiles.GetCorrectCustomColorsPath(), ConciergeFiles.CustomColorsName));

            consolReadWrite.Clear(Path.Combine(ConciergeFiles.AppDataDirectory, ConciergeFiles.ConsoleOutput));
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

        public static CustomItemService CustomItemService { get; private set; }

        public static MessageService MessageService { get; private set; }

        public static ConciergeVersion AssemblyVersion
        {
            get
            {
                if (!version.IsEmpty)
                {
                    return version;
                }

                version = new ConciergeVersion(Assembly.GetExecutingAssembly().GetName().Version ?? new Version());
                return version;
            }
        }

        public static bool IsWindows11
        {
            get
            {
                if (buildNumber >= 0)
                {
                    return buildNumber >= Windows11;
                }

                var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

                var currentBuildStr = reg?.GetValue("CurrentBuild") as string;
                if (int.TryParse(currentBuildStr, out int currentBuild))
                {
                    Logger.Info($"Found current Windows version: {currentBuild}.");
                    buildNumber = currentBuild;
                    return currentBuild >= Windows11;
                }

                return false;
            }
        }

        public static MainWindow? MainWindow { get; private set; }

        private static CharacterSheet BaseState { get; set; }

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
