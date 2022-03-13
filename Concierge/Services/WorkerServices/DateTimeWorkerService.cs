// <copyright file="DateTimeWorkerService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.WorkerServices
{
    using System;
    using System.ComponentModel;
    using System.Threading;

    public class DateTimeWorkerService : IWorkerService
    {
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

        public static string FormattedDateTimeNow => DateTime.Now.ToString("h:mm tt   yyyy-MM-dd");

        private BackgroundWorker UpdateTimer { get; }

        private string CurrentSetDateTime { get; set; }

        public void StartWorker(string message)
        {
            this.UpdateTimer.RunWorkerAsync();
        }

        public void StopWorker()
        {
            this.UpdateTimer.CancelAsync();
        }

        private void UpdateTimer_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            var newDateTime = FormattedDateTimeNow;

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
                worker.ReportProgress(0);
                Thread.Sleep(1000);
            }
        }
    }
}
