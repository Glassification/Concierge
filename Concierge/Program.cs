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
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the main program logic and settings for the application.
    /// </summary>
    public static class Program
    {
        private static ConciergeVersion version = new ();
        private static CharacterSheet baseState = new ();

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
            IsDrawing = false;
            ErrorService = new ErrorService(Logger);
            UndoRedoService = new UndoRedoService();
            MessageService = new MessageService();
            CcsFile = new CcsFile();
            MainWindow = null;
            baseState = new CharacterSheet();

            SoundService.SetVolume();

            var colorReadWriter = new CustomColorReadWriter(ErrorService);
            var consolReadWrite = new ConsoleReadWriter(ErrorService);

            CustomItemService = new CustomItemService();
            CustomColorService = colorReadWriter.ReadJson<CustomColorService>(Path.Combine(ConciergeFiles.CustomColorsPath, ConciergeFiles.CustomColorsName));

            consolReadWrite.Clear(Path.Combine(ConciergeFiles.AppDataDirectory, ConciergeFiles.ConsoleOutput));
        }

        /// <summary>
        /// Handler for event that occurs when the modification status changes.
        /// </summary>
        public delegate void ModifiedChangedEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Event that occurs when the modification status changes.
        /// </summary>
        public static event ModifiedChangedEventHandler? ModifiedChanged;

        /// <summary>
        /// Gets a value indicating whether the application is in debug mode.
        /// </summary>
        public static bool IsDebug { get; }

        /// <summary>
        /// Gets a value indicating whether the user is typing.
        /// </summary>
        public static bool IsTyping { get; private set; }

        /// <summary>
        /// Gets a value indicating whether a window is drawing fields.
        /// </summary>
        public static bool IsDrawing { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the current character sheet is modified.
        /// </summary>
        public static bool IsModified => !baseState.Equals(CcsFile.Character);

        /// <summary>
        /// Gets or sets the current character sheet file.
        /// </summary>
        public static CcsFile CcsFile { get; set; }

        /// <summary>
        /// Gets the logger instance for logging application messages.
        /// </summary>
        public static Logger Logger { get; private set; }

        /// <summary>
        /// Gets the error service instance for handling errors.
        /// </summary>
        public static ErrorService ErrorService { get; private set; }

        /// <summary>
        /// Gets the undo/redo service instance.
        /// </summary>
        public static UndoRedoService UndoRedoService { get; private set; }

        /// <summary>
        /// Gets the custom color service instance for managing custom colors.
        /// </summary>
        public static CustomColorService CustomColorService { get; private set; }

        /// <summary>
        /// Gets the custom item service instance for managing custom items.
        /// </summary>
        public static CustomItemService CustomItemService { get; private set; }

        /// <summary>
        /// Gets the message service instance for displaying messages.
        /// </summary>
        public static MessageService MessageService { get; private set; }

        /// <summary>
        /// Gets the version of the currently executing assembly.
        /// </summary>
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

        /// <summary>
        /// Gets the main application window.
        /// </summary>
        public static MainWindow? MainWindow { get; private set; }

        /// <summary>
        /// Initializes the main application window.
        /// </summary>
        /// <param name="mainWindow">The main window instance to initialize.</param>
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

        /// <summary>
        /// Triggers the ModifiedChanged event.
        /// </summary>
        public static void Modify()
        {
            ModifiedChanged?.Invoke(IsModified, new EventArgs());
        }

        /// <summary>
        /// Sets the base state of the character sheet to the current state, triggering the ModifiedChanged event.
        /// </summary>
        public static void Unmodify()
        {
            baseState = CcsFile.Character.DeepCopy();
            ModifiedChanged?.Invoke(IsModified, new EventArgs());
            Logger.Info($"Updated Base State.");
        }

        /// <summary>
        /// Sets the IsTyping flag to true, indicating that the user is typing.
        /// </summary>
        public static void Typing()
        {
            IsTyping = true;
        }

        /// <summary>
        /// Sets the IsTyping flag to false, indicating that the user is not typing.
        /// </summary>
        public static void NotTyping()
        {
            IsTyping = false;
        }

        /// <summary>
        /// Sets the IsDrawing flag to true, indicating that a window is drawing fields.
        /// </summary>
        public static void Drawing()
        {
            IsDrawing = true;
        }

        /// <summary>
        /// Sets the IsDrawing flag to false, indicating that a window is finished drawing fields.
        /// </summary>
        public static void NotDrawing()
        {
            IsDrawing = false;
        }

        /// <summary>
        /// Serializes the character base state to a formatted string.
        /// </summary>
        public static string GetBaseState()
        {
            return JsonConvert.SerializeObject(baseState, Formatting.Indented);
        }
    }
}
