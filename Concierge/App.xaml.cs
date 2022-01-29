// <copyright file="App.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge
{
    using System.Windows;
    using System.Windows.Threading;

    using Concierge.Configuration;
    using Concierge.Exceptions;
    using Concierge.Interfaces;
    using Concierge.Interfaces.UtilityInterface;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Dispatcher.UnhandledException += this.Dispatcher_UnhandledException;
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Program.ErrorService.LogError(new UnhandledException(e.Exception));
            e.Handled = true;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow();
            if (AppSettingsManager.StartUp.ShowSplashScreen)
            {
                new SplashScreenWindow().ShowWindow();
            }

            mainWindow.Show();
        }
    }
}
