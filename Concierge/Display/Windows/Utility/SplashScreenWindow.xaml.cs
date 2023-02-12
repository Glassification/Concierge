// <copyright file="SplashScreenWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Display.Utility
{
    using System;
    using System.Windows;

    using Concierge.Services.Enums;
    using Concierge.Services.WorkerServices;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for SplashScreenWindow.xaml.
    /// </summary>
    public partial class SplashScreenWindow : Window
    {
        private readonly AnimatedLoadingTextWorkerService animatedLoadingTextWorkerService = new (LoadingType.Random, 20);

        public SplashScreenWindow()
        {
            this.InitializeComponent();

            this.animatedLoadingTextWorkerService.TextUpdated += this.SplashScreenWindow_TextUpdated;
            this.animatedLoadingTextWorkerService.WorkerCompleted += this.SplashScreenWindow_WorkerCompleted;

            this.VersionText.Text = $"v{Program.AssemblyVersion}{(Program.IsDebug ? " - Debug" : string.Empty)}";
            this.Background = ConciergeColors.ProficiencyBrush;
        }

        public void ShowWindow()
        {
            this.animatedLoadingTextWorkerService.StartWorker("Loading");
            this.ShowDialog();
        }

        private void SplashScreenWindow_TextUpdated(object sender, EventArgs e)
        {
            if (sender is string updatedText)
            {
                this.LoadingText.Text = updatedText;
            }
        }

        private void SplashScreenWindow_WorkerCompleted(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
