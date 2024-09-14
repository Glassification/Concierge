// <copyright file="AutosaveService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System;
    using System.Windows.Threading;

    /// <summary>
    /// Provides functionality for automatically saving data at regular intervals.
    /// </summary>
    public sealed class AutosaveService
    {
        private readonly FileAccessService fileAccessService;
        private readonly DispatcherTimer dispatcherTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutosaveService"/> class with the specified file access service.
        /// </summary>
        /// <param name="fileAccessService">The file access service to use for saving data.</param>
        public AutosaveService(FileAccessService fileAccessService)
        {
            this.fileAccessService = fileAccessService;
            this.dispatcherTimer = new DispatcherTimer();
            this.dispatcherTimer.Tick += this.DispatcherTimer_Autosave;

            Program.Logger.Info($"Initialized {nameof(AutosaveService)}.");
        }

        /// <summary>
        /// Starts the autosave timer with the specified interval.
        /// </summary>
        /// <param name="interval">The interval in minutes between autosaves.</param>
        public void Start(int interval)
        {
            Program.Logger.Info($"Start autosave timer.");

            this.Stop();
            this.dispatcherTimer.Interval = TimeSpan.FromMinutes(interval);
            this.dispatcherTimer.Start();
        }

        /// <summary>
        /// Stops the autosave timer.
        /// </summary>
        public void Stop()
        {
            Program.Logger.Info($"Stop autosave timer.");

            this.dispatcherTimer.Stop();
        }

        private void DispatcherTimer_Autosave(object? sender, EventArgs e)
        {
            Program.Logger.Info($"Autosaving...");

            this.fileAccessService.SaveCcs(Program.CcsFile);
        }
    }
}
