// <copyright file="AnimatedTextWorkerService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services.WorkerServices
{
    using System;
    using System.ComponentModel;
    using System.Threading;

    using Concierge.Services.Enums;
    using Concierge.Utility.Utilities;

    public class AnimatedTextWorkerService : IWorkerService
    {
        private readonly TextAnimations textAnimations;
        private readonly int displayTime;
        private readonly Random random = new ();

        public AnimatedTextWorkerService(TextAnimations textAnimations, int displayTime)
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
            this.textAnimations = textAnimations;
            this.displayTime = displayTime;
        }

        public delegate void TextUpdatedEventHandler(object sender, EventArgs e);

        public delegate void WorkerCompletedEventHandler(object sender, EventArgs e);

        public event TextUpdatedEventHandler? TextUpdated;

        public event WorkerCompletedEventHandler? WorkerCompleted;

        private BackgroundWorker UpdateText { get; }

        private string Message { get; set; }

        private int Counter { get; set; }

        private int SleepTime
        {
            get
            {
                return this.textAnimations switch
                {
                    TextAnimations.LoadingDots => this.random.Next(110, 190),
                    _ => 150,
                };
            }
        }

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

        private void NoneAnimation()
        {
            this.TextUpdated?.Invoke(this.Message, new EventArgs());
        }

        private void LoadingDotsAnimation()
        {
            if (this.Counter > 5)
            {
                this.Counter = 0;
            }

            this.TextUpdated?.Invoke($"{this.Message}{StringUtility.CreateCharacters(".", this.Counter)}", new EventArgs());
            this.Counter++;
        }

        private void SpinningEndsAnimation()
        {
        }

        private void UpdateText_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            switch (this.textAnimations)
            {
                case TextAnimations.None:
                    this.NoneAnimation();
                    break;
                case TextAnimations.LoadingDots:
                    this.LoadingDotsAnimation();
                    break;
                case TextAnimations.SpinningEnds:
                    this.SpinningEndsAnimation();
                    break;
            }
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
                Thread.Sleep(this.SleepTime);
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
