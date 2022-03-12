// <copyright file="AlertMessageService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System;
    using System.ComponentModel;
    using System.Threading;

    public class AlertMessageService
    {
        private const int DisplayTime = 17;
        private const int SleepTime = 150;

        public AlertMessageService()
        {
            this.UpdateMessage = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            this.UpdateMessage.DoWork += this.UpdateMessage_DoWork;
            this.UpdateMessage.ProgressChanged += this.UpdateMessage_ProgressChanged;
            this.UpdateMessage.RunWorkerCompleted += this.UpdateMessage_RunWorkerCompleted;

            this.Message = string.Empty;
        }

        public delegate void MessageUpdatedEventHandler(object sender, EventArgs e);

        public event MessageUpdatedEventHandler? MessageUpdated;

        private BackgroundWorker UpdateMessage { get; }

        private string Message { get; set; }

        public void StartTimer(string message)
        {
            this.Message = message;

            if (this.UpdateMessage.IsBusy)
            {
                this.UpdateMessage.CancelAsync();
            }
            else
            {
                this.UpdateMessage.RunWorkerAsync();
            }
        }

        public void StopTimer()
        {
            this.UpdateMessage.CancelAsync();
        }

        private void UpdateMessage_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            this.MessageUpdated?.Invoke(this.Message, new EventArgs());
        }

        private void UpdateMessage_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (sender is not BackgroundWorker worker)
            {
                return;
            }

            for (int i = 0; i < DisplayTime; i++)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }

                worker.ReportProgress(i);
                Thread.Sleep(SleepTime);
            }
        }

        private void UpdateMessage_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            this.MessageUpdated?.Invoke(string.Empty, new EventArgs());

            if (e.Cancelled)
            {
                this.UpdateMessage.RunWorkerAsync();
            }
        }
    }
}
