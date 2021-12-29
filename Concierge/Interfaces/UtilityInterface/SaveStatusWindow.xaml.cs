// <copyright file="SaveStatusWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.UtilityInterface
{
    using System.ComponentModel;
    using System.Threading;

    using Concierge.Interfaces.Components;

    /// <summary>
    /// Interaction logic for SaveStatusWindow.xaml.
    /// </summary>
    public partial class SaveStatusWindow : ConciergeWindow
    {
        public SaveStatusWindow()
        {
            this.InitializeComponent();

            this.ProgressBarFiller = new BackgroundWorker
            {
                WorkerReportsProgress = true,
            };
            this.ProgressBarFiller.DoWork += this.ProgressBarFiller_DoWork;
            this.ProgressBarFiller.ProgressChanged += this.ProgressBarFiller_ProgressChanged;
            this.ProgressBarFiller.RunWorkerCompleted += this.ProgressBarFiller_RunWorkerCompleted;
        }

        private BackgroundWorker ProgressBarFiller { get; }

        public void ShowWindow()
        {
            this.ProgressBarFiller.RunWorkerAsync();

            this.ShowConciergeWindow();
        }

        private void ProgressBarFiller_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.HideConciergeWindow();
        }

        private void ProgressBarFiller_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(3);
            }
        }

        private void ProgressBarFiller_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.SaveProgressBar.Value = e.ProgressPercentage;
        }
    }
}
