// <copyright file="AboutConciergeWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interfaces.UtilityInterface
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Concierge.Utility;

    /// <summary>
    /// Interaction logic for AboutConciergeWindow.xaml.
    /// </summary>
    public partial class AboutConciergeWindow : Window
    {
        private const char CopyrightSymbol = (char)169;

        public AboutConciergeWindow()
        {
            this.InitializeComponent();
        }

        public void ShowWindow()
        {
            this.Read();
            this.ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            this.Hide();
        }

        private void Read()
        {
            this.VersionField.Text = Program.AssemblyVersion;
            this.DesignerField.Text = Constants.Designer;
            this.LicenseField.Text = Constants.License;
            this.CopyrightField.Text = $"{CopyrightSymbol}{Constants.Copyright}";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
