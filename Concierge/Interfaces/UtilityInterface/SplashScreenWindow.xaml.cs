// <copyright file="SplashScreenWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.UtilityInterface
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows;

    using Concierge.Utility.Colors;
    using Concierge.Utility.Utilities;

    /// <summary>
    /// Interaction logic for SplashScreenWindow.xaml.
    /// </summary>
    public partial class SplashScreenWindow : Window
    {
        private const int DisplayTime = 20;
        private const int MinHoldTime = 110;
        private const int MaxHoldTime = 190;

        public SplashScreenWindow()
        {
            this.InitializeComponent();
            this.WindowTimer = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
            };
            this.WindowTimer.DoWork += this.WindowTimer_DoWork;
            this.WindowTimer.ProgressChanged += this.WindowTimer_ProgressChanged;
            this.WindowTimer.RunWorkerCompleted += this.WindowTimer_RunWorkerCompleted;
            this.Counter = 0;
            this.VersionText.Text = $"v{Program.AssemblyVersion}{(Program.IsDebug ? " - Debug" : string.Empty)}";
            this.Background = ConciergeColors.ProficiencyBrush;
        }

        private BackgroundWorker WindowTimer { get; }

        private int Counter { get; set; }

        public void ShowWindow()
        {
            this.WindowTimer.RunWorkerAsync();
            this.ShowDialog();
        }

        private void WindowTimer_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            if (this.Counter > 5)
            {
                this.Counter = 0;
            }

            this.LoadingText.Text = $"Loading{StringUtility.CreateCharacters(".", this.Counter)}";

            this.Counter++;
        }

        private void WindowTimer_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        private void WindowTimer_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (sender is not BackgroundWorker worker)
            {
                return;
            }

            var random = new Random();
            for (int i = 0; i < DisplayTime; i++)
            {
                worker.ReportProgress(i);
                Thread.Sleep(random.Next(MinHoldTime, MaxHoldTime));
            }
        }
    }
}
