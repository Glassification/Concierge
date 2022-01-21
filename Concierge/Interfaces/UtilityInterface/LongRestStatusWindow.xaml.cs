// <copyright file="LongRestStatusWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.UtilityInterface
{
    using System.ComponentModel;
    using System.Threading;

    using Concierge.Interfaces.Components;

    /// <summary>
    /// Interaction logic for LongRestStatusWindow.xaml.
    /// </summary>
    public partial class LongRestStatusWindow : ConciergeWindow
    {
        public LongRestStatusWindow()
        {
            this.InitializeComponent();

            this.WindowDisplayTimer = new BackgroundWorker
            {
                WorkerReportsProgress = true,
            };
            this.WindowDisplayTimer.DoWork += this.ProgressBarFiller_DoWork;
            this.WindowDisplayTimer.RunWorkerCompleted += this.ProgressBarFiller_RunWorkerCompleted;
        }

        private BackgroundWorker WindowDisplayTimer { get; }

        public override void ShowWindow()
        {
            this.WindowDisplayTimer.RunWorkerAsync();
            this.ShowConciergeWindow();
        }

        private void ProgressBarFiller_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            this.HideConciergeWindow();
        }

        private void ProgressBarFiller_DoWork(object? sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1100);
        }
    }
}
