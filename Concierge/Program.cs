// <copyright file="Program.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Character;
    using Concierge.Interfaces;
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

            Logger = new LocalLogger(IsDebug);
            Logger.Start(AssemblyVersion);

            IsTyping = false;
            ErrorService = new ErrorService(Logger);
            UndoRedoService = new UndoRedoService();
            CcsFile = new CcsFile();
            MainWindow = null;
            BaseState = new ConciergeCharacter();
        }

        public delegate void ModifiedChangedEventHandler(object sender, EventArgs e);

        public static event ModifiedChangedEventHandler? ModifiedChanged;

        public static bool IsDebug { get; }

        public static bool IsTyping { get; set; }

        public static bool IsModified => !BaseState.Equals(CcsFile.Character);

        public static CcsFile CcsFile { get; set; }

        public static Logger Logger { get; private set; }

        public static ErrorService ErrorService { get; private set; }

        public static UndoRedoService UndoRedoService { get; private set; }

        public static string AssemblyVersion
        {
            get
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return $"{version?.Major}.{version?.Minor}.{version?.Build}";
            }
        }

        public static bool DeviceHasTouchInput
        {
            get
            {
                foreach (TabletDevice tabletDevice in Tablet.TabletDevices)
                {
                    if (tabletDevice.Type == TabletDeviceType.Touch)
                    {
                        return true;
                    }
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
    }
}
