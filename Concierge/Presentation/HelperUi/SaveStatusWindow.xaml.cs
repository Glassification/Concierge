// <copyright file="SaveStatusWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation.HelperUi
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows;

    /// <summary>
    /// Interaction logic for SaveStatusWindow.xaml.
    /// </summary>
    public partial class SaveStatusWindow : Window
    {
        public SaveStatusWindow()
        {
            this.InitializeComponent();

            this.Left = SystemParameters.PrimaryScreenWidth - this.Width - 10;
            this.Top = SystemParameters.PrimaryScreenHeight - this.Height - 10;
        }

        public void ShowWindow()
        {
            this.ShowDialog();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            var progressBarFiller = new BackgroundWorker
            {
                WorkerReportsProgress = true,
            };
            progressBarFiller.DoWork += this.ProgressBarFiller_DoWork;
            progressBarFiller.ProgressChanged += this.ProgressBarFiller_ProgressChanged;
            progressBarFiller.RunWorkerCompleted += this.ProgressBarFiller_RunWorkerCompleted;

            progressBarFiller.RunWorkerAsync();
        }

        private void ProgressBarFiller_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Hide();
        }

        private void ProgressBarFiller_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(10);
            }
        }

        private void ProgressBarFiller_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.SaveProgressBar.Value = e.ProgressPercentage;
        }
    }
}
