// <copyright file="App.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;

    using Concierge.Common.Exceptions;
    using Concierge.Common.Utilities;
    using Concierge.Configuration;
    using Concierge.Display;
    using Concierge.Display.Enums;
    using Concierge.Display.Utility;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        private readonly CommandLineService commandLineService = new ();

        public App()
        {
            this.Dispatcher.UnhandledException += this.Dispatcher_UnhandledException;
        }

        private static bool VerifyResourceManager()
        {
            try
            {
                var testResult = Concierge.Properties.Resources.ResourceValidation;
                return testResult.Equals("Success");
            }
            catch (Exception ex)
            {
                Program.ErrorService.LogError(new ResourceManagerException(ex));
                return false;
            }
        }

        private static void LoadAllPages(MainWindow mainWindow)
        {
            var pages = EnumUtility.GetValues<ConciergePage>();
            foreach (var page in pages)
            {
                mainWindow.MoveSelection(page);
            }

            mainWindow.MoveSelection(ConciergePage.Overview);
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Program.ErrorService.LogError(new UnhandledException(e.Exception));
            e.Handled = true;
        }

        /// <summary>
        /// Main entry point to the application.
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Program.Logger.Info($"Verify Resource Manager.");
            if (!VerifyResourceManager())
            {
                return;
            }

            var mainWindow = new MainWindow();
            Program.Logger.Info($"Cycle through all pages.");
            LoadAllPages(mainWindow);
            if (AppSettingsManager.StartUp.ShowSplashScreen)
            {
                new SplashScreenWindow().ShowWindow();
            }

            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue));
            mainWindow.Show();

            this.commandLineService.ReadCommandLineArgs();
            mainWindow.MessageBar.DrawActiveFile(Program.CcsFile);
            mainWindow.DrawAll();
            Program.Unmodify();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            SoundService.CloseAll();
        }
    }
}
