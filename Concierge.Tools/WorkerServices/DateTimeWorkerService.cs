// <copyright file="DateTimeWorkerService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.WorkerServices
{
    using System;
    using System.ComponentModel;
    using System.Threading;

    using Concierge.Common;

    public sealed class DateTimeWorkerService : IWorkerService
    {
        private const int OneSecond = 1000;

        public DateTimeWorkerService()
        {
            this.UpdateTimer = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            this.UpdateTimer.DoWork += this.UpdateTimer_DoWork;
            this.UpdateTimer.ProgressChanged += this.UpdateTimer_ProgressChanged;
            this.CurrentSetDateTime = string.Empty;
        }

        public delegate void TimeUpdatedEventHandler(object sender, EventArgs e);

        public event TimeUpdatedEventHandler? TimeUpdated;

        private BackgroundWorker UpdateTimer { get; }

        private string CurrentSetDateTime { get; set; }

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
            var newDateTime = ConciergeDateTime.StatusMenuNow;

            if (!this.CurrentSetDateTime.Equals(newDateTime))
            {
                this.CurrentSetDateTime = newDateTime;
                this.TimeUpdated?.Invoke(this.CurrentSetDateTime, new EventArgs());
            }
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
                Thread.Sleep(OneSecond);
            }
        }
    }
}
