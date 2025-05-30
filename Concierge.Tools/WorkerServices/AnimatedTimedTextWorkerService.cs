﻿// <copyright file="AnimatedTimedTextWorkerService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.WorkerServices
{
    using System;
    using System.ComponentModel;
    using System.Threading;

    public sealed class AnimatedTimedTextWorkerService : IWorkerService
    {
        private const int SleepTime = 150;

        private readonly int displayTime;

        public AnimatedTimedTextWorkerService(int displayTime)
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
            this.displayTime = displayTime;
        }

        public delegate void TextUpdatedEventHandler(object sender, EventArgs e);

        public delegate void WorkerCompletedEventHandler(object sender, EventArgs e);

        public event TextUpdatedEventHandler? TextUpdated;

        public event WorkerCompletedEventHandler? WorkerCompleted;

        private BackgroundWorker UpdateText { get; }

        private string Message { get; set; }

        public void StartWorker(string message)
        {
            this.Message = message;

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
            this.TextUpdated?.Invoke(this.Message, new EventArgs());
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
