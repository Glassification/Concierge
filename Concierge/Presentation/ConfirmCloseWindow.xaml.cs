// <copyright file="ConfirmCloseWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for ConfirmCloseWindow.xaml
    /// </summary>
    public partial class ConfirmCloseWindow : Window
    {
        public ConfirmCloseWindow()
        {
            this.InitializeComponent();
        }

        private bool ConfirmSave { get; set; }

        public bool ShowWindow()
        {
            this.ShowDialog();

            return this.ConfirmSave;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.ConfirmSave = false;
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.ConfirmSave = false;
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.ConfirmSave = false;
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ConfirmSave = true;
            this.Hide();
        }
    }
}
