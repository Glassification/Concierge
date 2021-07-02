// <copyright file="ConfirmCloseWindow.xaml.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Presentation
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    using Concierge.Presentation.Enums;

    /// <summary>
    /// Interaction logic for ConfirmCloseWindow.xaml
    /// </summary>
    public partial class ConfirmCloseWindow : Window
    {
        public ConfirmCloseWindow()
        {
            this.InitializeComponent();
        }

        private DialogResult ConfirmSaveResult { get; set; }

        public DialogResult ShowWindow()
        {
            this.ShowDialog();

            return this.ConfirmSaveResult;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.ConfirmSaveResult = Enums.DialogResult.Cancel;
                    this.Hide();
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.ConfirmSaveResult = Enums.DialogResult.Cancel;
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.ConfirmSaveResult = Enums.DialogResult.Cancel;
            this.Hide();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            this.ConfirmSaveResult = Enums.DialogResult.No;
            this.Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.ConfirmSaveResult = Enums.DialogResult.Yes;
            this.Hide();
        }

        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            this.CloseButton.Foreground = Brushes.Black;
        }

        private void CloseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            this.CloseButton.Foreground = Brushes.White;
        }
    }
}
