// <copyright file="App.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge
{
    using System.Windows;

    using Concierge.Characters;
    using Concierge.Persistence;
    using Concierge.Services;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Program.CcsFile = new CcsFile()
            {
                Character = new Character(),
            };

            var commandLineService = new CommandLineService();
            commandLineService.ReadCommandLineArgs(e.Args);
        }
    }
}
