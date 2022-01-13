// <copyright file="App.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge
{
    using System.Windows;

    using Concierge.Exceptions.Enums;
    using Concierge.Interfaces;
    using Concierge.Interfaces.UtilityInterface;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Dispatcher.UnhandledException += this.OnDispatcherUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Program.ErrorService.LogError(e.Exception, Severity.Unhandled);
            e.Handled = true;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var splashScreen = new SplashScreenWindow();
            var mainWindow = new MainWindow();

            splashScreen.ShowWindow();
            mainWindow.Show();
        }
    }
}
