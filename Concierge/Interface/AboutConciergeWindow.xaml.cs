// <copyright file="AboutConciergeWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Interface
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

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

        private void Read()
        {
            this.VersionField.Text = Constants.AssemblyVersion;
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
            ConciergeSound.ButtonClick();
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ConciergeSound.ButtonClick();
            this.Hide();
        }

        private void Button_MouseEnter(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.Black;
        }

        private void Button_MouseLeave(object sender, RoutedEventArgs e)
        {
            (sender as Button).Foreground = Brushes.White;
        }
    }
}
