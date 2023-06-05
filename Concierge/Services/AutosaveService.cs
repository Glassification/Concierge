// <copyright file="AutosaveService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System;
    using System.Windows.Threading;

    public sealed class AutosaveService
    {
        private readonly FileAccessService fileAccessService;

        public AutosaveService(FileAccessService fileAccessService)
        {
            this.fileAccessService = fileAccessService;
            this.DispatcherTimer = new DispatcherTimer();
            this.DispatcherTimer.Tick += this.DispatcherTimer_Autosave;

            Program.Logger.Info($"Initialized {nameof(AutosaveService)}.");
        }

        private DispatcherTimer DispatcherTimer { get; set; }

        public void Start(int interval)
        {
            Program.Logger.Info($"Start autosave timer.");

            this.Stop();
            this.DispatcherTimer.Interval = TimeSpan.FromMinutes(interval);
            this.DispatcherTimer.Start();
        }

        public void Stop()
        {
            Program.Logger.Info($"Stop autosave timer.");

            this.DispatcherTimer.Stop();
        }

        private void DispatcherTimer_Autosave(object? sender, EventArgs e)
        {
            Program.Logger.Info($"Autosaving...");

            this.fileAccessService.Save(Program.CcsFile);
        }
    }
}
