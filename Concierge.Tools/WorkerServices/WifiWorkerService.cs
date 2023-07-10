// <copyright file="WifiWorkerService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.WorkerServices
{
    using System;
    using System.ComponentModel;
    using System.Threading;

    public sealed class WifiWorkerService : IWorkerService
    {
        private const int OneMinute = 60000;

        public WifiWorkerService()
        {
            this.UpdateTimer = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            this.UpdateTimer.DoWork += this.UpdateTimer_DoWork;
            this.UpdateTimer.ProgressChanged += this.UpdateTimer_ProgressChanged;
        }

        public delegate void WifiUpdatedEventHandler(object sender, EventArgs e);

        public event WifiUpdatedEventHandler? WifiUpdated;

        private BackgroundWorker UpdateTimer { get; }

        public void StartWorker(string message)
        {
            if (this.UpdateTimer.IsBusy)
            {
                this.UpdateTimer.CancelAsync();
            }
            else
            {
                this.UpdateTimer.RunWorkerAsync();
            }
        }

        public void StopWorker()
        {
            this.UpdateTimer.CancelAsync();
        }

        private void UpdateTimer_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            this.WifiUpdated?.Invoke(new object(), new EventArgs());
        }

        private void UpdateTimer_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (sender is not BackgroundWorker worker)
            {
                return;
            }

            while (true)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }

                worker.ReportProgress(0);
                Thread.Sleep(OneMinute);
            }
        }
    }
}
