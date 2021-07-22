// <copyright file="SaveStatusWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface.HelperUi
{
    using System.ComponentModel;
    using System.Threading;
    using System.Windows;

    /// <summary>
    /// Interaction logic for SaveStatusWindow.xaml.
    /// </summary>
    public partial class SaveStatusWindow : Window
    {
        private const int MarginOffset = 10;

        public SaveStatusWindow()
        {
            this.InitializeComponent();

            this.Left = SystemParameters.PrimaryScreenWidth - this.Width - MarginOffset;
            this.Top = SystemParameters.PrimaryScreenHeight - this.Height - MarginOffset;

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
            this.ShowDialog();
        }

        private void ProgressBarFiller_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Hide();
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
