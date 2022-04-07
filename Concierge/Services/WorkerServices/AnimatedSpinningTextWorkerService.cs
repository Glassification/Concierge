// <copyright file="AnimatedSpinningTextWorkerService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.WorkerServices
{
    using System;
    using System.ComponentModel;
    using System.Threading;

    public class AnimatedSpinningTextWorkerService : IWorkerService
    {
        private const int SleepTime = 150;

        private readonly int displayTime;
        private readonly char[] cursor = new char[4] { '|', '\\', '-', '/' };

        public AnimatedSpinningTextWorkerService(int displayTime)
        {
            this.UpdateText = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            this.UpdateText.DoWork += this.UpdateText_DoWork;
            this.UpdateText.ProgressChanged += this.UpdateText_ProgressChanged;
            this.UpdateText.RunWorkerCompleted += this.UpdateText_RunWorkerCompleted;

            this.Message = string.Empty;
            this.Counter = 0;
            this.displayTime = displayTime;
        }

        public delegate void TextUpdatedEventHandler(object sender, EventArgs e);

        public delegate void WorkerCompletedEventHandler(object sender, EventArgs e);

        public event TextUpdatedEventHandler? TextUpdated;

        public event WorkerCompletedEventHandler? WorkerCompleted;

        private BackgroundWorker UpdateText { get; }

        private int Counter { get; set; }

        private string Message { get; set; }

        public void StartWorker(string message)
        {
            this.Message = message;
            this.Counter = 0;

            if (this.UpdateText.IsBusy)
            {
                this.UpdateText.CancelAsync();
            }
            else
            {
                this.UpdateText.RunWorkerAsync();
            }
        }

        public void StopWorker()
        {
            this.UpdateText.CancelAsync();
        }

        private void UpdateText_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            if (this.Counter > 3)
            {
                this.Counter = 0;
            }

            this.TextUpdated?.Invoke($"{this.cursor[this.Counter]}{this.Message}{this.cursor[this.Counter]}", new EventArgs());
            this.Counter++;
        }

        private void UpdateText_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (sender is not BackgroundWorker worker)
            {
                return;
            }

            for (int i = 0; i < this.displayTime; i++)
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

        private void UpdateText_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            this.TextUpdated?.Invoke(string.Empty, new EventArgs());

            if (e.Cancelled)
            {
                this.UpdateText.RunWorkerAsync();
            }
            else
            {
                this.WorkerCompleted?.Invoke(this, new EventArgs());
            }
        }
    }
}
