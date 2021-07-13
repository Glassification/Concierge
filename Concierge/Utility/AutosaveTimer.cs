// <copyright file="AutosaveTimer.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Utility
{
    using System;
    using System.Windows.Threading;

    using Concierge.Persistence;

    public class AutosaveTimer
    {
        public AutosaveTimer()
        {
            this.DispatcherTimer = new DispatcherTimer();
            this.DispatcherTimer.Tick += this.DispatcherTimer_Autosave;

            Program.Logger.Info($"Initialized {nameof(AutosaveTimer)}.");
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

        private void DispatcherTimer_Autosave(object sender, EventArgs e)
        {
            Program.Logger.Info($"Autosaving...");

            CharacterSaver.SaveCharacterSheetJson(Program.CcsFile);
        }
    }
}
