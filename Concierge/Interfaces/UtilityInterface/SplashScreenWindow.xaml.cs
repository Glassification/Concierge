// <copyright file="SplashScreenWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.UtilityInterface
{
    using System;
    using System.Windows;

    using Concierge.Services.WorkerServices;
    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for SplashScreenWindow.xaml.
    /// </summary>
    public partial class SplashScreenWindow : Window
    {
        private readonly AnimatedTextWorkerService animatedTextWorkerService = new (Services.Enums.TextAnimations.LoadingDots, 20);

        public SplashScreenWindow()
        {
            this.InitializeComponent();

            this.animatedTextWorkerService.TextUpdated += this.SplashScreenWindow_TextUpdated;
            this.animatedTextWorkerService.WorkerCompleted += this.SplashScreenWindow_WorkerCompleted;

            this.VersionText.Text = $"v{Program.AssemblyVersion}{(Program.IsDebug ? " - Debug" : string.Empty)}";
            this.Background = ConciergeColors.ProficiencyBrush;
        }

        public void ShowWindow()
        {
            this.animatedTextWorkerService.StartWorker("Loading");
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
